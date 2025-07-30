using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Common.Result;

namespace TechLife.CSDLMoiTruong.Model.SinhVatGayHai
{
    public class SinhVatGayHaiCreateRequest : RequestBase
    {
        [Required(ErrorMessage = "Vui lòng nhập tên sinh vật gây hại")]
        [Display(Name = "Tên sinh vật gây hại")]
        public string Name { get; set; }

        [Display(Name = "Mã sinh vật gây hại")]
        public string Code { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại cây trồng")]
        [Display(Name = "Loại cây trồng")]
        public int LoaiCayTrongId { get; set; }
    }
}
