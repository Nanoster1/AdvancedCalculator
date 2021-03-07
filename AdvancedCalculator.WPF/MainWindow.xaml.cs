using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AdvancedCalculator.Logic;

namespace AdvancedCalculator.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DrawOX();
            DrawOY();
            DrawNumsX();
            DrawNumsY();
            DrawGrid();
            DrawPointsX();
            DrawPointsY();
        }

        private void Center_Click(object sender, RoutedEventArgs e)
        {
            svField.ScrollToVerticalOffset(Field.Height/2 - Window.ActualHeight/2);
            svField.ScrollToHorizontalOffset(Field.Width/2 - Window.ActualWidth / 2);
        }
        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            tbX.Text = "X:";
            tbAnswer.Text = "Ответ:";
            tbRPN.Text = "Обратная польская запись:";
            StringBuilder x = new StringBuilder(tbX.Text);
            StringBuilder answer = new StringBuilder(tbAnswer.Text);
            StringBuilder rpn = new StringBuilder(tbRPN.Text);
            try
            {
                InfoWorker infoWorker = new InfoWorker(tbxExpression.Text, tbxRange.Text, tbxStep.Text);
                for (int i = 0; i < infoWorker.Calculators.Count; i++)
                {
                    x.Append($"\n{infoWorker.Calculators[i].X}");
                    answer.Append($"\n{infoWorker.Calculators[i].Answer}");
                    rpn.Append($"\n{infoWorker.Calculators[i].RPNStr}");
                }
                tbX.Text = x.Append("   ").ToString();
                tbAnswer.Text = answer.Append("   ").ToString();
                tbRPN.Text = rpn.Append("   ").ToString();
                Draw(infoWorker);
                btnCenter.Visibility = Visibility.Visible;
                Center_Click(sender, e);
            }
            catch 
            {
                tbAnswer.Text = "";
                tbRPN.Text = ""; 
                tbX.Text = "Некорректные данные"; 
            }
        }
        private void Draw(InfoWorker infoWorker)
        {
            Field.Children.Remove(Field.Children[^1]);           
            DrawFunction(infoWorker);
        }
        private void DrawOY()
        {
            Line oyLine = new Line();
            oyLine.X1 = Field.Width / 2;
            oyLine.Y1 = 0;
            oyLine.X2 = Field.Width / 2;
            oyLine.Y2 = Field.Height;
            oyLine.Stroke = Brushes.Black;
            Field.Children.Add(oyLine);
        }
        private void DrawOX()
        {
            Field.Width = 10000;
            Field.Height = 10000;
            Line oxLine = new Line();
            oxLine.X1 = 0;
            oxLine.Y1 = Field.Height / 2;
            oxLine.X2 = Field.Width;
            oxLine.Y2 = Field.Height / 2;
            oxLine.Stroke = Brushes.Black;
            Field.Children.Add(oxLine);
        }
        private void DrawFunction(InfoWorker infoWorker)
        {
            Polyline polyline = new Polyline();
            polyline.Points = new PointCollection();
            for (int i = 0; i < infoWorker.Calculators.Count; i++)
            {
                polyline.Points.Add(new Point(5000 + 40 * double.Parse(infoWorker.Calculators[i].X), 5000 - 40 * infoWorker.Calculators[i].Answer));
            }
            polyline.Stroke = Brushes.Black;
            Field.Children.Add(polyline);
        }
        private void DrawPointsY()
        {          
            for (int i = 0; i <= 250; i++)
            {
                Ellipse ellipseY = new Ellipse();  
                ellipseY.Width = 6;
                ellipseY.Height = 6;
                ellipseY.Stroke = Brushes.Black;
                Canvas.SetLeft(ellipseY, Field.Width / 2 - 3);
                Canvas.SetTop(ellipseY, i * 40 - 3);
                ellipseY.Fill = Brushes.Black;
                Field.Children.Add(ellipseY);
            }
        }
        private void DrawPointsX()
        {
            for (int i = 0; i <= 250; i++)
            {
                Ellipse ellipseX = new Ellipse();
                ellipseX.Width = 6;
                ellipseX.Height = 6;
                ellipseX.Stroke = Brushes.Black;
                Canvas.SetLeft(ellipseX, i * 40 - 3);
                Canvas.SetTop(ellipseX, Field.Height / 2 - 3);
                ellipseX.Fill = Brushes.Black;
                Field.Children.Add(ellipseX);
            }
        }
        private void DrawNumsY()
        {
            for (int i = -125; i <= 125; i++) 
            { 
                Label numY = new Label();               
                numY.Content = $"{i}";
                Canvas.SetLeft(numY, Field.Width / 2 + 6);
                Canvas.SetTop(numY, (i + 125) * 40 - 15);
                Field.Children.Add(numY);
            }
        }
        private void DrawNumsX()
        {
            for (int i = -125; i <= 125; i++)
            {
                Label numX = new Label();
                numX.Content = $"{i}";
                Canvas.SetLeft(numX, (i + 125) * 40 - 8);
                Canvas.SetTop(numX, Field.Height / 2 + 6);
                Field.Children.Add(numX);
            }
        }
        private void DrawGrid()
        {
            for (int i = 0; i < 250; i++)
            {
                Line lineX = new Line();
                Line lineY = new Line();
                lineX.X1 = 0;
                lineX.Y1 = i * 40;
                lineX.X2 = Field.Width;
                lineX.Y2 = i * 40;
                lineY.X1 = i * 40;
                lineY.Y1 = 0;
                lineY.X2 = i * 40;
                lineY.Y2 = Field.Height;
                lineX.Stroke = Brushes.Gray;
                lineY.Stroke = Brushes.Gray;
                Field.Children.Add(lineX);
                Field.Children.Add(lineY);
            }
        }
    }
}
