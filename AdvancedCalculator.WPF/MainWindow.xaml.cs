using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Field.SetCanvas(Canvas);
        }
        InfoWorker InfoWorker { get; set; }
        FunctionDrawer FunctionDrawer { get; set; } = default;
        Point StartPoint { get; set; }
        private void Center_Click(object sender, RoutedEventArgs e)
        {
            Field.X1 = -Canvas.Width / 2;
            Field.Y1 = -Canvas.Height / 2;
            FunctionDrawer.Draw();
        }
        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                InfoWorker = new InfoWorker(tbxExpression.Text, tbxRange.Text, tbxStep.Text);
                Table.ItemsSource = InfoWorker.Calculators;
                Table.Columns[2].Header = "RPN";
                FunctionDrawer = new FunctionDrawer(InfoWorker);
                FunctionDrawer.Draw();
                btnCenter.Visibility = Visibility.Visible;
                Center_Click(sender, e);
            }
            catch
            {
                MessageBox.Show("Неверное условие (Проверьте на наличие пробелов и скобочек)");
            }
        }
        private void Field_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StartPoint = Mouse.GetPosition(this);
        }
        private void Field_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var moveChange = Mouse.GetPosition(this) - StartPoint;
                Field.X1 -= moveChange.X;
                Field.Y1 -= moveChange.Y;
                FunctionDrawer.Draw();
                StartPoint = Mouse.GetPosition(this);
            }
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas.Width = this.ActualWidth;
            Canvas.Height = this.ActualHeight - (Row1.ActualHeight + Row2.ActualHeight + Row3.ActualHeight);
            if (InfoWorker != null) { FunctionDrawer.Draw(); }
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                Field.Scale += 5;
            else if (Field.Scale > 40)
                Field.Scale -= 5;
            if (InfoWorker != null) { FunctionDrawer.Draw(); }
        }

        private void FunctionPointVisible_Click(object sender, RoutedEventArgs e)
        {
            Field.FunctionPointVisible = (bool)FunctionPointVisible.IsChecked;
            if (InfoWorker != null) { FunctionDrawer.Draw(); }
        }
        private void PointVisible_Click(object sender, RoutedEventArgs e)
        {
            Field.PointVisible = (bool)PointVisible.IsChecked;
            if (InfoWorker != null) { FunctionDrawer.Draw(); }
        }

        private void SetScale_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double scale = double.Parse(tbScale.Text);
                if (scale <= 400 && scale >= 40)
                {
                    Field.Scale = double.Parse(tbScale.Text);
                    if (InfoWorker != null) { FunctionDrawer.Draw(); }
                }
                else
                    MessageBox.Show("Максимальный масштаб - 400%, минимальный - 40%");
            }
            catch
            {
                MessageBox.Show("Ошибка при выставлении масштаба");
            }
        }
    }
}