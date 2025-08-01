using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechLife.CSDLMoiTruong.Common.Enums;

namespace TechLife.CSDLMoiTruong.Data.Entities
{
    public class TinhHinhGayHaiCayTrong : BaseEntity
    {
        public int Id { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public int SinhVatGayHaiId { get; set; }
        public virtual SinhVatGayHai SinhVatGayHai { get; set; }
        public int DiaBanId { get; set; }
        public virtual DiaBanAnhHuong DiaBan { get; set; }
        public MucDoNhiem MucDoNhiem { get; set; }
        public double DienTichNhiem { get; set; }
    }
}