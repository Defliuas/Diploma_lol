using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OxyPlot;

namespace DiplomaLol
{
    public partial class Game
    {
        private void Economy()
        {
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

            MaximumWeeksAxis++;
            dataPoints.Add(new DataPoint(weeks, Price));

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
    }
}
