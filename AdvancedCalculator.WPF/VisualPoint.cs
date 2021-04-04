using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace AdvancedCalculator.WPF
{
    public class VisualPoint
    {
        public Point Point { get; private set; }
        public bool Axis { get; private set; } //X = true, Y = false
        public double Number { get; set; }
        public VisualPoint(double x, double y, bool axis)
        {
            Point = new Point(x, y);
            Axis = axis;
        }
    }
}
