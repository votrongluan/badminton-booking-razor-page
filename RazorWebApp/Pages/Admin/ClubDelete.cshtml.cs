﻿using BusinessObjects.Dtos.Club;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebApp.Mappers;
using Services.IService;

namespace RazorWebApp.Pages.Admin
{
    public class ClubDeleteModel : PageModel
    {
        private readonly IServiceManager _service;

        public Club DeleteClub { get; set; }
        public ResponseClubDto ClubDto { get; set; }

        public ClubDeleteModel(IServiceManager service)
        {
            _service = service;
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                DeleteClub = _service.ClubService.GetClubById(id ?? -1);
            }
            else
            {
                return RedirectToPage("AllClubManage", new { msg = "Không tìm thấy câu lạc bộ với id cần xóa" });
            }

            if (DeleteClub == null)
            {
                return RedirectToPage("AllClubManage", new { msg = "Không tìm thấy câu lạc bộ với id cần xóa" });
            }

            ClubDto = DeleteClub.ToResponseClubDto();

            return Page();
        }

        public IActionResult OnPost(int clubId)
        {
            try
            {
                _service.ClubService.DeleteClub(clubId);
                return RedirectToPage("AllClubManage", new { msg = "Xóa câu lạc bộ thành công" });
            }
            catch (Exception ex) 
            {
                return RedirectToPage("AllClubManage", new { msg = $"Xóa câu lạc bộ thất bại do lỗi hệ thống: {ex.Message}" });
            }
        }
    }
}
