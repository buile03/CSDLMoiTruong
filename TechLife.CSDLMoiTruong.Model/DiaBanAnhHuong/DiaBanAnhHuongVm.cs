using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLife.CSDLMoiTruong.Model.DiaBanAnhHuong
{
    public class DiaBanAnhHuongVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public int? ParentId { get; set; }
        public string ParentName { get; set; }

        public bool IsStatus { get; set; }
        public int Order { get; set; }
    }
}