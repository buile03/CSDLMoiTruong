using Microsoft.AspNetCore.Mvc;
using TechLife.CSDLMoiTruong.App.ApiClients;
using TechLife.CSDLMoiTruong.App.Extensions;
using TechLife.CSDLMoiTruong.Common.Result;
using TechLife.CSDLMoiTruong.Common;
using TechLife.CSDLMoiTruong.Service;
using TechLife.CSDLMoiTruong.Model.SoLieuSinhTruong;
using Microsoft.AspNetCore.Mvc.Rendering;
using Azure.Core;

namespace TechLife.CSDLMoiTruong.App.Controllers
{
    public class SoLieuSinhTruongController : BaseController
    {
        private readonly ILogger<SoLieuSinhTruongController> _logger;
        private readonly ISoLieuSinhTruongService _soLieuSinhTruongService;
        private readonly ILoaiCayTrongService _loaiCayTrongService;

        public SoLieuSinhTruongController(
            IUserApiClient userApiClient,
            ILogger<SoLieuSinhTruongController> logger,
            ISoLieuSinhTruongService soLieuSinhTruongService,
            ILoaiCayTrongService loaiCayTrongService)
            : base(userApiClient, logger)
        {
            _logger = logger;
            _soLieuSinhTruongService = soLieuSinhTruongService;
            _loaiCayTrongService = loaiCayTrongService;
        }

        public async Task<IActionResult> Index(SoLieuSinhTruongGetPagingRequest request)
        {
            var listLoaiCayTrong = await _loaiCayTrongService.GetAll();
            ViewBag.LoaiCayTrongItems = listLoaiCayTrong.Select(v => new SelectListItem()
            {
                Text = v.Name,
                Value = v.Id.ToString(),
                Selected = request.CayTrongId == v.Id
            }).ToList();
            return View(request);
        }

        public async Task<IActionResult> List(SoLieuSinhTruongGetPagingRequest request)
        {
            try
            {
                var data = await _soLieuSinhTruongService.GetPagings(request);
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
            var listLoaiCayTrong = await _loaiCayTrongService.GetAll();
            ViewBag.LoaiCayTrongItems = listLoaiCayTrong.Select(v => new SelectListItem()
            {
                Text = v.Name,
                Value = v.Id.ToString(),
            }).ToList();
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(SoLieuSinhTruongCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return IsValidResult();
                }

                request.UserId = User.GetUserId();

                return await ActionResult(await _soLieuSinhTruongService.Create(request));
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
                
                var data = await _soLieuSinhTruongService.GetById(Convert.ToInt32(SystemHashUtil.DecodeID(id, SystemConstants.AppSettings.Key)));
                var model = new SoLieuSinhTruongUpdateRequest()
                {
                    CayTrongId = data.CayTrongId,
                    TuNgay = data.TuNgay,
                    DenNgay = data.DenNgay,
                    KeHoach = data.KeHoach,
                    DaGieoTrong = data.DaGieoTrong,
                    MoTa = data.MoTa,
                    Id = id
                };
                var listLoaiCayTrong = await _loaiCayTrongService.GetAll();
                ViewBag.LoaiCayTrongItems = listLoaiCayTrong.Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString(),
                    Selected = data.CayTrongId == v.Id
                }).ToList();
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
        public async Task<IActionResult> Edit(SoLieuSinhTruongUpdateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return IsValidResult();
                }

                request.UserId = User.GetUserId();
                return await ActionResult(await _soLieuSinhTruongService.Update(request));
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
                return await ActionResult(await _soLieuSinhTruongService.Delete(request));
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
                return await ActionResult(await _soLieuSinhTruongService.UpdateStatus(request));
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
                return await ActionResult(await _soLieuSinhTruongService.UpdateOrder(request));
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
                return await ActionResult(await _soLieuSinhTruongService.ImportExcel(request));
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
                return await _soLieuSinhTruongService.ExportExcel(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra khi export Excel");
                return ErrorResult();
            }
        }
    }
}