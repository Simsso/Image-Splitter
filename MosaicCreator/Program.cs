using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace MosaicCreator
{
    /// <summary>
    /// Creates an image mosaic out of many single, randomly chosen images. The random images are taken from a directory and all its subdirectories.
    /// The static variables define the height and width of the mosaic parts and the number of horizontal (xCount) and vertical (yCount) elements.
    /// The directory location is on the user desktop by default. This can be changed with the GetAllFilePaths method.
    /// </summary>
    class Program
    {
        private static readonly Random rand = new Random();
        private static readonly int height = 48, width = 48, xCount = 32, yCount = 18;

        static void Main(string[] args)
        {
            string[] files = GetAllFilePaths(@"48\train");
            using (Bitmap composition = new Bitmap(xCount * width, yCount * height))
            {
                using (Graphics g = Graphics.FromImage(composition))
                    for (int x = 0; x < xCount; x++)
                        for (int y = 0; y < yCount; y++)
                            using (Image i = GetRandomImage(files))
                                g.DrawImage(i, new Point(x * width, y * height));

                string fileName = "output_" + xCount + "_" + yCount + ".png";
                composition.Save(fileName);
                Process.Start("Explorer", fileName);
            }
        }

        private static Image GetRandomImage(string[] paths)
        {
            while (true)
            {
                try
                {
                    return Image.FromFile(paths[rand.Next(paths.Length)]);
                }
                catch { }
            }
        }

        private static string[] GetAllFilePaths(string folderPath)
        {
            return Directory.GetFiles(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), folderPath), "*.*", System.IO.SearchOption.AllDirectories);
        }
    }
}
