using System.Windows;

namespace DiplomaLol
{

    public partial class Difficulty : Window
    {
        public int Diff { get; set; }
        public Difficulty()
        {
            InitializeComponent();
        }

        private void Setup_difficult(object sender, RoutedEventArgs e)
        {
            if (radbut1.IsChecked == true)
            {
                Diff = 2;
            }
            else if (radbut2.IsChecked == true)
            {
                Diff = 4;
            }
            else
            {
                Diff = 6;
            }
            DialogResult = true;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

    }
}
