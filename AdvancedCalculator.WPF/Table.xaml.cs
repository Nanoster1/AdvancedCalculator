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
            dataGrid.ItemsSource = infoWorker.Calculators;

        }
    }
}