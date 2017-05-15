using System;

namespace APIVCF
{
	public class Conversions
	{
        public static readonly double densH20at60 = 999.016; // kg/m3 at 60 F
        public static readonly double deltaT60 = 0.01374979547; // Temperature shift value at 60 F
        public static readonly double baseT60 = 60.0068749;
		public static readonly double[] aCoeffs = { -0.148759, -0.267408, 1.080760, 1.269056, -4.089591, -1.871251, 7.438081, -3.536296 };

		public static double TempITS90toITPS68(double deg, string uom = "degF")
		{
			double t = deg;
			bool isF = uom.ToLower() == "degf";
			if (isF)
				t = DegFToDegC(t);
			else if (uom.ToLower() != "degc")
				throw (new ArgumentException("Units of measure {0} not supported - must be degC or degF"));
			double tau = t / 630;
			double deltaT = aCoeffs[aCoeffs.Length - 1];
			for (int i = aCoeffs.Length - 2; i >= 0; i--)
			{
				deltaT = aCoeffs[i] + deltaT * tau;
			}
            deltaT = deltaT * tau; // One last time
			t = t - deltaT;
			if (isF)
				return DegCToDegF(t);
			return t;
		}

        public static double Api60ITS90tokgm3ITPS68(double api60, KCoeffs coeffs)
        {
            if (coeffs == null)
                throw (new ArgumentNullException(nameof(coeffs),new ArgumentException("Must pass and initialize coffs object")));

            // Get density in kg/m3
            double rho60 = Conversions.APItoKgm3(api60);

            // Section 11.1.6.1 Step 3 
            double A = (deltaT60 / 2.0) * ((coeffs.k0 / rho60 + coeffs.k1) / rho60 + coeffs.k2);
            double B = (2 * coeffs.k0 + coeffs.k1 * rho60) / (coeffs.k0 + (coeffs.k1 + coeffs.k2 * rho60) * rho60);
            double rhoITPS68 = rho60 * (1 + (Math.Exp(A * (1 + 0.8 * A)) - 1)/(1+A*(1+1.6*A)*B));

            return rhoITPS68;
        }

		public static double DegCToDegF(double degC)
		{
			return 1.8 * degC + 32;
		}

		public static double DegFToDegC(double degF)
		{
			return (degF - 32) / 1.8;
		}

		public static double APItoSG(double api)
		{
			return 141.5 / (131.5 + api);
		}

		public static double SGtoAPI(double sg)
		{
			return 141.5 / sg - 131.5;
		}

        public static double PSItokPa(double psi)
        {
            return 6.895 * psi;
        }

        public static double kPatoPSI(double kpa)
        {
            return kpa / 6.895;
        }

        public static double PSItoBar(double psi)
        {
            return 0.069 * psi;
        }

        public static double BarToPSI(double bar)
        {
            return bar / 0.069;
        }

        public static double CoeffThermExpFtoC(double invDegF)
        {
            return 1.8/invDegF;
        }

        public static double CoeffThermExpCtoF(double invDegC)
        {
            return invDegC/1.8;
        }

        public static double APItoKgm3(double api)
        {
            var sg = APItoSG(api);
            return densH20at60 * sg;
        }

        public static double Kgm3toAPI(double kgm3)
        {
            var sg = kgm3 / densH20at60;
            return SGtoAPI(sg);
        }
	}
}
