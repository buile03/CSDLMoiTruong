using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Common.Result;

namespace TechLife.CSDLMoiTruong.Model.SanPhamCongBo
{
    public class SanPhamCongBoUpdateRequest : UpdateRequestBase
    {
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Mã sản phẩm")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn đơn vị")]
        [Display(Name = "Đơn vị công bố")]
        public int? DonViCongBoId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số công bố")]
        [Display(Name = "Số công bố")]
        public string SoCongBo { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày công bố")]
        [Display(Name = "Ngày công bố")]
        public DateTime NgayCongBo { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
    }
}