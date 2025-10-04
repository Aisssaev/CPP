using System;

namespace Lab4
{
    class Program
    {
        // Функція
        private static double F(double x)
        {
            return x * x - 4; // має корені ±2
        }

        // Перша похідна
        private static double Fp(double x)
        {
            return 2 * x;
        }

        // Друга похідна (аналітично)
        private static double F2p(double x)
        {
            return 2;
        }

        // Безпечне зчитування double
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

        private static void ReadParameters(out double a, out double b, out double eps, out int kmax)
        {
            while (true)
            {
                a = ReadDouble("Введіть a: ");
                b = ReadDouble("Введіть b: ");
                eps = ReadDouble("Введіть ε: ");
                kmax = (int)ReadDouble("Введіть максимальну кількість ітерацій Kmax: ");

                if (eps <= 0)
                {
                    Console.WriteLine("❌ Помилка: ε має бути > 0!");
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
            Console.WriteLine("🔹 Метод Ньютона для розв’язання нелінійного рівняння f(x)=0\n");

            ReadParameters(out var a, out var b, out var eps, out var Kmax);

            double x, D, Dx;
            D = eps / 100.0; // крок для чисельних похідних (не обов’язковий, тут похідні аналітичні)
            x = b; // початкове наближення

            // Вибір початкового наближення за умовою збіжності
            if (F(x) * F2p(x) < 0)
            {
                x = a;
            }
            else if (F(x) * F2p(x) > 0)
            {
                // усе добре, x = b залишається
            }
            else
            {
                Console.WriteLine("⚠ Для заданого рівняння збіжність методу Ньютона не гарантується.");
                Console.ReadLine();
                return;
            }

            int i;
            for (i = 1; i <= Kmax; i++)
            {
                Dx = F(x) / Fp(x);
                x = x - Dx;

                if (Math.Abs(Dx) < eps)
                {
                    Console.WriteLine($"\n✅ Корінь знайдено: x = {x:F6}");
                    Console.WriteLine($"Кількість ітерацій: {i}");
                    Console.ReadLine();
                    return;
                }
            }

            Console.WriteLine("\n❌ За задану кількість ітерацій корінь з точністю ε не знайдено.");
            Console.ReadLine();
        }
    }
}