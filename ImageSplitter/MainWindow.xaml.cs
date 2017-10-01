using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        private string imageName = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetImage(String path)
        {
            imageName = Path.GetFileNameWithoutExtension(path);
            Label_ImageName.Content = imageName;
            SetImage(Image.FromFile(path));
        }

        private void SetImage(Image image)
        {
            this.originalImage = image;
            this.Image_Small.Source = image.ToImageSource();
            this.Image_Primary.Source = image.ToImageSource();
            splitterControl = new SplitterControl(Canvas_PrimaryOverlay, Image_Primary, (Bitmap)originalImage);
            UpdateSplitInfoLabel();
        }

        private void Image_Primary_Changed(object sender, EventArgs e)
        {
            splitterControl?.RenderLines();
            UpdateSplitInfoLabel();
        }

        private void Button_Export_Click(object sender, EventArgs e)
        {
            SplitConfig config = new SplitConfig
            {
                OutputPath = @"D:\Development\D2A\images",
                FileNames = new string[][]
                {
                    new string[] { "2s", "3s", "4s", "5s", "6s", "7s", "8s", "9s", "ts", "js", "qs", "ks", "as" },
                    new string[] { "2d", "3d", "4d", "5d", "6d", "7d", "8d", "9d", "td", "jd", "qd", "kd", "ad" },
                    new string[] { "2c", "3c", "4c", "5c", "6c", "7c", "8c", "9c", "tc", "jc", "qc", "kc", "ac" },
                    new string[] { "2h", "3h", "4h", "5h", "6h", "7h", "8h", "9h", "th", "jh", "qh", "kh", "ah" }
                }
            };
            splitterControl.Export(config, ParseSize(), imageName ?? "");
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

        private void Button_LoadImage_Click(object sender, EventArgs e)
        {
            String filePath = new OpenFileDialog(".png", "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif").Show();
            if (filePath != null)
            {
                SetImage(filePath);
            }
        }

        private void UpdateSplitInfoLabel()
        {
            int verticalSplits = splitterControl?.GetSplits(SplitOrientation.Vertical).Length ?? 0,
                horizontalSplist = splitterControl?.GetSplits(SplitOrientation.Horizontal).Length ?? 0;

            Label_SplitInfo.Content = verticalSplits.ToString() + " x " + horizontalSplist.ToString();
        }
    }
}
