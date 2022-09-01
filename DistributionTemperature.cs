namespace At.Matus.BevMetrology
{
    public class DistributionTemperature
    {
        public double TD { get; } = double.NaN;
        public double RelativeDeviation { get; } = double.NaN;
        public RadiationTemperatureStatus Status => AssessStatus();

        public DistributionTemperature(double t, double delta)
        {
            TD = t;
            RelativeDeviation = delta;
        }

        private RadiationTemperatureStatus AssessStatus()
        {
            if (double.IsNaN(TD)) return RadiationTemperatureStatus.Unknown;
            if (double.IsNaN(RelativeDeviation)) return RadiationTemperatureStatus.Unknown;
            if (RelativeDeviation <= 0.10) return RadiationTemperatureStatus.ValidDistributionTemperature;
            return RadiationTemperatureStatus.NotValid;
        }
    }


}
