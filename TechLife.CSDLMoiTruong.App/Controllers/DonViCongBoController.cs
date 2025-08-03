using Microsoft.AspNetCore.Mvc;
using TechLife.CSDLMoiTruong.App.ApiClients;
using TechLife.CSDLMoiTruong.App.Extensions;
using TechLife.CSDLMoiTruong.Common.Result;
using TechLife.CSDLMoiTruong.Common;
using TechLife.CSDLMoiTruong.Service;
using TechLife.CSDLMoiTruong.Model.DonViCongBo;

namespace TechLife.CSDLMoiTruong.App.Controllers
{
    public class DonViCongBoController : BaseController
    {
        private readonly ILogger<DonViCongBoController> _logger;
        private readonly IDonViCongBoService _donViCongBoService;

        public DonViCongBoController(
            IUserApiClient userApiClient,
            ILogger<DonViCongBoController> logger,
            IDonViCongBoService donViCongBoService)
            : base(userApiClient, logger)
        {
            _logger = logger;
            _donViCongBoService = donViCongBoService;
        }

        public IActionResult Index(DonViCongBoGetPagingRequest request)
        {
            return View(request);
        }

        public async Task<IActionResult> List(DonViCongBoGetPagingRequest request)
        {
            try
            {
                var data = await _donViCongBoService.GetPagings(request);
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
        public async Task<IActionResult> Create(DonViCongBoCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return IsValidResult();

                request.UserId = User.GetUserId();
                return await ActionResult(await _donViCongBoService.Create(request));
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
                var data = await _donViCongBoService.GetById(Convert.ToInt32(SystemHashUtil.DecodeID(id, SystemConstants.AppSettings.Key)));
                var model = new DonViCongBoUpdateRequest()
                {
                    Id = id,
                    Name = data.Name,
                    Code = data.Code,
                    DiaChi = data.DiaChi,
                    SoDienThoai = data.SoDienThoai,
                    Email = data.Email,
                    Description = data.Description
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
        public async Task<IActionResult> Edit(DonViCongBoUpdateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return IsValidResult();

                request.UserId = User.GetUserId();
                return await ActionResult(await _donViCongBoService.Update(request));
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
                    return IsValidResult();

                request.UserId = User.GetUserId();
                return await ActionResult(await _donViCongBoService.Delete(request));
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
                    return IsValidResult();

                request.UserId = User.GetUserId();
                return await ActionResult(await _donViCongBoService.UpdateStatus(request));
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
                    return IsValidResult();

                request.UserId = User.GetUserId();
                return await ActionResult(await _donViCongBoService.UpdateOrder(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra");
                return ErrorResult();
            }
        }
    }
}