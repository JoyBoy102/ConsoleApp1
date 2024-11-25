using System;
using ConsoleTables;
namespace ConsoleApp1
{
    class Program
    {
        public static List<double> approxValues = new List<double>();
        public static List<double> approxValues1 = new List<double>();
        public static List<double> approxValues2 = new List<double>();
        static void Main(string[] args)
        {
            double a_f = 0;
            double b_f = 1;
            double a1_f1 = 0;
            double b1_f1 = 1.5;
            double a2_f1 = 0.001;
            double b2_f1 = 1.5;
            double exactValue = g(a_f, b_f); // Точное значение интеграла
            
            var table = new ConsoleTable("n", "Приближенное значение", "Точное значение", "Дельта K", "Дельта точное", "Дельта Рунге", "Дельта теор.");
            var table2 = new ConsoleTable("n", "Приближенное значение", "Дельта K", "Дельта Рунге", "Дельта теор.");
            var table3 = new ConsoleTable("n", "Приближенное значение", "Дельта K", "Дельта Рунге", "Дельта теор.");

            for (int n = 1; n<=65536; n *= 2)
            {
                double h = (b_f - a_f) / n;
                double approxValue = SimpsonMethod(a_f, b_f, n, h, f); // Метод Симпсона
                approxValues.Add(approxValue);
                double deltaExact = exactValue - approxValue;
                string rungeEstimate = getDeltaRunge(a_f, b_f, n, approxValues) == 0?"-": getDeltaRunge(a_f, b_f, n,approxValues).ToString();
                double deltaTeor = getTeorDelta(a_f, b_f, h);
                string K = getK(a_f, b_f, h, n,approxValues) == 0? "-": getK(a_f, b_f, h, n, approxValues).ToString();
                table.AddRow(n, $"{approxValue}", $"{exactValue}", $"{K}", $"{deltaExact}", $"{rungeEstimate}", $"{deltaTeor}");

                double h1 = (b1_f1 - a1_f1) / n;
                double approxValue1 = SimpsonMethod(a1_f1, b1_f1, n, h1, f1); // Метод Симпсона
                approxValues1.Add(approxValue1);
                string rungeEstimate1 = getDeltaRunge(a1_f1, b1_f1, n, approxValues1) == 0 ? "-" : getDeltaRunge(a1_f1, b1_f1, n, approxValues1).ToString();
                double deltaTeor1 = getTeorDelta(a1_f1, b1_f1, h1);
                string K1 = getK(a1_f1, b1_f1, h1, n, approxValues1) == 0 ? "-" : getK(a1_f1, b1_f1, h1, n, approxValues1).ToString();
                table2.AddRow(n, $"{approxValue1}", $"{K1}", $"{rungeEstimate1}", $"{deltaTeor1}");

                double h2 = (b2_f1 - a2_f1) / n;
                double approxValue2 = SimpsonMethod(a2_f1, b2_f1, n, h2, f1); // Метод Симпсона
                approxValues2.Add(approxValue2);
                string rungeEstimate2 = getDeltaRunge(a2_f1, b2_f1, n, approxValues2) == 0 ? "-" : getDeltaRunge(a2_f1, b2_f1, n, approxValues2).ToString();
                double deltaTeor2 = getTeorDelta(a2_f1, b2_f1, h2);
                string K2 = getK(a2_f1, b2_f1, h2, n, approxValues2) == 0? "-": getK(a2_f1, b2_f1, h2, n, approxValues2).ToString();
                table3.AddRow(n, $"{approxValue2}", $"{K2}", $"{rungeEstimate2}", $"{deltaTeor2}");

            }
            Console.WriteLine("Функция f(x) = 6*x^5");
            table.Write();
            Console.WriteLine("Функция f(x) = x^(1/30)*sqrt(1+x^2) a=0 b=1.5");
            table2.Write();
            Console.WriteLine("Функция f(x) = x^(1/30)*sqrt(1+x^2) a=0.001 b=1.5");
            table3.Write();
        }

        // Функция f(x) = 6*x^5
        public static double f(double x)
        {
            return 6 * Math.Pow(x, 5);
        }

        public static double f2(double x)
        {
            return Math.Pow(Math.Pow(x, 5) + 3,3);
        }

        // Точное значение интеграла от функции f(x) = 6*x^5
        public static double g(double a, double b)
        {
            return Math.Pow(b, 6) - Math.Pow(a, 6);
        }

        //Функция f(x) = x^(1/30)*sqrt(1+x^2)
        public static double f1(double x)
        {
            return Math.Pow(x, (1/30))*Math.Sqrt(1+Math.Pow(x, 2));
        }


        // Метод Симпсона
        public static double SimpsonMethod(double a, double b, int n, double h, Func<double, double> f)
        {
            double sum = f(a) + f(b);
            double sum1 = 0;
            double sum2 = 0;
            for (int i = 1; i < n; i++)
            {
                sum1 += 2 * f(a + i * h);
            }
            for (int i = 0; i< n; i++)
            {
                sum2 += 4 * f(a + i * h + h / 2);
            }
            sum += sum1 + sum2;
            return sum * h / 6;
        }

        // Оценка погрешности по правилу Рунге
        public static double getDeltaRunge(double a, double b, int n, List<double> approxValues)
        {
            if (n > 1) return (approxValues.Last() - approxValues[approxValues.Count-2]) / 15;
            return 0;

        }

        //M для функции f(x) = 6*x^5
        public static double getM(double a, double b, double h)
        {
            double max = -999999999;
            for (double i = a; i<= b; i += h)
            {
                if (720 * i > max)
                {
                    max = 720 * i;
                }
            }
            return (b-a)*max;
        }

        //Оценка погрешности теоретическая
        public static double getTeorDelta(double a, double b, double h)
        {
            double M = getM(a, b, h);
            double TeorDelta = (M / 2880) * Math.Pow(h, 4);
            return TeorDelta;
        }

        //Стабильность погрешности
        public static double getK(double a, double b, double h, int n, List<double> approxValues)
        {
            if (n > 2)
            {
                double J4 = approxValues.Last();
                double J2 = approxValues[approxValues.Count - 2];
                double J1 = approxValues[approxValues.Count - 3];
                return (J2 - J1) / (J4 - J2);
            }
            return 0;
        }
    }
}
