using System;

namespace APIVCF
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check roundups
            double t = 5.34;
            string uom = "degC";
            Calcs calc = new Calcs();
            Console.WriteLine("For temp of {0} {1} roundup is {2} {1}",t,uom,calc.RoundUp(t,uom));
            t = -5.34;
            Console.WriteLine("For temp of {0} {1} roundup is {2} {1}", t, uom, calc.RoundUp(t, uom));
            t = 10.05;
            uom = "degF";
            Console.WriteLine("For temp of {0} {1} roundup is {2} {1}", t, uom, calc.RoundUp(t, uom));


            // Check temperature conversions
            t = 32;
            Console.WriteLine("Temp {0} in degF converts to {1} in degC",t,Conversions.DegFToDegC(t));
			t = -40;
			Console.WriteLine("Temp {0} in degF converts to {1} in degC", t, Conversions.DegFToDegC(t));
			t = 60;
			Console.WriteLine("Temp {0} in degF converts to {1} in degC", t, Conversions.DegFToDegC(t));
			t = 212;
			Console.WriteLine("Temp {0} in degF converts to {1} in degC", t, Conversions.DegFToDegC(t));
			t = 0;
			Console.WriteLine("Temp {0} in degC converts to {1} in degF", t, Conversions.DegCToDegF(t));
			t = -40;
			Console.WriteLine("Temp {0} in degC converts to {1} in degF", t, Conversions.DegCToDegF(t));
			t = 15;
			Console.WriteLine("Temp {0} in degC converts to {1} in degF", t, Conversions.DegCToDegF(t));
			t = 100;
			Console.WriteLine("Temp {0} in degC converts to {1} in degF", t, Conversions.DegCToDegF(t));
			t = 32;
            uom = "degF";
            Console.WriteLine("Temp {0} in {1} ITS90 converts to {2} {1} ITPS68 - rounded up", t, uom,calc.RoundUp(Conversions.TempITS90toITPS68(t,uom),uom));
			t = -40;
            Console.WriteLine("Temp {0} in {1} ITS90 converts to {2} {1} ITPS68 - rounded up", t, uom,calc.RoundUp(Conversions.TempITS90toITPS68(t, uom),uom));
			t = 60;
            Console.WriteLine("Temp {0} in {1} ITS90 converts to {2} {1} ITPS68 - rounded up", t, uom,calc.RoundUp(Conversions.TempITS90toITPS68(t, uom),uom));
			t = 212;
			Console.WriteLine("Temp {0} in {1} ITS90 converts to {2} {1} ITPS68", t, uom, Conversions.TempITS90toITPS68(t, uom));
			t = 0;
            uom = "degC";
			Console.WriteLine("Temp {0} in {1} ITS90 converts to {2} {1} ITPS68", t, uom, Conversions.TempITS90toITPS68(t, uom));
			t = -40;
			Console.WriteLine("Temp {0} in {1} ITS90 converts to {2} {1} ITPS68", t, uom, Conversions.TempITS90toITPS68(t, uom));
			t = 15;
			Console.WriteLine("Temp {0} in {1} ITS90 converts to {2} {1} ITPS68", t, uom, Conversions.TempITS90toITPS68(t, uom));
			t = 100;
			Console.WriteLine("Temp {0} in {1} ITS90 converts to {2} {1} ITPS68", t, uom, Conversions.TempITS90toITPS68(t, uom));

            // Check Density Conversions
            double d = 10;
            uom = "API";
            Console.WriteLine("Density {0} in {1} converts to {2} in SG",d,uom,Conversions.APItoSG(d));
            Console.WriteLine("Density {0} in {1} converts to {2} in kg/m3", d, uom, Conversions.APItoKgm3(d));
            d = 1;
            uom = "SG";
			Console.WriteLine("Density {0} in {1} converts to {2} in API", d, uom, Conversions.SGtoAPI(d));
            Console.WriteLine("Density {0} in {1} converts to {2} in kg/m3", d, uom, Conversions.APItoKgm3(Conversions.SGtoAPI(d)));
            d = 999.016;
            uom = "kg/m3";
            Console.WriteLine("Density {0} in {1} converts to {2} in API", d, uom, Conversions.Kgm3toAPI(d));
            Console.WriteLine("Density {0} in {1} converts to {2} in SG", d, uom, Conversions.APItoSG(Conversions.Kgm3toAPI(d)));
			d = 610.6;
			Console.WriteLine("Density {0} in {1} converts to {2} in API", d, uom, Conversions.Kgm3toAPI(d));
            Console.WriteLine("Density {0} in {1} converts to {2} in SG", d, uom, Conversions.APItoSG(Conversions.Kgm3toAPI(d)));
			d = 800;
			Console.WriteLine("Density {0} in {1} converts to {2} in API", d, uom, Conversions.Kgm3toAPI(d));
			Console.WriteLine("Density {0} in {1} converts to {2} in SG", d, uom, Conversions.APItoSG(Conversions.Kgm3toAPI(d)));

            // Get kCoeffs and print them
            var grp = COMMODITY_GROUP.CRUDE_OIL;
            var kc = calc.GetKCoeffs(grp);
            Console.WriteLine("Commodity Group {0} has k0={1}, k1={2}, k2={3}",kc.CommodityGroup,kc.k0,kc.k1,kc.k2);
            grp = COMMODITY_GROUP.FUEL_OILS;
            kc=calc.GetKCoeffs(grp);
			Console.WriteLine("Commodity Group {0} has k0={1}, k1={2}, k2={3}", kc.CommodityGroup, kc.k0, kc.k1, kc.k2);
            grp = COMMODITY_GROUP.GASOLINES;
			kc = calc.GetKCoeffs(grp);
			Console.WriteLine("Commodity Group {0} has k0={1}, k1={2}, k2={3}", kc.CommodityGroup, kc.k0, kc.k1, kc.k2);
			grp = COMMODITY_GROUP.JET_FUELS;
			kc = calc.GetKCoeffs(grp);
			Console.WriteLine("Commodity Group {0} has k0={1}, k1={2}, k2={3}", kc.CommodityGroup, kc.k0, kc.k1, kc.k2);
            grp = COMMODITY_GROUP.LUBRICATING_OIL;
			kc = calc.GetKCoeffs(grp);
			Console.WriteLine("Commodity Group {0} has k0={1}, k1={2}, k2={3}", kc.CommodityGroup, kc.k0, kc.k1, kc.k2);
			grp = COMMODITY_GROUP.TRANSITION_ZONE;
			kc = calc.GetKCoeffs(grp);
			Console.WriteLine("Commodity Group {0} has k0={1}, k1={2}, k2={3}", kc.CommodityGroup, kc.k0, kc.k1, kc.k2);

            // Validate range checks
            double pres = 0;
            double temp = 0;
            double dens = 800;
            double api = Conversions.Kgm3toAPI(dens);
            grp = COMMODITY_GROUP.CRUDE_OIL;
			Console.WriteLine("Testing for commodity group {0}", grp);
            double vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);  // All good
            Console.WriteLine("Values in range test completed");
            // Temp to low
            temp = -58.1;
            try
            {
                vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);
                Console.WriteLine("Test for low temp for {0} failed", grp);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("Temp {0} is too low", temp);
            }
            // Temp to high
			temp = 302.1;
			try
			{
				vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);
				Console.WriteLine("Test for high temp for {0} failed", grp);
			}
			catch (ArgumentOutOfRangeException e) {
                Console.WriteLine("Temp {0} is too high",temp); 
            }
			temp = 0;
            // Press to low
			pres = -0.1;
            try
            {
                vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);
                Console.WriteLine("Test for low press for {0} failed", grp);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("Press {0} is too low", pres);
            }
            // Press to high
			pres = 1500.1;
            try
            {
                vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);
                Console.WriteLine("Test for high press for {0} failed", grp);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("Press {0} is too high", pres);

            }
			// Density too low
			dens = 610.5;
			api = Conversions.Kgm3toAPI(dens);
			try
			{
				vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);
				Console.WriteLine("Test for low density for {0} failed", grp);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.WriteLine("Density {0} is too low", dens);
			}
			// Density too high
			dens = 1163.5;
			api = Conversions.Kgm3toAPI(dens);
			try
			{
				vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);
				Console.WriteLine("Test for high density for {0} failed", grp);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.WriteLine("Density {0} is too high", dens);
			}
			pres = 0;
			temp = 0;
            dens = 900;
			api = Conversions.Kgm3toAPI(dens);
			grp = COMMODITY_GROUP.FUEL_OILS;
			Console.WriteLine("Testing for commodity group {0}", grp);
			vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);  // All good
			Console.WriteLine("Values in range test completed");
			// Density too low
			dens = 818.3126;
			api = Conversions.Kgm3toAPI(dens);
			try
			{
				vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);
				Console.WriteLine("Test for low density for {0} failed", grp);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.WriteLine("Density {0} is too low", dens);
			}
			// Density too high
			dens = 1163.6;
			api = Conversions.Kgm3toAPI(dens);
			try
			{
				vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);
				Console.WriteLine("Test for high density for {0} failed", grp);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.WriteLine("Density {0} is too high", dens);
			}
            dens = 800;
			api = Conversions.Kgm3toAPI(dens);
            grp = COMMODITY_GROUP.JET_FUELS;
			Console.WriteLine("Testing for commodity group {0}", grp);
			vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);  // All good
			Console.WriteLine("Values in range test completed");
			// Density too low
			dens = 787.5194;
			api = Conversions.Kgm3toAPI(dens);
			try
			{
				vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);
				Console.WriteLine("Test for low density for {0} failed", grp);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.WriteLine("Density {0} is too low", dens);
			}
			// Density too high
			dens = 838.3127;
			api = Conversions.Kgm3toAPI(dens);
			try
			{
				vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);
				Console.WriteLine("Test for high density for {0} failed", grp);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.WriteLine("Density {0} is too high", dens);
			}
			dens = 780;
            api = Conversions.Kgm3toAPI(dens);
			grp = COMMODITY_GROUP.TRANSITION_ZONE;
			Console.WriteLine("Testing for commodity group {0}", grp);
			vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);  // All good
			Console.WriteLine("Values in range test completed");
			// Density too low
			dens = 770.351;
			api = Conversions.Kgm3toAPI(dens);
			try
			{
				vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);
				Console.WriteLine("Test for low density for {0} failed", grp);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.WriteLine("Density {0} is too low", dens);
			}
			// Density too high
			dens = 787.5195;
			api = Conversions.Kgm3toAPI(dens);
			try
			{
				vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);
				Console.WriteLine("Test for high density for {0} failed", grp);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.WriteLine("Density {0} is too high", dens);
			}
			dens = 650;
            api = Conversions.Kgm3toAPI(dens);
            grp = COMMODITY_GROUP.GASOLINES;
			Console.WriteLine("Testing for commodity group {0}", grp);
			vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);  // All good
			Console.WriteLine("Values in range test completed");
			// Density too low
			dens = 610.5;
			api = Conversions.Kgm3toAPI(dens);
			try
			{
				vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);
				Console.WriteLine("Test for low density for {0} failed", grp);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.WriteLine("Density {0} is too low", dens);
			}
			// Density too high
			dens = 770.352;
			api = Conversions.Kgm3toAPI(dens);
			try
			{
				vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);
				Console.WriteLine("Test for high density for {0} failed", grp);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.WriteLine("Density {0} is too high", dens);
			}
			dens = 900;
            api = Conversions.Kgm3toAPI(dens);
            grp = COMMODITY_GROUP.LUBRICATING_OIL;
			Console.WriteLine("Testing for commodity group {0}", grp);
			vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);  // All good
			Console.WriteLine("Values in range test completed");
			// Density too low
			dens = 800.8;
			api = Conversions.Kgm3toAPI(dens);
			try
			{
				vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);
				Console.WriteLine("Test for low density for {0} failed", grp);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.WriteLine("Density {0} is too low", dens);
			}
			// Density too high
			dens = 1163.5;
			api = Conversions.Kgm3toAPI(dens);
			try
			{
				vcf = calc.GetCTPLFromAPIDegFPsig(grp, api, temp, pres);
				Console.WriteLine("Test for high density for {0} failed", grp);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.WriteLine("Density {0} is too high", dens);
			}

			Console.WriteLine("Press ENTER to terminate");
            Console.ReadLine();
        }
    }
}
