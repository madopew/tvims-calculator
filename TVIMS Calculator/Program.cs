using System;

namespace TVIMS_Calculator
{
    static class Program
    {
        static void Main()
        {
            Console.WriteLine("Введи свой ряд через пробел:");
            string input = Console.ReadLine();
            string[] values = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            double[] samples = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                samples[i] = Convert.ToDouble(values[i]);
            }

            Console.WriteLine("1) Дискретная величина");
            Console.WriteLine("2) Не дискретная");
            bool isDiscrete = Console.ReadLine() == "1";

            var sample = SampleFactory.CreateSample(samples, isDiscrete);

            Console.WriteLine(sample);

            Console.ReadLine();
        }
    }
}
