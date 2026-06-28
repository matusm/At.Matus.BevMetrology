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
            string filename = string.Empty;
            if (args.Length==0)
            {
                filename = @"L:\BEV\e2ref3\Fotometrie\Untersuchungen\2026_CSS45_Untersuchungen\Auswertung_Kalibrierung_CSS-45_V2_LampeOrange.csv";
                //Console.WriteLine("no file name provided!");
                //return 1;
            }
            if (args.Length == 1)
                filename = args[0];
            #endregion

            SpectralQuantity spectralIrradiance = SpectralQuantity.LoadFromCsv(filename);

            Console.WriteLine();
            Console.WriteLine($"File '{spectralIrradiance.Name}' with {spectralIrradiance.NumberOfValues} values.");
            Console.WriteLine($"Wavelength range: {spectralIrradiance.MinWavelength} nm to {spectralIrradiance.MaxWavelength} nm.");
            Console.WriteLine($"Value range: {spectralIrradiance.MinValue} to {spectralIrradiance.MaxValue}");
            Console.WriteLine();

            var color = spectralIrradiance.Color;
            Console.WriteLine($"Color (CIE 1931): x = {color.x:F4}, y = {color.y:F4}");
            Console.WriteLine($"Correlated Color Temperature (CCT): {spectralIrradiance.CCT:F0} K");
            Console.WriteLine($"Distribution temperature (400 nm - 750nm): {spectralIrradiance.TD:F0} K");

            //double z = BevCie.Integrate(spectralIrradiance.GetValueFor, BevCie.CieIlluminantA, BevCie.CieV);
            //double n = BevCie.Integrate(BevCie.CieIlluminantA, BevCie.CieV);
            //double lightTransmission = z / n;
            //Console.WriteLine($"Light transmission factor (Illuminant A, V_lambda): {lightTransmission:F4} %");
            Console.WriteLine();

            #region Pretty useless Monte Carlo simulation
            //double uFixed = 0.15;
            //int nTries = 100;
            //StatisticPod sp = new StatisticPod();
            //SpectralQuantity specMC;
            //Console.WriteLine("Pretty useless Monte Carlo simulation");
            //for (int i = 0; i <= nTries; i++)
            //{
            //    specMC = spectralTransmission.Randomize(uFixed, 0.0);
            //    if (i % 10 == 0)
            //        Console.Write("|");
            //    else
            //        Console.Write("-");
            //    double zMC = BevCie.Integrate(specMC.GetValueFor, BevCie.CieIlluminantA, BevCie.CieV);
            //    sp.Update(zMC / n);
            //}
            //Console.WriteLine();
            //Console.WriteLine($"MC (u_fixed = {uFixed:F2} %) : {sp.AverageValue:F4} ({2*sp.StandardDeviation:F4}) %");
            //Console.WriteLine();
            #endregion

            return 0;
        }
    }
}
