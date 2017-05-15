using System;
using System.Collections.Generic;

namespace APIVCF
{
    public enum VARIABLE_TYPE {DENSITY,TEMPERATURE,PRESSURE,THERMAL_EXPANSION_COEFF,CTL,SCALED_COMPRESSIBILITY_FACTOR,CPL,CTPL};
    public enum COMMODITY_GROUP { ANY, CRUDE_OIL, FUEL_OILS, JET_FUELS, TRANSITION_ZONE, GASOLINES, LUBRICATING_OIL,GENERALIZED_REFINED_PRODUCT };
    public enum COMPARE { INCLUDE, INSIDE}; // meaning include = in comparing 

    public class ValueLimit
    {
        public COMMODITY_GROUP Group;
        public double Min;
        public COMPARE MinCompare = COMPARE.INCLUDE; // Default
        public double Max;
		public COMPARE MaxCompare = COMPARE.INCLUDE; // Default
        public void CheckLimits(double val)
        {
            bool minOk = MinCompare == COMPARE.INSIDE ? val > Min : val >= Min;
            if (!minOk)
                throw (new ArgumentOutOfRangeException(String.Format("Value must be greater than {1}{0}", Min, MinCompare==COMPARE.INCLUDE ? "or equal to " : "")));
			bool maxOk = MaxCompare == COMPARE.INSIDE ? val < Max : val <= Max;
			if (!maxOk)
				throw (new ArgumentOutOfRangeException(String.Format("Value must be less than {1}{0}", Max, MaxCompare == COMPARE.INCLUDE ? "or equal to " : "")));
        }
	}

    public class KCoeffs
    {
        public COMMODITY_GROUP CommodityGroup;
        public double k0;
        public double k1;
        public double k2;
    }


    public class UnitOfMeasure
	{
		public VARIABLE_TYPE VariableType;
		public string Name;
		public double Precision;
        public Dictionary<COMMODITY_GROUP, ValueLimit> Limits;
	}

    public class Calcs
    {
        Dictionary<string, UnitOfMeasure> uoms = new Dictionary<string, UnitOfMeasure>();
        Dictionary<COMMODITY_GROUP, KCoeffs> kCoeffs = new Dictionary<COMMODITY_GROUP, KCoeffs>();

        // Constructor
        public Calcs()
        {
            loadUoMs();
            loadKCoeffs();
        }

        #region Public methods

        // Section 11.1.2.2 - Equation 16
        public double GetThermExpCoeff60(double densITPS68, KCoeffs coeffs)
        {
            double alpha60 = coeffs.k0 / Math.Pow(densITPS68, 2) + coeffs.k1 / densITPS68 + coeffs.k2;
            return alpha60;
        }

		// Section 11.1.6.1 - Step 6
		public double  GetCompressFactor(double densITPS68,double tempITPS68)
		{
            double fp = Math.Exp(-1.9947 + 0.00013427 * tempITPS68 + (793920 + 2326.0 * tempITPS68) / Math.Pow(densITPS68,2));
            return fp;
		}

        // Section 11.1.6.1 Step 3 Ki Table
        public KCoeffs GetKCoeffs(COMMODITY_GROUP cgroup)
        {
            return kCoeffs[cgroup];
        }

        // Section 

        // Section 11.1.6.1  CTPL (commonly known as VCF)
        public double GetCTPLFromAPIDegFPsig(COMMODITY_GROUP grp,double api60,double tempF,double presPsig=0)
        {
            // Step 1 - Check range for density,temperature and pressure
            checkRange(api60, "API", grp);
            checkRange(tempF, "degF");
            if (presPsig < 0)
                presPsig = 0;
            checkRange(presPsig, "psig");

            KCoeffs coeffs = GetKCoeffs(grp);

            // Step 2 and 3  - Get corrected temp and density at ITP68 basis
            double tempITPS68 = Conversions.TempITS90toITPS68(tempF, "degF");
            double densITSP68 = Conversions.Api60ITS90tokgm3ITPS68(api60, coeffs); // kg/m3

            // Step 4 - Compute coefficient of thermal expansion
            double thermExpCoeff60 = GetThermExpCoeff60(densITSP68, coeffs);

            // Step 5 - Compute temperature correction factor
            double CTL = GetCTL(thermExpCoeff60, tempITPS68);

            double CPL = 1.0; // No compensations
            // If not ATM pressure correct for pressure
            if(presPsig>0)
            {
                // Step 6 - Compute compressibility factor
                double Fp = GetCompressFactor(densITSP68, tempITPS68);

                // Step 7 - Compute pressure correction
                CPL = GetCPL(Fp, presPsig);
			}

            // Step 8
            double CTPL = CTL * CPL;


            return RoundUp(CTPL,"CTPL");
        }


        // Section 11.1.3.5 
        // Coeff of Thermal Expansion
        public double GetCoeffOfThermExp()
        {
            throw (new NotImplementedException());
        }

		// Section 11.1.3.3. Equation 14 and Section 11.1.1.6 Step 5        
        // Temperature compensation CTL
		public double GetCTL(double thermExpCoeff60, double tempITPS68)
		{
			double deltaT = tempITPS68 - Conversions.baseT60;
			double ctl = Math.Exp(-thermExpCoeff60 * deltaT * (1 + 0.8 * thermExpCoeff60 * (deltaT + Conversions.deltaT60)));
            return ctl;
		}

		// Section 11.1.3.3. Equation 15 and Section 11.1.1.6 Step 7
		// Pressure compensation CPL
        public double GetCPL(double compressFactor,double pressPsig)
		{
            double cpl = 1 / (1 - 0.00001 * compressFactor * pressPsig);
			return cpl;
		}

        // Roundup calculations
        // Section 11.1.5.4 Rounding of Values
        public double RoundUp(double val, string units)
        {
            // Validate
            UnitOfMeasure uom = null;
            if (!uoms.TryGetValue(units.ToLower(), out uom))
                throw (new ArgumentException("Units {0} not recognized or supported"));

            // Find delta
            var delta = uom.Precision;

			// Compute norm
            double norm = Math.Abs(val)/delta;

            // Store sign
            double sign = val < 0 ? -1.0 : 1.0;

            // Convert to integer
            double i = Math.Truncate(norm+0.5);
            double inorm = Math.Truncate(norm);
            if((norm-inorm).CompareTo(0.5)==0)  // Fraction of norm is exactly 0.5
            {
                if(((long)inorm)%2==0)  // Is even
                    i = inorm;
            }

            // Rescale
            double x = sign * delta * i;

            return x;
        }

        #endregion

        #region Private methods

        void loadUoMs()
        {
            uoms = RulesLoader.GetUoMs();
        }
		void loadKCoeffs()
		{
            kCoeffs = RulesLoader.GetKCoeffs();
		}

        void checkRange(double val,string units,COMMODITY_GROUP grp=COMMODITY_GROUP.ANY)
        {
			// Validate and get UoM that contains the limits
			UnitOfMeasure uom = null;
			if (!uoms.TryGetValue(units.ToLower(), out uom))
				throw (new ArgumentException("Units {0} not recognized or supported"));

            // If not limits defined, there is nothig to check
            if (uom.Limits == null || uom.Limits.Count < 1)
                return;

            // Ignore commodity if limits are for ANY commodity
            if(uom.Limits.ContainsKey(COMMODITY_GROUP.ANY))
            {
                try
                {
                    uom.Limits[COMMODITY_GROUP.ANY].CheckLimits(val);
                }
                catch(ArgumentOutOfRangeException e)
                {
                    throw(new ArgumentOutOfRangeException(String.Format("{0} with units={1}",e.Message,units)));
                }
                return;
            }

            // Check limit for commodity
            ValueLimit limit=null;
            if(!uom.Limits.TryGetValue(grp,out limit))
                throw (new ArgumentException("Limits for commodity group {0} not supported"));
            try
            {
                limit.CheckLimits(val);
			}
			catch (ArgumentOutOfRangeException e)
			{
                throw (new ArgumentOutOfRangeException(String.Format("{0} with units={1} and commodity group={2}", e.Message, units,grp)));
			}
            return;
        }

		#endregion
	}

}
