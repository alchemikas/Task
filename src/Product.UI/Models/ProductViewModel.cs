using System;
using System.ComponentModel.DataAnnotations;

namespace ProductUI.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Photo { set; get; }
        public DateTime LastUpdated { get; set; }

    }
}
