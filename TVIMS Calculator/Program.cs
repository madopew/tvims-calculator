using System;
#pragma warning disable S125

namespace TVIMS_Calculator
{
    static class Program
    {
        static void Main()
        {
            Console.WriteLine("Введи свой ряд через пробел (вещественные числа с точкой)");
            string input = Console.ReadLine();
            //string input = "12586 15121 12114 17599 9376 11156 8357 8524 15880 10078 14431 5903 12898 8272 7493 9921 9853 13423 10149 8847 3230 8855 14965 12747 6441 11629 15171 3636 6245 9940 14236 10064 15732 8838 4458 9486 10072 8287 10609 11593 6854 7756 8097 7822 14500 2925 7634 12896 2018 8257 9572 10659 7085 8072 3424 6215 3266 8725 7555 5287 9228 2922 7490 10045 13838 13398 4473 9741 2189 334 4436 5574 92 6154 12051 5069 1954 1657 2129 4509 4482 4928 3279 170 2272 2125 2817 5445 1135 5347 783 ";
            //string input = "0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 2 2 2 2 2 2 2 2 2 2 2 2 2 3 3 3 3 3 3 4 5 6 7 8";
            string[] values = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            double[] samples = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                samples[i] = Convert.ToDouble(values[i]);
            }

            Console.WriteLine("1) Дискретная величина");
            Console.WriteLine("2) Не дискретная");
            bool isDiscrete = Console.ReadLine() == "1";

            var nds = SampleFactory.CreateSample(samples, isDiscrete);

            nds.OutputInfo();

            Console.ReadLine();
        }
    }
}
