using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Product.Api.DomainCore.Services;

namespace Product.Api.Infrastructure.Services
{
    public class ImageFileResizeService : IImageFileResizeService
    {
        public byte[] ResizeImage(byte[] imageBytes, int height, int width)
        {
            MemoryStream myMemStream = new MemoryStream(imageBytes);
            Image image = Image.FromStream(myMemStream);

            Image resizedImage = ResizeImage(image, height, width);
            MemoryStream myResult = new MemoryStream();
            resizedImage.Save(myResult, ImageFormat.Jpeg); 
            return myResult.ToArray();
        }

        private Image ResizeImage(Image image, int height, int width)
        {
            return image.GetThumbnailImage(width, height, null, IntPtr.Zero);
        }
    }
}
