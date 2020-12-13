using System;
using System.Text;
using TVIMS_Calculator.Interfaces;

namespace TVIMS_Calculator.Implementations
{
    sealed class NonDiscreteSampleWriter : ISampleWriter
    {
        private readonly NonDiscreteSample _sample;

        public NonDiscreteSampleWriter(NonDiscreteSample sample)
        {
            _sample = sample;
        }

        private string GetDistributionFunction()
        {
            StringBuilder distFunc = new StringBuilder();
            double sum = 0;

            distFunc
                .AppendFormat("{0,10} | {1,10}", "x", "F*(x)").Append(Environment.NewLine);
            for (int i = 0; i < _sample.FrequencyRange.Frequencies.Length; i++)
            {
                double lower = _sample.FrequencyRange.Frequencies[i].First[0];
                distFunc.AppendFormat("{0,10:F2} | {1,10:F2}", lower, sum).Append(Environment.NewLine);
                sum += _sample.FrequencyRange[i] / (double)_sample.Volume;
            }

            distFunc.AppendFormat("{0,10:F2} | {1,10:F2}", _sample.FrequencyRange.Frequencies[^1].First[1], 1);

            return distFunc.ToString();
        }

        private string GetRange()
        {
            StringBuilder rangeBuilder = new StringBuilder();
            rangeBuilder.AppendFormat("{0,3} | {1,25} | {2,3} | {3,10} | {4,10}", "№", "Границы", "ni", "ni/h", "wi/h")
                .Append(Environment.NewLine);
            for (int i = 0; i < _sample.FrequencyRange.Frequencies.Length; i++)
            {
                string range = string.Format("[{0,5:F2}; {1,5:F2})", _sample.FrequencyRange.Frequencies[i].First[0],
                    _sample.FrequencyRange.Frequencies[i].First[1]);
                int freq = _sample.FrequencyRange[i];
                double nih = freq / _sample.IntervalStep;
                double wih = nih / _sample.Volume;
                rangeBuilder
                    .AppendFormat("{0,3:D} | {1,25} | {2,3:D} | {3,10:E3} | {4,10:E3}", i + 1, range, freq, nih, wih)
                    .Append(Environment.NewLine);
            }

            return rangeBuilder.ToString();
        }

        private string GetMode()
        {
            if (_sample.Mode.Length == 0)
            {
                return "Моды нет";
            }

            if (_sample.Mode.Length == 1)
            {
                return $"Мода:                     {_sample.Mode[0]}";
            }

            return "Моды:                     " + string.Join("; ", _sample.Mode);
        }

        public string Info()
        {
            StringBuilder infoBuilder = new StringBuilder();
            infoBuilder.AppendLine("Вариационный ряд:")
                .AppendLine(string.Join(" ", _sample.VariationRange))
                .AppendLine()

                .AppendLine("Интервальный ряд:")
                .AppendLine(GetRange())

                .AppendLine("Эмпирическая функция")
                .AppendLine(GetDistributionFunction())
                .AppendLine()

                .AppendLine($"Объем выборки:            {_sample.Volume}")
                .AppendLine($"Среднее:                  {_sample.Average}")
                .AppendLine(GetMode())
                .AppendLine($"Медиана:                  {_sample.Median}")
                .AppendLine($"Межгрупповая дисперсия:   {_sample.InterGroupDispersion}")
                .AppendLine($"Среднее из групповых:     {_sample.AverageGroupDispersion}")
                .AppendLine($"Сумма дисперсий:          {_sample.AverageGroupDispersion + _sample.InterGroupDispersion}")
                .AppendLine($"Общая дисперсия:          {_sample.Dispersion}")
                .AppendLine($"Коэффициент детерминации: {_sample.DeterminationCoefficient}")
                .AppendLine($"Размах вариации:          {_sample.VariationScope}")
                .AppendLine($"Коэффициент вариации:     {Convert.ToInt32(_sample.VariationCoefficient)}%")
                .AppendLine($"Асимметрия:               {_sample.Asymmetry}")
                .AppendLine($"Эксцесс:                  {_sample.Excess}");

            return infoBuilder.ToString();
        }
    }
}
