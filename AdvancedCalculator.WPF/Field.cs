using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using AdvancedCalculator.Logic;

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

        public void Draw(InfoWorker infoWorker)
        {
            DrawGrid();
            DrawFunction(infoWorker);
        }
        public void DrawGrid()
        {
            Canvas.Children.Clear();
            DrawRightY();
            DrawLeftY();
            DrawBotX();
            DrawUpX();
        }
        private void DrawRightY()
        {
            for (double i = 0; i < X2; i += OneCmScale) //для правых y
            {
                if (i > X1)
                    AddYLine(i);
            }
        }
        private void DrawLeftY()
        {
            for (double i = 0; i > X1; i -= OneCmScale) //для левых y
            {
                if (i < X2)
                    AddYLine(i);
            }
        }
        private void DrawBotX()
        {
            for (double i = 0; i < Y2; i += OneCmScale) //для нижних x
            {
                if (i > Y1)
                    AddXLine(i);
            }
        }
        private void DrawUpX()
        {
            for (double i = 0; i > Y1; i -= OneCmScale) //для верхних x
            {
                if (i < Y2)
                    AddXLine(i);
            }
        }
        private void AddYLine(double i)
        {
            Canvas.Children.Add(GetLine(i, Axis.AY));
            var vpoint = new VisualPoint(i, 0, this);
            if (0 > Y1 && 0 < Y2)
            {
                if (AxisEllipsesVisible)
                    Canvas.Children.Add(vpoint.GetEllipse(Brushes.Black));
                if (AxisPointsVisible)
                    Canvas.Children.Add(vpoint.GetVisualNumber(i / OneCmScale));
            }
        }
        private void AddXLine(double i)
        {
            Canvas.Children.Add(GetLine(i, Axis.AX));
            var vpoint = new VisualPoint(0, i, this);
            if (0 > X1 && 0 < X2)
            {
                if (AxisEllipsesVisible)
                    Canvas.Children.Add(vpoint.GetEllipse(Brushes.Black));
                if (AxisPointsVisible)
                    if (i != 0) { Canvas.Children.Add(vpoint.GetVisualNumber(-i / OneCmScale)); }
            }
        }
        private Line GetLine(double i, Axis axis)
        {
            Line line = new Line() { Stroke = Brushes.White };
            if (i == 0)
                line.Stroke = Brushes.Black;
            else if (GridVisible)
                line.Stroke = Brushes.Gray;
            if (axis == Axis.AY)
            {
                line.X1 = i - X1;
                line.X2 = i - X1;
                line.Y1 = 0;
                line.Y2 = Canvas.Height;
            }
            else
            {
                line.X1 = 0;
                line.X2 = Canvas.Width;
                line.Y1 = i - Y1;
                line.Y2 = i - Y1;
            }
            return line;
        }
        private void DrawFunction(InfoWorker infoWorker)
        {
            Calculator[] calculators = infoWorker.Calculators.ToArray();
            Polyline function = new Polyline() { Stroke = Brushes.Black };
            for (int i = calculators.Length - 1; i > 0; i--)
            {
                if (СheckBorder(calculators[i].X, calculators[i].Y))
                {
                    var vpoint = new VisualPoint(calculators[i].X * OneCmScale, -calculators[i].Y * OneCmScale, this);
                    function.Points.Add(vpoint.GetPointForFunction());
                    if (FunctionPointsVisible)
                    {
                        Ellipse ellipse = vpoint.GetEllipse(Brushes.Red);
                        Canvas.Children.Add(ellipse);
                    }
                    if (!CheckOnlyXBorder(calculators[i - 1].X)) //Если x выходит за границы, то мы обрываем проверку ост. x
                        break;
                }
            }
            Canvas.Children.Add(function);
        }
        private bool СheckBorder(double x, double y)
        {
            if (x * OneCmScale > X1 &&
                x * OneCmScale < X2 &&
               -y * OneCmScale > Y1 &&
               -y * OneCmScale < Y2)
                return true;
            else
                return false;
        }
        private bool CheckOnlyXBorder(double x)
        {
            if (x * OneCmScale > X1 &&
                x * OneCmScale < X2)
                return true;
            else
                return false;
        }
    }
}