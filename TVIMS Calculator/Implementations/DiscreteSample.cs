using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TVIMS_Calculator.Interfaces;

namespace TVIMS_Calculator.Implementations
{
    class DiscreteSample : ISample
    {
        public double[] VariationRange { get; private set; }
        public double VariationScope { get; private set; }
        public double VariationCoefficient { get; private set; }
        public int Volume { get; private set; }
        public ISampleRange<double> FrequencyRange { get; private set; }
        public double Average { get; private set; }
        public double Mode { get; private set; }
        public double Median { get; private set; }
        public double Dispersion { get; private set; }
        public double Asymmetry { get; private set; }
        public double Excess { get; private set; }

        public string GetDistributionFunction()
        {
            StringBuilder distFunc = new StringBuilder();
            string funcStr = "F*(x) = <   ";
            string include = "         |  ";
            double sum = 0;

            distFunc.Append(include)
                .AppendFormat("{0,5:F2}, x <= {1,5:F2}", sum, FrequencyRange.Frequencies[0].First).Append(Environment.NewLine);
            for (int i = 0; i < FrequencyRange.Frequencies.Length - 1; i++)
            {
                double lower = FrequencyRange.Frequencies[i].First;
                double upper = FrequencyRange.Frequencies[i + 1].First;
                sum += FrequencyRange[i] / (double)Volume;
                distFunc.Append(i == (FrequencyRange.Frequencies.Length / 2 - 1) ? funcStr : include);
                distFunc.AppendFormat("{0,5:F2}, {1,5:F2} < x <= {2,5:F2}", sum, lower, upper).Append(Environment.NewLine);
            }

            distFunc.Append(include).AppendFormat("{0,5:F2}, {1,5:F2} < x", 1, FrequencyRange.Frequencies[^1].First);

            return distFunc.ToString();
        }

        public string GetRange()
        {
            StringBuilder rangeBuilder = new StringBuilder();
            rangeBuilder.AppendFormat("{0,4} | {1,5} | {2,6}", "xi", "ni", "wi/h")
                .Append(Environment.NewLine);
            for (int i = 0; i < FrequencyRange.Frequencies.Length; i++)
            {
                int freq = FrequencyRange[i];
                double wi = freq / (double)Volume;
                rangeBuilder
                    .AppendFormat("{0,4:F1} | {1,5:D} | {2,6:E3}", FrequencyRange.Frequencies[i].First, freq, wi)
                    .Append(Environment.NewLine);
            }

            return rangeBuilder.ToString();
        }

        public void OutputInfo()
        {
            Console.WriteLine("Вариационный ряд:");
            Console.WriteLine(string.Join(" ", VariationRange));
            Console.WriteLine();

            Console.WriteLine("Дисретный ряд:");
            Console.WriteLine(GetRange());

            Console.WriteLine("Эмпирическая функция");
            Console.WriteLine(GetDistributionFunction());
            Console.WriteLine();

            Console.WriteLine($"Объем выборки: {Volume}");
            Console.WriteLine($"Среднее: {Average}");
            Console.WriteLine($"Мода: {Mode}");
            Console.WriteLine($"Медиана: {Median}");
            Console.WriteLine($"Дисперсия: {Dispersion}");
            Console.WriteLine($"Среднеквадратичное: {Math.Pow(Dispersion, 0.5)}");
            Console.WriteLine($"Размах вариации: {VariationScope}");
            Console.WriteLine($"Коэффициент вариации: {VariationCoefficient}%");
            Console.WriteLine($"Асимметрия: {Asymmetry}");
            Console.WriteLine($"Эксцесс: {Excess}");
        }

        public double GetCenterMoment(int m)
        {
            double moment = 0;
            foreach (var el in VariationRange)
            {
                moment += Math.Pow(el - Average, m);
            }

            moment /= Volume;
            return moment;
        }

        public DiscreteSample(double[] samples)
        {
            InitVariationRange(samples);
            InitFrequencyRange();
            InitAverage();
            InitMode();
            InitMedian();
            InitDispersion();
            InitAsymmetry();
            InitExcess();
            InitVariationCoefficient();
        }

        private void InitVariationRange(double[] samples)
        {
            this.Volume = samples.Length;
            this.VariationRange = new double[Volume];
            Array.Copy(samples, VariationRange, Volume);
            Array.Sort(VariationRange);
        }

        private void InitFrequencyRange()
        {
            VariationScope = VariationRange[^1] - VariationRange[0];

            List<double> frequencyRange = new List<double>();
            List<int> frequencies = new List<int>();

            foreach (var el in VariationRange)
            {
                if (frequencyRange.Contains(el))
                {
                    frequencies[^1]++;
                }
                else
                {
                    frequencyRange.Add(el);
                    frequencies.Add(1);
                }
            }

            FrequencyRange = new SampleRange<double>(frequencyRange.ToArray(), frequencies.ToArray());
        }

        private void InitAverage()
        {
            Average = VariationRange.Sum() / Volume;
        }

        private void InitMode()
        {
            int modeIndex = 0;
            for (int i = 1; i < FrequencyRange.Frequencies.Length; i++)
            {
                if (FrequencyRange[i] > FrequencyRange[modeIndex])
                {
                    modeIndex = i;
                }
            }

            if (modeIndex == 0 && FrequencyRange[0] == 1)
            {
                Console.Error.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Console.Error.WriteLine("-->Что-то не то с модой.<--");
                Console.Error.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Mode = Double.Epsilon;
                return;
            }

            Mode = FrequencyRange[modeIndex];
        }

        private void InitMedian()
        {
            if (Volume % 2 != 0)
            {
                Median = VariationRange[(Volume / 2) + 1];
            }
            else
            {
                Median = VariationRange[Volume / 2];
                Median += VariationRange[(Volume / 2) + 1];
                Median /= 2;
            }
        }

        private void InitDispersion()
        {
            Dispersion = GetCenterMoment(2);
        }

        private void InitAsymmetry()
        {
            Asymmetry = GetCenterMoment(3) / Math.Pow(Dispersion, 3.0 / 2.0);
        }

        private void InitExcess()
        {
            Excess = GetCenterMoment(4) / Math.Pow(Dispersion, 2);
            Excess -= 3;
        }

        private void InitVariationCoefficient()
        {
            VariationCoefficient = Math.Pow(Dispersion, 0.5) / Average * 100;
        }
    }
}
