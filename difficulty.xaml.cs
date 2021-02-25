using System.Windows;

namespace DiplomaLol {

    public partial class Difficulty : Window {
        public int Difficulty1 { get; set; }
        public Difficulty() {
            InitializeComponent();
        }

        private void Setup_difficult(object sender, RoutedEventArgs e) {
            if (radbut1.IsChecked == true) {
                Difficulty1 = 2;
            } else if (radbut2.IsChecked == true) {
                Difficulty1 = 4;
            } else {
                Difficulty1 = 6;
            }
            DialogResult = true;
        }

        private void Back_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }

    }
}
