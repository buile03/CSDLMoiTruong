﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLife.CSDLMoiTruong.Data.Entities
{
    public class SoLieuSinhTruong : BaseEntity
    {
        public int Id { get; set; }
       
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public double KeHoach { get; set; }
        public double DaGieoTrong { get; set; }
        public string MoTa { get; set; }
        public virtual LoaiCayTrong LoaiCayTrong { get; set; }
        public int CayTrongId { get; set; }
    }
}