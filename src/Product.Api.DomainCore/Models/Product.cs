using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.Api.DomainCore.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime LastUpdated { get; set; }
        public virtual Image Image { get; set; }
        public virtual ImageThumbnail ImageThumbnail { get; set; }
    }
}
