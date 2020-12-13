using TVIMS_Calculator.Factories;
using TVIMS_Calculator.Interfaces;

namespace TVIMS_Calculator.Implementations
{
    sealed class DiscreteSample : ISample
    {
        public double[] VariationRange { get; }
        public double VariationScope { get; }
        public double VariationCoefficient { get; }
        public int Volume { get; }
        public ISampleRange<double> FrequencyRange { get; }
        public double Average { get; }
        public double[] Mode { get; }
        public double Median { get; }
        public double Dispersion { get; }
        public double Asymmetry { get; }
        public double Excess { get; }
        public ISampleWriter Writer { get; }

        private readonly SampleCalculator _calculator;

        public double GetCenterMoment(int m)
        {
            return _calculator.CalculateCenterMoment(m);
        }

        public DiscreteSample(double[] samples)
        {
            _calculator = new SampleCalculator(this);
            Volume = _calculator.CalculateVolume(samples);
            VariationRange = _calculator.CalculateVariationRange(samples);
            VariationScope = _calculator.CalculateVariationScope();
            FrequencyRange = _calculator.CalculateFrequencyRange();
            Average = _calculator.CalculateAverage();
            Mode = _calculator.CalculateMode();
            Median = _calculator.CalculateMedian();
            Dispersion = _calculator.CalculateDispersion();
            Asymmetry = _calculator.CalculateAsymmetry();
            Excess = _calculator.CalculateExcess();
            VariationCoefficient = _calculator.CalculateVariationCoefficient();
            Writer = SampleWriterFactory.CreateSampleWriter(this);
        }

        public override string ToString()
        {
            return Writer.Info();
        }
    }
}
