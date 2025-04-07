using System.ComponentModel;

namespace EShop.Domain.Models
{
	public class Product:BaseModel
	{
		public int id {  get; set; }
		public string name { get; set; } = default!;
		public string ean { get; set; } = default!;
		public decimal price { get; set; }
		public int stock { get; set; } = 0;
		public string sku { get; set; } = default!;
		public Category category { get; set; } = default!;

	}
}
