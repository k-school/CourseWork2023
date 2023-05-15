using System;

namespace SystemOfEquations
{
    class Program
    {
        static void Main(string[] args)
        {
            //Ввод количества неизвестных 
            Console.WriteLine("Подскажите, сколько неизвестных в системе уравнений?");
            int n;
            Enter:
            String entered = Console.ReadLine();
            //Проверка введенного значения
            bool canConvert = int.TryParse(entered, out n);
            if (canConvert == false)
            {
                Console.WriteLine("Введено некорректное значение. Пожалуйста, попробуйте снова.");
                goto Enter;
            }
            if (n < 2 || n > 15)
            {
                Console.WriteLine("Введено слишком малое или слишком большое значение. Пожалуйста, попробуйте снова.");
                goto Enter;
            }
            //Создание двумерного массива для хранения коэффициентов и свободных членов
            int[,] a = new int[n + 1, n];
            //Ввод коэффициентов и свободных членов и их запись в массив
            Console.WriteLine("Пожалуйста, введите коэффициенты при неизвестных и свободный член для каждого уравнения системы.");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine((i + 1) + " уравнение:");
                for (int j = 0; j < n + 1; j++)
                {
                    Enterk:
                    String enteredk = Console.ReadLine();
                    canConvert = int.TryParse(enteredk, out a[j, i]);
                    if (canConvert == false)
                    {
                        Console.WriteLine("Введено некорректное значение. Пожалуйста, попробуйте снова.");
                        goto Enterk;
                    }
                }
            }
            //Вывод полученной системы с проверкой коэффициентов на ноль, единицу и минус единницу для правильного отображения при выводе
            Console.WriteLine("\nВаша система:");
            for (int i = 0; i < n; i++)
            {
                //Вывод x1
                if (a[0, i] == 1)
                    Console.Write("X1");
                else if (a[0, i] == -1)
                    Console.Write("-X1");
                else if (a[0, i] > 1 || a[0, i] < -1)
                    Console.Write(a[0, i] + "X1");
                //Вывод остальных неизвестных (со знаками)
                for (int j = 1; j < n; j++)
                {
                    if (a[j - 1, i] == 0)
                    {
                        if (a[j, i] == 1)
                            Console.Write(" + " + "X" + (j + 1));
                        else if (a[j, i] == -1)
                            Console.Write(" - " + "X" + (j + 1));
                        else if (a[j, i] > 1 || a[j, i] < -1)
                            Console.Write(a[j, i] + "X" + (j + 1));
                    }
                    else
                    {
                        if (a[j, i] == 1)
                            Console.Write(" + " + "X" + (j + 1));
                        else if (a[j, i] == -1)
                            Console.Write(" - " + "X" + (j + 1));
                        else if (a[j, i] < 0)
                            Console.Write(" - " + Math.Abs(a[j, i]) + "X" + (j + 1));
                        else if (a[j, i] > 1)
                            Console.Write(" + " + a[j, i] + "X" + (j + 1));
                    }

                }
                //Вывод свободного члена
                Console.Write(" = " + a[n, i] + " \n");
            }
            //Проверка условия (например, в 1 уравнении, модуль коэффициента a11 должен быть больше суммы остальных коэффициентов при неизвестных; во 2 уравнении – коэффициент a22, и т.д.)
            Console.WriteLine("\nНеобходимо проверить условие. Ожидайте, пожалуйста...");
            for (int i = 0; i < n; i++)
            {
                int sum = 0; //сумма остальных коэффициентов уравнения
                for (int j = 1; j < n; j++)
                    sum += Math.Abs(a[j, i]);
                sum -= Math.Abs(a[i, i]);
                if (Math.Abs(a[i, i]) <= sum) //если условие не выполняется, завершаем программу
                {
                    Console.WriteLine("Условие частично или полностью не выполняется. Сожалеем, нельзя решить данную систему методом простой итерации.");
                    Environment.Exit(0);
                }
            }
            Console.WriteLine("Условие выполняется. Переходим к решению системы методом простой итерации.");
            //Создание массива для хранения приближений
            double[] p = new double[n];
            double[] buff = new double[n];
            double[] delta = new double[n]; //массив для хранения погрешностей
            //Расчёт нулевого приближения
            Console.WriteLine("\nРассчитываем приближения...\nНулевое приближение:    ");
            for (int i = 0; i < n; i++)
            {
                p[i] = a[n, i] / a[i, i];
                Console.Write("X" + (i + 1) + " = " + Math.Round(p[i], 5) + "    ");
            }
            //Расчёт приближений (15)
            for (int k = 0; k < 16; k++)
            {
                Console.Write("\n" + k + " приближение:    ");
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                        buff[i] -= a[j, i] * p[j];
                    buff[i] += a[i, i] * p[i] + a[n, i];
                    buff[i] /= a[i, i];
                }
                for (int j = 0; j < n; j++)
                {
                    delta[j] = (Math.Abs(buff[j] - p[j])) / Math.Abs(buff[j]); //вычисление погрешности
                    p[j] = buff[j];
                    Console.Write("X" + (j + 1) + " = " + Math.Round(p[j], 5) + "    ");
                    buff[j] = 0;
                }
            }
            //Вывод погрешностей для последнего приближения
            Console.WriteLine("\nПогрешности вычислений неизвестных равны: ");
            for (int j = 0; j < n; j++)
                Console.WriteLine(Math.Round(delta[j], 5));
            Console.WriteLine("\nСистема уравнений решена методом простой итерации. Спасибо, что воспользовались данной программой!");
        }

    }
}
