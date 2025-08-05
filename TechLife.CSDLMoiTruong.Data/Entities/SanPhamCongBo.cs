using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLife.CSDLMoiTruong.Data.Entities
{
    public class SanPhamCongBo : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public int? DonViCongBoId { get; set; }
        public virtual DonViCongBo DonViCongBo { get; set; }

        public string SoCongBo { get; set; }
        public DateTime NgayCongBo { get; set; }
        
    }
}