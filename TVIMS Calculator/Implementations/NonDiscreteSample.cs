using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TVIMS_Calculator.Implementations;
using TVIMS_Calculator.Interfaces;

namespace TVIMS_Calculator
{
    class NonDiscreteSample : ISample
    {
        public double[] VariationRange { get; private set; }
        public double VariationScope { get; private set; }
        public double VariationCoefficient { get; private set; }
        public int Volume { get; private set; }
        public ISampleRange<double[]> FrequencyRange { get; private set; }
        public int IntervalAmount { get; private set; }
        public double IntervalStep { get; private set; }
        public double Average { get; private set; }
        public double Mode { get; private set; }
        public double Median { get; private set; }
        public double Dispersion { get; private set; }
        public double Asymmetry { get; private set; }
        public double Excess { get; private set; }
        private double[] GroupAverages { get; set; }
        public double[] GroupDispersions { get; private set; }
        public double AverageGroupDispersion { get; private set; }
        public double IntergroupDispersion { get; private set; }
        public double DeterminationCoefficient { get; private set; }

        public string GetDistributionFunction()
        {
            StringBuilder distFunc = new StringBuilder();
            string funcStr = "F*(x) = <   ";
            string include = "         |  ";
            double sum = 0;

            distFunc.Append(include)
                .AppendFormat("{0,5:F2}, x <= {1,5:F2}", sum, FrequencyRange.Frequencies[0].First[0]).Append(Environment.NewLine);
            for (int i = 0; i < FrequencyRange.Frequencies.Length - 1; i++)
            {
                double lower = FrequencyRange.Frequencies[i].First[0];
                double upper = FrequencyRange.Frequencies[i].First[1];
                sum += FrequencyRange[i] / (double)Volume;
                distFunc.Append(i == (FrequencyRange.Frequencies.Length / 2 - 1) ? funcStr : include);
                distFunc.AppendFormat("{0,5:F2}, {1,5:F2} < x <= {2,5:F2}", sum, lower, upper).Append(Environment.NewLine);
            }

            distFunc.Append(include).AppendFormat("{0,5:F2}, {1,5:F2} < x", 1, FrequencyRange.Frequencies[^1].First[0]);

            return distFunc.ToString();
        }

        public string GetRange()
        {
            StringBuilder rangeBuilder = new StringBuilder();
            rangeBuilder.AppendFormat("{0,3} | {1,25} | {2,3} | {3,10} | {4,10}", "№", "Границы", "ni", "ni/h", "wi/h")
                .Append(Environment.NewLine);
            for (int i = 0; i < FrequencyRange.Frequencies.Length; i++)
            {
                string range = string.Format("[{0,5:F2}; {1,5:F2})", FrequencyRange.Frequencies[i].First[0],
                    FrequencyRange.Frequencies[i].First[1]);
                int freq = FrequencyRange[i];
                double nih = freq / IntervalStep;
                double wih = nih / Volume;
                rangeBuilder
                    .AppendFormat("{0,3:D} | {1,25} | {2,3:D} | {3,10:E3} | {4,10:E3}", i + 1, range, freq, nih, wih)
                    .Append(Environment.NewLine);
            }

            return rangeBuilder.ToString();
        }

        public void OutputInfo()
        {
            Console.WriteLine("Вариационный ряд:");
            Console.WriteLine(string.Join(" ", VariationRange));
            Console.WriteLine();

            Console.WriteLine("Интервальный ряд:");
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
            Console.WriteLine($"Межгрупповая дисперсия: {IntergroupDispersion}");
            Console.WriteLine($"Коэф (сумма дисперсий): {DeterminationCoefficient}");
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

        public NonDiscreteSample(double[] samples)
        {
            InitVariationRange(samples);
            InitFrequencyRange();
            InitAverage();
            InitMode();
            InitMedian();
            InitDispersions();
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
            IntervalAmount = 1 + (int) (3.322 * Math.Log10(Volume));
            VariationScope = VariationRange[^1] - VariationRange[0];
            IntervalStep = VariationScope / IntervalAmount;

            double[][] frequencyRange = new double[IntervalAmount][];
            int[] frequencies = new int[IntervalAmount];

            double intervalLimit = VariationRange[0];
            for (int i = 0; i < IntervalAmount; i++)
            {
                frequencyRange[i] = new double[2];
                frequencyRange[i][0] = intervalLimit;
                intervalLimit += IntervalStep;
                frequencyRange[i][1] = intervalLimit;

                frequencies[i] = GetFrequency(frequencyRange[i], i == IntervalAmount - 1);
            }

            FrequencyRange = new SampleRange<double[]>(frequencyRange, frequencies);
        }

        private int GetFrequency(double[] range, bool isLast)
        {
            int amount = 0;
            foreach (double el in VariationRange)
            {
                bool upper = isLast ? el <= range[1] : el < range[1];
                if (el >= range[0] && upper)
                {
                    amount++;
                }
            }

            return amount;
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

            if (modeIndex == 0 || modeIndex == FrequencyRange.Frequencies.Length - 1)
            {
                Console.Error.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Console.Error.WriteLine("-->Что-то не то с модой.<--");
                Console.Error.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Mode = Double.Epsilon;
                return;
            }

            Mode = FrequencyRange[modeIndex] - FrequencyRange[modeIndex - 1];
            Mode /= Mode + FrequencyRange[modeIndex] - FrequencyRange[modeIndex + 1];
            Mode *= IntervalStep;
            Mode += FrequencyRange.Frequencies[modeIndex].First[0];
        }

        private void InitMedian()
        {
            int medianIndex;
            double sum = 0;
            double threshold = FrequencyRange.FrequenciesSum / 2.0;
            for (medianIndex = 1; medianIndex < FrequencyRange.Frequencies.Length; medianIndex++)
            {
                sum += FrequencyRange[medianIndex - 1];
                if (sum + FrequencyRange[medianIndex] > threshold)
                {
                    break;
                }
            }

            Median = threshold - sum;
            Median /= FrequencyRange[medianIndex];
            Median *= IntervalStep;
            Median += FrequencyRange.Frequencies[medianIndex].First[0];
        }

        private void InitDispersions()
        {
            InitGroupAverages();
            InitGroupDispersion();
            InitAverageGroupDispersion();
            InitIntergroupDispersion();
            InitDispersion();
            DeterminationCoefficient = IntergroupDispersion / Dispersion;
        }

        private void InitGroupAverages()
        {
            GroupAverages = new double[FrequencyRange.Frequencies.Length];
            for (int i = 0; i < FrequencyRange.Frequencies.Length; i++)
            {
                bool isLast = i == FrequencyRange.Frequencies.Length - 1;
                GroupAverages[i] = GetGroupSum(i, isLast) / FrequencyRange[i];
            }
        }

        private void InitGroupDispersion()
        {
            GroupDispersions = new double[FrequencyRange.Frequencies.Length];
            for (int i = 0; i < FrequencyRange.Frequencies.Length; i++)
            {
                bool isLast = i == FrequencyRange.Frequencies.Length - 1;
                double sampleAvg = GroupAverages[i];
                foreach (var el in GetGroup(i, isLast))
                {
                    GroupDispersions[i] += Math.Pow(el - sampleAvg, 2);
                }

                GroupDispersions[i] /= FrequencyRange[i];
            }
        }

        private IEnumerable<double> GetGroup(int groupIndex, bool isLast)
        {
            double[] range = FrequencyRange.Frequencies[groupIndex].First;
            foreach (var el in VariationRange)
            {
                bool upper = isLast ? el <= range[1] : el < range[1];
                if (el >= range[0] && upper)
                {
                    yield return el;
                }
            }
        }

        private double GetGroupSum(int groupIndex, bool isLast)
        {
            return GetGroup(groupIndex, isLast).Sum();
        }

        private void InitAverageGroupDispersion()
        {
            for (int i = 0; i < GroupDispersions.Length; i++)
            {
                AverageGroupDispersion += GroupDispersions[i] * FrequencyRange[i];
            }

            AverageGroupDispersion /= Volume;
        }

        private void InitIntergroupDispersion()
        {
            for (int i = 0; i < GroupAverages.Length; i++)
            {
                IntergroupDispersion += Math.Pow(GroupAverages[i] - Average, 2) * FrequencyRange[i];
            }

            IntergroupDispersion /= Volume;
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
