using Microsoft.AspNetCore.Mvc;
using TechLife.CSDLMoiTruong.App.ApiClients;
using TechLife.CSDLMoiTruong.App.Extensions;
using TechLife.CSDLMoiTruong.Common.Result;
using TechLife.CSDLMoiTruong.Common;
using TechLife.CSDLMoiTruong.Service;
using TechLife.CSDLMoiTruong.Model.SanPhamCongBo;
using Microsoft.AspNetCore.Mvc.Rendering;
using Azure.Core;
using DocumentFormat.OpenXml.Office2016.Excel;

namespace TechLife.CSDLMoiTruong.App.Controllers
{
    public class SanPhamCongBoController : BaseController
    {
        private readonly ILogger<SanPhamCongBoController> _logger;
        private readonly ISanPhamCongBoService _sanPhamCongBoService;
        private readonly IDonViCongBoService _donViCongBoService;

        public SanPhamCongBoController(
            IUserApiClient userApiClient,
            ILogger<SanPhamCongBoController> logger,
            ISanPhamCongBoService sanPhamCongBoService,
            IDonViCongBoService donViCongBoService)
            : base(userApiClient, logger)
        {
            _logger = logger;
            _sanPhamCongBoService = sanPhamCongBoService;
            _donViCongBoService = donViCongBoService;
        }

        public async Task<IActionResult> Index(SanPhamCongBoGetPagingRequest request)
        {
            var listDonViCongBo = await _donViCongBoService.GetAll();
            ViewBag.DonViCongBoItems = listDonViCongBo.Select(v => new SelectListItem()
            {
                Text = v.Name,
                Value = v.Id.ToString(),
                Selected = request.DonViCongBoId == v.Id
            }).ToList();
            return View(request);
        }

        public async Task<IActionResult> List(SanPhamCongBoGetPagingRequest request)
        {
            try
            {
                var data = await _sanPhamCongBoService.GetPagings(request);
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
            var listDonViCongBo = await _donViCongBoService.GetAll();
            ViewBag.DonViCongBoItems = listDonViCongBo.Select(v => new SelectListItem()
            {
                Text = v.Name,
                Value = v.Id.ToString(),
            }).ToList();
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(SanPhamCongBoCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return IsValidResult();

                request.UserId = User.GetUserId();
                return await ActionResult(await _sanPhamCongBoService.Create(request));
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
                var data = await _sanPhamCongBoService.GetById(Convert.ToInt32(SystemHashUtil.DecodeID(id, SystemConstants.AppSettings.Key)));
                var model = new SanPhamCongBoUpdateRequest()
                {
                    Id = id,
                    Name = data.Name,
                    Code = data.Code,
                    DonViCongBoId = data.DonViCongBoId,
                    SoCongBo = data.SoCongBo,
                    NgayCongBo = data.NgayCongBo,
                    Description = data.Description
                };

                var listDonViCongBo = await _donViCongBoService.GetAll();
                ViewBag.DonViCongBoItems = listDonViCongBo.Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString(),
                    Selected = data.DonViCongBoId == v.Id
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
        public async Task<IActionResult> Edit(SanPhamCongBoUpdateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return IsValidResult();

                request.UserId = User.GetUserId();
                return await ActionResult(await _sanPhamCongBoService.Update(request));
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
                return await ActionResult(await _sanPhamCongBoService.Delete(request));
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
                return await ActionResult(await _sanPhamCongBoService.UpdateStatus(request));
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
                return await ActionResult(await _sanPhamCongBoService.UpdateOrder(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra");
                return ErrorResult();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ImportExcel(ImportExcelRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return IsValidResult();

                request.UserId = User.GetUserId();
                return await ActionResult(await _sanPhamCongBoService.ImportExcel(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra khi import Excel");
                return ErrorResult();
            }
        }
        [HttpGet]
        public async Task<IActionResult> ImportExcel()
        {
            var listDonViCongBo = await _donViCongBoService.GetAll();
            ViewBag.DonViCongBoItems = listDonViCongBo.Select(v => new SelectListItem()
            {
                Text = v.Name,
                Value = v.Id.ToString(),
            }).ToList();
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> ExportExcel(ExportExcelRequest request)
        {
            try
            {
                request.UserId = User.GetUserId();
                return await _sanPhamCongBoService.ExportExcel(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra khi export Excel");
                return ErrorResult();
            }
        }
    }
}