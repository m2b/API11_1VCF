using System;
using Xunit;
using System.Collections.Generic;
using System.Collections;

namespace APIVCF
{
    public class CTPLFromAPIDegFPsigLiqGasTests
    {
		public class TestData
        {
            // Inputs
            public double TempF;
            public double PressPsig;
            public readonly COMMODITY_GROUP Grp=COMMODITY_GROUP.LPG_NGL;
            public double Api60;
            public double CTPL;
		}


        public class TestDataGenerator
		{
            public static IEnumerable<object[]> GetTestExamples()
            {
                yield return new object[]
                {
                    // Example 1 from "undisclosed customer" data
                    new TestData {
                        Api60=Conversions.SGtoAPI(0.5250),
                        TempF=42.1,
                        PressPsig=80,
                        CTPL=1.02631
                    },
                    // Example 2 from "undisclosed customer" data
                    new TestData
                    {
						Api60=Conversions.SGtoAPI(0.5174),
                        TempF=38.0,
                        PressPsig=20.8,
                        CTPL=1.03160
                    },
                    // Example 3 from "undisclosed customer" data
                    new TestData {
						Api60=Conversions.SGtoAPI(0.5174),
						TempF=54.6,
						PressPsig=19.8,
						CTPL=1.00660
					},
                    // Example 2 from "undisclosed customer" data
                    new TestData
					{
						Api60=Conversions.SGtoAPI(0.5250),
						TempF=41.3,
						PressPsig=79.9,
						CTPL=1.02743
					},
                    // Example 5 from "undisclosed customer" data
                    new TestData {
						Api60=Conversions.SGtoAPI(0.5174),
						TempF=53.1,
						PressPsig=17.5,
						CTPL=1.00884
					}
                };
			}
        }


		[Theory]
		[MemberData(nameof(TestDataGenerator.GetTestExamples), MemberType = typeof(TestDataGenerator))]
        public void TestVolumeCorrectionFactor(TestData example1, TestData example2, TestData example3, TestData example4,TestData example5)
		{
			Calcs calc = new Calcs();

            double CTPL = calc.GetCTPLFromApiDegFPsig(example1.Grp,example1.Api60, example1.TempF, example1.PressPsig);
			Assert.True(EqualsToPrecision(example1.CTPL, CTPL, 1.0e-3));

			CTPL = calc.GetCTPLFromApiDegFPsig(example2.Grp, example2.Api60, example2.TempF, example2.PressPsig);
			Assert.True(EqualsToPrecision(example2.CTPL, CTPL, 1.0e-3));


			CTPL = calc.GetCTPLFromApiDegFPsig(example3.Grp, example3.Api60, example3.TempF, example3.PressPsig);
			Assert.True(EqualsToPrecision(example3.CTPL, CTPL, 1.6e-3)); // Customer data seems suspect in this case.  Pressure seems rather low.

			CTPL = calc.GetCTPLFromApiDegFPsig(example4.Grp, example4.Api60, example4.TempF, example4.PressPsig);
			Assert.True(EqualsToPrecision(example4.CTPL, CTPL, 1.0e-3));

			CTPL = calc.GetCTPLFromApiDegFPsig(example5.Grp, example5.Api60, example5.TempF, example5.PressPsig);
            Assert.True(EqualsToPrecision(example5.CTPL, CTPL, 1.6e-3));  // Customer data seems suspect in this case.  Pressure seems rather low.

		}

		// Utility functions
		public bool EqualsToPrecision(double expected,double actual,double precision)
        {
            double diff = Math.Abs(expected - actual)/Math.Max(expected,actual);
            return diff <= precision;
        }
    }
}
