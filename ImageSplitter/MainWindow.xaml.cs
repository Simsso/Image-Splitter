using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Input;

namespace ImageSplitter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SplitterControl splitterControl;
        private Image originalImage;
        private List<double> horizontalSplits = new List<double>(),
            verticalSplits = new List<double>();

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

        private void Button_Export_Click(object sender, RoutedEventArgs e)
        {
            Bitmap[][] parts = splitterControl.GetParts();
            for (int x = 0; x < parts.Length; x++)
            {
                for (int y = 0; y < parts[x].Length; y++)
                {
                    parts[x][y].Save(String.Format("{0:00000}_{1:00000}.jpg", x, y));
                }
            }
        }

        private void PrimaryArea_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Button_LoadImage_Click(object sender, RoutedEventArgs e)
        {
            String filePath = new OpenFileDialog(".png", "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif").Show();
            if (filePath != null)
            {
                SetImage(filePath);
            }
        }
    }
}
