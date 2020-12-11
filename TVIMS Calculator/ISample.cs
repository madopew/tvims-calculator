using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TVIMS_Calculator
{
    interface ISample<T>
    {
        double[] VariationRange { get; }
        int Volume { get; }
        IRange<T> FrequencyRange { get; }
        double Average { get; }
        double Mode { get; }
        double Median { get; }
        double Dispersion { get; }
        double ExpectedValue { get; }
        double Asymmetry { get; }
        double Excess { get; }
        string GetDistributionFunction();
        string GetRange();
    }
}
