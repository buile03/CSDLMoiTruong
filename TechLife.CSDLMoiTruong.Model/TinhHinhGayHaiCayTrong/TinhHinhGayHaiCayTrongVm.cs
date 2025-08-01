using System;
using TechLife.CSDLMoiTruong.Common.Enums;

namespace TechLife.CSDLMoiTruong.Model.TinhHinhGayHaiCayTrong
{
    public class TinhHinhGayHaiCayTrongVm
    {
        public int Id { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public int SinhVatGayHaiId { get; set; }
        public string SinhVatGayHaiName { get; set; }
        public int DiaBanId { get; set; }
        public string DiaBanName { get; set; }
        public MucDoNhiem MucDoNhiem { get; set; }
        public double DienTichNhiem { get; set; }
    }
}