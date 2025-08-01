using Azure.Core;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechLife.CSDLMoiTruong.App.ApiClients;
using TechLife.CSDLMoiTruong.App.Extensions;
using TechLife.CSDLMoiTruong.Common.Enums;
using TechLife.CSDLMoiTruong.Common.Result;

namespace TechLife.CSDLMoiTruong.App.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class BaseController : Controller
    {
        private readonly IUserApiClient userApiClient;
        private readonly ILogger<BaseController> _logger;

        public BaseController(IUserApiClient userApiClient, ILogger<BaseController> logger)
        {
            this.userApiClient = userApiClient;
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var tokenSession = context.HttpContext.Session.GetString("AccessToken");
            if (context.HttpContext.User.Identity.IsAuthenticated && String.IsNullOrWhiteSpace(tokenSession))
            {
                HttpContext.Session.SetString("AccessToken", context.HttpContext.Request.GetUser().AccessToken);
            }
            base.OnActionExecuting(context);
        }

        public async Task<IActionResult> ActionResult(Result result)
        {
            //await _trackingService.Create(User.GetUserId(), result);

            return result.Status switch
            {
                ResultStatus.Ok => new OkObjectResult(result),
                ResultStatus.Error => new BadRequestObjectResult(result),
                ResultStatus.NotFound => new NotFoundObjectResult(result),
                _ => throw new NotImplementedException(),
            };
        }

        public async Task<IActionResult> ActionResult<T>(Result<T> result)
        {
            var rs = new Result(result.IsSuccessed, result.Action, result.Message);

            //await _trackingService.Create(User.GetUserId(), rs);

            return result.Status switch
            {
                ResultStatus.Ok => new OkObjectResult(result),
                ResultStatus.Error => new BadRequestObjectResult(result),
                ResultStatus.NotFound => new NotFoundObjectResult(result),
                _ => throw new NotImplementedException(),
            };
        }
        public async Task Tracking(Result result)
        {
            //await _trackingService.Create(User.GetUserId(), result);
        }
        public IActionResult ErrorResult()
        {
            return new BadRequestObjectResult(new Result(false, "Lỗi!", "Cập nhật không thành công"));
        }

        public IActionResult IsValidResult()
        {
            return new BadRequestObjectResult(new Result(false, "Lỗi!", "Vui lòng nhập đầy đủ thông tin"));
        }

        protected void CellBorder(IXLWorksheet worksheet, int row, int col, bool isWrap = true, bool isCenter = true)
        {
            worksheet.Cell(row, col).Style.Alignment.WrapText = isWrap;
            worksheet.Cell(row, col).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(row, col).Style.Border.TopBorderColor = XLColor.Black;
            worksheet.Cell(row, col).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(row, col).Style.Border.BottomBorderColor = XLColor.Black;
            worksheet.Cell(row, col).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(row, col).Style.Border.LeftBorderColor = XLColor.Black;
            worksheet.Cell(row, col).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(row, col).Style.Border.RightBorderColor = XLColor.Black;
            if (isCenter)
            {
                worksheet.Cell(row, col).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Cell(row, col).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            }
        }

        protected void RangeBorder(IXLWorksheet worksheet, int rowStart, int colStart, int rowEnd, int colEnd, bool isWrap = true, bool isCenter = true)
        {
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Alignment.WrapText = isWrap;
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Border.TopBorderColor = XLColor.Black;
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Border.BottomBorderColor = XLColor.Black;
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Border.LeftBorderColor = XLColor.Black;
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Border.RightBorderColor = XLColor.Black;
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            if (isCenter)
            {
                worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            }
        }

        public List<SelectListItem> GetListMucDoNhiem(int id)
        {
            var listType = new List<SelectListItem>();
            listType.Add(new SelectListItem() { Value = ((int)MucDoNhiem.nhe).ToString(), Text = StringEnum.GetStringValue(MucDoNhiem.nhe), Selected = id == (int)MucDoNhiem.nhe ? true : false });
            listType.Add(new SelectListItem() { Value = ((int)MucDoNhiem.trungbinh).ToString(), Text = StringEnum.GetStringValue(MucDoNhiem.trungbinh), Selected = id == (int)MucDoNhiem.trungbinh ? true : false });
            listType.Add(new SelectListItem() { Value = ((int)MucDoNhiem.nang).ToString(), Text = StringEnum.GetStringValue(MucDoNhiem.nang), Selected = id == (int)MucDoNhiem.nang ? true : false });
            return listType;
        }
    }
}