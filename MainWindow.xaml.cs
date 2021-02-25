using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiplomaLol {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        //DispatcherTimer gameTimer = new DispatcherTimer();

        //double money;
        //bool gameOver;

        public MainWindow() {
            InitializeComponent();

        }

        private void StartClick(object sender, RoutedEventArgs e) {
            var difficulty = new Difficulty();  // сложность, которую мы получаем из окна difficulty
            difficulty.ShowDialog();
            if (difficulty.DialogResult == true) {
                var game = new Game(difficulty.Difficulty1);
                Name_frame.NavigationService.Navigate(game);
            }

        }

        private void CreditsClick(object sender, RoutedEventArgs e) {
            var c1 = new Credits();
            Name_frame.NavigationService.Navigate(c1);
        }

        private void ExitClick(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown(); // Выход
        }
    }
}

// NavigationUIVisibility="Hidden" для frame