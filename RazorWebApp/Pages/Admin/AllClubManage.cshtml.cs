﻿using BusinessObjects.Dtos.Club;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.IService;
using WebAppRazor.Mappers;

namespace WebAppRazor.Pages.Admin
{
    public class AllClubManageModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _service;
        [BindProperty] public CreateClubDto CreatedClub { get; set; }
        public List<City> Cities { get; set; }
        public List<Club> Clubs { get; set; }
        public List<ResponseClubDto> ClubsDto { get; set; }
        public List<ResponseClubDto> FilterClubsDto { get; set; }

        // Pagination properties
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        // MESSAGE FOR ACTION
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        private void InitializeData()
        {
            Clubs = _service.ClubService.GetAllClubs();
            Cities = _service.CityService.GetAllCities();

            ClubsDto = Clubs.Select(e => e.ToResponseClubDto()).ToList();
            FilterClubsDto = ClubsDto;

            ViewData["CityId"] = new SelectList(Cities, "CityId", "CityName");
        }

        public AllClubManageModel(IServiceManager service)
        {
            _service = service;
        }

        private void Paging(string searchString, string searchProperty, string sortProperty, int sortOrder, int page = 0)
        {
            const int PageSize = 10;  // Set the number of items per page

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                FilterClubsDto = searchProperty switch
                {
                    "ClubId" => ClubsDto.Where(e => e.ClubId.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "ClubName" => ClubsDto.Where(e => e.ClubName.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "Address" => ClubsDto.Where(e => e.Address.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    "ClubPhone" => ClubsDto.Where(e => e.ClubPhone.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
                    _ => FilterClubsDto,
                };
            }

            if (!string.IsNullOrWhiteSpace(sortProperty))
            {
                FilterClubsDto = sortProperty switch
                {
                    "ClubId" => sortOrder == -1 ? FilterClubsDto.OrderByDescending(e => e.ClubId).ToList() : sortOrder == 1 ? FilterClubsDto.OrderBy(e => e.ClubId).ToList() : FilterClubsDto,
                    "ClubName" => sortOrder == -1 ? FilterClubsDto.OrderByDescending(e => e.ClubName).ToList() : sortOrder == 1 ? FilterClubsDto.OrderBy(e => e.ClubName).ToList() : FilterClubsDto,
                    "Address" => sortOrder == -1 ? FilterClubsDto.OrderByDescending(e => e.Address).ToList() : sortOrder == 1 ? FilterClubsDto.OrderBy(e => e.Address).ToList() : FilterClubsDto,
                    "ClubPhone" => sortOrder == -1 ? FilterClubsDto.OrderByDescending(e => e.ClubPhone).ToList() : sortOrder == 1 ? FilterClubsDto.OrderBy(e => e.ClubPhone).ToList() : FilterClubsDto,
                    _ => FilterClubsDto,
                };
            }

            // Pagination logic
            page = page == 0 ? 1 : page;
            CurrentPage = page;
            TotalPages = (int)Math.Ceiling(FilterClubsDto.Count / (double)PageSize);
            FilterClubsDto = FilterClubsDto.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }

        public IActionResult OnGet(string searchString, string searchProperty, string sortProperty, int sortOrder)
        {
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Admin.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            string sM = Request.Query["SuccessMessage"];

            if (!string.IsNullOrEmpty(sM))
            {
                SuccessMessage = sM;
            }
            else
            {
                SuccessMessage = string.Empty;
            }

            string eM = Request.Query["ErrorMessage"];

            if (!string.IsNullOrEmpty(eM))
            {
                ErrorMessage = eM;
            }
            else
            {
                ErrorMessage = string.Empty;
            }

            InitializeData();

            int page = Convert.ToInt32(Request.Query["page"]);
            Paging(searchString, searchProperty, sortProperty, sortOrder, page);

            return Page();
        }

        public IActionResult OnPost()
        {
            SuccessMessage = string.Empty;
            ErrorMessage = string.Empty;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var result = _service.ClubService.CheckPhoneExisted(CreatedClub.ClubPhone);
                if (result == false)
                {
                    _service.ClubService.AddClub(CreatedClub.ToClub());

                    InitializeData();
                    Paging("", "", "", 0);
                    SuccessMessage = "Tạo mới câu lạc bộ thành công";
                }
                else
                {
                    InitializeData();
                    Paging("", "", "", 0);
                    ErrorMessage = "Số điện thoại đã tồn tại trong hệ thống";
                }
            }
            catch (Exception ex)
            {
                InitializeData();
                Paging("", "", "", 0);
                ErrorMessage = "Câu lạc bộ không được tạo do lỗi hệ thống vui lòng liên hệ đội ngũ hỗ trợ";
            }

            return Page();
        }

        public JsonResult OnGetDistrictsByCityId(int cityId)
        {
            InitializeData();
            var districts = _service.DistrictService.GetAllDistrictsByCityId(cityId);
            return new JsonResult(districts);
        }
    }
}