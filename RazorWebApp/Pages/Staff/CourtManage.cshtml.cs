﻿using BusinessObjects.Dtos.Court;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.IService;
using WebAppRazor.Constants;
using WebAppRazor.Mappers;

namespace WebAppRazor.Pages.Staff
{
    public class CourtManageModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _service;
        [BindProperty] public CreateCourtDto CreateCourt { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<Court> Courts { get; set; }
        public List<Club> Clubs { get; set; }
        public List<CourtType> CourtTypes { get; set; }
        public List<ResponseCourtDto> CourtsDto { get; set; }
        public List<ResponseCourtDto> FilterCourtsDto { get; set; }
        private void InitializeData()
        {
            CourtTypes = _service.CourtTypeService.GetAllCourtTypes();
            Clubs = _service.ClubService.GetAllClubs();

            int clubManageId = LoginedAccount?.ClubManageId ?? 0;
            Courts = _service.CourtService.GetCourtsByClubId(clubManageId);

            CourtsDto = Courts.Select(e => e.ToResponseCourtDto()).ToList();
            FilterCourtsDto = CourtsDto;

            ViewData["CourtTypeId"] = new SelectList(CourtTypes, "CourtTypeId", "TypeName");
        }
        public CourtManageModel(IServiceManager service)
        {
            _service = service;
        }
        private void Filter(string searchString, string searchProperty, string sortProperty, int sortOrder)
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                FilterCourtsDto = searchProperty switch
                {
                    "CourtId" => CourtsDto.Where(e => e.CourtId.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "TypeName" => CourtsDto.Where(e => CourtTypes.FirstOrDefault(ct => ct.CourtTypeId == e.CourtTypeId)?.TypeName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false).ToList(),
                    _ => FilterCourtsDto,
                };
            }

            if (!string.IsNullOrWhiteSpace(sortProperty))
            {
                FilterCourtsDto = sortProperty switch
                {
                    "CourtId" => sortOrder == -1 ? FilterCourtsDto.OrderByDescending(e => e.CourtId).ToList() : sortOrder == 1 ? FilterCourtsDto.OrderBy(e => e.CourtId).ToList() : FilterCourtsDto,
                    "TypeName" => sortOrder == -1 ? FilterCourtsDto.OrderByDescending(e => e.CourtTypeId).ThenBy(e => e.CourtId).ToList() :
                         sortOrder == 1 ? FilterCourtsDto.OrderBy(e => e.CourtTypeId).ThenBy(e => e.CourtId).ToList() :
                         FilterCourtsDto.OrderBy(e => e.CourtTypeId == 1 ? 0 : e.CourtTypeId == 2 ? 1 : 2).ThenBy(e => e.CourtId).ToList(),
                    _ => FilterCourtsDto,
                };
            }
        }

        public IActionResult OnGet(string searchString, string searchProperty, string sortProperty, int sortOrder)
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Staff.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            // Set and clear the message
            if (!string.IsNullOrWhiteSpace(Message))
            {
                Message = string.Empty;
            }

            if (TempData.ContainsKey("Message"))
            {
                Message = TempData["Message"].ToString();
            }

            InitializeData();

            Filter(searchString, searchProperty, sortProperty, sortOrder);

            return Page();
        }

        public IActionResult OnPost()
        {
            Message = string.Empty;

            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                var combinedErrorMessage = string.Join("\n", errorMessages);

                TempData["Message"] = $"{MessagePrefix.ERROR} Dữ liệu bạn nhập có lỗi:\n{combinedErrorMessage}";
                return RedirectToPage("CourtManage");
            }

            try
            {
                LoadAccountFromSession();
                var accLog = LoginedAccount.ClubManageId;
                var clubId = _service.ClubService.GetAllClubs();

                for (int i = 0; i < CreateCourt.Quantity; i++)
                {
                    var court = new Court
                    {
                        CourtTypeId = CreateCourt.CourtTypeId,
                        ClubId = CreateCourt.ClubId
                    };

                    _service.CourtService.AddCourt(court);
                }

                TempData["Message"] = $"{MessagePrefix.SUCCESS}{CreateCourt.Quantity} sân được tạo thành công";
                return RedirectToPage("CourtManage");
            }
            catch (Exception)
            {
                TempData["Message"] = $"{MessagePrefix.ERROR}Sân không được tạo do lỗi hệ thống vui lòng liên hệ đội ngũ hỗ trợ";
                return RedirectToPage("AllClubManage");
            }
        }

        public JsonResult OnGetCourtsByClubId(int id)
        {
            InitializeData();
            var courts = _service.CourtService.GetCourtsByClubId(id);
            return new JsonResult(courts);
        }
    }
}
