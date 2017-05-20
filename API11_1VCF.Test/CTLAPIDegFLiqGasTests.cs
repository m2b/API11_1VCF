using System;
using Xunit;
using System.Collections.Generic;
using System.Collections;

namespace APIVCF
{
    public class CTLFromAPIDegFLiqGasTests
    {
        // TODO: Add additional test from Example 24/*
		public class TestData
        {
            // Inputs
            public double TempF;
            public double Api60;

            // Intermediate results
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
                    // API 11.2.4 - Exmple 24/1
                    new TestData {
                        TempF=-48.0200,
                        Api60=Conversions.SGtoAPI(0.350130),
                        CTL=1.374174158511
                    },
                    // API 11.2.4 - Exmple 24/2
                    new TestData
					{
						TempF=24.95,
						Api60=Conversions.SGtoAPI(0.399950),
						CTL=1.100764647588
					},
                    // API 11.2.4 - Exmple 24/3
                    new TestData
					{
						TempF=87.42000,
						Api60=Conversions.SGtoAPI(0.451530),
						CTL=0.932749411288
					},
                    // API 11.2.4 - Exmple 24/4
                    new TestData
					{
						TempF=184.9700,
						Api60=Conversions.SGtoAPI(0.4904),
						CTL=0.615949186930
					},
                    // API 11.2.4 - Exmple 24/5
                    new TestData
					{
						TempF=155.0400,
						Api60=Conversions.SGtoAPI(0.540020),
						CTL=0.851071799690
					},
                    // API 11.2.4 - Exmple 24/6
                    new TestData
					{
						TempF=3.0330,
						Api60=Conversions.SGtoAPI(0.569980),
						CTL=1.062314380669
					},
                    // API 11.2.4 - Exmple 24/7
                    new TestData
                    {
                        TempF=110.0400,
                        Api60=Conversions.SGtoAPI(0.599970),
                        CTL=0.948465346003
					},
                    // API 11.2.4 - Exmple 24/8
                    new TestData
					{
						TempF=169.9700,
                        Api60=Conversions.SGtoAPI(0.625020),
						CTL=0.893815224960
					},
                    // API 11.2.4 - Exmple 24/9
                    new TestData
					{
						TempF=-12.0200,
						Api60=Conversions.SGtoAPI(0.640040),
						CTL=1.057304685863
					}
                };
			}
        }


		[Theory]
        [MemberData(nameof(TestDataGenerator.GetTestExamples), MemberType = typeof(TestDataGenerator))]
        public void TestCTL(TestData example1,TestData example2,TestData example3,TestData example4,TestData example5,TestData example6,
                            TestData example7,TestData example8,TestData example9)
		{
            Calcs calc = new Calcs();

            double CTL = calc.GetCTLLiqGas(example1.TempF,example1.Api60);
            Assert.True(EqualsToPrecision(example1.CTL,CTL,1.0e-5));
			CTL = calc.GetCTLLiqGas(example2.TempF, example2.Api60);
			Assert.True(EqualsToPrecision(example2.CTL, CTL, 1.0e-5));
			CTL = calc.GetCTLLiqGas(example3.TempF, example3.Api60);
			Assert.True(EqualsToPrecision(example3.CTL, CTL, 1.0e-5)); 
            CTL = calc.GetCTLLiqGas(example4.TempF, example4.Api60);
			Assert.True(EqualsToPrecision(example4.CTL, CTL, 1.0e-5));
			CTL = calc.GetCTLLiqGas(example5.TempF, example5.Api60);
			Assert.True(EqualsToPrecision(example5.CTL, CTL, 1.0e-5));
			CTL = calc.GetCTLLiqGas(example6.TempF, example6.Api60);
			Assert.True(EqualsToPrecision(example6.CTL, CTL, 1.0e-5));
			CTL = calc.GetCTLLiqGas(example7.TempF, example7.Api60);
			Assert.True(EqualsToPrecision(example7.CTL, CTL, 1.0e-5));
			CTL = calc.GetCTLLiqGas(example8.TempF, example8.Api60);
			Assert.True(EqualsToPrecision(example8.CTL, CTL, 1.0e-5));
			CTL = calc.GetCTLLiqGas(example9.TempF, example9.Api60);
			Assert.True(EqualsToPrecision(example9.CTL, CTL, 1.0e-5));
        }

		// Utility functions
		public bool EqualsToPrecision(double expected,double actual,double precision)
        {
            double diff = Math.Abs(expected - actual);
            return diff <= 0.5*precision;
        }
    }
}
