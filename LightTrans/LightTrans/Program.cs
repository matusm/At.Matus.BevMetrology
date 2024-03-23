using System;
using System.Globalization;
using At.Matus.BevMetrology;
using At.Matus.StatisticPod;

namespace LightTrans
{
    class Program
    {
        static int Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            #region filename logic
            if (args.Length==0)
            {
                Console.WriteLine("no file name provided!");
                return 1;
            }
            string filename = args[0];
            #endregion

            SpectralQuantity spectralTransmission = SpectralQuantity.LoadFromCsv(filename);

            Console.WriteLine();
            Console.WriteLine($"File '{spectralTransmission.Name}' with {spectralTransmission.NumberOfValues} values.");
            Console.WriteLine($"Wavelength range: {spectralTransmission.MinWavelength} nm to {spectralTransmission.MaxWavelength} nm.");
            Console.WriteLine($"Value range: {spectralTransmission.MinValue} % to {spectralTransmission.MaxValue} %.");
            Console.WriteLine();

            double z = BevCie.Integrate(spectralTransmission.GetValueFor, BevCie.CieIlluminantA, BevCie.CieV);
            double n = BevCie.Integrate(BevCie.CieIlluminantA, BevCie.CieV);
            double lightTransmission = z / n;

            Console.WriteLine($"Light transmission factor (Illuminant A, V_lambda): {lightTransmission:F4} %");
            Console.WriteLine();

            #region Pretty useless Monte Carlo simulation
            double uFixed = 0.15;
            int nTries = 100;
            StatisticPod sp = new StatisticPod();
            SpectralQuantity specMC;
            Console.WriteLine("Pretty useless Monte Carlo simulation");
            for (int i = 0; i <= nTries; i++)
            {
                specMC = spectralTransmission.Randomize(uFixed, 0.0);
                if (i % 10 == 0)
                    Console.Write("|");
                else
                    Console.Write("-");
                double zMC = BevCie.Integrate(specMC.GetValueFor, BevCie.CieIlluminantA, BevCie.CieV);
                sp.Update(zMC / n);
            }
            Console.WriteLine();
            Console.WriteLine($"MC (u_fixed = {uFixed:F2} %) : {sp.AverageValue:F4} ({2*sp.StandardDeviation:F4}) %");
            Console.WriteLine();
            #endregion

            return 0;
        }
    }
}
