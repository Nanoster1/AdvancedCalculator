using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        public InfoWorker InfoWorker { get; set; }
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
                if (InfoWorker != null) { FunctionDrawer.Draw(); }
                StartPoint = Mouse.GetPosition(this);
            }
            SetLblCoords();
        }
        private void SetLblCoords()
        {
            lblCoords.Content =
                $"X: {Math.Round((Field.X1 + Mouse.GetPosition(Canvas).X) / Field.OneCmScale, 2)} " +
                $"Y: {Math.Round(-(Field.Y1 + Mouse.GetPosition(Canvas).Y) / Field.OneCmScale, 2)}";
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas.Width = this.ActualWidth;
            Canvas.Height = this.ActualHeight - (Row1.ActualHeight + Row2.ActualHeight + Row3.ActualHeight);
            if (InfoWorker != null) { FunctionDrawer.Draw(); }
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (InfoWorker != null)
            {
                double startX = (Field.X1 + Canvas.Width / 2) / Field.Scale;
                double startY = (Field.Y1 + Canvas.Height / 2) / Field.Scale;
                if (e.Delta > 0)
                    Field.Scale += 5;
                else if (Field.Scale > 40)
                    Field.Scale -= 5;
                Field.X1 = startX * Field.Scale - Canvas.Width / 2;
                Field.Y1 = startY * Field.Scale - Canvas.Height / 2;
                FunctionDrawer.Draw();
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Field.FunctionPointsVisible = (bool)FunctionPointsVisible.IsChecked;
            Field.AxisPointsVisible = (bool)AxisPointsVisible.IsChecked;
            Field.GridVisible = (bool)GridVisible.IsChecked;
            Field.AxisEllipsesVisible = (bool)AxisEllipsesVisible.IsChecked;
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