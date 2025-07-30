using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Common.Result;

namespace TechLife.CSDLMoiTruong.Model.SinhVatGayHai
{
    public class SinhVatGayHaiGetPagingRequest : GetPagingRequest
    {
        public int? LoaiCayTrongId { get; set; }
    }
}
