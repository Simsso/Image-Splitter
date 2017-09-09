using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageSplitter
{
    /// <summary>
    /// Several extensions for classes related to images.
    /// </summary>
    static class ImageExtensions
    {
        /// <summary>
        /// Converts an image into a source that can be used as a source for the WPF Image control.
        /// </summary>
        /// <param name="image">The image</param>
        /// <returns>The ImageSource that was created from the parameter.</returns>
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

        /// <summary>
        /// Slices a rectangle out of a bitmap image. Each parameter is in [0,1]
        /// </summary>
        /// <param name="bitmap">The image that shall be sliced.</param>
        /// <param name="left">Left bound of the rectangle.</param>
        /// <param name="top">Upper bound of the rectangle.</param>
        /// <param name="right">Right bound of the rectangle.</param>
        /// <param name="bottom">Bottom bound of the rectangle.</param>
        /// <returns>A new Bitmap image, that represents a part of the original image.</returns>
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

        /// <summary>
        /// Slices a rectangle out of a bitmap image.
        /// </summary>
        /// <param name="bitmap">The image that shall be sliced.</param>
        /// <param name="section">Size and position of the section to cut.</param>
        /// <returns>A new Bitmap image that is the cut out section.</returns>
        public static Bitmap Slice(this Bitmap bitmap, Rectangle section)
        {
            return bitmap.Clone(section, bitmap.PixelFormat);
        }
    }
}
