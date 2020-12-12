using System;
using TVIMS_Calculator.Implementations;

namespace TVIMS_Calculator
{
    static class SampleFactory
    {
        public static ISample CreateSample(double[] samples, bool isDiscrete)
        {
            if (!isDiscrete)
            {
                return new NonDiscreteSample(samples);
            }

            return new DiscreteSample(samples);
        }
    }
}
