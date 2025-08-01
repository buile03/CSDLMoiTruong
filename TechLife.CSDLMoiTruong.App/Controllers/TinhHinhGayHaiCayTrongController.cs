using Microsoft.AspNetCore.Mvc;
using TechLife.CSDLMoiTruong.App.ApiClients;
using TechLife.CSDLMoiTruong.App.Extensions;
using TechLife.CSDLMoiTruong.Common.Result;
using TechLife.CSDLMoiTruong.Common;
using TechLife.CSDLMoiTruong.Service;
using TechLife.CSDLMoiTruong.Model.TinhHinhGayHaiCayTrong;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechLife.CSDLMoiTruong.Common.Enums;

namespace TechLife.CSDLMoiTruong.App.Controllers
{
    public class TinhHinhGayHaiCayTrongController : BaseController
    {
        private readonly ILogger<TinhHinhGayHaiCayTrongController> _logger;
        private readonly ITinhHinhGayHaiCayTrongService _tinhHinhGayHaiCayTrongService;
        private readonly ISinhVatGayHaiService _sinhVatGayHaiService;
        private readonly IDiaBanAnhHuongService _diaBanAnhHuongService;

        public TinhHinhGayHaiCayTrongController(
            IUserApiClient userApiClient,
            ILogger<TinhHinhGayHaiCayTrongController> logger,
            ITinhHinhGayHaiCayTrongService tinhHinhGayHaiCayTrongService,
            ISinhVatGayHaiService sinhVatGayHaiService,
            IDiaBanAnhHuongService diaBanAnhHuongService)
            : base(userApiClient, logger)
        {
            _logger = logger;
            _tinhHinhGayHaiCayTrongService = tinhHinhGayHaiCayTrongService;
            _sinhVatGayHaiService = sinhVatGayHaiService;
            _diaBanAnhHuongService = diaBanAnhHuongService;
        }

        public async Task<IActionResult> Index(TinhHinhGayHaiCayTrongGetPaging request)
        {
            var listSinhVatGayHai = await _sinhVatGayHaiService.GetAll();
            ViewBag.SinhVatGayHaiItems = listSinhVatGayHai.Select(v => new SelectListItem()
            {
                Text = v.Name,
                Value = v.Id.ToString(),
            }).ToList();

            var listDiaBan = await _diaBanAnhHuongService.GetAll();
            ViewBag.DiaBanItems = listDiaBan.Select(v => new SelectListItem()
            {
                Text = v.Name,
                Value = v.Id.ToString(),
            }).ToList();
            ViewBag.MucDoNhiemItems = GetListMucDoNhiem(!string.IsNullOrEmpty(request.MucDoNhiem.ToString()) ? (int)request.MucDoNhiem : 0);
            return View(request);
        }

        public async Task<IActionResult> List(TinhHinhGayHaiCayTrongGetPaging request)
        {
            try
            {
                var data = await _tinhHinhGayHaiCayTrongService.GetPagings(request);
                return PartialView(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra");
                return PartialView();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create(string MucDoNhiem)
        {
            try
            {
                var listSinhVatGayHai = await _sinhVatGayHaiService.GetAll();
                ViewBag.SinhVatGayHaiItems = listSinhVatGayHai.Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString(),
                }).ToList();

                var listDiaBan = await _diaBanAnhHuongService.GetAll();
                ViewBag.DiaBanItems = listDiaBan.Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString(),
                }).ToList();

                ViewBag.MucDoNhiemItems = GetListMucDoNhiem(Convert.ToInt32(0));

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
        public async Task<IActionResult> Create(TinhHinhGayHaiCayTrongCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return IsValidResult();
                }

                request.UserId = User.GetUserId();
                return await ActionResult(await _tinhHinhGayHaiCayTrongService.Create(request));
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
                var data = await _tinhHinhGayHaiCayTrongService.GetById(Convert.ToInt32(SystemHashUtil.DecodeID(id, SystemConstants.AppSettings.Key)));

                var listSinhVatGayHai = await _sinhVatGayHaiService.GetAll();
                ViewBag.SinhVatGayHaiItems = listSinhVatGayHai.Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString(),
                    Selected = v.Id == data.SinhVatGayHaiId
                }).ToList();

                var listDiaBan = await _diaBanAnhHuongService.GetAll();
                ViewBag.DiaBanItems = listDiaBan.Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString(),
                    Selected = v.Id == data.DiaBanId
                }).ToList();

                ViewBag.MucDoNhiemItems = GetListMucDoNhiem(Convert.ToInt32(data.MucDoNhiem));

                var model = new TinhHinhGayHaiCayTrongUpdateRequest()
                {
                    Id = id,
                    TuNgay = data.TuNgay,
                    DenNgay = data.DenNgay,
                    SinhVatGayHaiId = data.SinhVatGayHaiId,
                    DiaBanId = data.DiaBanId,
                    MucDoNhiem = (int)data.MucDoNhiem,
                    DienTichNhiem = data.DienTichNhiem
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
        public async Task<IActionResult> Edit(TinhHinhGayHaiCayTrongUpdateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return IsValidResult();
                }

                request.UserId = User.GetUserId();
                return await ActionResult(await _tinhHinhGayHaiCayTrongService.Update(request));
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
                return await ActionResult(await _tinhHinhGayHaiCayTrongService.Delete(request));
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

                return await ActionResult(await _tinhHinhGayHaiCayTrongService.UpdateOrder(request));
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

                return await ActionResult(await _tinhHinhGayHaiCayTrongService.UpdateStatus(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra");

                return ErrorResult();
            }
        }
    }
}