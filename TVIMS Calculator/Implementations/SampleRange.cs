using System;
using TVIMS_Calculator.Interfaces;

namespace TVIMS_Calculator.Implementations
{
    class SampleRange<T> : ISampleRange<T>
    {
        public Pair<T, int>[] Frequencies { get; }
        public int FrequenciesSum { get; }
        public int this[int index] => Frequencies[index].Second;

        public SampleRange(T[] ranges, int[] frequencies)
        {
            if (ranges is null)
            {
                throw new ArgumentNullException(nameof(ranges));
            }

            if (frequencies is null)
            {
                throw new ArgumentNullException(nameof(frequencies));
            }

            if (ranges.Length != frequencies.Length)
            {
                throw new ArgumentException("Ranges and frequencies length are not same", nameof(ranges));
            }

            Frequencies = new Pair<T, int>[ranges.Length];
            for (int i = 0; i < ranges.Length; i++)
            {
                FrequenciesSum += frequencies[i];
                var p = new Pair<T, int>(ranges[i], frequencies[i]);
                Frequencies[i] = p;
            }
        }
    }
}
