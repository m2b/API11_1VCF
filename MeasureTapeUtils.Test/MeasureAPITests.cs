using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace APIVCF
{
    public class MeasureAPITest
    {
        const int MAXITER = 5;
        const double MAXFEET = 60;

		public class TestData
		{
            // Inputs
            public MeasureOperationToken op1;
            public MeasureOperationToken op2;
            public MeasureOperationToken op3;
			public MeasureOperationToken op4;
			public MeasureOperationToken op5;
			public double dec1;
            public double dec2;
			public double dec3;
            public double dec4;
			public double dec5;
            public string str1;
			public string str2;
			public string str3;
			public string str4;
			public string str5;

			// Outputs
			public bool cannotBeAdded;
			public TapeMeasureFtInFract out1;
		}

		public class TestDataGenerator
		{
			public static IEnumerable<object[]> GetTestExamples()
			{
                yield return new object[]
                {
                    // Test1
                    new TestData {
                        dec1=1.1614583,
						dec2=0.171875,
						dec3=1.1197916,
						dec4=2.9947916,
						dec5=1.0052083,
                        str1="1-1-15/16",
                        str2="0-2-1/16",
                        str3="1-1-7/16",
                        str4="2-11-15/16",
                        str5="1-0-1/16",
                        op1=new MeasureOperationToken(){Operation=OPERATION.NOP,Measure=new TapeMeasureFtInFract("1-1-15/16")},
						op2=new MeasureOperationToken(){Operation=OPERATION.ADD,Measure=new TapeMeasureFtInFract("0-2-1/16")},
						op3=new MeasureOperationToken(){Operation=OPERATION.ADD,Measure=new TapeMeasureFtInFract("1-1-7/16")},
						op4=new MeasureOperationToken(){Operation=OPERATION.ADD,Measure=new TapeMeasureFtInFract("2-11-15/16")},
						op5=new MeasureOperationToken(){Operation=OPERATION.ADD,Measure=new TapeMeasureFtInFract("1-0-1/16")},
						out1=new TapeMeasureFtInFract("6-5-7/16")
                    },
                    // Test2
                    new TestData {
						dec1=1.1614583,
						dec2=0.171875,
						dec3=1.1197916,
						dec4=2.9947916,
						dec5=1.0052083,
						str1="1-1-15/16",
						str2="0-2-2/32",
						str3="1-1-7/16",
						str4="2-11-15/16",
						str5="1-0-1/16",
						op1=new MeasureOperationToken(){Operation=OPERATION.ADD,Measure=new TapeMeasureFtInFract("1-1-15/16")},
						op2=new MeasureOperationToken(){Operation=OPERATION.ADD,Measure=new TapeMeasureFtInFract("0-2-2/32")},
						op3=new MeasureOperationToken(){Operation=OPERATION.ADD,Measure=new TapeMeasureFtInFract("1-1-7/16")},
                        op4=new MeasureOperationToken(){Operation=OPERATION.SUBSTRACT,Measure=new TapeMeasureFtInFract("2-11-15/16")},
						op5=new MeasureOperationToken(){Operation=OPERATION.ADD,Measure=new TapeMeasureFtInFract("1-0-1/16")},
						out1=new TapeMeasureFtInFract("0-5-9/16")
					},
                    // Test3
                    new TestData {
						dec1=1.1614583,
						dec2=0.171875,
						dec3=1.1197916,
						dec4=2.9947916,
						dec5=1.0052083,
						str1="1-1-15/16",
						str2="0-2-2/32",
						str3="1-1-7/16",
						str4="2-11-15/16",
						str5="1-0-1/16",
                        op1=new MeasureOperationToken(){Operation=OPERATION.SUBSTRACT,Measure=new TapeMeasureFtInFract("1-1-15/16")},
						op2=new MeasureOperationToken(){Operation=OPERATION.ADD,Measure=new TapeMeasureFtInFract("0-2-2/32")},
						op3=new MeasureOperationToken(){Operation=OPERATION.ADD,Measure=new TapeMeasureFtInFract("1-1-7/16")},
                        op4=new MeasureOperationToken(){Operation=OPERATION.ADD,Measure=new TapeMeasureFtInFract("2-11-15/16")},
                        op5=new MeasureOperationToken(){Operation=OPERATION.SUBSTRACT,Measure=new TapeMeasureFtInFract("1-0-1/16")},
						out1=new TapeMeasureFtInFract("2-1-7/16")
					},
                    // Test4
                    new TestData {
						cannotBeAdded=true,
						dec1=1.1614583,
						dec2=0.171875,
						dec3=1.1197916,
						dec4=2.9947916,
						dec5=1.0052083,
						str1="1-1-15/16",
						str2="0-2-2/32",
						str3="1-1-7/16",
						str4="2-11-15/16",
						str5="1-0-1/16",
						op1=new MeasureOperationToken(){Operation=OPERATION.NOP,Measure=new TapeMeasureFtInFract("1-1-15/16")},
						op2=new MeasureOperationToken(){Operation=OPERATION.ADD,Measure=new TapeMeasureFtInFract("0-2-2/32")},
						op3=new MeasureOperationToken(){Operation=OPERATION.SUBSTRACT,Measure=new TapeMeasureFtInFract("1-1-7/16")},
                        op4=new MeasureOperationToken(){Operation=OPERATION.SUBSTRACT,Measure=new TapeMeasureFtInFract("2-11-15/16")},
                        op5=new MeasureOperationToken(){Operation=OPERATION.SUBSTRACT,Measure=new TapeMeasureFtInFract("1-0-1/16")},
					},
                };
			}
		}

		[Theory]
		[MemberData(nameof(TestDataGenerator.GetTestExamples), MemberType = typeof(TestDataGenerator))]
		public void TestOperations(
            TestData test1, TestData test2, TestData test3, TestData test4
        )
		{
            TapeMeasureAPI api = new TapeMeasureAPI();

            // Good Additions
            List<MeasureOperationToken> tokens = new List<MeasureOperationToken>();
            tokens.Add(test1.op1);
            tokens.Add(test1.op2);
            tokens.Add(test1.op3);
            tokens.Add(test1.op4);
            tokens.Add(test1.op5);
            TapeMeasureFtInFract result = api.Add(tokens);
            Assert.Equal(result, test1.out1);
            Assert.True(result == test1.out1);
            tokens.Clear();
            // Order should not matter
			tokens.Add(test2.op2);
			tokens.Add(test2.op3);
			tokens.Add(test2.op5);
			tokens.Add(test2.op4);
			tokens.Add(test2.op1);
			result = api.Add(tokens);
			Assert.Equal(result, test2.out1);
			Assert.True(result == test2.out1);
			tokens.Clear();
			// Order should not matter
			tokens.Add(test3.op3);
			tokens.Add(test3.op4);
			tokens.Add(test3.op5);
			tokens.Add(test3.op2);
			tokens.Add(test3.op1);
			result = api.Add(tokens);
			Assert.Equal(result, test3.out1);
			Assert.True(result == test3.out1);


			// Bad Substractions (resulting in negative tape measures!)
			tokens.Clear();
			// Order should not matter
			tokens.Add(test4.op1);
			tokens.Add(test4.op2);
			tokens.Add(test4.op3);
			tokens.Add(test4.op4);
			tokens.Add(test4.op5);            
            Exception ex = Assert.Throws<ArgumentException>(() => { result = api.Add(tokens); });
		}

		[Theory]
		[MemberData(nameof(TestDataGenerator.GetTestExamples), MemberType = typeof(TestDataGenerator))]
		public void GetDecimalValuesTest(
			TestData test1, TestData test2, TestData test3, TestData test4
		)
        {
			TapeMeasureAPI api = new TapeMeasureAPI();
            var decimals = api.GetDecimalValues(new string[]{test1.str1,test1.str2,test1.str3,test1.str4,test1.str5});
            Assert.True(EqualsToPrecision(decimals[0], test1.dec1, 1e-6));
			Assert.True(EqualsToPrecision(decimals[1], test1.dec2, 1e-6)); 
            Assert.True(EqualsToPrecision(decimals[2], test1.dec3, 1e-6)); 
            Assert.True(EqualsToPrecision(decimals[3], test1.dec4, 1e-6)); 
            Assert.True(EqualsToPrecision(decimals[4], test1.dec5, 1e-6));

            // Add bad decimal value
			Exception ex = Assert.Throws<ArgumentException>(() => { decimals = api.GetDecimalValues(new string[] { test1.str1, test1.str2, test1.str3, test1.str4, test1.str5, "0-11/16" }); });
		}

		[Theory]
		[MemberData(nameof(TestDataGenerator.GetTestExamples), MemberType = typeof(TestDataGenerator))]
		public void GetObjectsTest(
	        TestData test1, TestData test2, TestData test3, TestData test4
        )
		{
			TapeMeasureAPI api = new TapeMeasureAPI();
            var objects = api.GetObjects(new string[] { test1.str1, test1.str2, test1.str3, test1.str4, test1.str5 });
			Assert.Equal(objects[0].ToString(), test1.str1);
			Assert.Equal(objects[1].ToString(), test1.str2);
			Assert.Equal(objects[2].ToString(), test1.str3);
			Assert.Equal(objects[3].ToString(), test1.str4);
			Assert.Equal(objects[4].ToString(), test1.str5);

            // Add bad decimal value
            Exception ex = Assert.Throws<ArgumentException>(() => { objects = api.GetObjects(new string[] { test1.str1, test1.str2, test1.str3, test1.str4, test1.str5,"123-12-11/32" }); });
		}

		[Theory]
		[MemberData(nameof(TestDataGenerator.GetTestExamples), MemberType = typeof(TestDataGenerator))]
		public void GetObjectsFromValuesTest(
			TestData test1, TestData test2, TestData test3, TestData test4
		)
		{
			TapeMeasureAPI api = new TapeMeasureAPI();
            var objects = api.GetObjectsFromValues(new double[] { test1.dec1, test1.dec2, test1.dec3, test1.dec4, test1.dec5 });
            Assert.Equal(objects[0].ToString(), test1.str1);
			Assert.Equal(objects[1].ToString(), test1.str2);
			Assert.Equal(objects[2].ToString(), test1.str3);
			Assert.Equal(objects[3].ToString(), test1.str4);
			Assert.Equal(objects[4].ToString(), test1.str5);

			// Add bad decimal value
			Exception ex = Assert.Throws<ArgumentException>(() => { objects = api.GetObjectsFromValues(new double[] { test1.dec1, test1.dec2, test1.dec3, test1.dec4, test1.dec5,-12.4}); });

		}

		[Theory]
		[MemberData(nameof(TestDataGenerator.GetTestExamples), MemberType = typeof(TestDataGenerator))]
		public void StringValidationTest(
	           TestData test1, TestData test2, TestData test3, TestData test4
        )
		{
			TapeMeasureAPI api = new TapeMeasureAPI();
            var objects = api.AreValid(new string[] { test1.str1, test1.str2, test1.str3, test1.str4, test1.str5 });
            Assert.True(objects[0].Item1);
			Assert.True(objects[1].Item1);
			Assert.True(objects[2].Item1);
			Assert.True(objects[3].Item1);
			Assert.True(objects[4].Item1);

			// Add bad strings
			objects = api.AreValid(new string[] { test1.str1, test1.str2, test1.str3, test1.str4, test1.str5,"0-13-0" });
			Assert.True(objects[0].Item1);
			Assert.True(objects[1].Item1);
			Assert.True(objects[2].Item1);
			Assert.True(objects[3].Item1);
			Assert.True(objects[4].Item1);
            Assert.False(objects[5].Item1);
		}

		// Utilty functions
		public bool EqualsToPrecision(double expected, double actual, double precision)
		{
			double diff = Math.Abs(expected - actual);
			return diff <= precision;
		}
	}
}
