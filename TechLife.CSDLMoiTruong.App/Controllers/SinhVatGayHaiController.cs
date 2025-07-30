using Microsoft.AspNetCore.Mvc;
using TechLife.CSDLMoiTruong.App.ApiClients;
using TechLife.CSDLMoiTruong.App.Extensions;
using TechLife.CSDLMoiTruong.Common.Result;
using TechLife.CSDLMoiTruong.Common;
using TechLife.CSDLMoiTruong.Service;
using TechLife.CSDLMoiTruong.Model.SinhVatGayHai;
using Microsoft.AspNetCore.Mvc.Rendering;
using Azure.Core;

namespace TechLife.CSDLMoiTruong.App.Controllers
{
    public class SinhVatGayHaiController : BaseController
    {
        private readonly ILogger<SinhVatGayHaiController> _logger;
        private readonly ISinhVatGayHaiService _sinhVatGayHaiService;
        private readonly ILoaiCayTrongService _loaiCayTrongService;

        public SinhVatGayHaiController(IUserApiClient userApiClient
            , ILogger<SinhVatGayHaiController> logger
            , ISinhVatGayHaiService sinhVatGayHaiService
            , ILoaiCayTrongService loaiCayTrongService) : base(userApiClient, logger)
        {
            _logger = logger;
            _sinhVatGayHaiService = sinhVatGayHaiService;
            _loaiCayTrongService = loaiCayTrongService;
        }

        public async Task<IActionResult> Index(SinhVatGayHaiGetPagingRequest request)
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

        public async Task<IActionResult> List(SinhVatGayHaiGetPagingRequest request)
        {
            try
            {
                var data = await _sinhVatGayHaiService.GetPagings(request);
                
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
        public async Task<IActionResult> Create(SinhVatGayHaiCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return IsValidResult();
                }

                var userId = User.GetUserId();
                request.UserId = userId != Guid.Empty ? userId : Guid.Parse("11111111-1111-1111-1111-111111111111");

                return await ActionResult(await _sinhVatGayHaiService.Create(request));
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
                var data = await _sinhVatGayHaiService.GetById(Convert.ToInt32(SystemHashUtil.DecodeID(id, SystemConstants.AppSettings.Key)));
                var model = new SinhVatGayHaiUpdateRequest()
                {
                    Name = data.Name,
                    Code = data.Code,
                    Description = data.Description,
                    LoaiCayTrongId = data.LoaiCayTrongId,
                    Id = id
                };
                var listLoaiCayTrong = await _loaiCayTrongService.GetAll();
                ViewBag.LoaiCayTrongItems = listLoaiCayTrong.Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString(),
                    Selected = model.LoaiCayTrongId == v.Id
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
        public async Task<IActionResult> Edit(SinhVatGayHaiUpdateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return IsValidResult();
                }

                request.UserId = User.GetUserId();
                return await ActionResult(await _sinhVatGayHaiService.Update(request));
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
                return await ActionResult(await _sinhVatGayHaiService.Delete(request));
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
                return await ActionResult(await _sinhVatGayHaiService.UpdateStatus(request));
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
                return await ActionResult(await _sinhVatGayHaiService.UpdateOrder(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra");
                return ErrorResult();
            }
        }
    }
}