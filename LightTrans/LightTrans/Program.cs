using System;
using At.Matus.BevMetrology;

namespace LightTrans
{
    class Program
    {
        static int Main(string[] args)
        {


            #region filename logic
            if (args.Length==0)
            {
                Console.WriteLine("no file name provided!");
                return 1;
            }
            string filename = args[0];
            #endregion

            SpectralQuantity spectralTransmission = SpectralQuantity.LoadFromCsv(filename);
            
            Console.WriteLine($"File {spectralTransmission.Name} with {spectralTransmission.NumberOfValues} values.");
            Console.WriteLine($"Wavelength range {spectralTransmission.MinWavelength} nm to {spectralTransmission.MaxWavelength} nm.");
            Console.WriteLine();

            double z = BevCie.Integrate(spectralTransmission.GetValueFor, BevCie.CieIlluminantA, BevCie.CieV);
            double n = BevCie.Integrate(BevCie.CieIlluminantA, BevCie.CieV);
            double lightTransmission = z / n;

            Console.WriteLine($"Light transmission factor: {lightTransmission}");
            Console.WriteLine();
            
            return 0;
        }
    }
}
