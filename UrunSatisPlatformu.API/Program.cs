using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrunSatisPlatformu.Data;
using UrunSatisPlatformu.Data.Abstract;
using UrunSatisPlatformu.Data.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Repository'nin sisteme tanıtılması
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// 1. Veritabanı Bağlantısı Ayarı
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Identity (Üyelik) Sistemi Ayarı (Şifre zorluklarını test için basit tutuyoruz)
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllers();

// === DEĞİŞEN KISIM 1: OpenApi yerine Swagger servislerini ekliyoruz ===
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// === DEĞİŞEN KISIM 2: MapOpenApi yerine Swagger UI arayüzünü açıyoruz ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();