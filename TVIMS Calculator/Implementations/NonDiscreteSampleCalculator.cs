using System;
using System.Collections.Generic;
using System.Linq;
using TVIMS_Calculator.Interfaces;

namespace TVIMS_Calculator.Implementations
{
    sealed class NonDiscreteSampleCalculator : SampleCalculator
    {
        private readonly NonDiscreteSample nonDiscreteSample;

        public NonDiscreteSampleCalculator(ISample sample)
            : base(sample)
        {
            nonDiscreteSample = _sample as NonDiscreteSample ?? throw new ArgumentException("Invalid type");
        }

        public int CalculateIntervalAmount()
        {
            return 1 + (int) (3.322 * Math.Log10(_sample.Volume));
        }

        public double CalculateIntervalStep()
        {
           return Math.Ceiling(_sample.VariationScope / nonDiscreteSample.IntervalAmount);
        }

        public new ISampleRange<double[]> CalculateFrequencyRange()
        {
            double[][] frequencyRange = new double[nonDiscreteSample.IntervalAmount][];
            int[] frequencies = new int[nonDiscreteSample.IntervalAmount];

            double intervalLimit = _sample.VariationRange[0];
            for (int i = 0; i < nonDiscreteSample.IntervalAmount; i++)
            {
                frequencyRange[i] = new double[2];
                frequencyRange[i][0] = intervalLimit;
                intervalLimit += nonDiscreteSample.IntervalStep;
                frequencyRange[i][1] = intervalLimit;

                frequencies[i] = GetFrequency(frequencyRange[i], i == nonDiscreteSample.IntervalAmount - 1);
            }

            return new SampleRange<double[]>(frequencyRange, frequencies);
        }

        private int GetFrequency(double[] range, bool isLast)
        {
            int amount = 0;
            foreach (double el in _sample.VariationRange)
            {
                bool upper = isLast ? el <= range[1] : el < range[1];
                if (el >= range[0] && upper)
                {
                    amount++;
                }
            }

            return amount;
        }

        public override double[] CalculateMode()
        {
            if (nonDiscreteSample.FrequencyRange.Frequencies.All(p => p.Second == nonDiscreteSample.FrequencyRange[0]))
            {
                return Array.Empty<double>();
            }

            List<int> modeIndexes = new List<int>
            {
                0
            };
            int modeIndex = 0;
            for (int i = 1; i < nonDiscreteSample.FrequencyRange.Frequencies.Length; i++)
            {
                if (nonDiscreteSample.FrequencyRange[i] > nonDiscreteSample.FrequencyRange[modeIndex])
                {
                    modeIndex = i;
                    modeIndexes.Clear();
                    modeIndexes.Add(i);
                }
                else if (nonDiscreteSample.FrequencyRange[i] == nonDiscreteSample.FrequencyRange[modeIndex])
                {
                    modeIndexes.Add(i);
                }
            }

            double[] modes = new double[modeIndexes.Count];
            for (int i = 0; i < modeIndexes.Count; i++)
            {
                double mode = nonDiscreteSample.FrequencyRange[modeIndexes[i]];
                double lower = modeIndexes[i] == 0 ? 0 : nonDiscreteSample.FrequencyRange[modeIndexes[i] - 1];
                double upper = modeIndexes[i] == nonDiscreteSample.FrequencyRange.Frequencies.Length - 1
                    ? 0
                    : nonDiscreteSample.FrequencyRange[modeIndexes[i] + 1];
                modes[i] = mode - lower;
                modes[i] /= mode + mode - upper;
                modes[i] *= nonDiscreteSample.IntervalStep;
                modes[i] += nonDiscreteSample.FrequencyRange.Frequencies[modeIndexes[i]].First[0];
            }

            return modes;
        }

        public override double CalculateMedian()
        {
            int medianIndex;
            double sum = 0;
            double threshold = nonDiscreteSample.FrequencyRange.FrequenciesSum / 2.0;
            for (medianIndex = 1; medianIndex < nonDiscreteSample.FrequencyRange.Frequencies.Length; medianIndex++)
            {
                sum += nonDiscreteSample.FrequencyRange[medianIndex - 1];
                if (sum + nonDiscreteSample.FrequencyRange[medianIndex] > threshold)
                {
                    break;
                }
            }

            double median = threshold - sum;
            median /= nonDiscreteSample.FrequencyRange[medianIndex];
            median *= nonDiscreteSample.IntervalStep;
            median += nonDiscreteSample.FrequencyRange.Frequencies[medianIndex].First[0];

            return median;
        }

        public double[] CalculateGroupAverages()
        {
            double[] groupAverages = new double[nonDiscreteSample.FrequencyRange.Frequencies.Length];
            for (int i = 0; i < nonDiscreteSample.FrequencyRange.Frequencies.Length; i++)
            {
                bool isLast = i == nonDiscreteSample.FrequencyRange.Frequencies.Length - 1;
                groupAverages[i] = GetGroupSum(i, isLast) / nonDiscreteSample.FrequencyRange[i];
            }

            return groupAverages;
        }

        public double[] CalculateGroupDispersions()
        {
            double[] groupDispersions = new double[nonDiscreteSample.FrequencyRange.Frequencies.Length];
            for (int i = 0; i < nonDiscreteSample.FrequencyRange.Frequencies.Length; i++)
            {
                bool isLast = i == nonDiscreteSample.FrequencyRange.Frequencies.Length - 1;
                double sampleAvg = nonDiscreteSample.GroupAverages[i];
                foreach (var el in GetGroup(i, isLast))
                {
                    groupDispersions[i] += Math.Pow(el - sampleAvg, 2);
                }

                groupDispersions[i] /= nonDiscreteSample.FrequencyRange[i];
            }

            return groupDispersions;
        }

        public double CalculateAverageGroupDispersion()
        {
            double averageGroupDispersion = 0;
            for (int i = 0; i < nonDiscreteSample.GroupDispersions.Length; i++)
            {
                averageGroupDispersion += nonDiscreteSample.GroupDispersions[i] * nonDiscreteSample.FrequencyRange[i];
            }

            averageGroupDispersion /= nonDiscreteSample.Volume;
            return averageGroupDispersion;
        }

        public double CalculateInterGroupDispersion()
        {
            double interGroupDispersion = 0;
            for (int i = 0; i < nonDiscreteSample.GroupAverages.Length; i++)
            {
                interGroupDispersion += Math.Pow(nonDiscreteSample.GroupAverages[i] - _sample.Average, 2) * nonDiscreteSample.FrequencyRange[i];
            }

            interGroupDispersion /= _sample.Volume;
            return interGroupDispersion;
        }

        public double CalculateDeterminationCoefficient()
        {
            return nonDiscreteSample.InterGroupDispersion / _sample.Dispersion;
        }

        private double GetGroupSum(int groupIndex, bool isLast)
        {
            return GetGroup(groupIndex, isLast).Sum();
        }

        private IEnumerable<double> GetGroup(int groupIndex, bool isLast)
        {
            double[] range = nonDiscreteSample.FrequencyRange.Frequencies[groupIndex].First;
            foreach (var el in _sample.VariationRange)
            {
                bool upper = isLast ? el <= range[1] : el < range[1];
                if (el >= range[0] && upper)
                {
                    yield return el;
                }
            }
        }
    }
}
