using System;

namespace TVIMS_Calculator
{
    class NonDiscreteRange : IRange<double[]>
    {
        public Pair<double[], int>[] Frequencies { get; private set; }
        public int FrequenciesSum { get; private set; }
        public int this[int index] => Frequencies[index].Second;

        public NonDiscreteRange(double[][] ranges, int[] frequencies)
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

            this.Frequencies = new Pair<double[], int>[ranges.Length];
            for (int i = 0; i < ranges.Length; i++)
            {
                FrequenciesSum += frequencies[i];
                var p = new Pair<double[], int>(ranges[i], frequencies[i]);
                Frequencies[i] = p;
            }
        }
    }
}
