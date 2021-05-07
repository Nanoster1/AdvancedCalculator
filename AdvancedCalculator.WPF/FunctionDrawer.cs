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
    public static class FunctionDrawer
    {
        public static void Draw(InfoWorker infoWorker,Field field)
        {
            DrawGrid(field);
            DrawFunction(infoWorker, field);
        }
        public static void DrawGrid(Field field)
        {
            field.Canvas.Children.Clear();
            DrawRightY(field);
            DrawLeftY(field);
            DrawBotX(field);
            DrawUpX(field);
        }
        private static void DrawRightY(Field field)
        {
            for (double i = 0; i < field.X2; i += field.OneCmScale) //для правых y
            {
                if (i > field.X1)
                    AddYLine(i, field);
            }
        }
        private static void DrawLeftY(Field field)
        {
            for (double i = 0; i > field.X1; i -= field.OneCmScale) //для левых y
            {
                if (i < field.X2)
                    AddYLine(i, field);
            }
        }
        private static void DrawBotX(Field field)
        {
            for (double i = 0; i < field.Y2; i += field.OneCmScale) //для нижних x
            {
                if (i > field.Y1)
                    AddXLine(i, field);
            }
        }
        private static void DrawUpX(Field field)
        {
            for (double i = 0; i > field.Y1; i -= field.OneCmScale) //для верхних x
            {
                if (i < field.Y2)
                    AddXLine(i, field);
            }
        }
        private static void AddYLine(double i, Field field)
        {
            field.Canvas.Children.Add(GetLine(i, Axis.AY, field));
            var vpoint = new VisualPoint(i, 0, field);
            if (0 > field.Y1 && 0 < field.Y2)
            {
                if (field.AxisEllipsesVisible)
                    field.Canvas.Children.Add(vpoint.GetEllipse(Brushes.Black));
                if (field.AxisPointsVisible)
                    field.Canvas.Children.Add(vpoint.GetVisualNumber(i / field.OneCmScale));
            }
        }
        private static void AddXLine(double i, Field field)
        {
            field.Canvas.Children.Add(GetLine(i, Axis.AX, field));
            var vpoint = new VisualPoint(0, i, field);
            if (0 > field.X1 && 0 < field.X2)
            {
                if (field.AxisEllipsesVisible)
                    field.Canvas.Children.Add(vpoint.GetEllipse(Brushes.Black));
                if (field.AxisPointsVisible)
                    if (i != 0) { field.Canvas.Children.Add(vpoint.GetVisualNumber(-i / field.OneCmScale)); }
            }
        }
        private static Line GetLine(double i, Axis axis, Field field)
        {
            Line line = new Line() { Stroke = Brushes.White };
            if (i == 0) 
                line.Stroke = Brushes.Black; 
            else if (field.GridVisible) 
                line.Stroke = Brushes.Gray;
            if (axis == Axis.AY)
            {
                line.X1 = i - field.X1;
                line.X2 = i - field.X1;
                line.Y1 = 0;
                line.Y2 = field.Canvas.Height;
            }
            else
            {  
                line.X1 = 0;
                line.X2 = field.Canvas.Width;
                line.Y1 = i - field.Y1;
                line.Y2 = i - field.Y1;
            }
            return line;
        }
        private static void DrawFunction(InfoWorker infoWorker, Field field)
        {
            Calculator[] calculators = infoWorker.Calculators.ToArray();
            Polyline function = new Polyline() { Stroke = Brushes.Black };
            for (int i = calculators.Length - 1; i > 0; i--)
            {
                if (СheckBorder(calculators[i].X, calculators[i].Y, field))
                {
                    var vpoint = new VisualPoint(calculators[i].X * field.OneCmScale, -calculators[i].Y * field.OneCmScale, field);
                    function.Points.Add(vpoint.GetPointForFunction());
                    if (field.FunctionPointsVisible) 
                    {
                        Ellipse ellipse = vpoint.GetEllipse(Brushes.Red);
                        field.Canvas.Children.Add(ellipse);
                    }
                    if (!CheckOnlyXBorder(calculators[i - 1].X, field)) //Если x выходит за границы, то мы обрываем проверку ост. x
                        break;
                }
            }
            field.Canvas.Children.Add(function);
        }
        private static bool СheckBorder(double x, double y, Field field)
        {
            if (x * field.OneCmScale > field.X1 &&
                x * field.OneCmScale < field.X2 &&
               -y * field.OneCmScale > field.Y1 &&
               -y * field.OneCmScale < field.Y2)
                return true;
            else
                return false;
        }
        private static bool CheckOnlyXBorder(double x, Field field)
        {
            if (x * field.OneCmScale > field.X1 &&
                x * field.OneCmScale < field.X2)
                return true;
            else
                return false;
        }
    }
}
