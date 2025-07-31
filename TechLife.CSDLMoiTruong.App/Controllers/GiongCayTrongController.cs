using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechLife.CSDLMoiTruong.App.ApiClients;
using TechLife.CSDLMoiTruong.App.Extensions;
using TechLife.CSDLMoiTruong.Common;
using TechLife.CSDLMoiTruong.Common.Result;
using TechLife.CSDLMoiTruong.Model.GiongCayTrong;
using TechLife.CSDLMoiTruong.Service;

namespace TechLife.CSDLMoiTruong.App.Controllers
{
    public class GiongCayTrongController : BaseController
    {
        private readonly ILogger<GiongCayTrongController> _logger;
        private readonly IGiongCayTrongService _giongCayTrongService;
        private readonly ILoaiCayTrongService _loaiCayTrongService;

        public GiongCayTrongController(
            IUserApiClient userApiClient,
            ILogger<GiongCayTrongController> logger,
            IGiongCayTrongService giongCayTrongService,
            ILoaiCayTrongService loaiCayTrongService)
            : base(userApiClient, logger)
        {
            _logger = logger;
            _giongCayTrongService = giongCayTrongService;
            _loaiCayTrongService = loaiCayTrongService;
        }

        public async Task<IActionResult> Index(GiongCayTrongGetPagingRequest request)
        {
            var listLoaiCayTrong = await _loaiCayTrongService.GetAll();
            ViewBag.LoaiCayTrongItems = listLoaiCayTrong.Select(v => new SelectListItem()
            {
                Text = v.Name,
                Value = v.Id.ToString(),
                Selected = request.LoaiCayTrongId == v.Id
            }).ToList();
            return View(request);
        }

        public async Task<IActionResult> List(GiongCayTrongGetPagingRequest request)
        {
            try
            {
                var data = await _giongCayTrongService.GetPagings(request);
                return PartialView(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải danh sách giống cây trồng");
                return PartialView("Error");
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
        public async Task<IActionResult> Create(GiongCayTrongCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return IsValidResult();

                request.UserId = User.GetUserId();
                return await ActionResult(await _giongCayTrongService.Create(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm giống cây trồng");
                return ErrorResult();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var data = await _giongCayTrongService.GetById(Convert.ToInt32(SystemHashUtil.DecodeID(id, SystemConstants.AppSettings.Key)));
                var listLoaiCayTrong = await _loaiCayTrongService.GetAll();
                ViewBag.LoaiCayTrongItems = listLoaiCayTrong.Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString(),
                    Selected = data.LoaiCayTrongId == v.Id
                }).ToList();
                var model = new GiongCayTrongUpdateRequest
                {
                    Id = id,
                    Name = data.Name,
                    Code = data.Code,
                    Description = data.Description,
                    LoaiCayTrongId = data.LoaiCayTrongId
                };

                return PartialView(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi mở chỉnh sửa giống cây trồng: {id}");
                return PartialView("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GiongCayTrongUpdateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return IsValidResult();

                request.UserId = User.GetUserId();
                return await ActionResult(await _giongCayTrongService.Update(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi cập nhật giống cây trồng: {request.Id}");
                return ErrorResult();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return IsValidResult();

                request.UserId = User.GetUserId();
                return await ActionResult(await _giongCayTrongService.Delete(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi xóa giống cây trồng: {request.Id}");
                return ErrorResult();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(UpdateStatusRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return IsValidResult();

                request.UserId = User.GetUserId();
                return await ActionResult(await _giongCayTrongService.UpdateStatus(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi cập nhật trạng thái: {request.Id}");
                return ErrorResult();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(UpdateOrderRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return IsValidResult();

                request.UserId = User.GetUserId();
                return await ActionResult(await _giongCayTrongService.UpdateOrder(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi cập nhật thứ tự: {request.Id}");
                return ErrorResult();
            }
        }
    }
}