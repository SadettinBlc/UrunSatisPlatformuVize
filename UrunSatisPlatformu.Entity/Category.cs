namespace UrunSatisPlatformu.Entity
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        // İlişki: Bir kategorinin altında birden fazla ürün bulunabilir
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
