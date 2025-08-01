using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLife.CSDLMoiTruong.Data.Entities
{
    public class DiaBanAnhHuong : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public int? ParentId { get; set; }
        public virtual DiaBanAnhHuong Parent { get; set; }
        public virtual ICollection<DiaBanAnhHuong> Children { get; set; }

      
    }
}