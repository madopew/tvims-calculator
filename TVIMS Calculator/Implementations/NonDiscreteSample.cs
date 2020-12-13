using TVIMS_Calculator.Factories;
using TVIMS_Calculator.Implementations;
using TVIMS_Calculator.Interfaces;

namespace TVIMS_Calculator
{
    sealed class NonDiscreteSample : ISample
    {
        public double[] VariationRange { get; }
        public double VariationScope { get; }
        public double VariationCoefficient { get; }
        public int Volume { get; }
        public ISampleRange<double[]> FrequencyRange { get; }
        public int IntervalAmount { get; }
        public double IntervalStep { get; }
        public double Average { get; }
        public double[] Mode { get; }
        public double Median { get; }
        public double Dispersion { get; }
        public double Asymmetry { get; }
        public double Excess { get; }
        public double[] GroupAverages { get; set; }
        public double[] GroupDispersions { get; }
        public double AverageGroupDispersion { get; }
        public double InterGroupDispersion { get; }
        public double DeterminationCoefficient { get; }
        public ISampleWriter Writer { get; }

        private readonly NonDiscreteSampleCalculator _calculator;

        public double GetCenterMoment(int m)
        {
            return _calculator.CalculateCenterMoment(m);
        }

        public NonDiscreteSample(double[] samples)
        {
            _calculator = new NonDiscreteSampleCalculator(this);
            Volume = _calculator.CalculateVolume(samples);
            VariationRange = _calculator.CalculateVariationRange(samples);
            VariationScope = _calculator.CalculateVariationScope();
            IntervalAmount = _calculator.CalculateIntervalAmount();
            IntervalStep = _calculator.CalculateIntervalStep();
            FrequencyRange = _calculator.CalculateFrequencyRange();
            Average = _calculator.CalculateAverage();
            Mode = _calculator.CalculateMode();
            Median = _calculator.CalculateMedian();
            Dispersion = _calculator.CalculateDispersion();
            GroupAverages = _calculator.CalculateGroupAverages();
            GroupDispersions = _calculator.CalculateGroupDispersions();
            AverageGroupDispersion = _calculator.CalculateAverageGroupDispersion();
            InterGroupDispersion = _calculator.CalculateInterGroupDispersion();
            DeterminationCoefficient = _calculator.CalculateDeterminationCoefficient();
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
