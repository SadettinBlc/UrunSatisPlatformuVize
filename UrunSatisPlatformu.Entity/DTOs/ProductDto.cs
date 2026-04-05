namespace UrunSatisPlatformu.Entity.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }
        // Kategori ID'si veya Kategori nesnesi yerine sadece adını gönderiyoruz ki arayüzde temiz görünsün
        public string? CategoryName { get; set; }
    }
}