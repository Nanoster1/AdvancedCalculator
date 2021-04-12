using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AdvancedCalculator.Logic;

namespace AdvancedCalculator.WPF
{
    public class FunctionDrawer
    {
        Canvas Field { get; set; }
        public Window Window { get; private set; }
        List<object> Elements { get; set; } = new List<object>();
        InfoWorker InfoWorker { get; set; }
        Polyline Function { get; set; } = new Polyline() { Stroke = Brushes.Black};
        public FunctionDrawer(Canvas field, InfoWorker infoWorker, Control svField)
        {
            Field = field;
            Window = new Window(svField);
            InfoWorker = infoWorker;
            SetField();
            SetElements();
        }
        public void SetElements()
        {
            Elements.Clear();
            AddGridX();
            AddGridY();
            AddOX();
            AddOY();
            AddPointsX();
            AddNumsX();
            AddPointsY();
            AddNumsY();
            AddFunction();
        }
        public void Draw()
        {
            Function.Points.Clear();
            Field.Children.Clear();
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i] is Line line)
                {
                    if (line.Y1 == line.Y2)
                    {
                        if (line.Y1 <= Window.Y2 && line.Y2 >= Window.Y1)
                            Field.Children.Add(line);
                    }
                    else
                    {
                        if (line.X1 <= Window.X2 && line.X2 >= Window.X1)
                            Field.Children.Add(line);
                    }
                }
                else if (Elements[i] is VisualPoint point)
                {
                    if (point.Type == Type.Axis)
                    {
                        if (point.Point.X >= Window.X1 && point.Point.X <= Window.X2 && 
                            point.Point.Y >= Window.Y1 && point.Point.Y <= Window.Y2)
                        {
                            Field.Children.Add(point.GetEllipse());
                            Field.Children.Add(point.GetVisualNumber(Field));
                        }
                    }
                    else
                    {
                        if (point.Point.X >= Window.X1 - Window.Scale && point.Point.X <= Window.X2 + Window.Scale &&
                            point.Point.Y >= Window.Y1 - Window.Scale && point.Point.Y <= Window.Y2 + Window.Scale)
                        {
                            Function.Points.Add(point.Point);
                            var Ellipse = point.GetEllipse();
                            Ellipse.Fill = Brushes.Red;
                            Ellipse.ToolTip = $"X: {(point.Point.X - Field.Width / 2) / Window.Scale} Y: {-(point.Point.Y - Field.Height / 2) / Window.Scale}";
                            Field.Children.Add(Ellipse);
                        }
                    }
                } 
            }
            Field.Children.Add(Function);
        }

        void AddOY()
        {
            Line oyLine = new Line();
            oyLine.X1 = Field.Width / 2;
            oyLine.Y1 = 0;
            oyLine.X2 = Field.Width / 2;
            oyLine.Y2 = Field.Height;
            oyLine.Stroke = Brushes.Black;
            Elements.Add(oyLine);
        }
        void AddOX()
        {
            Line oxLine = new Line();
            oxLine.X1 = 0;
            oxLine.Y1 = Field.Height / 2;
            oxLine.X2 = Field.Width;
            oxLine.Y2 = Field.Height / 2;
            oxLine.Stroke = Brushes.Black;
            Elements.Add(oxLine);
        }
        public void SetField()
        {
            if (Math.Abs(InfoWorker.Range[0]) > Math.Abs(InfoWorker.Range[^1]))
                Field.Width = Math.Abs(Math.Round(InfoWorker.Range[0])) * Window.Scale * 2; //На 2 умножаем, т.к. у нас 2 стороны (Слева и справа)
            else
                Field.Width = Math.Abs(Math.Round(InfoWorker.Range[^1], 0)) * Window.Scale * 2;
            double maxY = 0;
            for (int i = 0; i < InfoWorker.Calculators.Count; i++)
            {
                if (double.IsNaN(InfoWorker.Calculators[i].Answer) || double.IsInfinity(InfoWorker.Calculators[i].Answer))
                    continue;
                else if (Math.Abs(InfoWorker.Calculators[i].Answer) > maxY)
                    maxY = Math.Abs(InfoWorker.Calculators[i].Answer);
            }
            Field.Height = Math.Round(maxY, 0) * Window.Scale * 2;
        }
        void AddFunction()
        {
            for (int i = 0; i < InfoWorker.Calculators.Count; i++)
            {
                if (double.IsNaN(InfoWorker.Calculators[i].Answer) || double.IsInfinity(InfoWorker.Calculators[i].Answer))
                    continue;
                Elements.Add(new VisualPoint((Field.Width / 2 + Window.Scale * double.Parse(InfoWorker.Calculators[i].X)), Math.Round(Field.Height / 2 - Window.Scale * InfoWorker.Calculators[i].Answer, 12), Window));
            }
        }
        void AddPointsY()
        {
            for (int i = 0; i <= Field.Height / Window.Scale; i++)
            {
                VisualPoint visualPoint = new VisualPoint(Field.Width / 2, i * Window.Scale, Window, Axis.AY);
                Elements.Add(visualPoint);
            }
        }
        void AddPointsX()
        {
            for (int i = 0; i <= Field.Width / Window.Scale; i++)
            {
                VisualPoint visualPoint = new VisualPoint(i * Window.Scale, Field.Height / 2, Window, Axis.AX);
                Elements.Add(visualPoint);
            }
        }
        void AddNumsY()
        {
            int k = 0;
            for (double i = Field.Height / (Window.Scale * 2); i >= Field.Height / -(Window.Scale * 2); i--)
            {
                if (Elements[k] is VisualPoint visualPoint && visualPoint.Axis == Axis.AY)
                    visualPoint.Number = i;
                else 
                    i++;
                k++;
            }
        }
        void AddNumsX()
        {
            int k = 0;
            for (double i = Field.Width / (-Window.Scale * 2); i <= Field.Width / (Window.Scale * 2); i++)
            {
                if (Elements[k] is VisualPoint visualPoint && visualPoint.Axis == Axis.AX)
                    visualPoint.Number = i;
                else
                    i--;
                k++;
            }
        }
        void AddGridX()
        {
            for (int i = 0; i <= Field.Height / Window.Scale; i++)
            {
                Line lineX = new Line();
                lineX.X1 = 0;
                lineX.Y1 = i * Window.Scale;
                lineX.X2 = Field.Width;
                lineX.Y2 = lineX.Y1;
                lineX.Stroke = Brushes.Gray;
                Elements.Add(lineX);
            }
        }
        void AddGridY()
        {
            for (int i = 0; i <= Field.Width / Window.Scale; i++)
            {
                Line lineY = new Line();
                lineY.X1 = i * Window.Scale;
                lineY.Y1 = 0;
                lineY.X2 = lineY.X1;
                lineY.Y2 = Field.Height;
                lineY.Stroke = Brushes.Gray;
                Elements.Add(lineY);
            }
        }
    }
}
