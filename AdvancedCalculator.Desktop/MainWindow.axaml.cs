using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AdvancedCalculator.Logic;
using Avalonia.Interactivity;
using System.Text;

namespace AdvancedCalculator.Desktop
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.AttachDevTools();   
        }
        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            var tbX = this.FindControl<TextBlock>("tbX");
            var tbAnswer = this.FindControl<TextBlock>("tbAnswer");
            var tbRPN = this.FindControl<TextBlock>("tbRPN");
            var tbxExpression = this.FindControl<TextBox>("Expression");
            var tbxRange = this.FindControl<TextBox>("Range");
            var tbxStep = this.FindControl<TextBox>("Step");
            string step = tbxStep.Text;
            string expression = tbxExpression.Text;
            string range = tbxRange.Text;
            tbX.IsVisible = true;
            tbAnswer.IsVisible = true;
            tbRPN.IsVisible = true;
            tbX.Text = "X:";
            tbAnswer.Text = "Answer:";
            tbRPN.Text = "RPN: ";
            StringBuilder x = new StringBuilder(tbX.Text);
            StringBuilder answer = new StringBuilder(tbAnswer.Text);
            StringBuilder rpn = new StringBuilder(tbRPN.Text);
            try
            {
                InfoWorker infoWorker = new InfoWorker(expression, range, step);
                for (int i = 0; i < infoWorker.Calculators.Count; i++)
                {
                    x.Append($"\n{infoWorker.Calculators[i].X}");
                    answer.Append($"\n{infoWorker.Calculators[i].Answer}");
                    rpn.Append($"\n{infoWorker.Calculators[i].RPNStr}");
                }
                tbX.Text = x.ToString();
                tbAnswer.Text = answer.ToString();
                tbRPN.Text = rpn.ToString();
            }
            catch { tbX.Text = "Некорректные данные"; }
        }


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
