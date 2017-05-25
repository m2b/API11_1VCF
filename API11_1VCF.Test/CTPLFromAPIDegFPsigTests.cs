using System;
using Xunit;
using System.Collections.Generic;
using System.Collections;

namespace APIVCF
{
    public class CTPLFromAPIDegFPsigTests
    {
        // TODO: Add 5 additional tests from API Test 7 - 11
		public class TestData
        {
            // Inputs
            public double TempF;
            public double PressPsig;
            public COMMODITY_GROUP Grp;
            public double Api60;

            // Intermediate results
            public double K0;
            public double K1;
            public double K2;
            public double TempITPS68;
            public double DensITSP68;
            public double ThermExpCoeff60;
            public double CompressFactor;

            // Results
            public double CTL;
            public double CPL;
            public double CTPL;
		}


        public class TestDataGenerator
		{
            public static IEnumerable<object[]> GetTestExamples()
            {
                yield return new object[]
                {
                    // Section 11.1.6.1 - Example 1
                    new TestData {
                        TempF=-27.7,
                        PressPsig=0,
                        Grp=COMMODITY_GROUP.CRUDE_OIL,
                        Api60=17.785,
                        K0=341.095700000000,
                        K1=0.000000000000,
                        K2=0.000000000000,
                        TempITPS68=-27.712499233089,
                        DensITSP68=946.921215770785,
                        ThermExpCoeff60=0.000380407044,
                        CompressFactor=0.305779891997,
                        CTL=1.033011591958,
                        CPL=1.000000000000,
                        CTPL=1.03301
                    },
                    // Section 11.1.6.1 - Example 2
                    new TestData
                    {
                        TempF=301.93,
                        PressPsig=1500,
                        Grp=COMMODITY_GROUP.CRUDE_OIL,
                        Api60=-10,
                        K0=341.095700000000,
                        K1=0.000000000000,
                        K2=0.000000000000,
                        TempITPS68=301.993163042978,
                        DensITSP68=1.16346509372e+03,
                        ThermExpCoeff60=0.000251982006,
                        CompressFactor=0.427958509999,
                        CTL=0.938051116886,
                        CPL=1.006460852301,
                        CTPL=0.94411
                    },
                    // Section 11.1.6.1 - Example 3
                    new TestData
                    {
                        TempF=48.04,
                        PressPsig=-7.3,
                        Grp=COMMODITY_GROUP.FUEL_OILS,
                        Api60=19.4,
                        K0=103.872000000000,
                        K1=0.270100000000,
                        K2=0.000000000000,
                        TempITPS68=48.043878159606,
                        DensITSP68=936.787006219757,
                        ThermExpCoeff60=0.000406689168,
                        CompressFactor=0.384339609206,
                        CTL=1.004858068990,
                        CPL=1.000000000000,
                        CTPL=1.00486
                    },
                    // Section 11.1.6.1 - Example 4
                    new TestData
                    {
                        TempF=85,
                        PressPsig=247.3,
                        Grp=COMMODITY_GROUP.JET_FUELS,
                        Api60=Conversions.SGtoAPI(0.7943),
                        K0=330.301000000000,
                        K1=0.000000000000,
                        K2=0.000000000000,
                        TempITPS68=85.013358222928,
                        DensITSP68=793.521270459968,
                        ThermExpCoeff60=0.000524557068,
                        CompressFactor=0.664706197066,
                        CTL=0.986832406683,
                        CPL=1.001646525013,
                        CTPL=0.98846
                    },
                    // Section 11.1.6.1 - Example 5
                    new TestData
                    {
                        TempF=55.9,
                        PressPsig=350,
                        Grp=COMMODITY_GROUP.TRANSITION_ZONE,
                        Api60=48.0015,
                        K0=1.48906700000e+3,
                        K1=0.000000000000,
                        K2=-0.001868400000,
                        TempITPS68=55.905838569594,
                        DensITSP68=787.521450184768,
                        ThermExpCoeff60=0.000532585048,
                        CompressFactor=0.608111538634,
                        CTL=1.002182725702,
                        CPL=1.002132930093,
                        CTPL=1.00432
                    },
                    // Section 11.1.6.1 - Example 6
                    new TestData
                    {
                        TempF=27.3,
                        PressPsig=1234.5,
                        Grp=COMMODITY_GROUP.GASOLINES,
                        Api60=Conversions.Kgm3toAPI(657.3),
                        K0=192.457100000000,
                        K1=0.243800000000,
                        K2=0.000000000000,
                        TempITPS68=27.298898616759,
                        DensITSP68=657.303689061482,
                        ThermExpCoeff60=0.000816362130,
                        CompressFactor=0.993527440282,
                        CTL=1.026475833518,
                        CPL=1.012417396817,
                        CTPL=1.03922
                    }
                };
			}
        }


		[Theory]
        [MemberData(nameof(TestDataGenerator.GetTestExamples), MemberType = typeof(TestDataGenerator))]
        public void TestKCoeffs(TestData example1,TestData example2,TestData example3,TestData example4,TestData example5,TestData example6)
		{
            Calcs calc = new Calcs();

            KCoeffs coeffs = calc.GetKCoeffs(example1.Grp);
            Assert.True(EqualsToPrecision(example1.K0,coeffs.k0,1.0e-12));
            Assert.True(EqualsToPrecision(example1.K1, coeffs.k1, 1.0e-12));
            Assert.True(EqualsToPrecision(example1.K2, coeffs.k2, 1.0e-12));

			coeffs = calc.GetKCoeffs(example2.Grp);
			Assert.True(EqualsToPrecision(example2.K0, coeffs.k0, 1.0e-12));
			Assert.True(EqualsToPrecision(example2.K1, coeffs.k1, 1.0e-12));
			Assert.True(EqualsToPrecision(example2.K2, coeffs.k2, 1.0e-12));

			coeffs = calc.GetKCoeffs(example3.Grp);
			Assert.True(EqualsToPrecision(example3.K0, coeffs.k0, 1.0e-12));
			Assert.True(EqualsToPrecision(example3.K1, coeffs.k1, 1.0e-12));
			Assert.True(EqualsToPrecision(example3.K2, coeffs.k2, 1.0e-12));

			coeffs = calc.GetKCoeffs(example4.Grp);
			Assert.True(EqualsToPrecision(example4.K0, coeffs.k0, 1.0e-12));
			Assert.True(EqualsToPrecision(example4.K1, coeffs.k1, 1.0e-12));
			Assert.True(EqualsToPrecision(example4.K2, coeffs.k2, 1.0e-12));

			coeffs = calc.GetKCoeffs(example5.Grp);
			Assert.True(EqualsToPrecision(example5.K0, coeffs.k0, 1.0e-12));
			Assert.True(EqualsToPrecision(example5.K1, coeffs.k1, 1.0e-12));
			Assert.True(EqualsToPrecision(example5.K2, coeffs.k2, 1.0e-12));

			coeffs = calc.GetKCoeffs(example6.Grp);
			Assert.True(EqualsToPrecision(example6.K0, coeffs.k0, 1.0e-12));
			Assert.True(EqualsToPrecision(example6.K1, coeffs.k1, 1.0e-12));
			Assert.True(EqualsToPrecision(example6.K2, coeffs.k2, 1.0e-12));

		}

		[Theory]
		[MemberData(nameof(TestDataGenerator.GetTestExamples), MemberType = typeof(TestDataGenerator))]
		public void TestITPS68Conditions(TestData example1, TestData example2, TestData example3, TestData example4, TestData example5, TestData example6)
		{
			Calcs calc = new Calcs();

            KCoeffs coeffs = calc.GetKCoeffs(example1.Grp);
            double tempITPS68 = Conversions.TempITS90toITPS68(example1.TempF);
            Assert.True(EqualsToPrecision(example1.TempITPS68, tempITPS68,1.0e-12));
            double densITPS68 = Conversions.Api60ITS90tokgm3ITPS68(example1.Api60, coeffs);
            Assert.True(EqualsToPrecision(example1.DensITSP68, densITPS68, 1.0e-12));

			coeffs = calc.GetKCoeffs(example2.Grp);
			tempITPS68 = Conversions.TempITS90toITPS68(example2.TempF);
			Assert.True(EqualsToPrecision(example2.TempITPS68, tempITPS68, 1.0e-12));
			densITPS68 = Conversions.Api60ITS90tokgm3ITPS68(example2.Api60, coeffs);
			Assert.True(EqualsToPrecision(example2.DensITSP68, densITPS68, 1.0e-8));  // Special case given the test value was to the power of 3!!

			coeffs = calc.GetKCoeffs(example3.Grp);
			tempITPS68 = Conversions.TempITS90toITPS68(example3.TempF);
			Assert.True(EqualsToPrecision(example3.TempITPS68, tempITPS68, 1.0e-12));
			densITPS68 = Conversions.Api60ITS90tokgm3ITPS68(example3.Api60, coeffs);
			Assert.True(EqualsToPrecision(example3.DensITSP68, densITPS68, 1.0e-12));

			coeffs = calc.GetKCoeffs(example4.Grp);
			tempITPS68 = Conversions.TempITS90toITPS68(example4.TempF);
			Assert.True(EqualsToPrecision(example4.TempITPS68, tempITPS68, 1.0e-12));
			densITPS68 = Conversions.Api60ITS90tokgm3ITPS68(example4.Api60, coeffs);
			Assert.True(EqualsToPrecision(example4.DensITSP68, densITPS68, 1.0e-12));

			coeffs = calc.GetKCoeffs(example5.Grp);
			tempITPS68 = Conversions.TempITS90toITPS68(example5.TempF);
			Assert.True(EqualsToPrecision(example5.TempITPS68, tempITPS68, 1.0e-12));
			densITPS68 = Conversions.Api60ITS90tokgm3ITPS68(example5.Api60, coeffs);
			Assert.True(EqualsToPrecision(example5.DensITSP68, densITPS68, 1.0e-12));

			coeffs = calc.GetKCoeffs(example6.Grp);
			tempITPS68 = Conversions.TempITS90toITPS68(example6.TempF);
			Assert.True(EqualsToPrecision(example6.TempITPS68, tempITPS68, 1.0e-12));
			densITPS68 = Conversions.Api60ITS90tokgm3ITPS68(example6.Api60, coeffs);
			Assert.True(EqualsToPrecision(example6.DensITSP68, densITPS68, 1.0e-12));
		}

		[Theory]
		[MemberData(nameof(TestDataGenerator.GetTestExamples), MemberType = typeof(TestDataGenerator))]
		public void TestThermExpAndCompress(TestData example1, TestData example2, TestData example3, TestData example4, TestData example5, TestData example6)
		{
			Calcs calc = new Calcs();

			KCoeffs coeffs = calc.GetKCoeffs(example1.Grp);
			double densITPS68 = Conversions.Api60ITS90tokgm3ITPS68(example1.Api60, coeffs);
            double thermExpCoeff60 = calc.GetThermExpCoeff60(densITPS68, coeffs);
            Assert.True(EqualsToPrecision(example1.ThermExpCoeff60, thermExpCoeff60, 1.0e-12));
            double tempITPS68 = Conversions.TempITS90toITPS68(example1.TempF);
            double compressFactor = calc.GetCompressFactor(densITPS68, tempITPS68);
            Assert.True(EqualsToPrecision(example1.CompressFactor,compressFactor,1.0e-12));

			coeffs = calc.GetKCoeffs(example2.Grp);
			densITPS68 = Conversions.Api60ITS90tokgm3ITPS68(example2.Api60, coeffs);
			thermExpCoeff60 = calc.GetThermExpCoeff60(densITPS68, coeffs);
			Assert.True(EqualsToPrecision(example2.ThermExpCoeff60, thermExpCoeff60, 1.0e-12));
			tempITPS68 = Conversions.TempITS90toITPS68(example2.TempF);
			compressFactor = calc.GetCompressFactor(densITPS68, tempITPS68);
			Assert.True(EqualsToPrecision(example2.CompressFactor, compressFactor, 1.0e-12));

			coeffs = calc.GetKCoeffs(example3.Grp);
			densITPS68 = Conversions.Api60ITS90tokgm3ITPS68(example3.Api60, coeffs);
			thermExpCoeff60 = calc.GetThermExpCoeff60(densITPS68, coeffs);
			Assert.True(EqualsToPrecision(example3.ThermExpCoeff60, thermExpCoeff60, 1.0e-12));
			tempITPS68 = Conversions.TempITS90toITPS68(example3.TempF);
			compressFactor = calc.GetCompressFactor(densITPS68, tempITPS68);
			Assert.True(EqualsToPrecision(example3.CompressFactor, compressFactor, 1.0e-12));

			coeffs = calc.GetKCoeffs(example4.Grp);
			densITPS68 = Conversions.Api60ITS90tokgm3ITPS68(example4.Api60, coeffs);
			thermExpCoeff60 = calc.GetThermExpCoeff60(densITPS68, coeffs);
			Assert.True(EqualsToPrecision(example4.ThermExpCoeff60, thermExpCoeff60, 1.0e-12));
			tempITPS68 = Conversions.TempITS90toITPS68(example4.TempF);
			compressFactor = calc.GetCompressFactor(densITPS68, tempITPS68);
			Assert.True(EqualsToPrecision(example4.CompressFactor, compressFactor, 1.0e-12));

			coeffs = calc.GetKCoeffs(example5.Grp);
			densITPS68 = Conversions.Api60ITS90tokgm3ITPS68(example5.Api60, coeffs);
			thermExpCoeff60 = calc.GetThermExpCoeff60(densITPS68, coeffs);
			Assert.True(EqualsToPrecision(example5.ThermExpCoeff60, thermExpCoeff60, 1.0e-12));
			tempITPS68 = Conversions.TempITS90toITPS68(example5.TempF);
			compressFactor = calc.GetCompressFactor(densITPS68, tempITPS68);
			Assert.True(EqualsToPrecision(example5.CompressFactor, compressFactor, 1.0e-12));

			coeffs = calc.GetKCoeffs(example6.Grp);
			densITPS68 = Conversions.Api60ITS90tokgm3ITPS68(example6.Api60, coeffs);
			thermExpCoeff60 = calc.GetThermExpCoeff60(densITPS68, coeffs);
			Assert.True(EqualsToPrecision(example6.ThermExpCoeff60, thermExpCoeff60, 1.0e-12));
			tempITPS68 = Conversions.TempITS90toITPS68(example6.TempF);
			compressFactor = calc.GetCompressFactor(densITPS68, tempITPS68);
			Assert.True(EqualsToPrecision(example6.CompressFactor, compressFactor, 1.0e-12));
		}

		[Theory]
		[MemberData(nameof(TestDataGenerator.GetTestExamples), MemberType = typeof(TestDataGenerator))]
		public void TestCorrectionFactors(TestData example1, TestData example2, TestData example3, TestData example4, TestData example5, TestData example6)
		{
			Calcs calc = new Calcs();

			KCoeffs coeffs = calc.GetKCoeffs(example1.Grp);
			double densITPS68 = Conversions.Api60ITS90tokgm3ITPS68(example1.Api60, coeffs);
			double thermExpCoeff60 = calc.GetThermExpCoeff60(densITPS68, coeffs);
			double tempITPS68 = Conversions.TempITS90toITPS68(example1.TempF);
			double compressFactor = calc.GetCompressFactor(densITPS68, tempITPS68);
            double CTL = calc.GetCTL(thermExpCoeff60, tempITPS68);
            Assert.True(EqualsToPrecision(example1.CTL, CTL, 1.0e-12));
            double CPL = example1.PressPsig<=0?1.0:calc.GetCPL(compressFactor,example1.PressPsig);
			Assert.True(EqualsToPrecision(example1.CPL, CPL, 1.0e-12));
            double CTPL = calc.GetCTPLFromApiDegFPsig(example1.Grp,example1.Api60, example1.TempF, example1.PressPsig);
			Assert.True(EqualsToPrecision(example1.CTPL, CTPL, 1.0e-5));

            coeffs=calc.GetKCoeffs(example2.Grp);
			densITPS68 = Conversions.Api60ITS90tokgm3ITPS68(example2.Api60, coeffs);
			thermExpCoeff60 = calc.GetThermExpCoeff60(densITPS68, coeffs);
			tempITPS68 = Conversions.TempITS90toITPS68(example2.TempF);
			compressFactor = calc.GetCompressFactor(densITPS68, tempITPS68);
			CTL = calc.GetCTL(thermExpCoeff60, tempITPS68);
			Assert.True(EqualsToPrecision(example2.CTL, CTL, 1.0e-12));
			CPL = example2.PressPsig <= 0 ? 1.0 : calc.GetCPL(compressFactor, example2.PressPsig);
			Assert.True(EqualsToPrecision(example2.CPL, CPL, 1.0e-12));
			CTPL = calc.GetCTPLFromApiDegFPsig(example2.Grp, example2.Api60, example2.TempF, example2.PressPsig);
			Assert.True(EqualsToPrecision(example2.CTPL, CTPL, 1.0e-5));

            coeffs=calc.GetKCoeffs(example3.Grp);
			densITPS68 = Conversions.Api60ITS90tokgm3ITPS68(example3.Api60, coeffs);
			thermExpCoeff60 = calc.GetThermExpCoeff60(densITPS68, coeffs);
			tempITPS68 = Conversions.TempITS90toITPS68(example3.TempF);
			compressFactor = calc.GetCompressFactor(densITPS68, tempITPS68);
			CTL = calc.GetCTL(thermExpCoeff60, tempITPS68);
			Assert.True(EqualsToPrecision(example3.CTL, CTL, 1.0e-12));
			CPL = example3.PressPsig <= 0 ? 1.0 : calc.GetCPL(compressFactor, example3.PressPsig);
			Assert.True(EqualsToPrecision(example3.CPL, CPL, 1.0e-12));
			CTPL = calc.GetCTPLFromApiDegFPsig(example3.Grp, example3.Api60, example3.TempF, example3.PressPsig);
			Assert.True(EqualsToPrecision(example3.CTPL, CTPL, 1.0e-5));

            coeffs=calc.GetKCoeffs(example4.Grp);
			densITPS68 = Conversions.Api60ITS90tokgm3ITPS68(example4.Api60, coeffs);
			thermExpCoeff60 = calc.GetThermExpCoeff60(densITPS68, coeffs);
			tempITPS68 = Conversions.TempITS90toITPS68(example4.TempF);
			compressFactor = calc.GetCompressFactor(densITPS68, tempITPS68);
			CTL = calc.GetCTL(thermExpCoeff60, tempITPS68);
			Assert.True(EqualsToPrecision(example4.CTL, CTL, 1.0e-12));
			CPL = example4.PressPsig <= 0 ? 1.0 : calc.GetCPL(compressFactor, example4.PressPsig);
			Assert.True(EqualsToPrecision(example4.CPL, CPL, 1.0e-12));
			CTPL = calc.GetCTPLFromApiDegFPsig(example4.Grp, example4.Api60, example4.TempF, example4.PressPsig);
			Assert.True(EqualsToPrecision(example4.CTPL, CTPL, 1.0e-5));

            coeffs=calc.GetKCoeffs(example5.Grp);
			densITPS68 = Conversions.Api60ITS90tokgm3ITPS68(example5.Api60, coeffs);
			thermExpCoeff60 = calc.GetThermExpCoeff60(densITPS68, coeffs);
			tempITPS68 = Conversions.TempITS90toITPS68(example5.TempF);
			compressFactor = calc.GetCompressFactor(densITPS68, tempITPS68);
			CTL = calc.GetCTL(thermExpCoeff60, tempITPS68);
			Assert.True(EqualsToPrecision(example5.CTL, CTL, 1.0e-12));
			CPL = example5.PressPsig <= 0 ? 1.0 : calc.GetCPL(compressFactor, example5.PressPsig);
			Assert.True(EqualsToPrecision(example5.CPL, CPL, 1.0e-12));
			CTPL = calc.GetCTPLFromApiDegFPsig(example5.Grp, example5.Api60, example5.TempF, example5.PressPsig);
			Assert.True(EqualsToPrecision(example5.CTPL, CTPL, 1.0e-5));

            coeffs=calc.GetKCoeffs(example6.Grp);
			densITPS68 = Conversions.Api60ITS90tokgm3ITPS68(example6.Api60, coeffs);
			thermExpCoeff60 = calc.GetThermExpCoeff60(densITPS68, coeffs);
			tempITPS68 = Conversions.TempITS90toITPS68(example6.TempF);
			compressFactor = calc.GetCompressFactor(densITPS68, tempITPS68);
			CTL = calc.GetCTL(thermExpCoeff60, tempITPS68);
			Assert.True(EqualsToPrecision(example6.CTL, CTL, 1.0e-12));
			CPL = example6.PressPsig <= 0 ? 1.0 : calc.GetCPL(compressFactor, example6.PressPsig);
			Assert.True(EqualsToPrecision(example6.CPL, CPL, 1.0e-12));
			CTPL = calc.GetCTPLFromApiDegFPsig(example6.Grp, example6.Api60, example6.TempF, example6.PressPsig);
			Assert.True(EqualsToPrecision(example6.CTPL, CTPL, 1.0e-5));
		}

		// Utility functions
		public bool EqualsToPrecision(double expected,double actual,double precision)
        {
            double diff = Math.Abs(expected - actual);
            return diff <= 0.5*precision;
        }
    }
}
