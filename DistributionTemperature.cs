namespace At.Matus.BevMetrology
{
    public class DistributionTemperature
    {
        public double TD { get; } = double.NaN;
        public double RelativeDeviation { get; } = double.NaN;
        public TdApplicability Status => AssessStatus();

        public DistributionTemperature(double t, double delta)
        {
            TD = t;
            RelativeDeviation = delta;
        }

        private TdApplicability AssessStatus()
        {
            if (double.IsNaN(TD)) return TdApplicability.Unknown;
            if (double.IsNaN(RelativeDeviation)) return TdApplicability.Unknown;
            if (RelativeDeviation <= 0.10) return TdApplicability.DistributionTemperature;
            return TdApplicability.NotApplicable;
        }
    }


}
