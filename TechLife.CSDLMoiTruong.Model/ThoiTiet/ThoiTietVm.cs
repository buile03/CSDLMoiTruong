using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLife.CSDLMoiTruong.Model.ThoiTiet
{
    public class ThoiTietVm
    {
        public int Id { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public double NhietDoCaoNhat { get; set; }
        public double NhietDoThapNhat { get; set; }
        public double DoAmTB { get; set; }
        public string NgayMua { get; set; }
        public bool IsStatus { get; set; }
        public int Order { get; set; }
    }
}