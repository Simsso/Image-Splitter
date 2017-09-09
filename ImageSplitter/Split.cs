using System;
using System.Drawing;
using System.Windows.Shapes;

namespace ImageSplitter
{
    /// <summary>
    /// Represents a split, i.e. a cut in an image.
    /// A horizontal split goes from left to right, a vertical split from top to bottom.
    /// An objects holds references to position and orientation (which are the actual attributes) and a reference to the Line object that visualizes the split.
    /// </summary>
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

        /// <summary>
        /// Returns an orientation depending on a point.
        /// If the point is close to the top or bottom, the orientation will be vertical. Otherwise horizontal.
        /// </summary>
        /// <param name="norm">Normlized point, i.e. x and y are in [0,1]</param>
        /// <returns>The new orientation.</returns>
        public static SplitOrientation GetOrientationFromPosition(PointF norm)
        {
            return (Math.Min(norm.X, 1 - norm.X) < Math.Min(norm.Y, 1 - norm.Y)) ? SplitOrientation.Horizontal : SplitOrientation.Vertical;
        }

        /// <summary>
        /// Split factory, that takes a point [0,1]x[0,1].
        /// Depending on where the point lays, the split will be horizontal or vertical.
        /// </summary>
        /// <param name="norm">Normalized point, i.e. x and y are in [0,1]</param>
        /// <returns>A new split object, with all attributes set.</returns>
        public static Split FromPosition(PointF norm)
        {
            SplitOrientation orientation = GetOrientationFromPosition(norm);
            return new Split((orientation == SplitOrientation.Horizontal) ? norm.Y : norm.X, orientation);
        }

        /// <summary>
        /// Updates the position of the line dependingon canvas and image.
        /// </summary>
        /// <param name="canvasSize">Size of the canvas that the line is drawn onto.</param>
        /// <param name="imageSize">Size of the image that lays behind the canvas.</param>
        /// <param name="imageOffset">Offset of the image relative to the canvas (always positive).</param>
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

        /// <summary>
        /// </summary>
        /// <returns>New empty line without position.</returns>
        private static Line GetLine()
        {
            Line line = new Line();
            line.Stroke = System.Windows.Media.Brushes.DarkBlue;
            line.StrokeThickness = 2;
            return line;
        }
    }
}
