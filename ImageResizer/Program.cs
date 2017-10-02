using System;
using System.Drawing;
using System.IO;

namespace ImageResizer
{
    /// <summary>
    /// Resizes all images in a given input folder and stores them in the output folder.
    /// </summary>
    class Program
    {
        private static string inputPath = @"D:\Development\D2A\images\128",
            outputPath = @"D:\Development\D2A\images\48";

        private static Size targetSize = new Size(48, 48);

        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(inputPath, "*", SearchOption.TopDirectoryOnly);
            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath);
                try
                {
                    using (Bitmap input = (Bitmap)Bitmap.FromFile(filePath))
                    {
                        Bitmap output = new Bitmap(input, targetSize);
                        output.Save(outputPath + @"\" + fileName);
                        output.Dispose();
                        Console.WriteLine(fileName + " was saved successfully.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine(filePath + " was skipped because an error occured.");
                    continue;
                }
            }
        }
    }
}
