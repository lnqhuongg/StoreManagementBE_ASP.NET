using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoreManagementBE.BackendServer.Enum;
using StoreManagementBE.BackendServer.Helpers;
using StoreManagementBE.BackendServer.Models.Entities;

namespace StoreManagementBE.BackendServer.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        //public DbSet<ChiTietDonHang> ChiTietDonHangs { set; get; }
        //public DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { set; get; }
        //public DbSet<DonHang> DonHangs { set; get; }
        //public DbSet<KhachHang> KhachHangs { set; get; }
        //public DbSet<KhuyenMai> KhuyenMais { set; get; }
        public DbSet<LoaiSanPham> LoaiSanPhams { set; get; }
        //public DbSet<NhaCungCap> NhaCungCaps { set; get; }
        //public DbSet<NhanVien> NhanViens { set; get; }
        //public DbSet<PhieuNhap> PhieuNhaps { set; get; }
        public DbSet<SanPham> SanPhams { set; get; }
        //public DbSet<ThanhToan> ThanhToans { set; get; }
        public static UnitEnum ParseUnit(string unit)
        {
            return unit switch
            {
                "hộp" => UnitEnum.HOP,
                "cái" => UnitEnum.CAI,
                "tuýp" => UnitEnum.TUYP,
                "lon" => UnitEnum.LON,
                "chai" => UnitEnum.CHAI,
                "gói" => UnitEnum.GOI,
                _ => throw new ArgumentException($"Unit không hợp lệ: {unit}")
            };
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var unitConverter = new ValueConverter<UnitEnum, string>(
                v => v.GetDisplayName(),       // Enum -> tiếng Việt khi lưu DB
                v => ParseUnit(v)              // tiếng Việt -> Enum khi đọc DB
            );

            modelBuilder.Entity<SanPham>()
                .Property(p => p.unit)
                .HasConversion(unitConverter);

            base.OnModelCreating(modelBuilder);
        }


    }
    
}