namespace At.Matus.BevMetrology
{
    public class ColorTemperature
    {
        public double Cct { get; } = double.NaN;
        public double ChomaticityDifference { get; } = double.NaN;
        public Applicability Status => CheckDifference();

        public ColorTemperature(double t, double delta)
        {
            Cct = t;
            ChomaticityDifference = delta;
        }

        private Applicability CheckDifference()
        {
            if (double.IsNaN(Cct)) return Applicability.Unknown;
            if (double.IsNaN(ChomaticityDifference)) return Applicability.Unknown;
            if(ChomaticityDifference <= 5e-4) return Applicability.ColorTemperature;
            if (ChomaticityDifference <= 5e-2) return Applicability.CorrelatedColorTemperature;
            return Applicability.NotApplicable;
        }

    }

    public enum Applicability
    {
        Unknown,
        ColorTemperature,
        CorrelatedColorTemperature,
        NotApplicable
    }
}
