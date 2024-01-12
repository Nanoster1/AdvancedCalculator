using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AdvancedCalculator.WPF
{
    public enum Axis { AX, AY }
    public class VisualPoint
    {
        public Point Point { get; private set; } //В пикселях
        public Axis Axis { get; private set; }
        public VisualPoint(double x, double y)
        {
            Point = new Point(x, y);
        }
        public VisualPoint(double x, double y, Axis axis)
        {
            Point = new Point(x, y);
            Axis = axis;
        }
        public Ellipse GetEllipse(SolidColorBrush brush)
        {
            double x = Point.X / Field.OneCmScale;
            double y = -Point.Y / Field.OneCmScale;
            if (y == -0) { y = 0; }
            Ellipse ellipse = new Ellipse
            {
                Width = Field.EllipseScale,
                Height = Field.EllipseScale,
                Stroke = Brushes.Black,
                Fill = brush,
                ToolTip = $"X:{x} Y:{y}"
            };
            Canvas.SetLeft(ellipse, Point.X - Field.X1 - Field.EllipseScale / 2);
            Canvas.SetTop(ellipse, Point.Y - Field.Y1 - Field.EllipseScale / 2);
            return ellipse;
        }
        //private void MouseEnterEllipse(object sender, MouseEventArgs e)
        //{
        //    ellipse.Width = Field.EllipseScale * 2;
        //    ellipse.Height = Field.EllipseScale * 2;
        //}
        //private void MousLeaveEllipse(Ellipse ellipse)
        //{
        //    ellipse.Width = Field.EllipseScale;
        //    ellipse.Height = Field.EllipseScale;
        //}
        public Point GetPointForFunction()
        {
            return new Point()
            {
                X = Point.X - Field.X1,
                Y = Point.Y - Field.Y1
            };
        }
        public Label GetVisualNumber(double number)
        {
            Label num = new Label { FontSize = Field.FontScale };
            num.Content = Convert.ToInt32(number);
            Canvas.SetLeft(num, Point.X - Field.X1 - num.ActualWidth + 4 / 2);
            Canvas.SetTop(num, Point.Y - Field.Y1 - 2);
            return num;
        }
    }
}