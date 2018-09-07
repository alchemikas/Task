namespace Product.Api.Contract
{
    public class FileExport
    {
        public byte[] Bytes { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
