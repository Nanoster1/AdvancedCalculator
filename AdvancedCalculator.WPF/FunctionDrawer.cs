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
        Canvas Field { get; set; } = new Canvas();
        public double X { get; set; }
        public double Y { get; set; }
        List<object> Elements { get; set; } = new List<object>();
        InfoWorker InfoWorker { get; set; }
        ScrollViewer SVField { get; set; }
        public FunctionDrawer(Canvas field, InfoWorker infoWorker, ScrollViewer svField)
        {
            Field = field;
            InfoWorker = infoWorker;
            SetField();
            SetElements();
            SVField = svField;
        }
        public void SetElements()
        {
            AddPointsX();
            AddNumsX();
            AddPointsY();
            AddNumsY();
            AddGridX();
            AddGridY();
            AddOX();
            AddOY();
            AddFunction();
        }
        public void Draw()
        {
            Field.Children.Clear();
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i] is Line line)
                {
                    if (line.Y1 == line.Y2)
                    {
                        if (line.Y1 <= Y + SVField.ActualHeight && line.Y2 >= Y)
                            Field.Children.Add(line);
                    }
                    else
                    {
                        if (line.X1 <= X + SVField.ActualWidth && line.X2 >= X)
                            Field.Children.Add(line);
                    }
                }
                else if (Elements[i] is VisualPoint point)
                {
                    if (point.Point.X >= X && point.Point.X <= X + SVField.ActualWidth && point.Point.Y >= Y && point.Point.Y <= Y + SVField.ActualHeight)
                    {
                        Ellipse ellipse = new Ellipse();
                        ellipse.Width = 6;
                        ellipse.Height = 6;
                        ellipse.Stroke = Brushes.Black;
                        Canvas.SetLeft(ellipse, point.Point.X - 3);
                        Canvas.SetTop(ellipse, point.Point.Y - 3);
                        Field.Children.Add(ellipse);
                        Label num = new Label();
                        if (point.Axis == true)
                        {
                            num.Content = $"{point.Number}";
                            Canvas.SetLeft(num, (point.Number + Field.Width / 80) * 40 - 8);
                            Canvas.SetTop(num, Field.Height / 2 + 6);
                        }
                        else
                        {
                            num.Content = $"{point.Number}";
                            Canvas.SetLeft(num, Field.Width / 2 + 6);
                            Canvas.SetTop(num, (Field.Height / 80 - point.Number) * 40 - 15);
                        }
                        Field.Children.Add(num);
                    }
                }
                else
                    Field.Children.Add(Elements[i] as UIElement);
            }
        }
        private void AddOY()
        {
            Line oyLine = new Line();
            oyLine.X1 = Field.Width / 2;
            oyLine.Y1 = 0;
            oyLine.X2 = Field.Width / 2;
            oyLine.Y2 = Field.Height;
            oyLine.Stroke = Brushes.Black;
            Elements.Add(oyLine);
        }
        private void AddOX()
        {
            Line oxLine = new Line();
            oxLine.X1 = 0;
            oxLine.Y1 = Field.Height / 2;
            oxLine.X2 = Field.Width;
            oxLine.Y2 = Field.Height / 2;
            oxLine.Stroke = Brushes.Black;
            Elements.Add(oxLine);
        }
        private void SetField()
        {
            if (Math.Abs(InfoWorker.Range[0]) > Math.Abs(InfoWorker.Range[^1]))
                Field.Width = Math.Abs(Math.Round(InfoWorker.Range[0])) * 80;
            else
                Field.Width = Math.Abs(Math.Round(InfoWorker.Range[^1], 0)) * 80;
            double maxY = 0;
            for (int i = 0; i < InfoWorker.Calculators.Count; i++)
            {
                if (double.IsNaN(InfoWorker.Calculators[i].Answer) || double.IsInfinity(InfoWorker.Calculators[i].Answer))
                    continue;
                else if (Math.Abs(InfoWorker.Calculators[i].Answer) > maxY)
                    maxY = Math.Abs(InfoWorker.Calculators[i].Answer);
            }
            Field.Height = Math.Round(maxY, 0) * 80;
        }
        private void AddFunction()
        {
            Polyline polyline = new Polyline();
            polyline.Points = new PointCollection();
            for (int i = 0; i < InfoWorker.Calculators.Count; i++)
            {
                if (double.IsNaN(InfoWorker.Calculators[i].Answer) || double.IsInfinity(InfoWorker.Calculators[i].Answer))
                    continue;
                polyline.Points.Add(new Point((Field.Width / 2 + 40 * double.Parse(InfoWorker.Calculators[i].X)), Math.Round(Field.Height / 2 - 40 * InfoWorker.Calculators[i].Answer, 12)));
            }
            polyline.Stroke = Brushes.Black;
            Elements.Add(polyline);
        }
        private void AddPointsY()
        {
            for (int i = 0; i <= Field.Height / 40; i++)
            {
                VisualPoint visualPoint = new VisualPoint(Field.Width / 2, i * 40, false);
                Elements.Add(visualPoint);
            }
        }
        private void AddPointsX()
        {
            for (int i = 0; i <= Field.Width / 40; i++)
            {
                VisualPoint visualPoint = new VisualPoint(i * 40, Field.Height / 2, true);
                Elements.Add(visualPoint);
            }
        }
        private void AddNumsY()
        {
            int k = 0;
            for (double i = Field.Height / 80; i >= Field.Height / -80; i--)
            {
                if (Elements[k] is VisualPoint visualPoint && visualPoint.Axis == false)
                    visualPoint.Number = i;
                else 
                    i++;
                k++;
            }
        }
        private void AddNumsX()
        {
            int k = 0;
            for (double i = Field.Width / -80; i <= Field.Width / 80; i++)
            {
                if (Elements[k] is VisualPoint visualPoint && visualPoint.Axis == true)
                    visualPoint.Number = i;
                else
                    i--;
                k++;
            }
        }
        private void AddGridX()
        {
            for (int i = 0; i <= Field.Height / 40; i++)
            {
                Line lineX = new Line();
                lineX.X1 = 0;
                lineX.Y1 = i * 40;
                lineX.X2 = Field.Width;
                lineX.Y2 = i * 40;
                lineX.Stroke = Brushes.Gray;
                Elements.Add(lineX);
            }
        }
        private void AddGridY()
        {
            for (int i = 0; i <= Field.Width / 40; i++)
            {
                Line lineY = new Line();
                lineY.X1 = i * 40;
                lineY.Y1 = 0;
                lineY.X2 = i * 40;
                lineY.Y2 = Field.Height;
                lineY.Stroke = Brushes.Gray;
                Elements.Add(lineY);
            }
        }
    }
}
