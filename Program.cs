using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThermalConductivity
{
    class Program
    {
        /// <summary>
        /// Программа для уравнения теплопроводности(1 порядок краевых условий)
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string stroka; //строковая переменная для записи
            int tempCPU = 500; //температура процессора
            int thickness = 50; //Толщина
            int tempOut = 20; //температура снаружи
            int time = 5; //время
            double coefA = 200; // коэф
            int rodUslovya; //переменная для рода условий
            double x; //ввод x
            double y; //ввод y
            int NSetka = 0; //переменная для шагов по сетке
            int MSetka = 0; //переменная для шагов по сетке
            Console.WriteLine("Уравнение теплопроводности имеет вид: d*U/d*t = (A * (d^2 * U)/(d * x^2))");
            #region Ввод Данных
            Console.Write("Введите температуру процессора: ");  //вывод запроса на экран
            stroka = Console.ReadLine();                        //считывание строки вводимой пользователем
            tempCPU = Convert.ToInt32(stroka);                  //конвертация

            Console.Write("Введите толщину стенки радиатора: ");//вывод запроса на экран
            stroka = Console.ReadLine();                        //считывание строки вводимой пользователем
            thickness = Convert.ToInt32(stroka);                //конвертация
            int pole = thickness / 10;

            Console.Write("Введите температуру снаружи: ");//вывод запроса на экран
            stroka = Console.ReadLine();                   //считывание строки вводимой пользователем
            tempOut = Convert.ToInt32(stroka);             //конвертация

            Console.Write("Введите время: ");//вывод запроса на экран
            stroka = Console.ReadLine();     //считывание строки вводимой пользователем
            time = Convert.ToInt32(stroka);  //конвертация

            Console.Write("Введите коэффициент A: ");//вывод запроса на экран
            stroka = Console.ReadLine();             //считывание строки вводимой пользователем
            coefA = Convert.ToDouble(stroka);         //конвертация

        Restart: //метка для рестарта если число x или y не подходят по условиям
            Console.Write("Введите x: ");//вывод запроса на экран
            stroka = Console.ReadLine(); //считывание строки вводимой пользователем
            x = Convert.ToDouble(stroka);//конвертация

            Console.WriteLine("Выберите t: 0, 0.02. 0.05, 0.1 ");//вывод запроса на экран
            stroka = Console.ReadLine();                         //считывание строки вводимой пользователем
            y = Convert.ToDouble(stroka);                        //конвертация

            //условие проверки x и y
            if (x < 0 || x > 1 || y < 0 || y > 1)
            {
                Console.WriteLine("x и t должны быть больше 0 и меньше 1");
                goto Restart; //отправляем на повторный запрос если условия не подходят
            }

            //Label:
            Console.Write("Введите N по пространству: ");//вывод запроса на экран
            stroka = Console.ReadLine();                 //считывание строки вводимой пользователем
            NSetka = Convert.ToInt32(stroka);            //конвертация
            Console.Write("Введите M по времени: ");//вывод запроса на экран
            stroka = Console.ReadLine();            //считывание строки вводимой пользователем
            MSetka = Convert.ToInt32(stroka);       //конвертация


            #endregion Ввод Данных
            //число курента
          //  Cu = coefA * (stepByTime / (stepBySpace * stepBySpace));
          //  if (Cu == 0.5 || Cu > 0.5)
          //  {
          //      Console.WriteLine("Число вышло за пределы, измените шаг");
          //      goto Label; //переход на Label если курент больше 0.5
          //  }
          //  Console.WriteLine("Число курента: {0}", Cu);

            #region Доп Условия
            Console.Write("Введите какого рода условие: ");
            stroka = Console.ReadLine(); //считывание рода условия
            rodUslovya = Convert.ToInt32(stroka); // конвертор рода условия
            float[,] massivTemp = new float[120, 120]; //переменная для массива температур

            //инициализация исход массива
            for (int j = 0; j != MSetka; j++)
            {
                massivTemp[0, j] = 0;
            }
            for (int i = 0; i != NSetka; i++)
            {
                massivTemp[i, 0] = (float)Math.Pow(Math.E, -20 * Math.Pow(x - 0.5, 2)) - (float)Math.Pow(Math.E, -20 * Math.Pow(x - 1.5, 2)) - (float)Math.Pow(Math.E, -20 * Math.Pow(x + 0.5, 2));
            }
            massivTemp[0, 0] = 0;


            for (int n = 0; n < MSetka; n++)
            {
                for (int i = 1; i < NSetka; i++)
                {
                    //решение метода
                    massivTemp[i, n + 1] = (float)(1 / Math.Sqrt(1 + 80 * y)) * (float)(Math.Pow(Math.E, -20 * Math.Pow(x - 0.5, 2) / (1 + 80 * y))) - (float)(Math.Pow(Math.E, -20 * Math.Pow(x - 1.5, 2) / (1 + 80 * y))) - (float)(Math.Pow(Math.E, -20 * Math.Pow(x + 0.5, 2) / (1 + 80 * y)));

                }
            }
            Console.WriteLine("Краевые условия {0} рода", rodUslovya);
            switch (rodUslovya) //свитч для выбора рода условий
            {
                case 1:
                    {
                        //первый род краев. условий и вызов метода
                        FirstRod();
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Такого рода условия нету");
                        break;
                    }
            }
            #endregion Доп Условия
            //метод вызова условий первого рода
            void FirstRod()
            {
                for (int n = 1; n < MSetka; n++)
                {
                    for (int i = 1; i < NSetka; i++)
                    {
                        //решение метода
                        // massivTemp[i, n + 1] = massivTemp[i, n] + (stepByTime * coefA * ((massivTemp[i + 1, n] - 2 * massivTemp[i, n] + massivTemp[i - 1, n]) / (stepBySpace * stepBySpace))) + F;
                        //massivTemp[i, n + 1] = ((-2 * coefA * (massivTemp[i, n + 1] - massivTemp[i, n - 1])) / (NSetka * 2 * MSetka)) + massivTemp[i - 1, n] / 2 + massivTemp[i + 1, n] / 2;
                        massivTemp[i, n] = (float)(massivTemp[i,n-1] - coefA * MSetka * ((massivTemp[i-1,n]-2*massivTemp[i,n]+massivTemp[i+1,n])/(2*Math.Pow(NSetka,2)*MSetka)) -coefA * MSetka * ((massivTemp[i - 1, n+1] - 2 * massivTemp[i, n+1] + massivTemp[i + 1, n+1]) / (2 * Math.Pow(NSetka, 2)*MSetka)));
                    }
                }

                for (int j = MSetka; j != 0; j--)
                {
                    Console.Write("|" + " ");

                    for (int i = 1; i < NSetka; i++)
                    {
                        //massivTemp[i, j] = Convert.ToInt32(massivTemp[i, j]);
                        Console.Write(massivTemp[i, j] + "\t" + "|");
                        //workSheet.Cells[i+1, j+1] = massivResult[i,j];
                    }
                    Console.WriteLine();
                    Console.Write(new string('-', NSetka * 5));
                    Console.WriteLine();
                }
                Console.Write("|" + " ");

             //   for (int i = 0; i < NSetka; i++)
             //   {
             //       Console.Write("{0}", massivTemp[i, 0] + "\t" + "|");
             //       //   workSheet.Cells[i+1, 1] = massivResult[i, 0];
             //   }
             //   Console.WriteLine();
             //   Console.Write(new string('-', NSetka * 5));


            }

            Console.ReadKey(true);
        }
    }
}
