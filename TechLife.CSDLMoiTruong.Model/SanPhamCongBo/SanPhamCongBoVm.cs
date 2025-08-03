using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Model.DonViCongBo;

namespace TechLife.CSDLMoiTruong.Model.SanPhamCongBo
{
    public class SanPhamCongBoVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public int DonViCongBoId { get; set; }
        public string DonViCongBoName { get; set; }

        public string SoCongBo { get; set; }
        public DateTime NgayCongBo { get; set; }
        

        public bool IsStatus { get; set; }
        public int Order { get; set; }
    }
}