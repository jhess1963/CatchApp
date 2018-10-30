using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp
{
    public class ImageHelper
    {
        public static Byte[] ShrinkImage(Byte[] originalImage, int imageHeight, int imageWidth)
        {
            if (originalImage != null)
            {
                var oImage = ByteArrayToImage(originalImage);
                using (var smallImage = oImage.GetThumbnailImage(imageWidth, imageHeight,
                    new Image.GetThumbnailImageAbort(
                    () => { return false; }), IntPtr.Zero))
                {
                    return ImageToByteArray(smallImage, ImageFormat.Png);
                }
            }
            else
                return originalImage;
        }

        public static Byte[] ResizeImage(Byte[] originalImage, int height, int width)
        {
            Bitmap imgIn = new Bitmap(ByteArrayToImage(originalImage));
            double y = imgIn.Height;
            double x = imgIn.Width;

            double factor = 1;
            if (width > 0)
            {
                factor = width / x;
            }
            else if (height > 0)
            {
                factor = height / y;
            }
            Bitmap imgOut = new Bitmap((int)(x * factor), (int)(y * factor));

            // Set DPI of image (xDpi, yDpi)
            imgOut.SetResolution(72, 72);

            Graphics g = Graphics.FromImage(imgOut);
            g.Clear(Color.White);
            g.DrawImage(imgIn, new Rectangle(0, 0, (int)(factor * x), (int)(factor * y)),
              new Rectangle(0, 0, (int)x, (int)y), GraphicsUnit.Pixel);

            MemoryStream outStream = new MemoryStream();
            imgOut.Save(outStream, ImageFormat.Jpeg);
            return outStream.ToArray();
        }

        public static Image ByteArrayToImage(Byte[] image)
        {
            MemoryStream ms = new MemoryStream(image);
            return Image.FromStream(ms);
        }

        public static Byte[] ImageToByteArray(Image image, ImageFormat imageFormat)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, imageFormat);
            return ms.ToArray();
        }
    }
}
