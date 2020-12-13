using System;
using System.Collections.Generic;
using System.Linq;
using TVIMS_Calculator.Interfaces;

namespace TVIMS_Calculator.Implementations
{
    class SampleCalculator
    {
        protected readonly ISample _sample;

        public SampleCalculator(ISample sample)
        {
            _sample = sample;
        }

        public double CalculateCenterMoment(int m)
        {
            double moment = 0;
            foreach (var el in _sample.VariationRange)
            {
                moment += Math.Pow(el - _sample.Average, m);
            }

            moment /= _sample.Volume;
            return moment;
        }

        public int CalculateVolume(double[] samples)
        {
            return samples.Length;
        }

        public double[] CalculateVariationRange(double[] samples)
        {
            double[] variationRange = new double[_sample.Volume];
            Array.Copy(samples, variationRange, samples.Length);
            Array.Sort(variationRange);
            return variationRange;
        }

        public double CalculateVariationScope()
        {
            return _sample.VariationRange[^1] - _sample.VariationRange[0];
        }

        public ISampleRange<double> CalculateFrequencyRange()
        {
            List<double> frequencyRange = new List<double>();
            List<int> frequencies = new List<int>();

            foreach (var el in _sample.VariationRange)
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

            return new SampleRange<double>(frequencyRange.ToArray(), frequencies.ToArray());
        }

        public double CalculateAverage()
        {
            return _sample.VariationRange.Sum() / _sample.Volume;
        }

        public virtual double[] CalculateMode()
        {
            DiscreteSample discreteSample = _sample as DiscreteSample ?? throw new ArgumentException("Invalid type");
            if (discreteSample.FrequencyRange.Frequencies.All(p => p.Second == discreteSample.FrequencyRange[0]))
            {
                return Array.Empty<double>();
            }

            List<double> modes = new List<double>
            {
                discreteSample.FrequencyRange.Frequencies[0].First
            };
            int modeIndex = 0;
            for (int i = 1; i < discreteSample.FrequencyRange.Frequencies.Length; i++)
            {
                if (discreteSample.FrequencyRange[i] > discreteSample.FrequencyRange[modeIndex])
                {
                    modeIndex = i;
                    modes.Clear();
                    modes.Add(discreteSample.FrequencyRange.Frequencies[i].First);
                }
                else if (discreteSample.FrequencyRange[i] == discreteSample.FrequencyRange[modeIndex])
                {
                    modes.Add(discreteSample.FrequencyRange.Frequencies[i].First);
                }
            }

            return modes.ToArray();
        }

        public virtual double CalculateMedian()
        {
            if (_sample.Volume % 2 != 0)
            {
                return _sample.VariationRange[(_sample.Volume / 2) + 1];
            }

            double median = _sample.VariationRange[_sample.Volume / 2];
            median += _sample.VariationRange[(_sample.Volume / 2) + 1];
            median /= 2;
            return median;
        }

        public double CalculateDispersion()
        {
            return CalculateCenterMoment(2);
        }

        public double CalculateAsymmetry()
        {
            return CalculateCenterMoment(3) / Math.Pow(_sample.Dispersion, 3.0 / 2.0);
        }

        public double CalculateExcess()
        {
            double excess = CalculateCenterMoment(4) / Math.Pow(_sample.Dispersion, 2);
            excess -= 3;
            return excess;
        }

        public double CalculateVariationCoefficient()
        {
            return Math.Pow(_sample.Dispersion, 0.5) / _sample.Average * 100;
        }
    }
}
