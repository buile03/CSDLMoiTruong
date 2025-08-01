using System;
using System.ComponentModel.DataAnnotations;
using TechLife.CSDLMoiTruong.Common.Enums;
using TechLife.CSDLMoiTruong.Common.Result;

namespace TechLife.CSDLMoiTruong.Model.TinhHinhGayHaiCayTrong
{
    public class TinhHinhGayHaiCayTrongUpdateRequest : UpdateRequestBase
    {
        [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu")]
        [Display(Name = "Từ ngày")]
        public DateTime TuNgay { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày kết thúc")]
        [Display(Name = "Đến ngày")]
        public DateTime DenNgay { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn sinh vật gây hại")]
        [Display(Name = "Sinh vật gây hại")]
        public int SinhVatGayHaiId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn địa bàn")]
        [Display(Name = "Địa bàn")]
        public int DiaBanId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn mức độ nhiễm")]
        [Display(Name = "Mức độ nhiễm")]
        public int? MucDoNhiem { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập diện tích nhiễm")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Diện tích phải lớn hơn 0")]
        [Display(Name = "Diện tích nhiễm (ha)")]
        public double DienTichNhiem { get; set; }
    }
}