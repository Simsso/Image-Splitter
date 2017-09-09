using System;
using System.Drawing;

namespace ImageSplitter
{
    /// <summary>
    /// This class wraps a two dimensional array of Bitmaps.
    /// It has several that manipulate these Bitmaps, e.g. resizing all to equal size.
    /// </summary>
    class PartProcessor : IDisposable
    {
        private readonly Bitmap[][] parts;

        private readonly Size minSize;

        public Bitmap[][] Parts => parts;

        public PartProcessor(Bitmap[][] parts)
        {
            this.parts = parts;
            minSize = GetMinSize();
        }


        /// <summary>
        /// Determines the minimum height and minimum width across all parts.
        /// If one part has the size 100x200 and another one 500x100, 100x100 will be returned.
        /// </summary>
        /// <returns>The lowest height and width across all parts.</returns>
        private Size GetMinSize()
        {
            Size min = new Size(parts[0][0].Width, parts[0][0].Height);
            for (int x = 0; x < parts.Length; x++)
            {
                for (int y = 0; y < parts[x].Length; y++)
                {
                    min.Width = Math.Min(min.Width, parts[x][y].Width);
                    min.Height = Math.Min(min.Height, parts[x][y].Height);
                }
            }
            return min;
        }


        /// <summary>
        /// Crops all bitmaps in the parts array. The size is the result of GetMinSize.
        /// Each bitmap will be cropped in a way that preserves the pixels in the center.
        /// </summary>
        public void CropParts()
        {
            for (int x = 0; x < parts.Length; x++)
            {
                for (int y = 0; y < parts[x].Length; y++)
                {
                    Bitmap img = parts[x][y];
                    parts[x][y] = img.Slice(new Rectangle(
                        (img.Width - minSize.Width) / 2, // left
                        (img.Height - minSize.Height) / 2, // top
                        minSize.Width, // width
                        minSize.Height)); // height
                    img.Dispose();
                }
            }
        }

        
        /// <summary>
        /// Scales the parts that are stored in the parts attribute to the desired size.
        /// If the proportions do not match, the parts will be cropped to match the width-height-ratio of the parameter.
        /// This cropping is conducted in a way that the center parts of the image will be preferred. 
        /// If size is e.g. 100x100 and the part has a size of 240x200, it will be scaled to 100x100 and 20 pixels on the left and on the right will be dropped.
        /// </summary>
        /// <param name="size">The size that all parts will have after the function call.</param>
        public void ScaleParts(Size size)
        {
            for (int x = 0; x < parts.Length; x++)
            {
                for (int y = 0; y < parts[0].Length; y++)
                {
                    Bitmap img = parts[x][y], cutted;
                    float horizontalScaling = img.Width / (float)size.Width,
                        verticalScaling = img.Height / (float)size.Height;
                    if (horizontalScaling > verticalScaling)
                    {
                        cutted = img.Slice(new Rectangle(
                            (int)Math.Round((img.Width - size.Width * verticalScaling) / 2.0), // middle part
                            0, // no need to crop in vertical direction
                            (int)Math.Round(size.Width * verticalScaling), 
                            img.Height)); // entire height
                    }
                    else
                    {
                        cutted = img.Slice(new Rectangle(
                            0,
                            (int)Math.Round((img.Height - size.Height * horizontalScaling) / 2.0), // middle part
                            img.Width,
                            (int)Math.Round(size.Height * horizontalScaling))); // entire height
                    }
                    img.Dispose();
                    parts[x][y] = new Bitmap(cutted, size);
                    cutted.Dispose();
                }
            }
        }

        /// <summary>
        /// All bitmaps need to be disposed. The function takes care of it and thereby prevents memory leaks.
        /// </summary>
        public void Dispose()
        {
            for (int x = 0; x < parts.Length; x++)
            {
                for (int y = 0; y < parts[x].Length; y++)
                {
                    parts[x][y].Dispose();
                }
            }
        }
    }
}
