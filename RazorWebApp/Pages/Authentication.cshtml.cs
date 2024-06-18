﻿using BusinessObjects.Dtos.Account;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RazorWebApp.Mappers;
using Services.IService;
using System.Diagnostics;
using System.Text.Json;

namespace RazorWebApp.Pages
{
    public class AuthenticationModel : PageModel
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

        public void OnGet()
        {
            InitialData();
        }

        public IActionResult OnPostLogin([Bind("UserName, Password")] AccountLoginDto AccountLogin)
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

        // ON POST REGISTER
        public IActionResult OnPostRegister([Bind("Username, Password, ConfirmPassword, UserPhone, Email")] AccountRegisterDto AccountRegister)
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
                SuccessMessage = "Đăng ký tài khoản thành công!";
            }
            catch (Exception ex)
            {
                Message.Add(ex.Message); // Set the exception message to Message
                TabIndex = 2;
                return Page();
            }

            TabIndex = 2;
            return Page();
        }

        private void InitialData()
        {
            SuccessMessage = string.Empty;
            Message.Clear();
        }
    }
}
