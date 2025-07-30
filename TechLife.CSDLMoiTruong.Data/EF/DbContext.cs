using Microsoft.EntityFrameworkCore;

using TechLife.CSDLMoiTruong.Data.Entities;
using TechLife.CSDLMoiTruong.Data.Extensions;

namespace TechLife.CSDLMoiTruong.Data.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<LoaiCayTrong> LoaiCayTrongs { get; set; }
    }
}