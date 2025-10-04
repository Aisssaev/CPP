using System;

namespace Lab4
{
    class Program
    {
        // Функція, для якої шукаємо корінь
        private static double F(double x)
        {
            return x * x - 4; // f(x) = x² - 4
        }

        // Перша похідна функції (чисельно)
        private static double FPrime(double x, double D)
        {
            return (F(x + D) - F(x)) / D;
        }

        // Друга похідна функції (чисельно)
        private static double F2Prime(double x, double D)
        {
            return (F(x + D) + F(x - D) - 2 * F(x)) / (D * D);
        }

        // Метод половинного ділення (бісекції)
        private static void Bisection(double a, double b, double eps)
        {
            var it = 0;

            if (F(a) * F(b) > 0)
            {
                Console.WriteLine("❌ Немає кореня на цьому проміжку.");
                Console.ReadLine();
                return;
            }

            while (Math.Abs(b - a) > eps)
            {
                var c = (a + b) / 2;
                it++;

                if (Math.Abs(F(c)) < eps)
                {
                    Console.WriteLine($"✅ Метод бісекції: x = {c:F6}, ітерацій = {it}");
                    Console.ReadLine();
                    return;
                }

                if (F(a) * F(c) < 0)
                    b = c;
                else
                    a = c;
            }

            Console.WriteLine($"✅ Метод бісекції: x = {(a + b) / 2:F6}, ітерацій = {it}");
            Console.ReadLine();
        }
        
        private static void Newton(double a, double b, double eps, int Kmax)
        {
            double D = eps / 100.0;
            double x, Dx;

            // Початкове наближення
            x = b;
            if (F(x) * F2Prime(x, D) < 0)
                x = a;
            else if (F(x) * F2Prime(x, D) > 0)
            {
                // все добре — використовуємо b
            }
            else
            {
                Console.WriteLine("⚠️ Для заданого рівняння збіжність методу Ньютона не гарантується.");
                Console.ReadLine();
                return;
            }

            // Ітераційний процес
            for (int i = 1; i <= Kmax; i++)
            {
                Dx = F(x) / FPrime(x, D);
                x = x - Dx;

                if (Math.Abs(Dx) < eps)
                {
                    Console.WriteLine($"✅ Метод Ньютона: x = {x:F6}, ітерацій = {i}");
                    Console.ReadLine();
                    return;
                }
            }

            Console.WriteLine($"❌ За {Kmax} ітерацій корінь з точністю {eps} не знайдено.");
            Console.ReadLine();
        }

        // Функція для безпечного зчитування числа
        private static double ReadDouble(string message)
        {
            while (true)
            {
                Console.Write(message);
                if (double.TryParse(Console.ReadLine(), out var value))
                    return value;

                Console.WriteLine("❌ Помилка: введіть число!");
            }
        }

        // Зчитування параметрів задачі
        private static void ReadParameters(out double a, out double b, out double eps, out int Kmax)
        {
            while (true)
            {
                a = ReadDouble("Введіть a: ");
                b = ReadDouble("Введіть b: ");
                eps = ReadDouble("Введіть ε: ");

                Console.Write("Введіть максимально допустиму кількість ітерацій Kmax: ");
                if (!int.TryParse(Console.ReadLine(), out Kmax) || Kmax <= 0)
                {
                    Console.WriteLine("❌ Помилка: Kmax має бути додатним цілим числом!");
                    continue;
                }

                if (eps <= 0)
                {
                    Console.WriteLine("❌ Помилка: ε має бути додатним числом!");
                    continue;
                }

                if (a == b)
                {
                    Console.WriteLine("❌ Помилка: a і b не можуть бути однаковими!");
                    continue;
                }

                break;
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Введення початкових даних:");
            ReadParameters(out var a, out var b, out var eps, out var Kmax);

            int choice;
            do
            {
                Console.WriteLine("\nОберіть дію:");
                Console.WriteLine("1 — Метод половинного ділення");
                Console.WriteLine("2 — Метод Ньютона");
                Console.WriteLine("3 — Ввести нові дані");
                Console.WriteLine("0 — Вихід");
                Console.Write("Ваш вибір: ");

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("❌ Помилка: введіть число від 0 до 3!");
                    continue;
                }

                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        Bisection(a, b, eps);
                        break;
                    case 2:
                        Newton(a, b, eps, Kmax);
                        break;
                    case 3:
                        Console.WriteLine("\n🔁 Введення нових даних:");
                        ReadParameters(out a, out b, out eps, out Kmax);
                        break;
                    case 0:
                        Console.WriteLine("👋 Вихід із програми...");
                        break;
                    default:
                        Console.WriteLine("❌ Невірний вибір пункту меню.");
                        break;
                }

            } while (choice != 0);

            Console.WriteLine("\n✅ Дякую за використання!");
        }
    }
}
