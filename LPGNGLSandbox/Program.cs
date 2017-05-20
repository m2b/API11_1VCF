using System;

namespace APIVCF
{
    class Program
    {
        static void Main(string[] args)
        {
            // Section 12.2 Example 5 in MPSM 11.2.2 
            double api60 = Conversions.SGtoAPI(0.5050);
            double temp = 88.1;
            double vapPress = 159.5;  // At temp=88.1
            double press = 425;

			// Get compressibility factor
			Calcs calc = new Calcs();
            ComputeCompress(calc, api60, temp, press, vapPress, cplCheck: 1.0133, fCheck:4.9285e-5);

            // Section 11.2.2 - 11.2.2.8.3M Example
            api60 = Conversions.SGtoAPI(0.5300);
			temp = Conversions.DegCtoDegF(5.0);
            ComputeCompress(calc, api60, temp, press, vapPress,aCheck:281093,bCheck:5.504);

		}

        static void ComputeCompress(Calcs calc, double api60, double tempF, double pressPsi, double vapPressPsi, double cplCheck=0,double fCheck=0,double aCheck=0,double bCheck=0)
        {
            double A;
            double B;
            double fp = calc.GetCompressFactorLiqGas(tempF, api60, pressPsi, vapPressPsi,out A,out B);
			Console.WriteLine("Compressibility Factor={0} for Commodity Group {1}", fp, COMMODITY_GROUP.LPG_NGL);
			// TODO NOT ABLE TO REPRODUCE THIS RESULT: fp = 4.9285e-5;
			Console.WriteLine("Print A={0} and B={1}", A, B);
            Console.WriteLine("Print A={0} for Dp in kPa", Conversions.PSItokPa(A));

			double CPL = calc.GetCPLLiquidGas(fp, pressPsi, vapPressPsi);
            CPL = calc.RoundUpAPI11_1(CPL, "cpl");
            Console.WriteLine("Pressure Correction CPL={0} for Commodity Group {1}", CPL, COMMODITY_GROUP.LPG_NGL);
            // TODO NOT ABLE TO REPRODUCE THIS RESULT: cpl = 1.0133


            // Checks
            if (cplCheck != 0)
                Console.WriteLine("CPL Expected {0}, Actual {1} differ by {2}%", cplCheck, CPL, 100*Math.Abs(CPL - cplCheck) / cplCheck);
            if (fCheck != 0)
				Console.WriteLine("Fp Expected {0}, Actual {1} differ by {2}%", fCheck, fp, 100*Math.Abs(fp - fCheck) / fCheck);
            if(aCheck!=0)
                Console.WriteLine("A Expected {0}, Actual {1} differ by {2}%", aCheck, Conversions.PSItokPa(A), 100*Math.Abs(Conversions.PSItokPa(A) - aCheck) / aCheck);
			if (aCheck != 0)
				Console.WriteLine("B Expected {0}, Actual {1} differ by {2}%", bCheck, B, 100*Math.Abs(B - bCheck) / aCheck);
		}
    }
}
