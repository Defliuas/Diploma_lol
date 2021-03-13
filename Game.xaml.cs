using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using OxyPlot;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DiplomaLol.Annotations;

namespace DiplomaLol {
    public partial class Game : Page, INotifyPropertyChanged {

        private readonly DispatcherTimer dt = new DispatcherTimer();
        private readonly Stopwatch sw = new Stopwatch();
        private string currentTime = "0 weeks 0 days";
        private int days;
        private int weeks;

        private readonly int difficulty;

        #region Properties

        private ObservableCollection<DataPoint> dataPoints { get; set; }
        public ObservableCollection<DataPoint> DataPoints {
            get => dataPoints;
            set {
                dataPoints = value;
                OnPropertyChanged(nameof(DataPoints));
            }
        }

        private int maximumWeeksAxis { get; set; } = 1;

        public int MaximumWeeksAxis
        {
            get => maximumWeeksAxis;
            set
            {
                maximumWeeksAxis = value;
                OnPropertyChanged();
            }
        }
        #endregion

        // Стартовые значения
        int AmountOfProducts = 5;   // Количество товаров изначально
        int Cash = 2000;    // Баланс
        int SizeOfStorage = 10; // Размер хранилища
        int Rent = 100; // Плата за аренду
        private int Price = 4;
        private int required = 0;

        int Mod;    // Модификатор цены
        int ModPrevious;    // Модификатор цены предыдущей недели для псевдорандома

        public Game(int difficulty) {

            InitializeComponent();
            
            DataContext = this;
            this.difficulty = difficulty;
            dt.Tick += new EventHandler(Dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);
            
            Update();   // Update начальных значений для их исходного отображения

            DataPoints = new ObservableCollection<DataPoint> {  // Коллекция точек для графика
                new DataPoint(0, 4) // Начальная точка графика в соответствии с ценой
            };
        }
        void Dt_Tick(object sender, EventArgs e) {

            if (sw.IsRunning)   // Пока работает секундомер
            {
                TimeSpan ts = sw.Elapsed;
                days = ts.Seconds;
                currentTime = $"{weeks} weeks {days} days"; // Обновляем изображение времени с каждым тактом
                
                if (ts.Seconds % 7 == 0 && ts.Seconds > 0) // Круг выполнения цикла While для временной составляющей
                {
                    weeks += 1;
                    sw.Restart();
                    EndOfWeek();
                }
                Update();  
            }
        }

        private void Update()   // Обновление интерфейса на экране
        {
            //-----Вывод статистики на среднюю панель-----
            storageinfo.Text = $"Products storaged: {AmountOfProducts} \nSize of Storage: {SizeOfStorage}\nRequirements: {required}"; 
            cashinfo.Text = $"Rent: {Rent}; Price: {Price*50}.";
            progressBar.Minimum = 0;                // Иллюстрация
            progressBar.Maximum = SizeOfStorage;    // Наполненности
            progressBar.Value = AmountOfProducts;   // Склада
            
            slidersell.Maximum = AmountOfProducts;  // Максимальная дальность слайдера для продажи

            cashtxtblock.Text = $"$ {Cash}";    // Вывод текущего баланса на нижней панели
            clocktxtblock.Text = currentTime;   // Вывод текущего времени на нижней панели

        }

        private void Startbtn_Click(object sender, RoutedEventArgs e) { // По нажатию кнопки Старт начинается игровой процесс
            sw.Start();
            dt.Start();
        }

        private void Stopbtn_Click(object sender, RoutedEventArgs e) {  // Можно нажать на Паузу, что более корректного обдумывания
            if (sw.IsRunning) {
                sw.Stop();
            }
        }

        private void IncreaseSize(object sender, RoutedEventArgs e) {   // Увеличение размера склада
            if (SizeOfStorage < 40) {
                SizeOfStorage += 10;
                Rent *= 2;
                Cash -= 250;
                rectanglestorage.Width += 100;
            }
        }
        
        private void DecreaseSize(object sender, RoutedEventArgs e) {   // Уменьшение размера склада
            if (SizeOfStorage > 10) {
                SizeOfStorage -= 10;
                Rent /= 2;
                Cash += 150;
                rectanglestorage.Width -= 100;
            }
        }
        
        private void EndOfWeek() {
            Cash -= Rent;   // Расходы на аренду
            Economy();  // Расчет экономики для следующей недели
        }

        private void Buy(object sender, RoutedEventArgs e) {
            if (AmountOfProducts + (int)sliderincome.Value > SizeOfStorage) {
                sw.Stop();
                MessageBox.Show("Увеличьте размер склада. Не помещается.");
            } else {
                AmountOfProducts += (int)sliderincome.Value;
                Cash -= Price * 50 * (int)sliderincome.Value;
                Update();
            }
        }
        private void Sell(object sender, RoutedEventArgs e) {
            AmountOfProducts -= (int)slidersell.Value;
            Cash += Price * 50 * (int)slidersell.Value;
            required -= (int) slidersell.Value;
            Update();
        }



        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
