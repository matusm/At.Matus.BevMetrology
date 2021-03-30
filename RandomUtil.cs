using System;

namespace At.Matus.BevMetrology
{
    public static class RandomUtil
    {
        private static Random rnd = new Random();

        // uniformaly distributed in [-1, 1]
        public static double GetUniformNoise()
        {
            return (rnd.NextDouble() * 2.0) - 1.0;
        }

        public static double GetUniformNoise(double hwhm)
        {
            return GetUniformNoise() * hwhm;
        }

        public static double GetGaussianNoise(double sigma)
        {
            double u1 = rnd.NextDouble();
            double u2 = rnd.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            double r = randStdNormal * sigma;
            return r;
        }




    }
}
