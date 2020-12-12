namespace TVIMS_Calculator
{
    interface ISample
    {
        double[] VariationRange { get; }
        double VariationScope { get; }
        double VariationCoefficient { get; }
        int Volume { get; }
        double Average { get; }
        double Mode { get; }
        double Median { get; }
        double Dispersion { get; }
        double Asymmetry { get; }
        double Excess { get; }
        double GetCenterMoment(int m);
        string GetDistributionFunction();
        string GetRange();
        void OutputInfo();
    }
}
