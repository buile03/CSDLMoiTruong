using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLife.CSDLMoiTruong.Model.SoLieuSinhTruong
{
    public class SoLieuSinhTruongVm
    {
        public int Id { get; set; }
        public int CayTrongId { get; set; }
        public string TenCayTrong { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public double KeHoach { get; set; }
        public double DaGieoTrong { get; set; }
        public string MoTa { get; set; }
        public bool IsStatus { get; set; }
        public int Order { get; set; }
    }
}