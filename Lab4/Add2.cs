using System;
using System.Collections.Generic;

namespace Lab4
{
    class Program
    {
        // Прикладна функція
        private static double F(double x)
        {
            return x * x - 4; // корені: x = -2, +2
        }

        // Безпечне зчитування double
        private static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out var v)) return v;
                Console.WriteLine("❌ Помилка: введіть число!");
            }
        }

        // Зчитування параметрів табуляції
        private static void ReadTabParams(out double start, out double end, out double h)
        {
            while (true)
            {
                start = ReadDouble("Введіть початок відрізка (start): ");
                end = ReadDouble("Введіть кінець відрізка (end): ");
                h = ReadDouble("Введіть крок табуляції (h > 0): ");

                if (h <= 0)
                {
                    Console.WriteLine("❌ Крок h має бути додатним.");
                    continue;
                }
                if (start == end)
                {
                    Console.WriteLine("❌ start і end не можуть бути однаковими.");
                    continue;
                }
                // упевнимось що start < end, якщо ні — поміняємо місцями
                if (start > end)
                {
                    Console.WriteLine("⚠ start > end — поміняю місцями.");
                    var tmp = start; start = end; end = tmp;
                }
                break;
            }
        }

        // Табулювання функції та пошук інтервалів локалізації коренів
        // Повертає список інтервалів (a,b) де f(a)*f(b) <= 0 (містить знакозміну або вузлову точку).
        private static List<(double a, double b)> FindRootIntervalsByTabulation(double start, double end, double h)
        {
            var intervals = new List<(double a, double b)>();

            double x = start;
            double fx = F(x);

            // Якщо вузлова точка на початку
            if (Math.Abs(fx) < Double.Epsilon * 10)
            {
                // Вважаємо як інтервал [x,x]
                intervals.Add((x, x));
            }

            while (x + h <= end + 1e-12) // додатковий запас для останньої точки
            {
                double xNext = x + h;
                if (xNext > end) xNext = end; // останній крок може бути меншим
                double fxNext = F(xNext);

                // якщо точне нульове значення у вузловій точці
                if (Math.Abs(fxNext) < 1e-15)
                {
                    intervals.Add((xNext, xNext));
                }
                else if (fx * fxNext < 0) // знакозміна → є корінь в (x, xNext)
                {
                    intervals.Add((x, xNext));
                }

                // рухаємось вперед
                x = xNext;
                fx = fxNext;
            }

            return intervals;
        }

        // Друк табуляції (опційно)
        private static void PrintTabulation(double start, double end, double h)
        {
            Console.WriteLine("\n x \t\t f(x)");
            Console.WriteLine(new string('-', 30));
            for (double x = start; x <= end + 1e-12; x += h)
            {
                double xx = x;
                if (x + h > end && Math.Abs(x - end) > 1e-12) xx = end; // остання точка
                Console.WriteLine($"{xx,8:F5}\t{F(xx),12:E5}");
                if (xx == end) break;
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Пошук інтервалів локалізації коренів шляхом табулювання f(x)\n");

            double start = 0, end = 0, h = 0;
            ReadTabParams(out start, out end, out h);
            int choice;
            do
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1 — Показати табуляцію f(x)");
                Console.WriteLine("2 — Знайти інтервали локалізації коренів");
                Console.WriteLine("3 — Ввести нові параметри (start, end, h)");
                Console.WriteLine("0 — Вихід");
                Console.Write("Вибір: ");
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("❌ Введіть число від 0 до 3.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        PrintTabulation(start, end, h);
                        Console.WriteLine("\nНатисніть Enter для продовження...");
                        Console.ReadLine();
                        break;

                    case 2:
                        var intervals = FindRootIntervalsByTabulation(start, end, h);
                        if (intervals.Count == 0)
                        {
                            Console.WriteLine("\nЗа табуляцією інтервали локалізації не знайдено.");
                        }
                        else
                        {
                            Console.WriteLine("\nЗнайдені інтервали локалізації (або вузлові точки):");
                            int idx = 1;
                            foreach (var itv in intervals)
                            {
                                if (Math.Abs(itv.a - itv.b) < 1e-15)
                                {
                                    Console.WriteLine($"{idx++}. Вузлова точка: x = {itv.a:F12}");
                                }
                                else
                                {
                                    Console.WriteLine($"{idx++}. ({itv.a:F12}, {itv.b:F12})");
                                }
                            }
                        }
                        Console.WriteLine("\nНатисніть Enter для продовження...");
                        Console.ReadLine();
                        break;

                    case 3:
                        ReadTabParams(out start, out end, out h);
                        break;

                    case 0:
                        Console.WriteLine("Вихід...");
                        break;

                    default:
                        Console.WriteLine("Невірний вибір.");
                        break;
                }

            } while (choice != 0);

            Console.WriteLine("Готово.");
        }
    }
}