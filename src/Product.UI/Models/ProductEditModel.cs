using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductUI.Models
{
    public class ProductEditModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(0, 999), DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public IFormFile Image { set; get; }

        public string Photo { set; get; }

        [HiddenInput]
        public string CodeBeforeEdit { get; set; }
    }
}
