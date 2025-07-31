using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Common.Result;

namespace TechLife.CSDLMoiTruong.Model.GiongCayTrong
{
    public class GiongCayTrongUpdateRequest : UpdateRequestBase
    {
        [Required(ErrorMessage = "Vui lòng nhập tên giống cây trồng")]
        [Display(Name = "Tên giống cây trồng")]
        public string Name { get; set; }

        [Display(Name = "Mã giống cây trồng")]
        public string Code { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn giống cây trồng")]
        [Display(Name = "Loại cây trồng")]
        public int LoaiCayTrongId { get; set; }
    }
}
