using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace AdvancedCalculator.WPF
{
    public static class Field
    {
        public static Canvas Canvas { get; private set; }
        public static double X1 { get; set; }
        public static double Y1 { get; set; }
        public static double X2 { get { return X1 + Canvas.Width; } }
        public static double Y2 { get { return Y1 + Canvas.Height; } }
        public static double Scale { get; set; } = 100;
        public static double EllipseScale { get { return Scale * 0.08; } }
        public static double OneCmScale { get { return Scale * 0.4; } }
        public static double FontScale { get { return Scale * 0.1; } }
        public static bool FunctionPointsVisible { get; set; } = true;
        public static bool AxisPointsVisible { get; set; } = true;
        public static bool GridVisible { get; set; } = true;
        public static bool AxisEllipsesVisible { get; set; } = true;
        public static void SetCanvas(Canvas canvas)
        {
            if (Canvas == null)
                Canvas = canvas;
        }
    }
}
