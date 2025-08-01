using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Common.Result;

namespace TechLife.CSDLMoiTruong.Model.ThoiTiet
{
    public class ThoiTietCreateRequest : RequestBase
    {
        [Required(ErrorMessage = "Vui lòng nhập ngày bắt đầu")]
        [Display(Name = "Từ ngày")]
        public DateTime TuNgay { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày kết thúc")]
        [Display(Name = "Đến ngày")]
        public DateTime DenNgay { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nhiệt độ cao nhất")]
        [Display(Name = "Nhiệt độ cao nhất")]
        public double NhietDoCaoNhat { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nhiệt độ thấp nhất")]
        [Display(Name = "Nhiệt độ thấp nhất")]
        public double NhietDoThapNhat { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập độ ẩm trung bình")]
        [Display(Name = "Độ ẩm trung bình")]
        public double DoAmTB { get; set; }

        [Display(Name = "Ngày mưa")]
        public string NgayMua { get; set; }
    }
}