using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageSplitter
{
    static class ImageExtensions
    {
        public static ImageSource ToImageSource(this Image image)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Bmp);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public static Bitmap Slice(this Bitmap bitmap, double left, double top, double right, double bottom)
        {
            left *= bitmap.Width;
            right *= bitmap.Width;
            top *= bitmap.Height;
            bottom *= bitmap.Height;

            Rectangle rectangle = new Rectangle(
                (int)Math.Round(left), // x
                (int)Math.Round(top), // y
                (int)Math.Round(right - left), // width
                (int)Math.Round(bottom - top)); // height

            return bitmap.Slice(rectangle);
        }

        public static Bitmap Slice(this Bitmap bitmap, Rectangle section)
        {
            return bitmap.Clone(section, bitmap.PixelFormat);
        }
    }
}
