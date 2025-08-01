using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Common.Result;

namespace TechLife.CSDLMoiTruong.Model.SoLieuSinhTruong
{
    public class SoLieuSinhTruongCreateRequest : RequestBase
    {
        [Required(ErrorMessage = "Vui lòng chọn cây trồng")]
        [Display(Name = "Cây trồng")]
        public int CayTrongId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày bắt đầu")]
        [Display(Name = "Từ ngày")]
        public DateTime TuNgay { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày kết thúc")]
        [Display(Name = "Đến ngày")]
        public DateTime DenNgay { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập kế hoạch")]
        [Display(Name = "Kế hoạch")]
        public double KeHoach { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng đã gieo trồng")]
        [Display(Name = "Đã gieo trồng")]
        public double DaGieoTrong { get; set; }

        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }
    }
}