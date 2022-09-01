namespace At.Matus.BevMetrology
{
    public class DistributionTemperature
    {
        public double TD { get; } = double.NaN;
        public double RelativeDeviation { get; } = double.NaN;

        public DistributionTemperature(double t, double delta)
        {
            TD = t;
            RelativeDeviation = delta;
        }
    }
}
