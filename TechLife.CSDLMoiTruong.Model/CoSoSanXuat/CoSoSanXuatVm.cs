using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Model.LoaiCayTrong;

namespace TechLife.CSDLMoiTruong.Model.CoSoSanXuat
{
    public class CoSoSanXuatVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }
        public string ChuCoSo { get; set; }
        public string MaSoThue { get; set; }
        public string GhiChu { get; set; }
        public bool IsStatus { get; set; }
        public int Order { get; set; }
    }
}