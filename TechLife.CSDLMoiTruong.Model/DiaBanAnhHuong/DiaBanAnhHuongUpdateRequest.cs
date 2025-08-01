using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Common.Result;

namespace TechLife.CSDLMoiTruong.Model.DiaBanAnhHuong
{
    public class DiaBanAnhHuongUpdateRequest : UpdateRequestBase
    {
        [Required(ErrorMessage = "Vui lòng nhập tên địa bàn")]
        [Display(Name = "Tên địa bàn")]
        public string Name { get; set; }

        [Display(Name = "Mã địa bàn")]
        public string Code { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Địa bàn cha")]
        public int? ParentId { get; set; }
    }
}