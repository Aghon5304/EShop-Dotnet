using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Models
{
	public class Category:BaseModel
	{
		[Key]
		public int Id { get; set; }

		[MaxLength(255)]
		[Required]
		public string Name { get; set; }	

		
	}	
}
