using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace At.Matus.BevMetrology
{
    public class SpectralQuantity
    {
        public string Name { get; private set; }
        public double MinWavelength => spectralValues.First().Lambda;
        public double MaxWavelength => spectralValues.Last().Lambda;
        public ColorCoordinates Color => CalculateColor();
        public ColorTemperature ColorTemperature => CalculateCct();

        public SpectralQuantity(string name)
        {
            Name = name.Trim();
        }

        public void AddValue(SpectralQuantityValue spectralValue)
        {
            spectralValues.Add(spectralValue);
            spectralValues.Sort();
        }

        public void AddValue(double wavelength, double value)
        {
            AddValue(new SpectralQuantityValue(wavelength, value));
        }

        public double GetValueFor(double wavelength)
        {
            // linear interpolation between adjecent data points
            if (wavelength < MinWavelength)
                return double.NaN;
            if (wavelength > MaxWavelength)
                return double.NaN;

            int index0 = spectralValues.FindLastIndex(x => x.Lambda <= wavelength);
            if (index0 < 0)
                return double.NaN;
            int index1 = spectralValues.FindIndex(x => x.Lambda >= wavelength);
            if (index1 < 0)
                return double.NaN;

            // we hit a existing wavelength value by chance
            if (index0 == index1)
                return spectralValues[index0].Value;

            double x0 = spectralValues[index0].Lambda;
            double x1 = spectralValues[index1].Lambda;
            double y0 = spectralValues[index0].Value;
            double y1 = spectralValues[index1].Value;

            // two values with the same wavelength
            if (x0 == x1)
                return (y0 + y1) / 2.0;

            double y = y0 + ((wavelength - x0) * (y1 - y0) / (x1 - x0));

            return y;
        }

        public double GetValueFor(int wavelength)
        {
            return GetValueFor((double)wavelength);
        }

        public static SpectralQuantity LoadFromCsv(string filename)
        {
            SpectralQuantity spectrum = new SpectralQuantity(Path.GetFileNameWithoutExtension(filename));
            var reader = new StreamReader(File.OpenRead(filename));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var tokens = line.Split(',', ';', ' ', '\t');
                if (tokens.Length == 2)
                {
                    double x = MyParse(tokens[0]);
                    double y = MyParse(tokens[1]);
                    if (!double.IsNaN(x) && !double.IsNaN(y))
                    {
                        spectrum.AddValue(new SpectralQuantityValue(x, y));
                    }
                }
            }
            reader.Close();
            return spectrum;
        }

        public SpectralQuantity Randomize(double constPart, double relPart)
        {
            var randomizedSpectralQuantity = new SpectralQuantity($"{Name} - randomized");
            foreach (var value in spectralValues)
            {
                double r1 = RandomUtil.GetUniformNoise(relPart);
                double r2 = RandomUtil.GetUniformNoise(constPart);
                double newValue = value.Value * (1.0 + r1) + r2;
                if (newValue < 0) newValue = 0.0;
                randomizedSpectralQuantity.AddValue(value.Lambda, newValue);
            }
            return randomizedSpectralQuantity;
        }

        private static double MyParse(string token)
        {
            if (double.TryParse(token, out double value))
                return value;
            else
                return double.NaN;
        }

        private ColorTemperature CalculateCct()
        {
            double cctTemp = EstimateCCT(Color, 500, 100000, 100);
            double[] tPrec = { 100, 10, 1, 0.1, 0.01 };
            foreach (var deltaT in tPrec)
            {
                cctTemp = EstimateCCT(Color, cctTemp - deltaT, cctTemp + deltaT, deltaT / 10);
            }
            var colorT = CalculateColorPlanck(cctTemp);
            double distance = ChromaDistance(colorT.uPrime, colorT.vPrime, Color.uPrime, Color.vPrime);
            return new ColorTemperature(cctTemp, distance);
        }

        private double EstimateCCT(ColorCoordinates color, double tMin, double tMax, double deltat)
        {
            double distanceMin = double.PositiveInfinity;
            double CCT = double.NaN;

            for (double T = tMin; T <= tMax; T = T + deltat)
            {
                var colorT = SpectralQuantity.CalculateColorPlanck(T);
                double distance = ChromaDistance(colorT.uPrime, colorT.vPrime, color.uPrime, color.vPrime);

                if (distance < distanceMin)
                {
                    distanceMin = distance;
                    CCT = T;
                }
            }
            return CCT;
        }

        private static double ChromaDistance(double up, double vp, double u, double v)
        {
            double us = (up - u) * (up - u);
            double vs = (vp - v) * (vp - v);
            return Math.Sqrt(us + vs * (4 / 9));
        }

        private ColorCoordinates CalculateColor()
        {
            double X2 = BevCie.Integrate(GetValueFor, BevCie.CieX2);
            double Y2 = BevCie.Integrate(GetValueFor, BevCie.CieY2);
            double Z2 = BevCie.Integrate(GetValueFor, BevCie.CieZ2);
            return new ColorCoordinates(X2, Y2, Z2);
        }

        private static ColorCoordinates CalculateColorPlanck(double t)
        {
            double LPlanck(int lamb) => BevCie.LPlanck(t, lamb);
            double X2 = BevCie.Integrate(LPlanck, BevCie.CieX2);
            double Y2 = BevCie.Integrate(LPlanck, BevCie.CieY2);
            double Z2 = BevCie.Integrate(LPlanck, BevCie.CieZ2);
            return new ColorCoordinates(X2, Y2, Z2);
        }       
        
        private readonly List<SpectralQuantityValue> spectralValues = new List<SpectralQuantityValue>();

    }
}
