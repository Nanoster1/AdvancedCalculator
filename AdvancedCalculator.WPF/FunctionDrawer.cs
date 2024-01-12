using System.Windows.Media;
using System.Windows.Shapes;

using AdvancedCalculator.Logic;

namespace AdvancedCalculator.WPF
{
    public class FunctionDrawer
    {
        InfoWorker InfoWorker { get; set; }
        public FunctionDrawer(InfoWorker infoWorker)
        {
            InfoWorker = infoWorker;
            DrawGrid();
        }
        public void Draw()
        {
            DrawGrid();
            DrawFunction();
        }
        public void DrawGrid()
        {
            Field.Canvas.Children.Clear();
            DrawRightY();
            DrawLeftY();
            DrawBotX();
            DrawUpX();
        }
        void DrawRightY()
        {
            for (double i = 0; i < Field.X2; i += Field.OneCmScale) //для правых y
            {
                if (i > Field.X1)
                    AddYLine(i);
            }
        }
        void DrawLeftY()
        {
            for (double i = 0; i > Field.X1; i -= Field.OneCmScale) //для левых y
            {
                if (i < Field.X2)
                    AddYLine(i);
            }
        }
        void DrawBotX()
        {
            for (double i = 0; i < Field.Y2; i += Field.OneCmScale) //для нижних x
            {
                if (i > Field.Y1)
                    AddXLine(i);
            }
        }
        void DrawUpX()
        {
            for (double i = 0; i > Field.Y1; i -= Field.OneCmScale) //для верхних x
            {
                if (i < Field.Y2)
                    AddXLine(i);
            }
        }
        void AddYLine(double i)
        {
            Field.Canvas.Children.Add(GetLine(i, Axis.AY));
            var vpoint = new VisualPoint(i, 0);
            if (0 > Field.Y1 && 0 < Field.Y2)
            {
                if (Field.AxisEllipsesVisible)
                    Field.Canvas.Children.Add(vpoint.GetEllipse(Brushes.Black));
                if (Field.AxisPointsVisible)
                    Field.Canvas.Children.Add(vpoint.GetVisualNumber(i / Field.OneCmScale));
            }
        }
        void AddXLine(double i)
        {
            Field.Canvas.Children.Add(GetLine(i, Axis.AX));
            var vpoint = new VisualPoint(0, i);
            if (0 > Field.X1 && 0 < Field.X2)
            {
                if (Field.AxisEllipsesVisible)
                    Field.Canvas.Children.Add(vpoint.GetEllipse(Brushes.Black));
                if (Field.AxisPointsVisible)
                    if (i != 0) { Field.Canvas.Children.Add(vpoint.GetVisualNumber(-i / Field.OneCmScale)); }
            }
        }
        Line GetLine(double i, Axis axis)
        {
            Line line = new Line() { Stroke = Brushes.White };
            if (i == 0)
                line.Stroke = Brushes.Black;
            else if (Field.GridVisible)
                line.Stroke = Brushes.Gray;
            if (axis == Axis.AY)
            {
                line.X1 = i - Field.X1;
                line.X2 = i - Field.X1;
                line.Y1 = 0;
                line.Y2 = Field.Canvas.Height;
            }
            else
            {
                line.X1 = 0;
                line.X2 = Field.Canvas.Width;
                line.Y1 = i - Field.Y1;
                line.Y2 = i - Field.Y1;
            }
            return line;
        }
        void DrawFunction()
        {
            Calculator[] calculators = InfoWorker.Calculators.ToArray();
            Polyline function = new Polyline() { Stroke = Brushes.Black };
            for (int i = calculators.Length - 1; i > 0; i--)
            {
                if (СheckBorder(calculators[i].X, calculators[i].Y))
                {
                    var vpoint = new VisualPoint(calculators[i].X * Field.OneCmScale, -calculators[i].Y * Field.OneCmScale);
                    function.Points.Add(vpoint.GetPointForFunction());
                    if (Field.FunctionPointsVisible)
                    {
                        Ellipse ellipse = vpoint.GetEllipse(Brushes.Red);
                        Field.Canvas.Children.Add(ellipse);
                    }
                    if (!CheckOnlyXBorder(calculators[i - 1].X)) //Если x выходит за границы, то мы обрываем проверку ост. x
                        break;
                }
            }
            Field.Canvas.Children.Add(function);
        }
        bool СheckBorder(double x, double y)
        {
            if (x * Field.OneCmScale > Field.X1 &&
                x * Field.OneCmScale < Field.X2 &&
               -y * Field.OneCmScale > Field.Y1 &&
               -y * Field.OneCmScale < Field.Y2)
                return true;
            else
                return false;
        }
        bool CheckOnlyXBorder(double x)
        {
            if (x * Field.OneCmScale > Field.X1 &&
                x * Field.OneCmScale < Field.X2)
                return true;
            else
                return false;
        }
    }
}