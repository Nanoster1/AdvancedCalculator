using AdvancedCalculator.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdvancedCalculator.WPF
{
    /// <summary>
    /// Логика взаимодействия для Table.xaml
    /// </summary>
    public partial class Table : Window
    {
        public Table(InfoWorker infoWorker)
        {
            InitializeComponent();
            calculators = infoWorker.Calculators;
            SetItemsSourse();
        }
        private readonly List<Calculator> calculators;
        private int Start { get; set; } = 0;
        private int End { get { return Start + 10; } }
        private void Table_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (Start > 0) Start--;
                SetItemsSourse();
            }
            else
            {
                if (Start < calculators.Count - 10) Start++;
                SetItemsSourse();
            }
        }
        private void SetItemsSourse()
        {
            List<Calculator> itemsSourse = new List<Calculator>();
            for (int i = Start; i < End; i++)
            {
                itemsSourse.Add(calculators[i]);
            }
            dataGrid.ItemsSource = itemsSourse;
        }
    }
}
