﻿using System.Text.Json;
using BusinessObjects.Dtos.Account;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.IService;
using WebAppRazor.Mappers;

namespace WebAppRazor.Pages
{
    public class AuthenticationModel : AuthorPageServiceModel
    {
        public readonly IServiceManager _service;

        public AccountLoginDto AccountLogin { get; set; }

        public AccountRegisterDto AccountRegister { get; set; }
        public List<string> Message { get; set; } = new List<string>();
        public string SuccessMessage { get; set; }
        public int TabIndex { get; set; }

        public AuthenticationModel(IServiceManager service)
        {
            _service = service;
            TabIndex = 1;
        }

        public IActionResult OnGet()
        {
            try
            {
                LoadAccountFromSession();

                if (LoginedAccount != null)
                {
                    var role = (string)LoginedAccount.Role;
                    if (role == AccountRoleEnum.Admin.ToString()) return RedirectToPage("/Admin/Index");
                    if (role == AccountRoleEnum.Staff.ToString()) return RedirectToPage("/Staff/Index");
                    return RedirectToPage("/Index");
                }

                InitialData();

                return Page();
            }
            catch (Exception)
            {
                return RedirectToPage("/Error");
            }
        }

        public IActionResult OnPostLogin([Bind("UserName, Password")] AccountLoginDto AccountLogin)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TabIndex = 1;

                    // Return to page with errors
                    return Page();
                }

                // Get account from database
                var result = _service.AccountService.GetAccount(AccountLogin.UserName, AccountLogin.Password);
                try
                {
                    if (result != null)
                    {
                        // Serialize the result to a JSON string and put into session
                        HttpContext.Session.SetString("Account", JsonSerializer.Serialize(result));

                        var role = result.Role;

                        switch (role)
                        {
                            case "Admin":
                                return RedirectToPage("/Admin/Index");
                            case "Staff":
                                return RedirectToPage("/Staff/Index");
                            default:
                                return RedirectToPage("/Index");
                        }

                    }

                    Message.Add("Tên đăng nhập hoặc mật khẩu không đúng!");
                }
                catch (Exception ex)
                {
                    Message.Add(ex.Message);
                }

                TabIndex = 1;
                return Page();
            }
            catch (Exception)
            {
                return RedirectToPage("/Error");
            }
        }

        // ON POST REGISTER
        public IActionResult OnPostRegister([Bind("Username, Password, ConfirmPassword, Fullname, UserPhone, Email")] AccountRegisterDto AccountRegister)
        {
            try
            {
                // Check if model state is valid
                if (!ModelState.IsValid)
                {
                    TabIndex = 2;
                    return Page();
                }

                try
                {
                    List<bool> checkingCondition = new List<bool>
                    {
                        _service.AccountService.CheckUsernameExisted(AccountRegister.Username),
                        AccountRegister.ConfirmPassword != AccountRegister.Password,
                        _service.AccountService.CheckPhoneExisted(AccountRegister.UserPhone),
                        _service.AccountService.CheckEmailExisted(AccountRegister.Email)
                    };

                    // If any checking conditions fail, add appropriate error messages and return page
                    if (checkingCondition.Contains(true))
                    {
                        if (checkingCondition[0])
                        {
                            Message.Add("Tên đăng nhập đã tồn tại!");

                        }
                        if (checkingCondition[1])
                        {
                            Message.Add("Mật khẩu không khớp!");
                        }
                        if (checkingCondition[2])
                        {
                            Message.Add("Số điện thoại đã tồn tại!");
                        }
                        if (AccountRegister.Email != null && checkingCondition[3])
                        {
                            Message.Add("Email đã tồn tại!");
                        }
                        TabIndex = 2;
                        return Page();
                    }

                    // Add new account to database
                    AccountRegister.Role = AccountRoleEnum.Customer.ToString();
                    _service.AccountService.RegisterAccount(AccountRegister.ToAccount());
                    TempData["SuccessMessage"] = "Đăng ký tài khoản thành công!";

                    TabIndex = 1;
                    return RedirectToPage("/Authentication");
                }
                catch (Exception ex)
                {
                    Message.Add(ex.Message); // Set the exception message to Message
                    TabIndex = 2;
                    return Page();
                }
            }
            catch (Exception)
            {
                return RedirectToPage("/Error");
            }
        }

        public IActionResult OnPostOwnerRegister([Bind("Username, Password, ConfirmPassword,Fullname, UserPhone, Email")] AccountRegisterDto AccountRegister)
        {
            try
            {
                // Check if model state is valid
                if (!ModelState.IsValid)
                {
                    TabIndex = 3;
                    return Page();
                }

                try
                {
                    List<bool> checkingCondition = new List<bool>
                    {
                        _service.AccountService.CheckUsernameExisted(AccountRegister.Username),
                        AccountRegister.ConfirmPassword != AccountRegister.Password,
                        _service.AccountService.CheckPhoneExisted(AccountRegister.UserPhone),
                        _service.AccountService.CheckEmailExisted(AccountRegister.Email)
                    };

                    // If any checking conditions fail, add appropriate error messages and return page
                    if (checkingCondition.Contains(true))
                    {
                        if (checkingCondition[0])
                        {
                            Message.Add("Tên đăng nhập đã tồn tại!");

                        }
                        if (checkingCondition[1])
                        {
                            Message.Add("Mật khẩu không khớp!");
                        }
                        if (checkingCondition[2])
                        {
                            Message.Add("Số điện thoại đã tồn tại!");
                        }
                        if (AccountRegister.Email != null && checkingCondition[3])
                        {
                            Message.Add("Email đã tồn tại!");
                        }

                        TabIndex = 3;
                        return Page();
                    }

                    // Add new account to database
                    AccountRegister.Role = AccountRoleEnum.Staff.ToString();
                    _service.AccountService.RegisterAccount(AccountRegister.ToAccount());
                    TempData["SuccessMessage"] = "Đăng ký tài khoản dành cho chủ sân thành công!";

                    TabIndex = 1;
                    return RedirectToPage("/Authentication");
                }
                catch (Exception ex)
                {
                    Message.Add(ex.Message); // Set the exception message to Message
                    TabIndex = 3;
                    return Page();
                }
            }
            catch (Exception)
            {
                return RedirectToPage("/Error");
            }
        }

        private void InitialData()
        {
            if (TempData["SuccessMessage"] is string msg)
            {
                SuccessMessage = msg;
            }
            else
            {
                SuccessMessage = string.Empty;
            }
            Message.Clear();
        }
    }
}
