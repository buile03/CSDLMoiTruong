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

        public DbSet<LoaiCayTrong> LoaiCayTrong { get; set; }
        public DbSet<SinhVatGayHai> SinhVatGayHai { get; set; }
        public DbSet<DiaBanAnhHuong> DiaBanAnhHuong { get; set; }
        public DbSet<ThoiTiet> ThoiTiet { get; set; }
        public DbSet<SoLieuSinhTruong> SoLieuSinhTruong { get; set; }
        public DbSet<TinhHinhGayHaiCayTrong> TinhHinhGayHaiCayTrong { get; set; }
        public DbSet<DonViCongBo> DonViCongBo { get; set; }
        public DbSet<SanPhamCongBo> SanPhamCongBo { get; set; }


    }
}