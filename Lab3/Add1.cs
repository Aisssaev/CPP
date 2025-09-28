using System;

namespace bisection_method
{
    class Program
    {
        static double f(double x)
        {
            return x * x * x - x - 2;
        }

        static void Main(string[] args)
        {
            double a = 1, b = 2;  
            double eps = 1e-6;    
            double c = 0;

            if (f(a) * f(b) > 0)
            {
                Console.WriteLine("На відрізку [{0}, {1}] немає кореня!", a, b);
                return;
            }

            while ((b - a) / 2 > eps)
            {
                c = (a + b) / 2;
                if (f(c) == 0)
                    break;

                if (f(a) * f(c) < 0)
                    b = c;
                else
                    a = c;
            }

            Console.WriteLine("Корінь ≈ {0}", c);
            Console.WriteLine("\nPress <ENTER> to exit.");
            Console.ReadLine();
        }
    }
}
