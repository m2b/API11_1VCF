using System;
using Xunit;
using System.Collections.Generic;
using System.Collections;

namespace APIVCF
{
    public class DensityCalcsTests
    {
		public class TestData
        {
            // Inputs
            public double Api60;
            public double Api;
            public double TempF;
            public COMMODITY_GROUP Grp;
            public double PressPsig=0;
            public double VapPressPsig=0;
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
                        Api=17.785/1.03301
					},
                    // API 11.2.4 - Exmple 24/2 Contrived with pressures=0
                    new TestData
					{
                        Grp=COMMODITY_GROUP.LPG_NGL,
						TempF=24.95,
						Api60=Conversions.SGtoAPI(0.399950),
                        Api=Conversions.SGtoAPI(0.399950)/1.10076

					},
                    // Section 11.1.6.1 - Example 4
                    new TestData
					{
						TempF=85,
						PressPsig=247.3,
						Grp=COMMODITY_GROUP.JET_FUELS,
						Api60=Conversions.SGtoAPI(0.7943),
						Api=Conversions.SGtoAPI(0.7943)/0.98846
					},
                    // API 11.2.4 - Example 24/1
                    new TestData
					{
						Grp=COMMODITY_GROUP.LPG_NGL,
						TempF=-48.0200,
						Api60=Conversions.SGtoAPI(0.3501),
						Api=Conversions.SGtoAPI(0.3501)/1.37417
					},
                    // Section 11.1.6.1 - Example 6
                    new TestData
					{
						TempF=27.3,
						PressPsig=1234.5,
						Grp=COMMODITY_GROUP.GASOLINES,
						Api60=Conversions.Kgm3toAPI(657.3),
						Api=Conversions.Kgm3toAPI(657.3)/1.03922
					}
                    ,
                    // API 11.2.4 - Exmple 24/6
                    new TestData
					{
						Grp=COMMODITY_GROUP.LPG_NGL,
						TempF=3.0330,
						Api60=Conversions.SGtoAPI(0.569980),
						Api=Conversions.SGtoAPI(0.569980)/1.06231
					}
                };
			}
        }


		[Theory]
        [MemberData(nameof(TestDataGenerator.GetTestExamples), MemberType = typeof(TestDataGenerator))]
        public void TestDensityFromDensity60(TestData example1,TestData example2,TestData example3,TestData example4,TestData example5,TestData example6)
		{
            Calcs calc = new Calcs();

            double Api = calc.GetDensityFromDensity60(example1.Grp,example1.Api60,example1.TempF,example1.PressPsig);
			Assert.True(EqualsToPrecision(example1.Api, Api, 1.0e-1)); 
            Api = calc.GetDensityFromDensity60(example2.Grp, example2.Api60, example2.TempF,example2.PressPsig);
			Assert.True(EqualsToPrecision(example2.Api, Api, 1.0e-1));
            Api = calc.GetDensityFromDensity60(example3.Grp, example3.Api60, example3.TempF,example3.PressPsig);
			Assert.True(EqualsToPrecision(example3.Api, Api, 1.0e-1));
            Api = calc.GetDensityFromDensity60(example4.Grp, example4.Api60, example4.TempF,example4.PressPsig);
			Assert.True(EqualsToPrecision(example4.Api, Api, 1.0e-1));
            Api = calc.GetDensityFromDensity60(example5.Grp, example5.Api60, example5.TempF,example5.PressPsig);
			Assert.True(EqualsToPrecision(example5.Api, Api, 1.0e-1));
            Api = calc.GetDensityFromDensity60(example6.Grp, example6.Api60, example6.TempF,example6.PressPsig);
			Assert.True(EqualsToPrecision(example6.Api, Api, 1.0e-1));    
        }

		[Theory]
		[MemberData(nameof(TestDataGenerator.GetTestExamples), MemberType = typeof(TestDataGenerator))]
		public void TestDensity60FromDensity(TestData example1, TestData example2, TestData example3, TestData example4, TestData example5, TestData example6)
		{
			Calcs calc = new Calcs();

            double CTPL = 0;
            double Api60 = calc.GetDensity60FromDensity(example1.Grp, example1.Api, example1.TempF,out CTPL,example1.PressPsig);
			Assert.True(EqualsToPrecision(example1.Api60, Api60, 1.0e-1));
			Api60 = calc.GetDensity60FromDensity(example2.Grp, example2.Api, example2.TempF, out CTPL, example2.PressPsig);
			Assert.True(EqualsToPrecision(example2.Api60, Api60, 1.0e-1));
			Api60 = calc.GetDensity60FromDensity(example3.Grp, example3.Api, example3.TempF, out CTPL, example3.PressPsig);
			Assert.True(EqualsToPrecision(example3.Api60, Api60, 1.0e-1));
			Api60 = calc.GetDensity60FromDensity(example4.Grp, example4.Api, example4.TempF, out CTPL, example4.PressPsig);
			Assert.True(EqualsToPrecision(example4.Api60, Api60, 1.0e-1));
			Api60 = calc.GetDensity60FromDensity(example5.Grp, example5.Api, example5.TempF, out CTPL, example5.PressPsig);
			Assert.True(EqualsToPrecision(example5.Api60, Api60, 1.0e-1));
			Api60 = calc.GetDensity60FromDensity(example6.Grp, example6.Api, example6.TempF, out CTPL, example6.PressPsig);
			Assert.True(EqualsToPrecision(example6.Api60, Api60, 1.0e-1));
		}

		// Utility functions
		public bool EqualsToPrecision(double expected,double actual,double precision)
        {
            double diff = Math.Abs(expected - actual);
            return diff <= precision;
        }
    }
}
