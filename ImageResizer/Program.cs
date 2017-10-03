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
            Resize();
            MoveIntoLabelFolders(.8f);
        }

        private static void Resize()
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

        private static void MoveIntoLabelFolders(float trainingRatio)
        {
            Random random = new Random();
            string[] files = Directory.GetFiles(outputPath, "*", SearchOption.TopDirectoryOnly);
            foreach (string filePath in files)
            {
                string label = Path.GetFileNameWithoutExtension(filePath).Split('_')[0];
                string path = outputPath + @"\" + ((random.NextDouble() < trainingRatio) ? "train" : "test") + @"\" + LabelToFolderName(label);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                File.Move(filePath, path + @"\" + Path.GetFileName(filePath));
            }
        }

        private static string LabelToFolderName(string label)
        {
            int number;
            char[] chars = label.ToLower().ToCharArray();
            switch (chars[0])
            {
                case '2': number = 0; break;
                case '3': number = 1; break;
                case '4': number = 2; break;
                case '5': number = 3; break;
                case '6': number = 4; break;
                case '7': number = 5; break;
                case '8': number = 6; break;
                case '9': number = 7; break;
                case 't': number = 8; break;
                case 'j': number = 9; break;
                case 'q': number = 10; break;
                case 'k': number = 11; break;
                case 'a': number = 12; break;
                default:
                    throw new ArgumentException();
            }

            switch (chars[1])
            {
                case 's': number += 13 * 0; break;
                case 'd': number += 13 * 1; break;
                case 'c': number += 13 * 2; break;
                case 'h': number += 13 * 3; break;
                default:
                    throw new ArgumentException();
            }

            return number.ToString();
        }
    }
}
