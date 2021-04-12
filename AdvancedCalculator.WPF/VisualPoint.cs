using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AdvancedCalculator.WPF
{
    public enum Type { Function, Axis}
    public enum Axis { AX, AY}
    public class VisualPoint
    {
        public Point Point { get; private set; }
        public Axis Axis { get; private set; }
        public double Number { get; set; }
        public Type Type { get; private set; }
        private Window Control { get; set; }
        public VisualPoint(double x, double y, Window control)
        {
            Point = new Point(x, y);
            Type = Type.Function;
            Control = control;
        }
        public VisualPoint(double x, double y, Window control ,Axis axis)
        {
            Point = new Point(x, y);
            Type = Type.Axis;
            Axis = axis;
            Control = control;
        }
        public Ellipse GetEllipse()
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = 8;
            ellipse.Height = 8;
            ellipse.Stroke = Brushes.Black;
            Canvas.SetLeft(ellipse, Point.X - 4);
            Canvas.SetTop(ellipse, Point.Y - 4);
            return ellipse;
        }  
        public Label GetVisualNumber(Canvas Field)
        {
            Label num = new Label();
            if (Axis == Axis.AX)
            {
                num.Content = $"{Number}";
                Canvas.SetLeft(num, (Number + Field.Width / (Control.Scale * 2)) * Control.Scale - 8);
                Canvas.SetTop(num, Field.Height / 2 + 6);
            }
            else
            {
                num.Content = $"{Number}";
                Canvas.SetLeft(num, Field.Width / 2 + 6);
                Canvas.SetTop(num, (Field.Height / (Control.Scale * 2) - Number) * Control.Scale - 15);
            }
            return num;
        }
    }
}