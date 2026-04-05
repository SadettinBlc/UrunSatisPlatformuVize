using Microsoft.AspNetCore.Mvc;
using UrunSatisPlatformu.Data.Abstract;
using UrunSatisPlatformu.Entity;
using UrunSatisPlatformu.Entity.DTOs;

namespace UrunSatisPlatformu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<Category> _categoryRepository;

        // Ürünleri çekerken kategori isimlerine de ihtiyacımız olduğu için iki repository'yi de çağırıyoruz
        public ProductsController(IGenericRepository<Product> productRepository, IGenericRepository<Category> categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // 1. ÜRÜNLERİ LİSTELEME
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();

            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                ImageUrl = p.ImageUrl,
                // Ürünün CategoryId'sini kullanarak kategoriler tablosundan ismini buluyoruz
                CategoryName = categories.FirstOrDefault(c => c.Id == p.CategoryId)?.Name ?? "Kategori Yok"
            }).ToList();

            return Ok(productDtos);
        }

        // 2. YENİ ÜRÜN EKLEME
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductCreateDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId, // Az önce eklediğimiz kategorinin ID'si (Örn: 1)
                IsActive = true
            };

            await _productRepository.AddAsync(product);
            return Ok(new { message = "Ürün başarıyla eklendi." });
        }
        // 3. ÜRÜN GÜNCELLEME (PUT İSTEĞİ)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductCreateDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound(new { message = "Güncellenecek ürün bulunamadı." });

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.ImageUrl = dto.ImageUrl;
            product.CategoryId = dto.CategoryId;

            _productRepository.Update(product);
            return Ok(new { message = "Ürün başarıyla güncellendi." });
        }

        // 4. ÜRÜN SİLME (DELETE İSTEĞİ)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound(new { message = "Silinecek ürün bulunamadı." });

            _productRepository.Delete(product);
            return Ok(new { message = "Ürün başarıyla silindi." });
        }
    }
}