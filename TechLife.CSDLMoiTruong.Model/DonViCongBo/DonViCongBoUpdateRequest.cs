using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Common.Result;

namespace TechLife.CSDLMoiTruong.Model.DonViCongBo
{
    public class DonViCongBoUpdateRequest : UpdateRequestBase
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đơn vị")]
        [Display(Name = "Tên đơn vị")]
        public string Name { get; set; }

        [Display(Name = "Mã đơn vị")]
        public string Code { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string SoDienThoai { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }
    }
}