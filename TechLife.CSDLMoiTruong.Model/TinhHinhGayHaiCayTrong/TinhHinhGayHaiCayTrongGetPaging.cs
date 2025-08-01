using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLife.CSDLMoiTruong.Common.Result;

namespace TechLife.CSDLMoiTruong.Model.TinhHinhGayHaiCayTrong
{
    public class TinhHinhGayHaiCayTrongGetPaging : GetPagingRequest
    {
        public int? SinhVatGayHaiId { get; set; }
        public int? DiaBanAnhHuongId { get; set; }
        public int? MucDoNhiem { get; set; }
    }
}
