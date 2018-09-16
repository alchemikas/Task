namespace Product.Api.DomainCore.Services
{
    public interface IImageFileResizeService
    {
        byte[] ResizeImage(byte[] imageBytes, int height, int width);
    }
}
