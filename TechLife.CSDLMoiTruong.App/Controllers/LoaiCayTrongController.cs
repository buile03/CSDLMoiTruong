using Microsoft.AspNetCore.Mvc;
using TechLife.CSDLMoiTruong.App.ApiClients;
using TechLife.CSDLMoiTruong.App.Extensions;
using TechLife.CSDLMoiTruong.Common.Result;
using TechLife.CSDLMoiTruong.Common;
using TechLife.CSDLMoiTruong.Service;
using TechLife.CSDLMoiTruong.Model.LoaiCayTrong;

namespace TechLife.CSDLMoiTruong.App.Controllers
{
    public class LoaiCayTrongController : BaseController
    {
        private readonly ILogger<LoaiCayTrongController> _logger;
        private readonly ILoaiCayTrongService _loaiCayTrongService;
        public LoaiCayTrongController(IUserApiClient userApiClient
            , ILogger<LoaiCayTrongController> logger
            , ILoaiCayTrongService loaiCayTrongService) : base(userApiClient, logger)
        {
            _logger = logger;
            _loaiCayTrongService = loaiCayTrongService;
        }

        public IActionResult Index(LoaiCayTrongGetPagingRequest request)
        {
            return View(request);
        }

        public async Task<IActionResult> List(LoaiCayTrongGetPagingRequest request)
        {
            try
            {
                var data = await _loaiCayTrongService.GetPagings(request);

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
        public async Task<IActionResult> Create(LoaiCayTrongCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return IsValidResult();
                }

                var userId = User.GetUserId();
                request.UserId = userId != Guid.Empty ? userId : Guid.Parse("11111111-1111-1111-1111-111111111111");


                return await ActionResult(await _loaiCayTrongService.Create(request));
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
                var data = await _loaiCayTrongService.GetById(Convert.ToInt32(SystemHashUtil.DecodeID(id, SystemConstants.AppSettings.Key)));
                var model = new LoaiCayTrongUpdateRequest()
                {
                    Name = data.Name,
                    Code = data.Code,
                    Description = data.Description,
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
        public async Task<IActionResult> Edit(LoaiCayTrongUpdateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return IsValidResult();
                }

                request.UserId = User.GetUserId();

                return await ActionResult(await _loaiCayTrongService.Update(request));
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

                return await ActionResult(await _loaiCayTrongService.Delete(request));
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

                return await ActionResult(await _loaiCayTrongService.UpdateStatus(request));
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

                return await ActionResult(await _loaiCayTrongService.UpdateOrder(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra");

                return ErrorResult();
            }
        }
    }
}
