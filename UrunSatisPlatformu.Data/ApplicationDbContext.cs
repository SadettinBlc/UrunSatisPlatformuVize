using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrunSatisPlatformu.Entity;

namespace UrunSatisPlatformu.Data
{
    // IdentityDbContext'ten miras alıyoruz ki kullanıcı, rol ve şifre tabloları otomatik gelsin.
    // IdentityUser ve IdentityRole default sınıflardır.
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Kendi oluşturduğumuz tabloları (DbSet) buraya ekliyoruz
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Eğer veritabanı tablolarında özel kurallar (örn: Name alanı max 100 karakter olsun) 
            // belirtmek istersek Fluent API kodlarını buraya yazacağız. Şimdilik temel ayarlarda bırakıyoruz.
        }
    }
}