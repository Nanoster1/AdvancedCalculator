using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace AdvancedCalculator.WPF
{
    public class Field
    {
        public Field(Canvas canvas)
        {
            Canvas = canvas;
        }
        public Canvas Canvas { get; private set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get { return X1 + Canvas.Width; } }
        public double Y2 { get { return Y1 + Canvas.Height; } }
        public double Scale { get; set; } = 100;
        public double EllipseScale { get { return Scale * 0.08; } }
        public double OneCmScale { get { return Scale * 0.4; } }
        public double FontScale { get { return Scale * 0.1; } }
        public bool FunctionPointsVisible { get; set; } = true;
        public bool AxisPointsVisible { get; set; } = true;
        public bool GridVisible { get; set; } = true;
        public bool AxisEllipsesVisible { get; set; } = true;
    }
}
