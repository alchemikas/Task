using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ProductUI.Models
{
    public class ProductEditModel
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(0, 999), DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public IFormFile Image { set; get; }
        public string Photo { set; get; }
    }
}
