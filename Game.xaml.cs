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

        // ��������� ��������
        int AmountOfProducts = 5;   // ���������� ������� ����������
        int Cash = 2000;    // ������
        int SizeOfStorage = 10; // ������ ���������
        int Rent = 100; // ����� �� ������
        private int Price = 4;
        private int required = 0;

        int Mod;    // ����������� ����
        int ModPrevious;    // ����������� ���� ���������� ������ ��� �������������

        public Game(int difficulty) {

            InitializeComponent();
            
            DataContext = this;
            this.difficulty = difficulty;
            dt.Tick += new EventHandler(Dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);
            
            Update();   // Update ��������� �������� ��� �� ��������� �����������

            DataPoints = new ObservableCollection<DataPoint> {  // ��������� ����� ��� �������
                new DataPoint(0, 4) // ��������� ����� ������� � ������������ � �����
            };
        }
       	void Dt_Tick(object sender, EventArgs e) {

            if (sw.IsRunning)   // ���� �������� ����������
            {
                TimeSpan ts = sw.Elapsed;
                days = ts.Seconds;
                currentTime = $"{weeks} weeks {days} days"; // ��������� ����������� ������� � ������ ������
                
                if (ts.Seconds % 7 == 0 && ts.Seconds > 0) // ���� ���������� ����� While ��� ��������� ������������
                {
                    weeks += 1;
                    sw.Restart();
                    EndOfWeek();
                }
                Update();  
            }
        }

        private void Update()   // ���������� ���������� �� ������
        {
            //-----����� ���������� �� ������� ������-----
            storageinfo.Text = $"Products storaged: {AmountOfProducts} \nSize of Storage: {SizeOfStorage}\nRequirements: {required}"; 
            cashinfo.Text = $"Rent: {Rent}; Price: {Price*50}.";
            progressBar.Minimum = 0;                // �����������
            progressBar.Maximum = SizeOfStorage;    // �������������
            progressBar.Value = AmountOfProducts;   // ������
            
            slidersell.Maximum = AmountOfProducts;  // ������������ ��������� �������� ��� �������

            cashtxtblock.Text = $"$ {Cash}";    // ����� �������� ������� �� ������ ������
            clocktxtblock.Text = currentTime;   // ����� �������� ������� �� ������ ������

        }

        private void Startbtn_Click(object sender, RoutedEventArgs e) { // �� ������� ������ ����� ���������� ������� �������
            sw.Start();
            dt.Start();
        }

        private void Stopbtn_Click(object sender, RoutedEventArgs e) {  // ����� ������ �� �����, ��� ����� ����������� �����������
            if (sw.IsRunning) {
                sw.Stop();
            }
        }

        private void IncreaseSize(object sender, RoutedEventArgs e) {   // ���������� ������� ������
            if (SizeOfStorage < 40) {
                SizeOfStorage += 10;
                Rent += 150;
                Cash -= 200;
                rectanglestorage.Width += 80;
                Update();
            }
        }
        
        private void DecreaseSize(object sender, RoutedEventArgs e) {   // ���������� ������� ������
            if (SizeOfStorage > 10) {
                SizeOfStorage -= 10;
                if (AmountOfProducts > SizeOfStorage) AmountOfProducts = SizeOfStorage;
                Rent -= 150;
                Cash += 150;
                rectanglestorage.Width -= 80;
                Update();
            }
        }
        
        private void EndOfWeek() {
            Cash -= Rent;   // ������� �� ������
            Economy();  // ������ ��������� ��� ��������� ������
        }

        private void Buy(object sender, RoutedEventArgs e) {
            if (AmountOfProducts + (int)sliderincome.Value > SizeOfStorage) {
                sw.Stop();
                MessageBox.Show("��������� ������ ������. �� ����������.");
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
