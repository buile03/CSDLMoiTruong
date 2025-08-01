using System;

namespace TechLife.CSDLMoiTruong.Data.Entities
{
    public class BaseEntity
    {
        public int Order { get; set; }
        public int? OrganId { get; set; }
        public bool IsDelete { get; set; }
        public bool IsStatus { get; set; }
        public Guid? CreateByUserId { get; set; }
        public DateTime? CreateOnDate { get; set; }
        public Guid? LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }
    }
}