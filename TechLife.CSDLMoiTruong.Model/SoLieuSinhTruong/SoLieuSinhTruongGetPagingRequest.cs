﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Common.Result;

namespace TechLife.CSDLMoiTruong.Model.SoLieuSinhTruong
{
    public class SoLieuSinhTruongGetPagingRequest : GetPagingRequest
    {
        public int? CayTrongId { get; set; }
    }
}