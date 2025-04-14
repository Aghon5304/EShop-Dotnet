using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Domain.Models
{
    [Table("Products")]
	public class Product : BaseModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(13)]
		public string Ean { get; set; } = string.Empty;

        [Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal Price { get; set; }

		[Required]
		[Column(TypeName = "int")]
		public int Stock { get; set; } = 0;

        [Required]
        [MaxLength(12)]
        public string Sku { get; set; } = string.Empty;

        [Required]
		public Category Category { get; set; } = default!;
    }
}
