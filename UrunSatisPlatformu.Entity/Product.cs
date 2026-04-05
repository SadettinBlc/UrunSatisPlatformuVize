using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UrunSatisPlatformu.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; } // Fotoğraf yükleme işlemi için
        public bool IsActive { get; set; } = true;

        // İlişki: Her ürünün bir kategorisi olmak zorundadır (Foreign Key)
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }
    }
}