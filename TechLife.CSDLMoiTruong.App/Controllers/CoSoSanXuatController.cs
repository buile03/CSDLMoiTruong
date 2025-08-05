using Microsoft.AspNetCore.Mvc;
using TechLife.CSDLMoiTruong.App.ApiClients;
using TechLife.CSDLMoiTruong.App.Extensions;
using TechLife.CSDLMoiTruong.Common.Result;
using TechLife.CSDLMoiTruong.Common;
using TechLife.CSDLMoiTruong.Service;
using TechLife.CSDLMoiTruong.Model.CoSoSanXuat;

namespace TechLife.CSDLMoiTruong.App.Controllers
{
    public class CoSoSanXuatController : BaseController
    {
        private readonly ILogger<CoSoSanXuatController> _logger;
        private readonly ICoSoSanXuatService _coSoSanXuatService;

        public CoSoSanXuatController(IUserApiClient userApiClient
            , ILogger<CoSoSanXuatController> logger
            , ICoSoSanXuatService coSoSanXuatService
            , ILoaiCayTrongService loaiCayTrongService) : base(userApiClient, logger)
        {
            _logger = logger;
            _coSoSanXuatService = coSoSanXuatService;
        }

        public IActionResult Index(CoSoSanXuatGetPagingRequest request)
        {
            return View(request);
        }

        public async Task<IActionResult> List(CoSoSanXuatGetPagingRequest request)
        {
            try
            {
                var data = await _coSoSanXuatService.GetPagings(request);

                return PartialView(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra");
                return PartialView();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(CoSoSanXuatCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return IsValidResult();
                }

                request.UserId = User.GetUserId();

                return await ActionResult(await _coSoSanXuatService.Create(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra");
                return ErrorResult();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var data = await _coSoSanXuatService.GetById(Convert.ToInt32(SystemHashUtil.DecodeID(id, SystemConstants.AppSettings.Key)));
                var model = new CoSoSanXuatUpdateRequest()
                {
                    Name = data.Name,
                    Code = data.Code,
                    DiaChi = data.DiaChi,
                    DienThoai = data.DienThoai,
                    Email = data.Email,
                    ChuCoSo = data.ChuCoSo,
                    MaSoThue = data.MaSoThue,
                    GhiChu = data.GhiChu,
                    Id = id
                };
                return PartialView(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra");
                return PartialView();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit(CoSoSanXuatUpdateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return IsValidResult();
                }

                request.UserId = User.GetUserId();

                return await ActionResult(await _coSoSanXuatService.Update(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra");
                return ErrorResult();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return IsValidResult();
                }

                request.UserId = User.GetUserId();

                return await ActionResult(await _coSoSanXuatService.Delete(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra");
                return ErrorResult();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(UpdateStatusRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return IsValidResult();
                }

                request.UserId = User.GetUserId();

                return await ActionResult(await _coSoSanXuatService.UpdateStatus(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra");
                return ErrorResult();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(UpdateOrderRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return IsValidResult();
                }

                request.UserId = User.GetUserId();

                return await ActionResult(await _coSoSanXuatService.UpdateOrder(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra");
                return ErrorResult();
            }
        }
        [HttpGet]
        public async Task<IActionResult> ImportExcel()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> ImportExcel(ImportExcelRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return IsValidResult();

                request.UserId = User.GetUserId();
                return await ActionResult(await _coSoSanXuatService.ImportExcel(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra khi import Excel");
                return ErrorResult();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExportExcel(ExportExcelRequest request)
        {
            try
            {
                request.UserId = User.GetUserId();
                return await _coSoSanXuatService.ExportExcel(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra khi export Excel");
                return ErrorResult();
            }
        }
    }
}