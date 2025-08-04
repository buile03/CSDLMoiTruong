using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Common.Result;

namespace TechLife.CSDLMoiTruong.Model.CoSoSanXuat
{
    public class CoSoSanXuatUpdateRequest : UpdateRequestBase
    {
        [Required(ErrorMessage = "Vui lòng nhập tên cơ sở")]
        [Display(Name = "Tên cơ sở")]
        public string Name { get; set; }
        [Display(Name = "Mã cơ sở")]
        public string Code { get; set; }
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Display(Name = "Điện thoại")]
        public string DienThoai { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Chủ cơ sở")]
        public string ChuCoSo { get; set; }



        [Display(Name = "Mã số thuế")]
        public string MaSoThue { get; set; }



        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }
    }
}
