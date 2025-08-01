using Microsoft.AspNetCore.Mvc;
using TechLife.CSDLMoiTruong.App.ApiClients;
using TechLife.CSDLMoiTruong.App.Extensions;
using TechLife.CSDLMoiTruong.Common.Result;
using TechLife.CSDLMoiTruong.Common;
using TechLife.CSDLMoiTruong.Service;
using TechLife.CSDLMoiTruong.Model.DiaBanAnhHuong;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechLife.CSDLMoiTruong.App.Controllers
{
    public class DiaBanAnhHuongController : BaseController
    {
        private readonly ILogger<DiaBanAnhHuongController> _logger;
        private readonly IDiaBanAnhHuongService _diaBanAnhHuongService;

        public DiaBanAnhHuongController(
            IUserApiClient userApiClient,
            ILogger<DiaBanAnhHuongController> logger,
            IDiaBanAnhHuongService diaBanAnhHuongService)
            : base(userApiClient, logger)
        {
            _logger = logger;
            _diaBanAnhHuongService = diaBanAnhHuongService;
        }

        public async Task<IActionResult> Index(DiaBanAnhHuongGetPagingRequest request)
        {
            var listDiaBan = await _diaBanAnhHuongService.GetAll();
            ViewBag.ParentItems = listDiaBan.Select(v => new SelectListItem()
            {
                Text = v.Name,
                Value = v.Id.ToString(),
            }).ToList();

            return View(request);
        }

        public async Task<IActionResult> List(DiaBanAnhHuongGetPagingRequest request)
        {
            try
            {
                var data = await _diaBanAnhHuongService.GetPagings(request);
                return PartialView(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra");
                return PartialView();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var list = await _diaBanAnhHuongService.GetAll();
                ViewBag.ParentItems = list.Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString(),
                }).ToList();

                return PartialView();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra");
                return PartialView();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiaBanAnhHuongCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return IsValidResult();
                }

                request.UserId = User.GetUserId();
                return await ActionResult(await _diaBanAnhHuongService.Create(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra");
                return ErrorResult();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var data = await _diaBanAnhHuongService.GetById(Convert.ToInt32(SystemHashUtil.DecodeID(id, SystemConstants.AppSettings.Key)));

                var list = await _diaBanAnhHuongService.GetAll();
                ViewBag.ParentItems = list.Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString(),
                    Selected = v.Id == data.ParentId
                }).ToList();

                var model = new DiaBanAnhHuongUpdateRequest()
                {
                    Id = id,
                    Name = data.Name,
                    Code = data.Code,
                    Description = data.Description,
                    ParentId = data.ParentId
                };

                return PartialView(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra");
                return PartialView();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DiaBanAnhHuongUpdateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return IsValidResult();
                }

                request.UserId = User.GetUserId();
                return await ActionResult(await _diaBanAnhHuongService.Update(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra");
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
                return await ActionResult(await _diaBanAnhHuongService.Delete(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra");
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
                return await ActionResult(await _diaBanAnhHuongService.UpdateStatus(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra");
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
                return await ActionResult(await _diaBanAnhHuongService.UpdateOrder(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra");
                return ErrorResult();
            }
        }
    }
}