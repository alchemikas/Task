using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.Api.DomainCore.Models
{
    public class ImageThumbnail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string Title { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}
