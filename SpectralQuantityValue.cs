using System;

namespace At.Matus.BevMetrology
{
    public class SpectralQuantityValue : IComparable<SpectralQuantityValue>
    {
        public double Lambda { get; }
        public double Value { get; }

        public SpectralQuantityValue(double lambda, double value)
        {
            Lambda = lambda;
            Value = value;
        }

        public int CompareTo(SpectralQuantityValue other) => Lambda.CompareTo(other.Lambda);
    }
}
