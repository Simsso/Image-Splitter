using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Input;
using Windows = System.Windows;

namespace ImageSplitter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Windows.Window
    {
        private SplitterControl splitterControl;
        private Image originalImage;
        private List<double> horizontalSplits = new List<double>(),
            verticalSplits = new List<double>();

        private readonly Size defaultSize = new Size(64, 64);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetImage(String path)
        {
            SetImage(Image.FromFile(path));
        }

        private void SetImage(Image image)
        {
            this.originalImage = image;
            this.Image_Small.Source = image.ToImageSource();
            this.Image_Primary.Source = image.ToImageSource();
            splitterControl = new SplitterControl(Canvas_PrimaryOverlay, Image_Primary, (Bitmap)originalImage);
        }

        private void Image_Primary_Changed(object sender, EventArgs e)
        {
            splitterControl?.RenderLines();
        }

        private void Button_Export_Click(object sender, EventArgs e)
        {
            Bitmap[][] parts = splitterControl.GetParts();
            using (PartProcessor processor = new PartProcessor(parts))
            {
                processor.CropParts();
                processor.ScaleParts(ParseSize());
                parts = processor.Parts;
                for (int x = 0; x < parts.Length; x++)
                {
                    for (int y = 0; y < parts[x].Length; y++)
                    {
                        parts[x][y].Save(String.Format("{0:00000}_{1:00000}.jpg", x, y));
                        parts[x][y].Dispose();
                    }
                }
            }
            Windows.MessageBox.Show("Images saved successfully");
        }

        private Size ParseSize()
        {
            try
            {
                int width = int.Parse(TextBox_ResolutionX.Text);
                int height = int.Parse(TextBox_ResolutionY.Text);
                return new Size(width, height);
            }
            catch (Exception)
            {
                TextBox_ResolutionX.Text = defaultSize.Width.ToString();
                TextBox_ResolutionY.Text = defaultSize.Height.ToString();
                return defaultSize;
            }
        }

        private void PrimaryArea_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Button_LoadImage_Click(object sender, EventArgs e)
        {
            String filePath = new OpenFileDialog(".png", "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif").Show();
            if (filePath != null)
            {
                SetImage(filePath);
            }
        }
    }
}
