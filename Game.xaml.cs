using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using OxyPlot;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DiplomaLol
{

    class MainViewModel
    {
        public MainViewModel()
        {      
           var GraphPoints = new ObservableCollection<DataPoint>
            {
                new DataPoint(0, 4),
                new DataPoint(10, 13),
                new DataPoint(20, 15),
                new DataPoint(30, 16),
                new DataPoint(40, 12),
                new DataPoint(50, 12)
            };
        }

        public string Title { get; private set; }
        public IList<DataPoint> GraphPoints { get; set; }
    }

    public partial class Game : Page
    {
        readonly DispatcherTimer dt = new DispatcherTimer();
        readonly Stopwatch sw = new Stopwatch();
        string currentTime;
        int days;
        int weeks;

        private int difficulty;

        int AmountOfProducts = 5;   // Количество товаров
        int Cash = 2000;    // Баланс
        int SizeOfStorage = 10; // Размер хранилища
        int Rent = 100; // Плата за аренду
        int Mod;    // Модификатор цены
        int ModPrevious;    // Модификатор цены предыдущей недели для псевдорандома
        private int Price = 4;

         ObservableCollection <MainViewModel> graphPoints= new ObservableCollection<MainViewModel>();   // создаю лист из класса ебаная хуйня блять
        
      //  GraphPoints = new ObservableCollection<int,int> {}

        public Game(int difficulty)
        {
            InitializeComponent();
            this.difficulty = difficulty;
            currentTime = "0 weeks 0 days";
            dt.Tick += new EventHandler(Dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);

            clocktxtblock.Text = "0 weeks 0 days";
            cashtxtblock.Text = $"$ {Cash}";
            progressBar.Minimum = 0;                // Иллюстрация
            progressBar.Maximum = SizeOfStorage;    // Наполненности
            progressBar.Value = AmountOfProducts;   // Склада
            storageinfo.Text = $"Products storaged: {AmountOfProducts} \nSize of Storage: {SizeOfStorage}.";
            cashinfo.Text = $"Rent: {Rent}; Price: {Price}.";

        }
        void Dt_Tick(object sender, EventArgs e)
        {
            if (sw.IsRunning)   // Пока работает секундомер
            {
                TimeSpan ts = sw.Elapsed;
                days = ts.Seconds;
                currentTime = $"{weeks} weeks {days} days";
                if (ts.Seconds % 7 == 0 && ts.Seconds >0) // Круг выполнения цикла While
                {
                    weeks += 1;
                    sw.Restart();
                    EndOfWeek();
                }
                cashtxtblock.Text = $"$ {Cash}";
                clocktxtblock.Text = currentTime;   // Вывод времени на экран
                storageinfo.Text = $"Products storaged: {AmountOfProducts} \nSize of Storage: {SizeOfStorage}."; //нужно сделать update по клику
                cashinfo.Text = $"Rent: {Rent}; Price: {Price}."; //нужно сделать update по клику
                                                                // привязка нахуй не всралась, я сделал по таймингу миллисекунд
                progressBar.Minimum = 0;                // Иллюстрация
                progressBar.Maximum = SizeOfStorage;    // Наполненности
                progressBar.Value = AmountOfProducts;   // Склада
                
                slidersell.Maximum = AmountOfProducts;
                //   plotGraph
                MainViewModel.GraphPoints.Add(new DataPoint(days, Price));
            }
        }

        private void Startbtn_Click(object sender, RoutedEventArgs e)
        {
            sw.Start();
            dt.Start();
        }
        private void Stopbtn_Click(object sender, RoutedEventArgs e)
        {
            if (sw.IsRunning)
            {
                sw.Stop();
            }
        }
        private void IncreaseSize(object sender, RoutedEventArgs e)
        {
            if (SizeOfStorage < 40)
            {
                SizeOfStorage += 10;
                Rent *= 2;
                Cash -= 250;
                rectanglestorage.Width += 100;
            }
        }
        private void DecreaseSize(object sender, RoutedEventArgs e)
        {
            if (SizeOfStorage > 10)
            {
                SizeOfStorage -= 10;
                Rent /= 2;
                Cash += 150;
                rectanglestorage.Width -= 100;
            }
        }
        private void EndOfWeek()
        {
            Cash -= Rent;   // Расходы на аренду

            var rand = new Random();    // Создание экземпляра класса Random для получения функция рандома
            Mod = rand.Next(-1 * difficulty, 1 * difficulty + 1); // Присвоение Модификатору случайного значения из диапазона
           
            while (Mod == ModPrevious)  // Пока в предыдущей неделе был такой же модификатор
            {
                Mod = rand.Next(-1 * difficulty, 1 * difficulty + 1); // Реролл
            }

            Price += Mod;   // Меняем цену на новую
            ModPrevious = Mod;  // Перезаписываем Модификатор для следующего цикла

            if (weeks % 8 == 0)
            {
                Price += 2;
            }

            if (weeks % 16 == 0)
            {
                Price /= 2;
            }

            if (Price <= 0)  // Проверка на отрицательную стоимость
            {
                Price = 1;
                ModPrevious = 1;
            }

            if (Cash < -1000)   // Условие проигрыша
            {
                sw.Stop();
                MessageBox.Show("Вы проиграли.");
            }

            if (Cash >= 8000) // Условие победы
            {
                sw.Stop();
                MessageBox.Show("Поздравляю. Вы справились с задачей.");
            }
        }

        private void Buy(object sender, RoutedEventArgs e)
        {
            if (AmountOfProducts + (int)sliderincome.Value > SizeOfStorage)
            {
                MessageBox.Show("Увеличьте размер склада. Не помещается.");
            }
            else
            {
                AmountOfProducts += (int) sliderincome.Value;
                Cash -= Price * 50 * (int) sliderincome.Value;
            }
        }
        private void Sell(object sender, RoutedEventArgs e)
        {
            AmountOfProducts -= (int)slidersell.Value;
            Cash += Price * 50 * (int)slidersell.Value;
        }
    }
}
