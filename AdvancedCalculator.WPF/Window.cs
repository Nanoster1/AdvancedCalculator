using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace AdvancedCalculator.WPF
{
    public class Window
    {
        public Control Control { get; }
        public double X1 { get; set; } = 0;
        public double Y1 { get; set; } = 0;
        public double X2 { get { return X1 + Control.ActualWidth; } }
        public double Y2 { get { return Y1 + Control.ActualHeight; } }

        private double scale = 100;
        public double Scale { get { return scale * 0.4; } set { scale = value / 0.4; } }
        public Window(Control control)
        {
            Control = control;
        }
    }
}
