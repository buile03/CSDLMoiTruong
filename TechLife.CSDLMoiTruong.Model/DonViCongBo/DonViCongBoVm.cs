using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLife.CSDLMoiTruong.Model.DonViCongBo
{
    public class DonViCongBoVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public bool IsStatus { get; set; }
        public int Order { get; set; }
    }
}