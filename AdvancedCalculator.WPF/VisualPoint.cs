using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AdvancedCalculator.WPF
{
    public enum Axis { AX, AY }
    public class VisualPoint
    {
        public VisualPoint(double x, double y, Field _field)
        {
            Point = new Point(x, y);
            field = _field;
        }
        private readonly Field field;
        public Point Point { get; private set; } //В пикселях
        public Ellipse GetEllipse(SolidColorBrush brush)
        {
            double x = Point.X / field.OneCmScale;
            double y = -Point.Y / field.OneCmScale;
            if (y == -0) { y = 0; }
            Ellipse ellipse = new Ellipse
            {
                Width = field.EllipseScale,
                Height = field.EllipseScale,
                Stroke = Brushes.Black,
                Fill = brush,
                ToolTip = $"X:{x} Y:{y}"
            };
            ellipse.MouseEnter += MouseEnterEllipse;
            ellipse.MouseLeave += MouseLeaveEllipse;
            Canvas.SetLeft(ellipse, Point.X - field.X1 - field.EllipseScale / 2);
            Canvas.SetTop(ellipse, Point.Y - field.Y1 - field.EllipseScale / 2);
            return ellipse;
        }
        private void MouseEnterEllipse(object sender, MouseEventArgs e)
        {
            (sender as Ellipse).Width = field.EllipseScale * 2;
            (sender as Ellipse).Height = field.EllipseScale * 2;
            Canvas.SetLeft((sender as Ellipse), Point.X - field.X1 - field.EllipseScale);
            Canvas.SetTop((sender as Ellipse), Point.Y - field.Y1 - field.EllipseScale);
        }
        private void MouseLeaveEllipse(object sender, MouseEventArgs e)
        {
            (sender as Ellipse).Width = field.EllipseScale;
            (sender as Ellipse).Height = field.EllipseScale;
            Canvas.SetLeft((sender as Ellipse), Point.X - field.X1 - field.EllipseScale / 2);
            Canvas.SetTop((sender as Ellipse), Point.Y - field.Y1 - field.EllipseScale / 2);
        }
        public Point GetPointForFunction()
        {
            return new Point()
            {
                X = Point.X - field.X1,
                Y = Point.Y - field.Y1
            };
        }
        public Label GetVisualNumber(double number)
        {
            Label num = new Label { FontSize = field.FontScale };
            num.Content = Convert.ToInt32(number);
            Canvas.SetLeft(num, Point.X - field.X1 - num.ActualWidth + 4 / 2);
            Canvas.SetTop(num, Point.Y - field.Y1 - 2);
            return num;
        }
    }
}
