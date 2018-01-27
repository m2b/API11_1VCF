using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace APIVCF
{
    public enum INCHFRACT { HALF = 2, FOURTH = 4, EIGHTH = 8, SIXTEENTH = 16, THIRTYSECOND = 32 }

    public class InchFraction : IComparable<InchFraction>,IEquatable<InchFraction>
    {

        static double[] _decimalDenominators = { 0.50000, 0.25000, 0.12500, 0.06250, 0.03125 };
        static Dictionary<INCHFRACT, double> _fracts = null;

        object _lock = new object();

        string _strFraction;
        int _numerator;
        INCHFRACT _denominator;

		#region Operator overloading

		// Overloaded '=='
        // https://blogs.msdn.microsoft.com/abhinaba/2005/10/11/c-comparison-operator-overloading-and-spaceship-operator/
        public static bool operator ==(InchFraction f1, InchFraction f2)
		{
			return Compare(f1, f2)==0;
		}

		// Recommended overload to not create confusion
		public override bool Equals(object obj)
		{
            if (!(obj is InchFraction))
				return false;

            return this == (InchFraction)obj;
		}

		// Also recommended to get comparable Hashcodes for dictionaries and such
		public override int GetHashCode()
		{
			int hash = 17;
            hash = hash * 31 + Numerator;
            hash = hash * 31 + Denominator.GetHashCode();
			return hash; ;
		}

		// Overloaded '!='
        public static bool operator !=(InchFraction f1, InchFraction f2)
		{
            return Compare(f1, f2)!=0;
		}

		// Overloaded '>'
        public static bool operator >(InchFraction f1, InchFraction f2)
		{
			return Compare(f1,f2) > 1;
		}

		// Overloaded '<'
        public static bool operator <(InchFraction f1, InchFraction f2)
		{
			return Compare(f1,f2) < 1;
		}

		// Overloaded '>='
        public static bool operator >=(InchFraction f1, InchFraction f2)
		{
			return Compare(f1,f2) >= 0;
		}

		// Overloaded '<='
        public static bool operator <=(InchFraction f1, InchFraction f2)
		{
			return Compare(f1,f2) <= 0;
		}


		#endregion


		#region Properties and Public Methods

		public override string ToString()
        {
            return StrFraction;
        }

        public string StrFraction
        {
            get
            {
                return _strFraction;
            }
            set
            {
                if (IsValidString(value))
                {
                    _strFraction = value;
                    string[] parts = value.Split('/');
                    _numerator = Int32.Parse(parts[0]);
                    _denominator = (INCHFRACT)Enum.ToObject(typeof(INCHFRACT), Int32.Parse(parts[1]));
                }
                else
                    throw (new ArgumentException("Fraction string must be one of 1/32...31/32 or 1/16..15/16 or 1/8...7/8 or 1/4...3/4 or 1/2"));
            }
        }

        public int Numerator
        {
            get
            {
                return _numerator;
            }
            set
            {
                StrFraction = toString(value, (int)_denominator);
            }
        }

        public INCHFRACT Denominator
        {
            get
            {
                return _denominator;
            }
            set
            {
                StrFraction = toString(_numerator, (int)value);
            }
        }

        public static double GetDecimalValue(InchFraction fract)
        {
            if ((System.Object)fract == null)  // Cast needed to prevent stack overflow
                return 0;
            return fract.Numerator * _fracts[fract.Denominator];
        }

        public static double GetDecimalDenominator(INCHFRACT fract)
        {
            return _fracts[fract];
        }

        public static bool IsValidString(string input)
        {
            if (String.IsNullOrEmpty(input))
                return false;

            string pattern = @"^(3[0-1]\/32|[1-2][0-9]\/32|[1-9]\/32|1[0-5]\/16|[1-9]\/16|[1-7]\/8|[1-3]\/4|1\/2)$";
            return Regex.IsMatch(input, pattern);
        }

        public static InchFraction ConvertTo(INCHFRACT to, InchFraction fract)
        {
            if ((System.Object)fract == null)
				throw (new ArgumentNullException(nameof(fract)));

            int f = (int)fract.Denominator;
            int t = (int)to;
            if (t < f)
            {
                int fn = fract.Numerator;
                int fd = f;
                do
                {
                    if ((fn % 2) != 0)
                        throw (new ArgumentException(String.Format("Conversion not possible from {0} to {1} when numerator is {2}", f, t, fract.Numerator)));
                    fn = fn / 2;
                    fd = fd / 2;
                }
                while (fd > t);
                return new InchFraction(fn, t);
            }
            else if (f == t)
                return fract;
            return new InchFraction((t / f) * fract.Numerator, t);
        }

        public static InchFraction Reduce(InchFraction fract)
        {
            if (fract.Denominator == INCHFRACT.HALF)
                return fract;
            var denoms = Enum.GetValues(typeof(INCHFRACT)).Cast<INCHFRACT>();
            InchFraction ret = fract;
            foreach (var denom in denoms)
            {
                if (fract.Denominator < denom)
                    break;
                try
                {
                    ret = InchFraction.ConvertTo(denom, ret);
                    break; // Once found
                }
                catch (ArgumentException e) {; }
            }
            return ret;
        }

        #endregion

        #region Constructors

        public InchFraction()
        {
            init();
        }

        public InchFraction(string fract)
        {
            StrFraction = fract;
            init();
        }

        public InchFraction(int numerator, int denominator)
        {
            _strFraction = toString(numerator, denominator);
            _numerator = numerator;
            _denominator = (INCHFRACT)Enum.ToObject(typeof(INCHFRACT), denominator);
            init();
        }

        #endregion

        #region Private methods

        void init()
        {
            lock (_lock)
            {
                if (_fracts == null)
                {
                    _fracts = new Dictionary<INCHFRACT, double>();
                    int idx = 0;
                    foreach (var fract in Enum.GetValues(typeof(INCHFRACT)).Cast<INCHFRACT>())
                    {
                        _fracts.Add(fract, _decimalDenominators[idx]);
                        idx++;
                    }
                }
            }
        }

        string toString(int nominator, int denominator)
        {
            string test = nominator + "/" + denominator;
            if (IsValidString(test))
                return test;
            else
                throw (new ArgumentException("Nominator and denominators must result in on of the following 1/32...31/32 or 1/16..15/16 or 1/8...7/8 or 1/4...3/4 or 1/2"));
        }

		#endregion


		#region IComparable,IEquatable

        public bool Equals(InchFraction other)
        {
            return this.CompareTo(other)==0;
        }

        public int CompareTo(InchFraction other)
        {
            return Compare(this, other);
        }

        public static int Compare(InchFraction f1, InchFraction f2)
        {
            if ((System.Object)f1 == null)  // Cast needed to prevent stack overflow
			{
                if ((System.Object)f2 == null)
                    return 0;
                else
                    return -1;
            }
            else if ((System.Object)f2 == null)
                return 1;
            
			var me = Reduce(f1);
			var oth = Reduce(f2);
			int numCompare = me.Numerator.CompareTo(oth.Numerator);
			if (numCompare == 0)
			{
				int denCompare = me.Denominator.CompareTo(oth.Denominator);
				if (denCompare == 0)
				{
					return 0;
				}
				else
					return denCompare;
			}
			else
				return numCompare;
        }

        #endregion
    }

    public class TapeMeasureFtInFract:IComparable<TapeMeasureFtInFract>,IEquatable<TapeMeasureFtInFract>
    {
        string _strMeasure;
        int _feet;
        int _inches;
        InchFraction _fract;


		#region Operator overloading

		// Overloaded '=='
		// https://blogs.msdn.microsoft.com/abhinaba/2005/10/11/c-comparison-operator-overloading-and-spaceship-operator/
		public static bool operator ==(TapeMeasureFtInFract tm1, TapeMeasureFtInFract tm2)
        {
            return Compare(tm1,tm2)==0;
        }

        // Recommended overload to not create confusion
		public override bool Equals(object obj)
		{

            if (!(obj is TapeMeasureFtInFract)) 
                return false;

            return this == (TapeMeasureFtInFract)obj;
		}

        // Also recommended to get comparable Hashcodes for dictionaries and such
        public override int GetHashCode()
        {
			int hash = 17;
			hash = hash * 31 + Feet;
            hash = hash * 31 + Inches;
            if (Fract!=null)
            {
				hash = hash * 31 + Fract.GetHashCode();                
            }
			return hash;;
        }

		// Overloaded '!='
		public static bool operator !=(TapeMeasureFtInFract tm1, TapeMeasureFtInFract tm2)
		{
            return Compare(tm1,tm2)!=0;
		}

		// Overloaded '>'
		public static bool operator >(TapeMeasureFtInFract tm1, TapeMeasureFtInFract tm2)
		{
            return Compare(tm1,tm2)>1;
		}

		// Overloaded '<'
		public static bool operator <(TapeMeasureFtInFract tm1, TapeMeasureFtInFract tm2)
		{
			return Compare(tm1,tm2)<1;
		}

		// Overloaded '>='
		public static bool operator >=(TapeMeasureFtInFract tm1, TapeMeasureFtInFract tm2)
		{
            return Compare(tm1,tm2) >=0;
		}

		// Overloaded '<='
		public static bool operator <=(TapeMeasureFtInFract tm1, TapeMeasureFtInFract tm2)
		{
			return Compare(tm1,tm2) <= 0;
		}

		// Overloaded (+)
		static readonly INCHFRACT maxDenominator = INCHFRACT.THIRTYSECOND;
        public static TapeMeasureFtInFract operator +(TapeMeasureFtInFract tm1,TapeMeasureFtInFract tm2)
		{

            TapeMeasureFtInFract ret = new TapeMeasureFtInFract();

            // Handle fractions
            int commonDenom = (int)maxDenominator;
            InchFraction fract=null;
            int inCarry = 0;
            if (tm2.Fract == null && tm1.Fract != null)
                fract = tm1.Fract;
            else if (tm1.Fract == null && tm2.Fract != null)
                fract = tm2.Fract;
            else if (tm2.Fract != null && tm2.Fract != null)
            {
                // Convert to common denominator 
                var tm1Fract = InchFraction.ConvertTo(maxDenominator, tm1.Fract);
                var tm2Fract = InchFraction.ConvertTo(maxDenominator, tm2.Fract);

                // Add fractions and carry
                int numerator = tm2Fract.Numerator + tm1Fract.Numerator;
                if (numerator >= commonDenom)
                    inCarry++;
                numerator = (numerator % commonDenom);

                if (numerator > 0)
                {
                    // Reduce fraction
                    fract = new InchFraction(numerator, commonDenom);
                    fract = InchFraction.Reduce(fract);
                }
			}
			// Handle inches
			int ftCarry = 0;
            inCarry = inCarry + tm1.Inches + tm2.Inches;
            if (inCarry >= 12)
                ftCarry++;
            inCarry = (inCarry % 12);

            // Handle feet
            ftCarry = ftCarry + tm1.Feet + tm2.Feet;

            ret = new TapeMeasureFtInFract(ftCarry, inCarry, fract);

			return ret;
		}

		// Overloaded (-)
		public static TapeMeasureFtInFract operator -(TapeMeasureFtInFract tm1, TapeMeasureFtInFract tm2)
		{
			TapeMeasureFtInFract ret = new TapeMeasureFtInFract();

			// Handle fractions
			int commonDenom = (int)maxDenominator;
			InchFraction fract = null;
			int inBorrow = 0;
			int numerator = 0;
			if (tm2.Fract == null && tm1.Fract != null)
                fract = tm1.Fract;
			else if (tm1.Fract == null && tm2.Fract != null)
            {
                inBorrow--;
                commonDenom = (int)tm2.Fract.Denominator;
                numerator = commonDenom-tm2.Fract.Numerator;
            }
            else if(tm1.Fract!=null && tm2.Fract!=null)
            {
				// Convert to common denominator 
				var tm1Fract = InchFraction.ConvertTo(maxDenominator, tm1.Fract);
				var tm2Fract = InchFraction.ConvertTo(maxDenominator, tm2.Fract);

                // Substract fractions and borrow if necessary
                if(tm2Fract.Numerator>tm1Fract.Numerator)
                {
                    inBorrow--;
                    numerator = commonDenom + tm1Fract.Numerator - tm2Fract.Numerator;
                }
                else
                {
                    numerator = tm1Fract.Numerator - tm2Fract.Numerator;
                }
			}

            if (numerator > 0)
            {
                // Reduce fraction
                fract = new InchFraction(numerator, commonDenom);
                fract = InchFraction.Reduce(fract);
            }

			// Handle inches
			int ftBorrow = 0;
            var inches = inBorrow + tm1.Inches;
            if(tm2.Inches>inches)
            {
                ftBorrow--;
                inches = 12 + inches - tm2.Inches;
            }
            else
            {
                inches = inches - tm2.Inches;
            }

			// Handle feet
			var feet = ftBorrow + tm1.Feet;
            if (tm2.Feet > feet)
				throw (new ArgumentException(String.Format("Cannot substract {0} from {1} since it will result in negative tape measure", tm2, tm1)));
            feet = feet - tm2.Feet;

			ret = new TapeMeasureFtInFract(feet,inches,fract);

			return ret;
		}


		#endregion

		#region Properties and Public Methods

		public override string ToString()
		{
			return StrMeasure;
		}

        public string StrMeasure
        {
            get => _strMeasure;
            set
            {
                if (IsValidString(value))
                {
                    _strMeasure = value;
                    string[] parts = _strMeasure.Split('-');
                    _feet = Int32.Parse(parts[0]);
                    _inches = Int32.Parse(parts[1]);
                    if (parts[2]=="0")
                        _fract = null;
                    else
                        _fract = new InchFraction(parts[2]);
                }
				else
					throw (new ArgumentException("String must be in the format feet-in-fraction where feet can be 0 or greater, inches can be 0 to 11 and fraction must be one of 1/32...31/32 or 1/16..15/16 or 1/8...7/8 or 1/4...3/4 or 1/2"));
			}
        }

        public int Feet
        {
            get => _feet;
            set
            {
                if (value >= 0)
                {
                    _feet = value;
					StrMeasure = toString(_feet, _inches, _fract.StrFraction);
                }
                else
                    throw (new ArgumentOutOfRangeException(nameof(Feet), "Feet must be greater than or equal to zero"));
            }
        }

        public int Inches
        {
            get => _inches;
            set
            {
                if (_inches < 11 && _inches >= 0)
                {
                    _inches = value;
                    StrMeasure = toString(_feet, _inches, _fract.StrFraction);
                }
                else
                    throw (new ArgumentOutOfRangeException(nameof(Inches), "Inches must be between 0 and 11"));
            }
        }

        public InchFraction Fract 
        { 
            get => _fract; 
            set
            {
                _fract = value; // Allow setting to null to mean 0
                StrMeasure = toString(_feet, _inches, (_fract==null?null:_fract.StrFraction));
			}
        }

        // Must be in the form ft-in-fraction where ft and in can be 0.
		public static bool IsValidString(string input)
		{
			if (String.IsNullOrEmpty(input))
				return false;

            string pattern = @"^(0|[^0]\d*)-(0|[1-9]|1[0-1])-(0|3[0-1]\/32|[1-2][0-9]\/32|[1-9]\/32|1[0-5]\/16|[1-9]\/16|[1-7]\/8|[1-3]\/4|1\/2)$";
			return Regex.IsMatch(input, pattern);
		}


        public static double GetDecimalValue(TapeMeasureFtInFract measure)
		{
            if ((System.Object)measure == null)  // Cast needed to prevent stack overflow
				return 0;
            return measure.Feet + (measure.Inches + InchFraction.GetDecimalValue(measure.Fract))/12;
		}

        public static TapeMeasureFtInFract GetFromDecimal(double val)
        {
            if (val < 0)
                throw (new ArgumentException("Negative values not supported"));
            else if (val == 0)
                return new TapeMeasureFtInFract();

			TapeMeasureFtInFract ret = new TapeMeasureFtInFract();
			int feet = 0;
			int inches = 0;
			InchFraction fract = null;
			double decimals = 0;

			// Get feet and decimals
			string strVal = val.ToString();
            string[] parts;
            int idx = strVal.IndexOf('.');
            if (idx < 0 || idx == (strVal.Length - 1))  // No decimals or ends with period (e.g. 123.) 
            {
                ret = new TapeMeasureFtInFract((int)val, 0, null);
                return ret;
            }
            parts = strVal.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)  // case of .2343 (all decimals!)
            {
                decimals = Double.Parse("0." + parts[0]);
            }
            else
            {
                feet = Int32.Parse(parts[0]);
                decimals = Double.Parse("0." + parts[1]);
            }

            // Split decimals inches into inches fractions
            decimals = 12.0 * decimals;
            strVal = decimals.ToString();
			idx = strVal.IndexOf('.');
            if (idx < 0 || idx == (strVal.Length - 1)) // No decimals or ends with period (e.g. 123.)
            {
                ret = new TapeMeasureFtInFract(feet, (int)decimals, null);
                return ret;
            }
            parts = strVal.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)  // case of .2343 (all decimals!)
            {
                decimals = Double.Parse("0." + parts[0]);
            }
            else
            {
                inches = Int32.Parse(parts[0]);
                decimals = Double.Parse("0." + parts[1]);
            }

            // Compute nearest fraction of an inch
            if (decimals > 0)
            {
                int denominator = (int)maxDenominator;
                int numerator = (int)Math.Round(denominator * decimals);
                // Reduce
                fract = new InchFraction(numerator, denominator);
                fract = InchFraction.Reduce(fract);
            }

            ret = new TapeMeasureFtInFract(feet, inches, fract);
            return ret;
		}

        #endregion

        #region Constructors

        public TapeMeasureFtInFract() { ; }

        public TapeMeasureFtInFract(string measure)
        {
            StrMeasure = measure;   
        }

        public TapeMeasureFtInFract(int feet, int inches, InchFraction fract)
        {
            _strMeasure = toString(feet, inches, (fract == null ? null : fract.StrFraction));
            _feet=feet;
            _inches = inches;
            _fract = fract;
        }


		#endregion

		#region Private methods

		string toString(int feet, int inches, string fract)
		{
            string test = feet + @"-" + inches + @"-" + (String.IsNullOrEmpty(fract) ? "0" : fract);
            if (IsValidString(test))
				return test;
			else
				throw (new ArgumentException("String must be in the format feet-in-fraction where feet can be 0 or greater, inches can be 0 to 11 and fraction must be one of 1/32...31/32 or 1/16..15/16 or 1/8...7/8 or 1/4...3/4 or 1/2"));
		}

		#endregion

		#region IComparable,IEquatable

        public bool Equals(TapeMeasureFtInFract other)
		{
			return this.CompareTo(other) == 0;
		}

        public int CompareTo(TapeMeasureFtInFract other)
		{
            return Compare(this, other);
		}

        public static int Compare(TapeMeasureFtInFract tm1,TapeMeasureFtInFract tm2)
        {
            if ((System.Object)tm1 == null)  // Cast necessare to prevent stack overflow
            {
                if ((System.Object)tm2 == null)
                    return 0;
                else
                    return -1;
            }
            else if ((System.Object)tm2 == null)
                return 1;
            
			int ftCompare = tm1.Feet.CompareTo(tm2.Feet);
			if (ftCompare == 0)
			{
				int inCompare = tm1.Inches.CompareTo(tm2.Inches);
				if (inCompare == 0)
                {
                    int fractCompare = InchFraction.Compare(tm1.Fract, tm2.Fract);
					if (fractCompare == 0)
						return 0;
					else
						return fractCompare;
				}
				else
					return inCompare;
			}
			else
				return ftCompare;
        }

		#endregion
	}
}
