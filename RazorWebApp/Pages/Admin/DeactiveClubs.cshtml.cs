﻿using BusinessObjects.Dtos.Club;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.IService;
using WebAppRazor.Constants;
using WebAppRazor.Mappers;

namespace WebAppRazor.Pages.Admin
{
    public class DeactiveClubsModel : AuthorPageServiceModel
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
        public string Message { get; set; }

        private void InitializeData()
        {
            Clubs = _service.ClubService.GetAllDeActiveClubs();
            Cities = _service.CityService.GetAllCities();

            ClubsDto = Clubs.Select(e => e.ToResponseClubDto()).ToList();
            FilterClubsDto = ClubsDto;

            ViewData["CityId"] = new SelectList(Cities, "CityId", "CityName");
        }

        public DeactiveClubsModel(IServiceManager service)
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
            // Authorize
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Admin.ToString());

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

            int page = Convert.ToInt32(Request.Query["page"]);
            Paging(searchString, searchProperty, sortProperty, sortOrder, page);

            return Page();
        }

        public IActionResult OnGetActive(int id)
        {
            // Authorize
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Admin.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage)) return RedirectToPage(navigatePage);

            try
            {
                var existingClub = _service.ClubService.GetDeActiveClubById(id);

                if (existingClub == null)
                {
                    TempData["Message"] = $"{MessagePrefix.ERROR}Không tìm thấy câu lạc bộ với mã {id}";
                    return RedirectToPage("DeactiveClubs");
                }

                existingClub.Status = true;

                _service.ClubService.UpdateClub(existingClub);

                TempData["Message"] = $"{MessagePrefix.SUCCESS}Câu lạc bộ với mã {id} được kích hoạt thành công";
                return RedirectToPage("DeactiveClubs");
            }
            catch (Exception)
            {
                TempData["Message"] =
                    $"{MessagePrefix.ERROR}Lỗi từ phía server vui lòng liên hệ đội ngũ phát triển để được hỗ trợ";
                return RedirectToPage("DeactiveClubs");
            }
        }
    }
}
