namespace At.Matus.BevMetrology
{
    public class ColorTemperature
    {
        public double CCT { get; } = double.NaN;
        public double ChomaticityDifference { get; } = double.NaN;
        public CctApplicability Status => AssessStatus();

        public ColorTemperature(double t, double delta)
        {
            CCT = t;
            ChomaticityDifference = delta;
        }

        private CctApplicability AssessStatus()
        {
            if (double.IsNaN(CCT)) return CctApplicability.Unknown;
            if (double.IsNaN(ChomaticityDifference)) return CctApplicability.Unknown;
            if(ChomaticityDifference <= 5e-4) return CctApplicability.ColorTemperature;
            if (ChomaticityDifference <= 5e-2) return CctApplicability.CorrelatedColorTemperature;
            return CctApplicability.NotApplicable;
        }

    }


}
