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

namespace DiplomaLol
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //DispatcherTimer gameTimer = new DispatcherTimer();

        //double money;
        //bool gameOver;
        public static int diff;

        public MainWindow()
        {
            InitializeComponent();
 
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            Game game = new Game();     // экземпляр Game.xaml для открытия во фрейме с игровой логикой
            var difficulty = new Difficulty();  // сложность, которую мы получаем из окна difficulty
            difficulty.ShowDialog();
            if (difficulty.DialogResult == true)
            {
                Name_frame.NavigationService.Navigate(game);
            }
            
        }

        private void CreditsClick(object sender, RoutedEventArgs e)
        {
            Credits c1 = new Credits();
            Name_frame.NavigationService.Navigate(c1);
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close(); // Закрытие окна
                                                    //      Environment.Exit(0);    // Используем это, т.к. при использовании this->Close или Application.Close - приложение остается в диспетчере   задач
        }
    }
}

// NavigationUIVisibility="Hidden" для frame