﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Common.Result;

namespace TechLife.CSDLMoiTruong.Model.DiaBanAnhHuong
{
    public class DiaBanAnhHuongGetPagingRequest : GetPagingRequest
    {
        public int? ParentId { get; set; }
    }
}