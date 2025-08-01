using Microsoft.AspNetCore.Mvc;
using TechLife.CSDLMoiTruong.App.ApiClients;
using TechLife.CSDLMoiTruong.App.Extensions;
using TechLife.CSDLMoiTruong.Common.Result;
using TechLife.CSDLMoiTruong.Common;
using TechLife.CSDLMoiTruong.Service;
using TechLife.CSDLMoiTruong.Model.ThoiTiet;

namespace TechLife.CSDLMoiTruong.App.Controllers
{
    public class ThoiTietController : BaseController
    {
        private readonly ILogger<ThoiTietController> _logger;
        private readonly IThoiTietService _thoiTietService;

        public ThoiTietController(IUserApiClient userApiClient
            , ILogger<ThoiTietController> logger
            , IThoiTietService thoiTietService) : base(userApiClient, logger)
        {
            _logger = logger;
            _thoiTietService = thoiTietService;
        }

        public IActionResult Index(ThoiTietGetPagingRequest request)
        {
            return View(request);
        }

        public async Task<IActionResult> List(ThoiTietGetPagingRequest request)
        {
            try
            {
                var data = await _thoiTietService.GetPagings(request);
                return PartialView(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra");
                return PartialView();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(ThoiTietCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return IsValidResult();
                }

                request.UserId = User.GetUserId();
                return await ActionResult(await _thoiTietService.Create(request));

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
                var data = await _thoiTietService.GetById(Convert.ToInt32(SystemHashUtil.DecodeID(id, SystemConstants.AppSettings.Key)));
                var model = new ThoiTietUpdateRequest()
                {
                    TuNgay = data.TuNgay,
                    DenNgay = data.DenNgay,
                    NhietDoCaoNhat = data.NhietDoCaoNhat,
                    NhietDoThapNhat = data.NhietDoThapNhat,
                    DoAmTB = data.DoAmTB,
                    NgayMua = data.NgayMua,
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
        public async Task<IActionResult> Edit(ThoiTietUpdateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return IsValidResult();
                }

                request.UserId = User.GetUserId();
                return await ActionResult(await _thoiTietService.Update(request));
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
                return await ActionResult(await _thoiTietService.Delete(request));
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
                return await ActionResult(await _thoiTietService.UpdateStatus(request));
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
                return await ActionResult(await _thoiTietService.UpdateOrder(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra");
                return ErrorResult();
            }
        }
    }
}