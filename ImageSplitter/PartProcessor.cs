using System;
using System.Drawing;

namespace ImageSplitter
{
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


        // Determines the minimum height and minimum width across all parts.
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
