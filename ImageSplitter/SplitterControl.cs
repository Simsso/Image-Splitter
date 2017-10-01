using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace ImageSplitter
{
    class SplitterControl
    {
        private List<Split> splits = new List<Split>();

        private Canvas canvas;
        private System.Windows.Controls.Image imageControl;
        private Bitmap image;
        private PointF canvasSize, imageSize, imageOffset;


        public SplitterControl(Canvas canvas, System.Windows.Controls.Image imageControl, Bitmap image)
        {
            this.canvas = canvas;
            this.canvas.Children.Clear();
            this.imageControl = imageControl;
            this.image = image;

            // click events
            this.imageControl.MouseLeftButtonDown += ImageClicked;
            this.canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
        }

        private void ImageClicked(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point position = e.GetPosition((System.Windows.IInputElement)sender);
            AddSplit(new PointF((float)position.X, (float)position.Y));
        }

        /// <summary>
        /// Adds a new split line at the passed position.
        /// </summary>
        /// <param name="position">Position is a point on the image (in pixels). If it is closer to the top then the the left side, the line will be vertical. Otherwise horizontal.</param>
        public void AddSplit(PointF position)
        {
            PointF norm = new PointF(position.X / imageSize.X, position.Y / imageSize.Y);
            Split newSplit = Split.FromPosition(norm);
            this.splits.Add(newSplit);
            this.canvas.Children.Add(newSplit.Line);
            RenderLines();
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Click on canvas will only be triggered, if a visible part of the canvas is clicked.
            // As the canvas is transparent, all other clicks will hit the image beneeth.
            Line clicked = (Line)e.OriginalSource;
            RemoveSplit(GetSplitByLine(clicked));
        }

        public void RemoveSplit(Split split)
        {
            if (split == null) return;

            this.canvas.Children.Remove(split.Line);
            this.splits.Remove(split);
        }

        private Split GetSplitByLine(Line line)
        {
            foreach (var split in splits)
            {
                if (split.Line == line)
                {
                    return split;
                }
            }
            return null;
        }

        public void RenderLines()
        {
            this.canvasSize = new PointF((float)canvas.ActualWidth, (float)canvas.ActualHeight);
            this.imageSize = new PointF((float)imageControl.ActualWidth, (float)imageControl.ActualHeight);
            this.imageOffset = new PointF((canvasSize.X - imageSize.X) / 2, (canvasSize.Y - imageSize.Y) / 2);

            foreach (var split in splits)
            {
                split.UpdateLine(canvasSize, imageSize, imageOffset);
            }
        }

        /// <summary>
        /// Get all splits with a given orientation in ascending order.
        /// </summary>
        /// <param name="orientation">The desired orientation (horizontal or vertical).</param>
        /// <returns></returns>
        public double[] GetSplits(SplitOrientation orientation)
        {
            List<double> positions = new List<double>();
            foreach (var split in splits)
            {
                if (split.Orientation == orientation)
                {
                    positions.Add(split.Position);
                }
            }
            positions.Sort();
            return positions.ToArray();
        }


        private Bitmap[][] GetParts()
        {
            double[] horizontalSplits = GetSplits(SplitOrientation.Horizontal),
                verticalSplits = GetSplits(SplitOrientation.Vertical);

            Bitmap[][] parts = new Bitmap[verticalSplits.Length + 1][];

            for (int x = 0; x < verticalSplits.Length + 1; x++)
            {
                double left = (x == 0) ? 0 : verticalSplits[x - 1],
                    right = (x == verticalSplits.Length) ? 1 : verticalSplits[x];

                parts[x] = new Bitmap[horizontalSplits.Length + 1];

                for (int y = 0; y < horizontalSplits.Length + 1; y++)
                {
                    double top = (y == 0) ? 0 : horizontalSplits[y - 1],
                        bottom = (y == horizontalSplits.Length) ? 1 : horizontalSplits[y];

                    parts[x][y] = this.image.Slice(left, top, right, bottom);
                }
            }

            return parts;
        }

        public void Export(SplitConfig config, Size size, string imageName)
        {
            Bitmap[][] parts = GetParts();
            using (PartProcessor processor = new PartProcessor(parts))
            {
                processor.CropParts();
                processor.ScaleParts(size);
                parts = processor.Parts;
                for (int x = 0; x < parts.Length; x++)
                {
                    for (int y = 0; y < parts[x].Length; y++)
                    {
                        if (config.FileNames[y][x] == null) continue;

                        string filePath = config.OutputPath + @"\" + config.FileNames[y][x] + "_" + imageName + ".jpg";
                        parts[x][y].Save(string.Format(filePath, x, y));
                        parts[x][y].Dispose();
                    }
                }
            }
        }
    }
}
