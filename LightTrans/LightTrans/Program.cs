using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            
            Console.WriteLine($"File {spectralTransmission.Name} with {spectralTransmission.NumberOfValues}");
            Console.WriteLine($"Wavelength range {spectralTransmission.MinWavelength} nm to{spectralTransmission.MaxWavelength} nm.");

            SpectralQuantity lamp = SpectralQuantity.FromCieIlluminantA();

            return 0;
        }
    }
}
