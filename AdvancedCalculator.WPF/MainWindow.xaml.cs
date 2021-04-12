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
        }
        InfoWorker InfoWorker { get; set; }
        FunctionDrawer FunctionDrawer { get; set; } = default;
        Point StartPoint { get; set; }
        Thickness StartMargin { get; set; }
        private void Center_Click(object sender, RoutedEventArgs e)
        {
            Field.Margin = new Thickness(-(Field.Width / 2 - svField.ActualWidth / 2), -(Field.Height / 2 - svField.ActualHeight / 2), 0, 0);
            FunctionDrawer.Window.X1 = (Field.Width / 2 - svField.ActualWidth / 2);
            FunctionDrawer.Window.Y1 = Field.Height / 2 - svField.ActualHeight / 2;
            FunctionDrawer.Draw();
        }
        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            try 
            { 
                InfoWorker = new InfoWorker(tbxExpression.Text, tbxRange.Text, tbxStep.Text);
                Table.ItemsSource = InfoWorker.Calculators;
                Table.Columns.Remove(Table.Columns[^1]);
                Table.Columns[1].Header = "RPN";
                FunctionDrawer = new FunctionDrawer(Field, InfoWorker, svField);
                FunctionDrawer.Draw();
                btnCenter.Visibility = Visibility.Visible;
                Field.Margin = new Thickness(0, 0, 0, 0); ;
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
            StartMargin = Field.Margin;
        }
        private void Field_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var MoveChange = Mouse.GetPosition(this) - StartPoint;
                var endMargin = new Thickness(StartMargin.Left, StartMargin.Top, 0, 0);
                if (FunctionDrawer.Window.X1 - MoveChange.X > 0 && FunctionDrawer.Window.X1 + svField.ActualWidth - MoveChange.X < Field.ActualWidth)
                {
                    FunctionDrawer.Window.X1 -= MoveChange.X;
                    endMargin.Left += MoveChange.X;
                }
                if (FunctionDrawer.Window.Y1 - MoveChange.Y > 0 && FunctionDrawer.Window.Y1 + svField.ActualHeight - MoveChange.Y < Field.ActualHeight)
                {
                    FunctionDrawer.Window.Y1 -= MoveChange.Y;
                    endMargin.Top += MoveChange.Y;
                }
                Field.Margin = endMargin;
                FunctionDrawer.Draw();
                StartPoint = Mouse.GetPosition(this);
                StartMargin = Field.Margin;
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (FunctionDrawer != null)
                FunctionDrawer.Draw();
        }

        private void svField_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            FunctionDrawer.Window.Scale++;
            FunctionDrawer.SetField();
            FunctionDrawer.SetElements();
            FunctionDrawer.Draw();
        }
    }
}