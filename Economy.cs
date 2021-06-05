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

            while (Mod == 0 && ModPrevious == 0)  // Пока в предыдущей неделе был такой же модификатор
            {
                Mod = rand.Next(-1 * difficulty, 1 * difficulty + 1); // Реролл
            }

            Price += Mod;   // Меняем цену на новую
            ModPrevious = Mod;  // Перезаписываем Модификатор для следующего цикла

            if (required < -1)
            {
                Price -= 2; // Если продали больше требуемых товаров - переполнение рынка - снижение цены
            } else if (required == 0)
            {
                Price += 1; // Если продали идеально, то награждается увеличением стоимости на 1
            }
            else if (required > 0)
            {
                Price += rand.Next(-1, 2); // Назначается случайный штраф, который равен либо повышению цены из-за недостатка товаров
                // Или штраф за неудовлетворение потребностей, который снижает на 1 ценность
            }

            required = rand.Next(0, 5);

            if (weeks % 16 == 0)
            {
                Price /= 2;
            } 
            else if (weeks % 8 == 0)
            {
                Price -= Mod;
                Price += 3;
            }

            if (Price <= 0)  // Проверка на отрицательную стоимость
            {
                Price = 3;
                ModPrevious = 0;
            }
            
            MaximumWeeksAxis++;
            dataPoints.Add(new DataPoint(weeks, Price));

            if (Cash < -1000)   // Условие проигрыша
            {
                sw.Stop();
                MessageBox.Show("Вы проиграли.");
                this.Visibility = Visibility.Collapsed;
            }

            if (Cash >= 2000) // Условие победы
            {
                sw.Stop();
                MessageBox.Show("Поздравляю. Вы справились с задачей. \nНажмите Ок, чтобы закрыть приложение");
                this.Visibility = Visibility.Collapsed;
                Application.Current.Shutdown();
            }
        }
    }
}
