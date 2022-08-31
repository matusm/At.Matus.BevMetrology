namespace At.Matus.BevMetrology
{
    public class DistributionTemperature
    {
        public double Td { get; } = double.NaN;
        public double RelativeDeviation { get; } = double.NaN;

        public DistributionTemperature(double t, double delta)
        {
            Td = t;
            RelativeDeviation = delta;
        }
    }
}
