using System;
using TVIMS_Calculator.Implementations;
using TVIMS_Calculator.Interfaces;

namespace TVIMS_Calculator.Factories
{
    static class SampleWriterFactory
    {
        public static ISampleWriter CreateSampleWriter(ISample sample)
        {
            return sample switch
            {
                NonDiscreteSample _ => new NonDiscreteSampleWriter((NonDiscreteSample)sample),
                DiscreteSample _ => new DiscreteSampleWriter((DiscreteSample)sample),
                _ => throw new ArgumentException("Invalid type"),
            };
        }
    }
}
