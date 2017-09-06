using System;
using System.Drawing;
using System.Windows;

namespace ImageSplitter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Image originalImage;


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
        }

        private void Button_LoadImage_Click(object sender, RoutedEventArgs e)
        {
            String filePath = new OpenFileDialog(".png", "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif").Show();
            if (filePath != null)
            {
                SetImage(filePath);
            }
        }
    }
}
