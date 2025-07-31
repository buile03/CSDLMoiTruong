using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLife.CSDLMoiTruong.Data.Entities
{
    public class GiongCayTrong : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int LoaiCayTrongId { get; set; }
        public virtual LoaiCayTrong LoaiCayTrong { get; set; }

    }
}
