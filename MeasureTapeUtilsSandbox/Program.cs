using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace APIVCF
{
    class MeasureTapeUtilsSandbox
    {
		const int upperMultBound = 4096;
        const int maxInteger = 100;
		const int iterations = 10;
        static double log2 = Math.Log10(2.0);

        static void Main(string[] args)
        {
            Console.WriteLine("Multiply by twos test");

            TimeMultiplications();

            Console.WriteLine("Testing InchFractions Object");

            // Good cases
            string[] validCases = { "1/32", "9/32", "17/32", "20/32", "27/32", "31/32", "1/16", "7/16", "11/16", "15/16", "1/8", "4/8", "7/8", "1/4", "2/4", "3/4", "1/2" };
            string[] badCases = { "0/32", "50/32", "32/32", "0/16", "23/16", "16/16", "0/8", "20/8", "8/8", "0/4", "7/4", "4/4", "0/2", "3/2", "2/2", "1", "12", "100" };
            foreach(var vc in validCases)
            {
                var obj1 = new InchFraction(vc);  // Constructor
                var parts = vc.Split('/');
                if (!(obj1.Numerator == Int32.Parse(parts[0]) && obj1.Denominator == (INCHFRACT)Enum.ToObject(typeof(INCHFRACT), Int32.Parse(parts[1]))
                      && obj1.StrFraction == vc))
                    throw (new Exception(String.Format("Object {0} test failed member access",vc)));
                // Test all valid conversions
                try
                {
                    var obj2=InchFraction.ConvertTo(INCHFRACT.THIRTYSECOND,obj1);
                    if (obj2.Numerator != obj1.Numerator * ((int)INCHFRACT.THIRTYSECOND / (int)obj1.Denominator))
                        throw(new Exception(String.Format("Object {0} test numerator conversion", vc)));
                }
                catch(Exception e)
                {
                    Console.WriteLine("Object {0} test failed to convert to 32nds due to exception {1}", vc, e.Message);
                }

                Console.WriteLine("Valid case {0} test succesful",vc);
			}

            // Specific valid conversions
            InchFraction frm = new InchFraction("10/16");
            INCHFRACT to = INCHFRACT.EIGHTH;
            InchFraction cvrted = InchFraction.ConvertTo(to, frm);
            Console.WriteLine("Conversion from {0} to {1} sucessful with result {2}", frm,to.ToString(),cvrted);
            frm = new InchFraction("12/16");
            to = INCHFRACT.EIGHTH;
			cvrted = InchFraction.ConvertTo(to, frm);
			Console.WriteLine("Conversion from {0} to {1} sucessful with result {2}", frm, to.ToString(), cvrted);
			to = INCHFRACT.FOURTH;
			cvrted = InchFraction.ConvertTo(to, frm);
			Console.WriteLine("Conversion from {0} to {1} sucessful with result {2}", frm, to.ToString(), cvrted);
			frm = new InchFraction("8/16");
            to = INCHFRACT.EIGHTH;
			cvrted = InchFraction.ConvertTo(to, frm);
			Console.WriteLine("Conversion from {0} to {1} sucessful with result {2}", frm, to.ToString(), cvrted);
			to = INCHFRACT.FOURTH;
			cvrted = InchFraction.ConvertTo(to, frm);
            to = INCHFRACT.HALF;
			cvrted = InchFraction.ConvertTo(to, frm);
			Console.WriteLine("Conversion from {0} to {1} sucessful with result {2}", frm, to.ToString(), cvrted);

			// Specific invalid conversions
			try
            {
                frm = new InchFraction("11/16");
                to = INCHFRACT.EIGHTH;
				cvrted = InchFraction.ConvertTo(to, frm);
                Console.WriteLine("Invalid conversion from {0} to {1} not caught", frm, to.ToString());
            }
            catch(ArgumentException ae)
            {
                if (ae.Message.ToLower().Contains("not possible"))
                    Console.WriteLine("Invalid conversion from {0} to {1} successful", frm, to.ToString());
                else
                    Console.WriteLine("Invalid conversion from {0} to {1} returned invalid exception {2}", frm, to.ToString(),ae.Message);
            }
            try
            {
			    frm = new InchFraction("10/16");
                to = INCHFRACT.FOURTH;
				cvrted = InchFraction.ConvertTo(to, frm);
				Console.WriteLine("Invalid conversion from {0} to {1} not caught", frm, to.ToString());
			}
			catch (ArgumentException ae)
			{
				if (ae.Message.ToLower().Contains("not possible"))
					Console.WriteLine("Invalid conversion from {0} to {1} successful", frm, to.ToString());
				else
					Console.WriteLine("Invalid conversion from {0} to {1} returned invalid exception {2}", frm, to.ToString(), ae.Message);
			}
            try
            {
			    frm = new InchFraction("12/16");
			    to = INCHFRACT.HALF;
				cvrted = InchFraction.ConvertTo(to, frm);
				Console.WriteLine("Invalid conversion from {0} to {1} not caught", frm, to.ToString());
			}
			catch (ArgumentException ae)
			{
				if (ae.Message.ToLower().Contains("not possible"))
					Console.WriteLine("Invalid conversion from {0} to {1} successful", frm, to.ToString());
				else
					Console.WriteLine("Invalid conversion from {0} to {1} returned invalid exception {2}", frm, to.ToString(), ae.Message);
			}
            try
            {
			    frm = new InchFraction("5/8");
			    to = INCHFRACT.FOURTH;
				cvrted = InchFraction.ConvertTo(to, frm);
				Console.WriteLine("Invalid conversion from {0} to {1} not caught", frm, to.ToString());
			}
			catch (ArgumentException ae)
			{
				if (ae.Message.ToLower().Contains("not possible"))
					Console.WriteLine("Invalid conversion from {0} to {1} successful", frm, to.ToString());
				else
					Console.WriteLine("Invalid conversion from {0} to {1} returned invalid exception {2}", frm, to.ToString(), ae.Message);
			}
            try
            {
			    frm = new InchFraction("3/4");
                to = INCHFRACT.HALF;
				cvrted = InchFraction.ConvertTo(to, frm);
				Console.WriteLine("Invalid conversion from {0} to {1} not caught", frm, to.ToString());
			}
			catch (ArgumentException ae)
			{
				if (ae.Message.ToLower().Contains("not possible"))
					Console.WriteLine("Invalid conversion from {0} to {1} successful", frm, to.ToString());
				else
					Console.WriteLine("Invalid conversion from {0} to {1} returned invalid exception {2}", frm, to.ToString(), ae.Message);
			}


			// Static functions
			foreach(var vc in validCases)
            {
                var obj = new InchFraction(vc);
                double denominator = InchFraction.GetDecimalDenominator(obj.Denominator);
                double val = InchFraction.GetDecimalValue(obj);
                if ((denominator * obj.Numerator) != val)
                    throw (new Exception(String.Format("Object {0} test failed static methods test with value {1}",vc,val)));
                Console.WriteLine("Valid case {0} static functions test succesful with value {1}", vc, val);                
            }

            // Bad cases
            foreach(var bc in badCases)
            {
                try
                {
                    var ojb1 = new InchFraction(bc);
                    Console.WriteLine("Invalid case {0} test failed",bc);
                }
                catch(ArgumentException ae)
                {
                    if (ae.Message.ToLower().Contains("fraction string"))
                        Console.WriteLine("Invalid case {0} test succesful",bc);
                    else
                        Console.WriteLine("Invalid case {0} test returned invalid exception {1}", bc, ae.Message);
                }
            }

            // Calculations from design example
            TapeMeasureAPI api = new TapeMeasureAPI();
            List<MeasureOperationToken> tokens = new List<MeasureOperationToken>();
            tokens.Add(new MeasureOperationToken() { Operation = OPERATION.NOP, Measure = new TapeMeasureFtInFract("48-10-1/2") });
            tokens.Add(new MeasureOperationToken() { Operation = OPERATION.SUBSTRACT, Measure = new TapeMeasureFtInFract("21-11-1/2") });
			tokens.Add(new MeasureOperationToken() { Operation = OPERATION.ADD, Measure = new TapeMeasureFtInFract("0-3-9/16") });
            TapeMeasureFtInFract out1 = api.Add(tokens);
            Console.WriteLine("Result Level Reading 1={0}",out1.ToString());
            tokens.Clear();
			tokens.Add(new MeasureOperationToken() { Operation = OPERATION.NOP, Measure = new TapeMeasureFtInFract("48-10-1/2") });
			tokens.Add(new MeasureOperationToken() { Operation = OPERATION.SUBSTRACT, Measure = new TapeMeasureFtInFract("21-11-9/16") });
			tokens.Add(new MeasureOperationToken() { Operation = OPERATION.ADD, Measure = new TapeMeasureFtInFract("0-3-1/2") });
			TapeMeasureFtInFract out2 = api.Add(tokens);
            Console.WriteLine("Result Level Reading 2={0}", out2.ToString());
			tokens.Clear();
			tokens.Add(new MeasureOperationToken() { Operation = OPERATION.NOP, Measure = out1});
			tokens.Add(new MeasureOperationToken() { Operation = OPERATION.ADD, Measure = out2});
            var decimals = api.GetDecimalValues(new string[]{ out1.ToString(), out2.ToString() });
            var avgLvlDecimal = (decimals[0] + decimals[1]) / 2.0;
            Console.WriteLine("Average of two readings={0}", api.GetObjectsFromValues(new double[]{avgLvlDecimal})[0].ToString());
            string[] waterMeas = { "0-3-3/16", "0-3-1/4" };
            Console.WriteLine("Water readings={0},{1}",waterMeas[0],waterMeas[1]);
            decimals = api.GetDecimalValues(waterMeas);
			avgLvlDecimal = (decimals[0] + decimals[1]) / 2.0;
			Console.WriteLine("Average of two water level readings={0}", api.GetObjectsFromValues(new double[] { avgLvlDecimal })[0].ToString());
            Console.WriteLine("Average of two readings={0}", api.Average(new List<TapeMeasureFtInFract>(){out1,out2}));
            var waterReadings = new List<TapeMeasureFtInFract>(api.GetObjects(waterMeas));
            Console.WriteLine("Average of two water level readings={0}", api.Average(waterReadings));

			Console.WriteLine("Press any key to end");
            Console.ReadKey();
        }


        static void TimeMultiplications()
        {
            Stopwatch s1 = new Stopwatch();
			Stopwatch s2 = new Stopwatch();
            double r = (1.0 * iterations) * TimeSpan.TicksPerMillisecond;
            var bound = upperMultBound;
            do
            {
                for (int number = 0; number <= 100;number++)
                {
                    // Simple Multiplication

                    int resultSimple;
                    int resultShift;
	                for (int i = 0; i < iterations; i++)
	                {
                        s1.Start();
                        resultSimple = MultByTwos(number, bound);
                        s1.Stop();

						s2.Start();
                        resultShift = MultByTwosBitShifting(number, bound);
                        s2.Stop();
                        if (resultShift != resultSimple)
                            throw (new Exception(String.Format("Results do not match for Number={0} and Multiplier={1}",number,bound)));
					}
                    Console.WriteLine("Number={0}, Multiplier={1}, Avg(ms) Simple={2}, Avg(ms) Shifting={3}",number,bound,s1.Elapsed.Ticks/r,s2.Elapsed.Ticks / r);
                    s1.Reset();
                    s2.Reset();
	            }

                bound = bound / 2;
            }
            while (bound>1);

        }

        static int MultByTwos(int number, int mult)
        {
            return number * mult;
        }

        static int MultByTwosBitShifting(int number, int mult)
        {
            int ret = number;
            do
            {
                ret = ret << 1;
                mult = mult >> 1;
            }
            while (mult > 1);
            return ret;
        }

		static int MultByTwosBitShifting3(int number, int mult)
		{
            int shifts = 0;
            while(mult>1)
            {
                shifts++;
				mult = mult >> 1;
			}
            return number<<shifts;
		}

		static int MultByTwosBitShifting2(int number, int mult)
		{
            int shifts = (int)(Math.Log10(mult)/log2) ;
            return number<<shifts;
		}
    }
}
