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
            Field.Children.Clear();
            DrawOX();
            DrawOY();
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
            Field.Width = 100000;
            Field.Height = 100000;
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
                polyline.Points.Add(new Point(50000 + 20 * double.Parse(infoWorker.Calculators[i].X), 50000 - 20 * infoWorker.Calculators[i].Answer));
            }
            polyline.Stroke = Brushes.Black;
            Field.Children.Add(polyline);
        }
    }
}
