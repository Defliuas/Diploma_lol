using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace DiplomaLol {
    /// <summary>
    /// Логика взаимодействия для Game.xaml
    /// </summary>
    public partial class Game {
        readonly DispatcherTimer dt = new DispatcherTimer();
        readonly Stopwatch sw = new Stopwatch();
        string currentTime;
        int days;
        int weeks;

        private int dif;


        int AmountOfProducts = 5;   // Количество товаров
        int Cash = 1000;    // Баланс
        int SizeOfStorage = 10; // Размер хранилища
        int Rent = 100; // Плата за аренду
        int Mod;    // Модификатор цены
        int ModPrevious = 0;    // Модификатор цены предыдущей недели для псевдорандома

        private int Price = 4;
        public Game(int dif) {
            InitializeComponent();
            this.dif = dif;
            currentTime = "0 weeks 0 days";
            dt.Tick += Dt_Tick;
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);

        }

        void Dt_Tick(object sender, EventArgs e) {
            if (sw.IsRunning)   // Пока работает секундомер
            {
                var ts = sw.Elapsed;
                days = ts.Seconds;
                currentTime = $"{weeks} weeks {days} days";
                if (ts.Seconds % 7 == 0 && ts.Seconds > 0) // Круг выполнения цикла While
                {
                    weeks += 1;
                    sw.Restart();
                    EndOfWeek();
                }
                cashtxtblock.Text = $"${Cash}";
                clocktxtblock.Text = currentTime;   // Вывод времени на экран
                storageinfo.Text = $"Products storaged: {AmountOfProducts} Size of Storage: {SizeOfStorage}."; //нужно сделать update по клику
                cashinfo.Text = $"Rent: {Rent}; Price: {Price}."; //нужно сделать update по клику
                                                                  // привязка нахуй не всралась, я сделал по таймингу миллисекунд
                progressBar.Minimum = 0;                // Иллюстрация
                progressBar.Maximum = SizeOfStorage;    // Наполненности
                progressBar.Value = AmountOfProducts;   // Склада

                slidersell.Maximum = AmountOfProducts;
            }
        }

        private void Startbtn_Click(object sender, RoutedEventArgs e) {
            sw.Start();
            dt.Start();
        }

        private void Stopbtn_Click(object sender, RoutedEventArgs e) {
            if (sw.IsRunning) {
                sw.Stop();
            }
        }

        private void IncreaseSize(object sender, RoutedEventArgs e) {
            if (SizeOfStorage < 40) {
                SizeOfStorage += 10;
                Rent *= 2;
                Cash -= 250;
                rectanglestorage.Width += 100;
            }
        }

        private void DecreaseSize(object sender, RoutedEventArgs e) {
            if (SizeOfStorage > 10) {
                SizeOfStorage -= 10;
                Rent /= 2;
                Cash += 150;
                rectanglestorage.Width -= 100;
            }
        }

        private void EndOfWeek() {
            Cash -= Rent;
            var rand = new Random();    // Создание экземпляра класса Random для получения функция рандома
            Mod = rand.Next(-1 * dif, 1 * dif + 1); // Присвоение Модификатору случайного значения из диапазона
            while (Mod == ModPrevious)  // Пока в предыдущей неделе был такой же модификатор
            {
                Mod = rand.Next(-1 * dif, 1 * dif + 1); // Реролл
            }

            Price += Mod;   // Меняем цену на новую
            ModPrevious = Mod;  // Перезаписываем Модификатор для следующего цикла

            if (Price < 0)  // Проверка на отрицательную стоимость
            {
                Price = 1;
            }

            if (Cash < -1000)   // Условие проигрыша
            {
                MessageBox.Show("Вы проиграли.");
            }
        }

        private void Buy(object sender, RoutedEventArgs e) {
            if (AmountOfProducts + (int)sliderincome.Value > SizeOfStorage) {
                MessageBox.Show("Увеличьте размер склада. Не помещается.");
            } else {
                AmountOfProducts += (int)sliderincome.Value;
            }

            Cash -= Price * 50 * (int)sliderincome.Value;

        }

        private void Sell(object sender, RoutedEventArgs e) {
            AmountOfProducts -= (int)slidersell.Value;

            Cash += Price * 50 * (int)slidersell.Value;
        }
    }
}
