namespace UrunSatisPlatformu.Entity.DTOs
{
    public class ProductCreateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; } // Eklerken hangi kategoriye ait olduğunu bilmemiz lazım
    }
}