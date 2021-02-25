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
using System.Windows.Shapes;

namespace DiplomaLol
{
    /// <summary>
    /// Логика взаимодействия для difficulty.xaml
    /// </summary>
    public partial class Difficulty : Window
    {
        public Difficulty()
        {
            InitializeComponent();
        }

        private void Setup_difficult(object sender, RoutedEventArgs e)
        {
            if (radbut1.IsChecked == true)      MainWindow.diff = 2;
            else if (radbut2.IsChecked == true) MainWindow.diff= 4;
            else                                MainWindow.diff= 6;
            this.DialogResult = true;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}
