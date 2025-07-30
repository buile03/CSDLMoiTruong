using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Common.Result;

namespace TechLife.CSDLMoiTruong.Model.LoaiCayTrong
{
    public class LoaiCayTrongCreateRequest : RequestBase
    {
        [Required(ErrorMessage = "Vui lòng nhập tên loại cây trồng")]
        [Display(Name = "Tên loại cây trồng")]
        public string Name { get; set; }
        [Display(Name = "Mã loại cây trồng")]
        public string Code { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
    }
}
