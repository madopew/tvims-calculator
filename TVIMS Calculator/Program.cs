using System;

namespace TVIMS_Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Дарова рыцарь ТВИМСа. Эта прога поможет тебе посчитать хуйню. Введи свой ряд через пробел (тока вещественные числа с точкой!!!)");
            //string input = Console.ReadLine();
            string input =
                "12586 15121 12114 17599 9376 11156 8357 8524 15880 10078 14431 5903 12898 8272 7493 9921 9853 13423 10149 8847 3230 8855 14965 12747 6441 11629 15171 3636 6245 9940 14236 10064 15732 8838 4458 9486 10072 8287 10609 11593 6854 7756 8097 7822 14500 2925 7634 12896 2018 8257 9572 10659 7085 8072 3424 6215 3266 8725 7555 5287 9228 2922 7490 10045 13838 13398 4473 9741 2189 334 4436 5574 92 6154 12051 5069 1954 1657 2129 4509 4482 4928 3279 170 2272 2125 2817 5445 1135 5347 783 ";
            string[] values = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            double[] samples = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                samples[i] = Convert.ToDouble(values[i]);
            }

            NonDiscreteSample nds = new NonDiscreteSample(samples);
            
            Console.WriteLine("Объем выборки:");
            Console.WriteLine(nds.Volume);
            Console.WriteLine();

            Console.WriteLine("Вариационный ряд:");
            Console.WriteLine(string.Join(" ", nds.VariationRange));
            Console.WriteLine();

            Console.WriteLine("Интервальный ряд:");
            Console.WriteLine(nds.GetRange());

            Console.WriteLine("Эмпирическая функция");
            Console.WriteLine(nds.GetDistributionFunction());
            Console.WriteLine();

            Console.WriteLine("Среднее:");
            Console.WriteLine(nds.Average);
            Console.WriteLine();

            Console.WriteLine("Мода:");
            Console.WriteLine(nds.Mode);
            Console.WriteLine();

            Console.WriteLine("Медиана:");
            Console.WriteLine(nds.Median);
            Console.WriteLine();

            Console.WriteLine("Дисперсия:");
            Console.WriteLine(nds.Dispersion);
            Console.WriteLine();

            Console.WriteLine("Межгрупповая дисперсия:");
            Console.WriteLine(nds.IntergroupDispersion);
            Console.WriteLine();

            Console.WriteLine("Коэф (сумма дисперсий):");
            Console.WriteLine(nds.EmpiricDeterminationCoefficient);
            Console.WriteLine();

            Console.WriteLine("----дальше по нужде сами полазийти в коде----");

            Console.ReadLine();
        }
    }
}
