using Microsoft.AspNetCore.Mvc;
using UrunSatisPlatformu.Data.Abstract;
using UrunSatisPlatformu.Entity;
using UrunSatisPlatformu.Entity.DTOs;

namespace UrunSatisPlatformu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        // Yazdığımız Repository'yi buraya çağırıyoruz (Dependency Injection)
        private readonly IGenericRepository<Category> _repository;

        public CategoriesController(IGenericRepository<Category> repository)
        {
            _repository = repository;
        }

        // 1. KATEGORİLERİ LİSTELEME METODU (GET İSTEĞİ)
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            // Veritabanından tüm kategorileri çekiyoruz
            var categories = await _repository.GetAllAsync();

            // Güvenlik ve temizlik için Entity nesnelerimizi DTO'ya dönüştürüyoruz
            var categoryDtos = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();

            return Ok(categoryDtos); // 200 Başarılı koduyla veriyi dışarı yolluyoruz
        }

        // 2. YENİ KATEGORİ EKLEME METODU (POST İSTEĞİ)
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto categoryDto)
        {
            // Dışarıdan gelen DTO'yu, veritabanına eklenecek Entity'ye çeviriyoruz
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                IsActive = true
            };

            await _repository.AddAsync(category);
            return Ok(new { message = "Kategori başarıyla eklendi." });
        }
        // 3. KATEGORİ GÜNCELLEME (PUT İSTEĞİ)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { message = "Güncellenecek kategori bulunamadı." });

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;

            _repository.Update(category);
            return Ok(new { message = "Kategori başarıyla güncellendi." });
        }

        // 4. KATEGORİ SİLME (DELETE İSTEĞİ)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { message = "Silinecek kategori bulunamadı." });

            _repository.Delete(category);
            return Ok(new { message = "Kategori başarıyla silindi." });
        }
    }
}