namespace Product.Api.Contract.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public ImageFile Photo { get; set; }
        public double Price { get; set; }
    }

    public class ImageFile
    {
        public string Title { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }
    }
}
