namespace At.Matus.BevMetrology
{
    public class ColorTemperature
    {
        public double CCT { get; } = double.NaN;
        public double ChomaticityDifference { get; } = double.NaN;
        public RadiationTemperatureStatus Status => AssessStatus();

        public ColorTemperature(double t, double delta)
        {
            CCT = t;
            ChomaticityDifference = delta;
        }

        private RadiationTemperatureStatus AssessStatus()
        {
            if (double.IsNaN(CCT)) return RadiationTemperatureStatus.Unknown;
            if (double.IsNaN(ChomaticityDifference)) return RadiationTemperatureStatus.Unknown;
            if (ChomaticityDifference <= 5e-4) return RadiationTemperatureStatus.ValidColorTemperature;
            if (ChomaticityDifference <= 5e-2) return RadiationTemperatureStatus.ValidCorrelatedColorTemperature;
            return RadiationTemperatureStatus.NotValid;
        }

    }


}
