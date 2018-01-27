using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace APIVCF
{
    public class MeasureTapeFtInFractTest
    {
        const int MAXITER = 5;
        const double MAXFEET = 60;

		public class TestData
		{
            // Inputs
            public TapeMeasureFtInFract in1;
            public TapeMeasureFtInFract in2;
            public double dec1;
            public double dec2;
            public bool IsSubstraction;
            // Outputs
            public TapeMeasureFtInFract out1;
		}

		public class TestDataGenerator
		{
			public static IEnumerable<object[]> GetTestExamples()
			{
                yield return new object[]
                {
                    // Addition 1
                    new TestData {
                        in1=new TapeMeasureFtInFract("1-1-15/16"),
                        in2=new TapeMeasureFtInFract("0-2-1/16"),
                        dec1=1.1614583,
                        dec2=0.171875,
                        out1=new TapeMeasureFtInFract("1-4-0")
                    },
                    // Addition 2
                    new TestData {
                        in1=new TapeMeasureFtInFract("1-1-7/16"),
                        in2=new TapeMeasureFtInFract("0-2-1/16"),
                        dec1=1.1197916,
                        dec2=0.171875,
                        out1=new TapeMeasureFtInFract("1-3-1/2")
                    },
                    // Addition 3
                    new TestData {
                        in1=new TapeMeasureFtInFract("2-11-15/16"),
                        in2=new TapeMeasureFtInFract("1-0-1/16"),
                        dec1=2.9947916,
                        dec2=1.0052083,
                        out1=new TapeMeasureFtInFract("4-0-0")
                    },
                    // Addition 4
                    new TestData {
                        in1=new TapeMeasureFtInFract("1-11-1/8"),
                        in2=new TapeMeasureFtInFract("1-1-1/16"),
                        dec1=1.927083,
                        dec2=1.0885416,
                        out1=new TapeMeasureFtInFract("3-0-3/16")
                    },
                    // Addition 5
                    new TestData {
						in1=new TapeMeasureFtInFract(),
						in2=new TapeMeasureFtInFract("1-1-1/16"),
                        dec1=0,
					    dec2=1.0885416,
						out1=new TapeMeasureFtInFract("1-1-1/16")
					},
                    // Addition 6
                    new TestData {
						in1=new TapeMeasureFtInFract("1-11-1/8"),
						in2=new TapeMeasureFtInFract(),
						dec1=1.927083,
                        dec2=0,
						out1=new TapeMeasureFtInFract("1-11-1/8")
					},                    
                    // Substraction 1
                    new TestData {
                        IsSubstraction=true,
						in1=new TapeMeasureFtInFract("1-1-15/16"),
						in2=new TapeMeasureFtInFract("0-2-1/16"),
						out1=new TapeMeasureFtInFract("0-11-7/8")
					},
                    // Substraction 2
                    new TestData {
						IsSubstraction=true,
                        in1=new TapeMeasureFtInFract("0-11-7/16"),
						in2=new TapeMeasureFtInFract("0-2-9/16"),
						out1=new TapeMeasureFtInFract("0-8-7/8")
					},
                    // Substraction 3
                    new TestData {
						IsSubstraction=true,
						in1=new TapeMeasureFtInFract("1-1-3/8"),
						in2=new TapeMeasureFtInFract("0-2-18/32"),
						out1=new TapeMeasureFtInFract("0-10-13/16")
					},
                    // Substraction 4
                    new TestData {
						IsSubstraction=true,
						in1=new TapeMeasureFtInFract("1-0-5/8"),  
						in2=new TapeMeasureFtInFract("0-2-23/32"),
						out1=new TapeMeasureFtInFract("0-9-29/32")
					},
                    // Substraction 5
                    new TestData {
						IsSubstraction=true,
						in1=new TapeMeasureFtInFract("1-0-5/8"),
						in2=new TapeMeasureFtInFract(),
						out1=new TapeMeasureFtInFract("1-0-5/8")
					},
                    // Bad Substraction 1
                    new TestData {
						IsSubstraction=true,
						in1=new TapeMeasureFtInFract("1-0-5/8"),
						in2=new TapeMeasureFtInFract("1-1-0")
					},
                    // Substraction 2
                    new TestData {
						IsSubstraction=true,
						in1=new TapeMeasureFtInFract("0-0-7/8"),
						in2=new TapeMeasureFtInFract("1-0-0")
					},
				};
			}
		}

		[Theory]
		[MemberData(nameof(TestDataGenerator.GetTestExamples), MemberType = typeof(TestDataGenerator))]
		public void TestOperations(
            TestData add1, TestData add2, TestData add3, TestData add4, TestData add5, TestData add6,
            TestData sub1, TestData sub2, TestData sub3, TestData sub4, TestData sub5,
            TestData bad1, TestData bad2
        )
		{
            // Additions
            TapeMeasureFtInFract in1 = add1.in1;
            TapeMeasureFtInFract in2 = add1.in2;
            string expected = add1.out1.ToString();
            string actual = (in1 + in2).ToString();
            Assert.Equal(expected,actual);
			in1 = add2.in1;
			in2 = add2.in2;
			expected = add2.out1.ToString();
			actual = (in1 + in2).ToString();
			Assert.Equal(expected, actual);
			in1 = add3.in1;
			in2 = add3.in2;
			expected = add3.out1.ToString();
			actual = (in1 + in2).ToString();
			Assert.Equal(expected, actual);
			in1 = add4.in1;
			in2 = add4.in2;
			expected = add4.out1.ToString();
			actual = (in1 + in2).ToString();
			Assert.Equal(expected, actual);
			in1 = add5.in1;
			in2 = add5.in2;
			expected = add5.out1.ToString();
			actual = (in1 + in2).ToString();
			Assert.Equal(expected, actual);
			in1 = add6.in1;
			in2 = add6.in2;
			expected = add6.out1.ToString();
			actual = (in1 + in2).ToString();
			Assert.Equal(expected, actual);

			// Substractions
            in1 = sub1.in1;
			in2 = sub1.in2;
			expected = sub1.out1.ToString();
			actual = (in1 - in2).ToString();
			Assert.Equal(expected, actual);
			in1 = sub2.in1;
			in2 = sub2.in2;
			expected = sub2.out1.ToString();
			actual = (in1 - in2).ToString();
			Assert.Equal(expected, actual);
			in1 = sub3.in1;
			in2 = sub3.in2;
			expected = sub3.out1.ToString();
			actual = (in1 - in2).ToString();
			Assert.Equal(expected, actual);
			in1 = sub4.in1;
			in2 = sub4.in2;
			expected = sub4.out1.ToString();
			actual = (in1 - in2).ToString();
			Assert.Equal(expected, actual);
			in1 = sub5.in1;
			in2 = sub5.in2;
			expected = sub5.out1.ToString();
			actual = (in1 - in2).ToString();
			Assert.Equal(expected, actual);

			// Bad Substractions (resulting in negative tape measures!)
			in1 = bad1.in1;
			in2 = bad1.in2;
            Exception ex = Assert.Throws<ArgumentException>(() => { actual = (in1 - in2).ToString(); });
			in1 = bad2.in1;
			in2 = bad2.in2;
			ex = Assert.Throws<ArgumentException>(() => { actual = (in1 - in2).ToString(); });

		}

		[Theory]
		[MemberData(nameof(TestDataGenerator.GetTestExamples), MemberType = typeof(TestDataGenerator))]
		public void GetDecimalValueTest(
			TestData add1, TestData add2, TestData add3, TestData add4, TestData add5, TestData add6,
			TestData sub1, TestData sub2, TestData sub3, TestData sub4, TestData sub5,
			TestData bad1, TestData bad2
		)
        {
            double res = TapeMeasureFtInFract.GetDecimalValue(add1.in1);
            double expected = add1.dec1;
            Assert.True(EqualsToPrecision(expected, res, 1e-6));
			res = TapeMeasureFtInFract.GetDecimalValue(add1.in2);
			expected = add1.dec2;
			Assert.True(EqualsToPrecision(expected, res, 1e-6));
			res = TapeMeasureFtInFract.GetDecimalValue(add2.in1);
			expected = add2.dec1;
			Assert.True(EqualsToPrecision(expected, res, 1e-6));
			res = TapeMeasureFtInFract.GetDecimalValue(add2.in2);
			expected = add2.dec2;
			Assert.True(EqualsToPrecision(expected, res, 1e-6));
			res = TapeMeasureFtInFract.GetDecimalValue(add3.in1);
			expected = add3.dec1;
			Assert.True(EqualsToPrecision(expected, res, 1e-6));
			res = TapeMeasureFtInFract.GetDecimalValue(add3.in2);
			expected = add3.dec2;
			Assert.True(EqualsToPrecision(expected, res, 1e-6));
			res = TapeMeasureFtInFract.GetDecimalValue(add4.in1);
			expected = add4.dec1;
			Assert.True(EqualsToPrecision(expected, res, 1e-6));
			res = TapeMeasureFtInFract.GetDecimalValue(add4.in2);
			expected = add4.dec2;
			Assert.True(EqualsToPrecision(expected, res, 1e-6));
			res = TapeMeasureFtInFract.GetDecimalValue(add5.in1);
			expected = add5.dec1;
			Assert.True(EqualsToPrecision(expected, res, 1e-6));
			res = TapeMeasureFtInFract.GetDecimalValue(add5.in2);
			expected = add5.dec2;
			Assert.True(EqualsToPrecision(expected, res, 1e-6));
			res = TapeMeasureFtInFract.GetDecimalValue(add6.in1);
			expected = add6.dec1;
			Assert.True(EqualsToPrecision(expected, res, 1e-6));
			res = TapeMeasureFtInFract.GetDecimalValue(add6.in2);
			expected = add6.dec2;
			Assert.True(EqualsToPrecision(expected, res, 1e-6));
		}

		[Theory]
		[MemberData(nameof(TestDataGenerator.GetTestExamples), MemberType = typeof(TestDataGenerator))]
		public void GetFromDecimalTest(
			TestData add1, TestData add2, TestData add3, TestData add4, TestData add5, TestData add6,
			TestData sub1, TestData sub2, TestData sub3, TestData sub4, TestData sub5,
			TestData bad1, TestData bad2
		)
		{
            TapeMeasureFtInFract res = TapeMeasureFtInFract.GetFromDecimal(add1.dec1);
            TapeMeasureFtInFract expected = add1.in1;
            Assert.True(expected.Equals(res));
			res = TapeMeasureFtInFract.GetFromDecimal(add1.dec2);
			expected = add1.in2;
			Assert.True(expected.Equals(res));
			res = TapeMeasureFtInFract.GetFromDecimal(add2.dec1);
			expected = add2.in1;
			Assert.True(expected.Equals(res));
			res = TapeMeasureFtInFract.GetFromDecimal(add2.dec2);
			expected = add2.in2;
			Assert.True(expected.Equals(res));
			res = TapeMeasureFtInFract.GetFromDecimal(add3.dec1);
			expected = add3.in1;
			Assert.True(expected.Equals(res));
			res = TapeMeasureFtInFract.GetFromDecimal(add3.dec2);
			expected = add3.in2;
			Assert.True(expected.Equals(res));
			res = TapeMeasureFtInFract.GetFromDecimal(add4.dec1);
			expected = add4.in1;
			Assert.True(expected.Equals(res));
			res = TapeMeasureFtInFract.GetFromDecimal(add4.dec2);
			expected = add4.in2;
			Assert.True(expected.Equals(res));
			res = TapeMeasureFtInFract.GetFromDecimal(add5.dec1);
			expected = add5.in1;
			Assert.True(expected.Equals(res));
			res = TapeMeasureFtInFract.GetFromDecimal(add5.dec2);
			expected = add5.in2;
			Assert.True(expected.Equals(res));
			res = TapeMeasureFtInFract.GetFromDecimal(add6.dec1);
			expected = add6.in1;
			Assert.True(expected.Equals(res));
			res = TapeMeasureFtInFract.GetFromDecimal(add6.dec2);
			expected = add6.in2;
			Assert.True(expected.Equals(res));
		}

		// Utility functions
		public bool EqualsToPrecision(double expected, double actual, double precision)
		{
			double diff = Math.Abs(expected - actual);
			return diff <= precision;
		}

		[Fact]
        public void ConstructorTest()
        {
			TapeMeasureFtInFract meas = new TapeMeasureFtInFract();
			Assert.NotNull(meas);

            string strMeas = "0-0-0";
            meas = new TapeMeasureFtInFract(strMeas);
            Assert.NotNull(meas);
            meas = new TapeMeasureFtInFract(0, 0, null);
			Assert.NotNull(meas);

            strMeas = "1-0-0";
			meas = new TapeMeasureFtInFract(strMeas);
			Assert.NotNull(meas);
			meas = new TapeMeasureFtInFract(1, 0, null);
            Assert.NotNull(meas);
                  
            strMeas = "0-1-0";
			meas = new TapeMeasureFtInFract(strMeas);
            Assert.NotNull(meas);
			meas = new TapeMeasureFtInFract(0,1, null);
            Assert.NotNull(meas);

			strMeas = "0-0-1/16";
			meas = new TapeMeasureFtInFract(strMeas);
			Assert.NotNull(meas);
            meas = new TapeMeasureFtInFract(0, 0, new InchFraction("1/16"));
            Assert.NotNull(meas);

			// Generate possible combinations
			var denoms = Enum.GetValues(typeof(INCHFRACT)).Cast<INCHFRACT>();
			Random rnd = new Random();
            for (int i = 0; i < MAXITER;i++)
            {
                // Feet
                int feet = (int)Math.Round(MAXFEET*rnd.NextDouble());
                // Inches
                int inches = (int)Math.Round(11.0 * rnd.NextDouble());

                string strRoot = feet + "-" + inches;

                foreach (var denom in denoms)
                {
                    int intDenom = (int)denom;
                    for (int j = 0; j < (intDenom-1); j++)
                    {
                        strMeas = strRoot + "-" + (j + 1) + "/"+intDenom;
                        meas = new TapeMeasureFtInFract(strMeas);
                        Assert.NotNull(meas);
                        meas = new TapeMeasureFtInFract(feet, inches, new InchFraction((j + 1), intDenom));
                        Assert.NotNull(meas);
                    }
                }            
            }
        }

        [Fact]
		public void ConstructorFailTest()
		{
			string strMeas = "0/0-1";
			Exception ex = Assert.Throws<ArgumentException>(() => { TapeMeasureFtInFract meas = new TapeMeasureFtInFract(strMeas); });

			strMeas = "0-0";
			ex = Assert.Throws<ArgumentException>(() => { TapeMeasureFtInFract meas = new TapeMeasureFtInFract(strMeas); });

			strMeas = "12";
			ex = Assert.Throws<ArgumentException>(() => { TapeMeasureFtInFract meas = new TapeMeasureFtInFract(strMeas); });

			strMeas = "1-12-0";
			ex = Assert.Throws<ArgumentException>(() => { TapeMeasureFtInFract meas = new TapeMeasureFtInFract(strMeas); });
			ex = Assert.Throws<ArgumentException>(() => { TapeMeasureFtInFract meas = new TapeMeasureFtInFract(1,12,null); });

			strMeas = "1-13-0";
			ex = Assert.Throws<ArgumentException>(() => { TapeMeasureFtInFract meas = new TapeMeasureFtInFract(strMeas); });
			ex = Assert.Throws<ArgumentException>(() => { TapeMeasureFtInFract meas = new TapeMeasureFtInFract(1, 13, null); });

			// Generate possible combinations
			var denoms = Enum.GetValues(typeof(INCHFRACT)).Cast<INCHFRACT>();
			Random rnd = new Random();
			// Feet
			int feet = (int)Math.Round(MAXFEET * rnd.NextDouble());
			// Inches
			int inches = (int)Math.Round(11.0 * rnd.NextDouble());
			string strRoot = feet + "-" + inches;
			foreach (var denom in denoms)
			{
				int intDenom = (int)denom;
				// Zero numerator
				strMeas = strRoot + "-" + 0 + "/" + intDenom;
				ex = Assert.Throws<ArgumentException>(() => { TapeMeasureFtInFract meas = new TapeMeasureFtInFract(strMeas); });
				ex = Assert.Throws<ArgumentException>(() => { TapeMeasureFtInFract meas = new TapeMeasureFtInFract(feet, inches, new InchFraction(0, intDenom)); });
				// Num=Denom
				strMeas = strRoot + "-" + intDenom + "/" + intDenom; 
				ex = Assert.Throws<ArgumentException>(() => { TapeMeasureFtInFract meas = new TapeMeasureFtInFract(strMeas); });
                ex = Assert.Throws<ArgumentException>(() => { TapeMeasureFtInFract meas = new TapeMeasureFtInFract(feet,inches,new InchFraction(intDenom,intDenom)); });
				// Num>Denom
                strMeas = strRoot + "-" + (intDenom+1) + "/" + intDenom;
				ex = Assert.Throws<ArgumentException>(() => { TapeMeasureFtInFract meas = new TapeMeasureFtInFract(strMeas); });
				ex = Assert.Throws<ArgumentException>(() => { TapeMeasureFtInFract meas = new TapeMeasureFtInFract(feet, inches, new InchFraction(intDenom+1, intDenom)); });
			}
		}
	}
}
