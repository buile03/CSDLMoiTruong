using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLife.CSDLMoiTruong.Model.SanPhamCongBo
{
    public class ExportExcelRequest : SanPhamCongBoGetPagingRequest
    {
        public Guid UserId { get; set; }
        public List<int> Ids { get; set; }
    }
}
