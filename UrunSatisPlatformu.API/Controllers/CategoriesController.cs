using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrunSatisPlatformu.Data.Abstract;
using UrunSatisPlatformu.Entity;
using UrunSatisPlatformu.Entity.DTOs; // Entity klasörünün adını kontrol et

namespace UrunSatisPlatformu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Bu sınıfa girmek için en azından giriş (Login) yapmış olmak şart
    public class CategoriesController : ControllerBase
    {
        private readonly IGenericRepository<Category> _repository;

        public CategoriesController(IGenericRepository<Category> repository)
        {
            _repository = repository;
        }

        // GET: Herkes (Giriş yapan) listeyi görebilir
        // GET: Herkes (Giriş yapan VEYA yapmayan) listeyi görebilir
        // CategoriesController.cs içinde GetAll metodunu güncelle
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // "Kategorileri getirirken yanına Products (Ürünler) listesini de koy" diyoruz
            var values = await _repository.GetAllWithIncludesAsync(x => x.Products);
            return Ok(values);
        }

        // GET BY ID: Herkes (Giriş yapan) tek bir kategoriyi görebilir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var value = await _repository.GetByIdAsync(id); // Sizin komuta göre güncellendi
            if (value == null) return NotFound("Kategori bulunamadı.");
            return Ok(value);
        }

        // POST: SADECE ADMİN YENİ KATEGORİ EKLEYEBİLİR
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryCreateDto dto) // Buraya artık yeni DTO'yu yazdık
        {
            // DTO'dan gelen temiz verileri asıl Category sınıfına aktarıyoruz
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true
            };

            await _repository.AddAsync(category);
            return Ok("Kategori başarıyla eklendi.");
        }

        // PUT: SADECE ADMİN GÜNCELLEYEBİLİR
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public IActionResult UpdateCategory(Category category)
        {
            _repository.Update(category); // Update metodu senkron yazılmış, aynen kullandık
            return Ok("Kategori başarıyla güncellendi.");
        }

        // DELETE: SADECE ADMİN SİLEBİLİR
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var value = await _repository.GetByIdAsync(id); // Önce sileceğimiz veriyi buluyoruz
            if (value == null) return NotFound("Silinecek kategori bulunamadı.");

            _repository.Delete(value); // Delete metodu senkron yazılmış, aynen kullandık
            return Ok("Kategori başarıyla silindi.");
        }
    }
}