using System;
using System.Drawing;
using System.Windows.Shapes;

namespace ImageSplitter
{
    class Split
    { 
        private double position;
        private SplitOrientation orientation;
        private Line line;

        public double Position
        {
            get
            {
                return position;
            }
        }
        public SplitOrientation Orientation
        {
            get
            {
                return orientation;
            }
        }

        public Line Line
        {
            get
            {
                return line;
            }
        }

        public Split(double position, SplitOrientation orientation) : this(position, orientation, GetLine())
        {

        }

        public Split(double position, SplitOrientation orientation, Line line)
        {
            this.position = position;
            this.orientation = orientation;
            this.line = line;
        }

        public static SplitOrientation GetOrientationFromPosition(PointF norm)
        {
            return (Math.Min(norm.X, 1 - norm.X) < Math.Min(norm.Y, 1 - norm.Y)) ? SplitOrientation.Horizontal : SplitOrientation.Vertical;
        }

        public static Split FromPosition(PointF norm)
        {
            SplitOrientation orientation = GetOrientationFromPosition(norm);
            return new Split((orientation == SplitOrientation.Horizontal) ? norm.Y : norm.X, orientation);
        }

        public void UpdateLine(PointF canvasSize, PointF imageSize, PointF imageOffset)
        {
            if (orientation == SplitOrientation.Horizontal)
            {
                line.X1 = Math.Round(imageOffset.X);
                line.X2 = Math.Round(imageOffset.X + imageSize.X);
                line.Y1 = Math.Round(imageOffset.Y + imageSize.Y * position);
                line.Y2 = Math.Round(imageOffset.Y + imageSize.Y * position);
            }
            else
            {
                line.X1 = Math.Round(imageOffset.X + imageSize.X * position);
                line.X2 = Math.Round(imageOffset.X + imageSize.X * position);
                line.Y1 = Math.Round(imageOffset.Y);
                line.Y2 = Math.Round(imageOffset.Y + imageSize.Y);
            }
        }

        private static Line GetLine()
        {
            Line line = new Line();
            line.Stroke = System.Windows.Media.Brushes.DarkBlue;
            line.StrokeThickness = 3;
            return line;
        }
    }
}
