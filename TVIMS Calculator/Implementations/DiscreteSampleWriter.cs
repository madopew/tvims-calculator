using System;
using System.Text;
using TVIMS_Calculator.Interfaces;

namespace TVIMS_Calculator.Implementations
{
    class DiscreteSampleWriter : ISampleWriter
    {
        private readonly DiscreteSample _sample;

        public DiscreteSampleWriter(DiscreteSample sample)
        {
            _sample = sample;
        }

        private string GetDistributionFunction()
        {
            StringBuilder distFunc = new StringBuilder();
            string funcStr = "F*(x) = <   ";
            string include = "         |  ";
            double sum = 0;

            distFunc.Append(include)
                .AppendFormat("{0,5:F2}, x <= {1,5:F2}", sum, _sample.FrequencyRange.Frequencies[0].First).Append(Environment.NewLine);
            for (int i = 0; i < _sample.FrequencyRange.Frequencies.Length - 1; i++)
            {
                double lower = _sample.FrequencyRange.Frequencies[i].First;
                double upper = _sample.FrequencyRange.Frequencies[i + 1].First;
                sum += _sample.FrequencyRange[i] / (double)_sample.Volume;
                distFunc.Append(i == (_sample.FrequencyRange.Frequencies.Length / 2 - 1) ? funcStr : include);
                distFunc.AppendFormat("{0,5:F2}, {1,5:F2} < x <= {2,5:F2}", sum, lower, upper).Append(Environment.NewLine);
            }

            distFunc.Append(include).AppendFormat("{0,5:F2}, {1,5:F2} < x", 1, _sample.FrequencyRange.Frequencies[^1].First);

            return distFunc.ToString();
        }

        private string GetRange()
        {
            StringBuilder rangeBuilder = new StringBuilder();
            rangeBuilder.AppendFormat("{0,4} | {1,5} | {2,6}", "xi", "ni", "wi")
                .Append(Environment.NewLine);
            for (int i = 0; i < _sample.FrequencyRange.Frequencies.Length; i++)
            {
                int freq = _sample.FrequencyRange[i];
                double wi = freq / (double)_sample.Volume;
                rangeBuilder
                    .AppendFormat("{0,4:F1} | {1,5:D} | {2,6:E3}", _sample.FrequencyRange.Frequencies[i].First, freq, wi)
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
                return $"Мода:                 {_sample.Mode[0]}";
            }

            return "Моды:                 " + string.Join("; ", _sample.Mode);
        }

        public string Info()
        {
            StringBuilder infoBuilder = new StringBuilder();
            infoBuilder.AppendLine("Вариационный ряд:")
                .AppendLine(string.Join(" ", _sample.VariationRange))
                .AppendLine()

                .AppendLine("Дисретный ряд:")
                .AppendLine(GetRange())

                .AppendLine("Эмпирическая функция")
                .AppendLine(GetDistributionFunction())
                .AppendLine()

                .AppendLine($"Объем выборки:        {_sample.Volume}")
                .AppendLine($"Среднее:              {_sample.Average}")
                .AppendLine(GetMode())
                .AppendLine($"Медиана:              {_sample.Median}")
                .AppendLine($"Дисперсия:            {_sample.Dispersion}")
                .AppendLine($"Размах вариации:      {_sample.VariationScope}")
                .AppendLine($"Коэффициент вариации: {Convert.ToInt32(_sample.VariationCoefficient)}%")
                .AppendLine($"Асимметрия:           {_sample.Asymmetry}")
                .AppendLine($"Эксцесс:              {_sample.Excess}");

            return infoBuilder.ToString();
        }
    }
}
