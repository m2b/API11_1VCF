using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace APIVCF
{
    public enum VARIABLE_TYPE {DENSITY,TEMPERATURE,PRESSURE,THERMAL_EXPANSION_COEFF,CTL,SCALED_COMPRESSIBILITY_FACTOR,CPL,CTPL};
    public enum COMMODITY_GROUP { ANY, CRUDE_OIL, LUBRICATING_OIL,GENERALIZED_REFINED_PRODUCT,LPG_NGL,FUEL_OILS=100, JET_FUELS=101, TRANSITION_ZONE=102, GASOLINES=103 };
    public enum COMPARE { INCLUDE, INSIDE}; // meaning include = in comparing 
    public enum LIQ_GAS_FLUID {EE_68_32=1,ETHANE,EP_65_35,EP_35_65,PROPANE,iBUTANE,nBUTANE,iPENTANE,nPENTANE,iHEXANE,nHEXANE,nHEPTANE }

    // API 11.2.5 Section 2 Table 1
    public class VapPressCorrParams
    {
        public ValueLimit RelDensityRange;
        public double A0;
        public double A1;
        public double A2;
        public double B0;
        public double B1;
        public double B2;
    }


    // API 11.2.4 Section 5.1.1.3 Table 1
    public class LiqGasProperties
    {
        public LIQ_GAS_FLUID Fluid;
        public double relDens60; // SG at 60
        public double tempCritK; // Deg Kelvin
        public double comprFactCrit; 
        public double densCrit; // Gram Moles per Liter
        public double k1;  // Saturation fitting parameters
        public double k2;
        public double k3;
        public double k4;
    }

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

        public bool ValueIsWithin(double val)
        {
			bool minOk = MinCompare == COMPARE.INSIDE ? val > Min : val >= Min;
            if (!minOk)
                return false;
			bool maxOk = MaxCompare == COMPARE.INSIDE ? val < Max : val <= Max;
            if (!maxOk)
                return false;
            return true;
        }

        // Override equals
        public override bool Equals(object obj)
        {
            ValueLimit other = obj as ValueLimit;
            if (other != null)
                return false;
            if (other.MaxCompare != this.MinCompare)
                return false;
            if (other.MaxCompare != this.MaxCompare)
                return false;
            if (other.Min != this.Min)
                return false;
            if (other.Max != this.Max)
                return false;
            return true;
        }

        // Override hashcode
        public override int GetHashCode()
		{
			int hash = 17;
			hash = hash * 23 + Min.GetHashCode();
			hash = hash * 23 + Max.GetHashCode();
            hash = hash * 23 + (1.0*(int)MinCompare).GetHashCode();
            hash = hash * 23 + (1.0*(int)MaxCompare).GetHashCode();
			return hash;
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
        Dictionary<LIQ_GAS_FLUID, LiqGasProperties> lgProps = new Dictionary<LIQ_GAS_FLUID, LiqGasProperties>();
        List<VapPressCorrParams> vpCorrParms = new List<VapPressCorrParams>();

        // Constructor
        public Calcs()
        {
            loadUoMs();
            loadKCoeffs();
            loadLiqGasProperties();
            loadVapCorrParams();
        }

        #region Public methods

        // Section 11.1.2.2 - Equation 16
        public double GetThermExpCoeff60(double densITPS68, KCoeffs coeffs)
        {
            double alpha60 = coeffs.k0 / (densITPS68*densITPS68) + coeffs.k1 / densITPS68 + coeffs.k2;
            return alpha60;
        }

		// Section 11.1.6.1 - Step 6
		public double  GetCompressFactor(double densITPS68,double tempITPS68)
		{
            double fp = Math.Exp(-1.9947 + 0.00013427 * tempITPS68 + (793920 + 2326.0 * tempITPS68) / (densITPS68 * densITPS68));
            return fp;
		}

        // Section 11.1.6.1 Step 3 Ki Table
        public KCoeffs GetKCoeffs(COMMODITY_GROUP cgroup,double api60=0.0)
        {
            // If density is passed, try to match commodity group to density
            if(api60>0.0)
			    cgroup = GetKCoeffsGroup(cgroup, api60);

            KCoeffs coeffs=null;
            if (!kCoeffs.TryGetValue(cgroup, out coeffs))
                throw (new ArgumentException(String.Format("Coefficients for COMMODITY_GROUP {0} not found", cgroup)));
            return coeffs;
        }

        // API 11.2.4 Section 5.1.1.3 Table 1
        public LiqGasProperties GetLiqGasProps(LIQ_GAS_FLUID fluid)
        {
            return lgProps[fluid];
        }

        // Convenient method to handle all in a single call
        public double GetCTPLFromApiDegFPsig(COMMODITY_GROUP grp, double api60, double tempF, double? presPsig = null,double? vapPress=null)
        {
            if (grp == COMMODITY_GROUP.LPG_NGL)
            {
                if (vapPress == null)
                    vapPress = GetVapPressPsia(api60,tempF) + Conversions.pressAtmPsi;  // From API 11.2.5 estimation
                return GetCTPLFromAPIDegFPsigLiqGas(api60, tempF, presPsig == null ? 0 : presPsig.Value, vapPress.Value);
            }
            return GetCTPLFromApiDegFPsigNONLiqGas(grp, api60, tempF, presPsig==null ? 0 : presPsig.Value);
        }

		// Section 11.1.3.3. Equation 14 and Section 11.1.1.6 Step 5        
        // Temperature compensation CTL
		public double GetCTL(double thermExpCoeff60, double tempITPS68)
		{
			double deltaT = tempITPS68 - Conversions.baseT60;
			double ctl = Math.Exp(-thermExpCoeff60 * deltaT * (1 + 0.8 * thermExpCoeff60 * (deltaT + Conversions.deltaT60)));
            return ctl;
		}

        // API 11.2.4 Section 5.1.1.3
        // Temperature compensation CTL
        public double GetCTLLiqGas(double tempF,double api60)
        {
			// Step T24/3
			// Check Density and Temperature range
			COMMODITY_GROUP grp = COMMODITY_GROUP.LPG_NGL;
            checkRange(api60, "API", grp);
			checkRange(tempF, "degF", grp); // Group must be specified

            // Step T24/2
            // Convert temp to Kelvin and roundup
            tempF = RoundUp(tempF, -1);
			double Tx = Conversions.DegFtoDegK(tempF);

            // Convert to relative density and roundup
			double relDens60 = Conversions.APItoSG(api60);
            relDens60 = RoundUp(relDens60, -4);

            // Step T24/4
            // Chose reference fluid subscripts (1,2)
            LiqGasProperties f1=null;
            LiqGasProperties f2=null;
            LiqGasProperties prev=null;
            foreach(var lgProp in lgProps)
            {
                if (prev!=null)
                {
                    if (lgProp.Value.relDens60>=relDens60)
                    {
                        f2 = lgProp.Value;
                        f1 = prev;
                        break;
                    }
                }
                prev = lgProp.Value;
            }
            if (f2 == null || f1==null)
                throw (new ArgumentException(String.Format("Relative density {0} is out of the range of API 11.2.4 Table 1",api60)));

            // Step T24/5 
            // Compute interpolation variable
            double delta = (relDens60 - f1.relDens60) / (f2.relDens60 - f1.relDens60);

            // Step T24/6
            // Compute interpolated critical temperature
            double Tc= f1.tempCritK + delta * (f2.tempCritK - f1.tempCritK);

            // Step T24/7
            // Compute reduced temperature ratio
            double Trx = Tx / Tc;
            if (Trx > 1)
                throw (new ArgumentException(String.Format("Temperature {0} will result in supercritical conditions which are not supported by this computation", tempF)));

            // Step 24/8
            // Compute reduced temperature at 60F
            double t60K = Conversions.DegFtoDegK(60);
            double Tr60 = t60K/Tc;

            // Step 24/9
            // Compute scaling factor
            double h2 = (f1.comprFactCrit * f1.densCrit) / (f2.comprFactCrit * f2.densCrit);

            // Step 24/10
            // Compute saturation density of both fluids at 60F
            double dens60_1 = getSatDensityAtTemp(Tr60, f1);
			double dens60_2 = getSatDensityAtTemp(Tr60, f2);

            // Step 24/11
            // Calculate interpolating factor
            Func<double,double,double> ratio = (dens1,dens2) => dens1/(1 + delta * (dens1 / (h2 * dens2) - 1));
            double X = ratio(dens60_1,dens60_2);

            // Step 24/12
            // Opbtain saturation density of both fluids at Trx
            double densX_1 = getSatDensityAtTemp(Trx, f1);
			double densX_2 = getSatDensityAtTemp(Trx, f2);

            // Step 24/13
            // Obtain CTL
            double CTL = ratio(densX_1,densX_2)/X;

            return CTL;
        }

        // API 11.2.2M Section 11.2.2.6M Basic Model
        public double GetCompressFactorLiqGas(double tempF,double api60,double pressPsig,double vaporPressPsig,out double A,out double B)
        {
			// API 11.2.2M - Section 11.2.8.1M Step 10
			// Check near critical temperature
			double tr = Conversions.DegFtoDegR(tempF);
			double tc = getCriticalTemperature(api60);
			if (tr > 0.960 * tc)
				throw (new ArgumentException(String.Format("Temperature {0} degR must be less than or equal to near (96%) of Critical Temperature {1} degR", tr, tc)));

            double A1 = -2.1465891e-6;
            double A2 = 1.5774390e-5;
            double A3 = -1.0502139e-5;
            double A4 = 2.8324481e-7;
            double A5 = -0.95495939;
            double A6 = 7.2900662e-8;
            double A7 = -2.7769343e-7;
            double A8 = 0.03645838;
            double A9 = -0.05110158;
            double A10 = 0.00795529;
            double A11 = 9.13114910;
            double B1 = -6.0357667e-10;
            double B2 = 2.2112678e-6;
            double B3 = 0.00088384;
            double B4 = -0.00204016;
            double TR = Conversions.DegFtoDegR(tempF);
            double TR2 = TR*TR;
            double TR3 = TR2*TR;
            double G = Conversions.APItoSG(api60);
            double G2 = G*G;
            double G4 =G2*G2;
            double G6 = G2*G4;

            A = (A1 * TR2) + (A2 * TR2 * G2) + (A3 * TR2 * G4) + (A4 * TR3 * G6) + A5 + (A6 * TR3 * G2) + (A7 * TR3 * G4) + (A8 * TR * G2) + (A9 * TR * G) + (A10 * TR) + (A11 * G);
            // This is to convert the factor to Kilopascals for the case that dP will be in Kilopascals
            // A = A * 6.894757; // Smith meter TP06005 rev 0.2 (3/11) is wrong in assuming this is only if temp is degC vs Rankine.  There is no linear correlation!
            A = A * 100000;
            // A = (int)(A * 100000 + 0.5);  // Round up to nearest whole number if needed on a table
            B = (B1 * TR2) + (B2 * TR * G2) + (B3 * G) + (B4 * G2);
            B = B * 100000;
            // B = (int)(B * 100000000.0 + 0.5) * 0.001;  // Round up to nearest 0.001

			double dP = pressPsig - vaporPressPsig;    

            double F = 1/(A + (dP > 0 ? dP : 0)*B);

            return F;
        }

		// Section 11.1.3.3. Equation 15 and Section 11.1.1.6 Step 7
		// Pressure compensation CPL
        public double GetCPL(double compressFactor,double pressPsig)
		{
            double cpl = 1 / (1 - 0.00001 * compressFactor * pressPsig);
			return cpl;
		}

        // API 11.2.2M Section 11.2.2.6M Basic Model
        // Presure compensation CPL
		public double GetCPLLiquidGas(double compressFactor, double pressPsig,double vapPress)
		{
            double deltaP = pressPsig - vapPress;
            double cpl = 1 / (1 - compressFactor * (deltaP < 0 ? 0 :deltaP));
			return cpl;
		}

		// Roundup calculations
		// Section 11.1.5.4 Rounding of Values
		public double RoundUpAPI11_1(double val, string units)
        {
            // Validate
            UnitOfMeasure uom = null;
            if (!uoms.TryGetValue(units.ToLower(), out uom))
                throw (new ArgumentException("Units {0} not recognized or supported"));

            // Round up
            return RoundUpAPI11_1(val,uom.Precision);
        }

		public double RoundUpAPI11_1(double val, double precision)
        {
            var delta = precision;

            // Compute norm
            double invDelta = (1 / delta);
			double norm = invDelta*Math.Abs(val);

			// Store sign
			double sign = val < 0 ? -1.0 : 1.0;

			// Convert to integer
			double i = Math.Truncate(norm + 0.5);
			double inorm = Math.Truncate(norm);
			if ((norm - inorm).CompareTo(0.5) == 0)  // Fraction of norm is exactly 0.5
			{
				if (((long)inorm) % 2 == 0)  // Is even
					i = inorm;
			}

			// Rescale
			double x = sign * delta * i;

			return x;
		}

        public double RoundUp(double val, int pow10)
        {
            var delta = Math.Pow(10,pow10);

            // Compute norm
            var invDelta = 1 / delta;
            double norm = invDelta*Math.Abs(val);

            // Store sign
            double sign = val < 0 ? -1.0 : 1.0;

            // Round
            double i = Math.Round(norm);

            // Rescale
            double x = sign * delta * i;

            return x;
        }


        // API 11.1. - Section 11.1.3.5
        const int MAXITERATIONS = 50;
		public double GetDensity60FromDensity(COMMODITY_GROUP grp, double api, double tempF, out double CTPL, double presPsi=0, double vapPresPsi=0)
		{
            CTPL = 1.0;

			if (tempF==60.0 && presPsi==0.0)
				return api;

            // Guess desired initially set to the same as input density
            double api60 = api;
            double apiGuessed = api;
            // Iterate
            int i = 0;
            do
            {
                if(i>MAXITERATIONS)
                    throw(new OperationCanceledException("Maximum iterations {0} have been exceeded when trying to compute api60 from api"));

                api60 = api60 - 0.61803*(apiGuessed-api);  // Using golden ratio for simplicity

				// Get CTPL using guessed number
				CTPL = GetCTPLFromApiDegFPsig(grp, api60, tempF, presPsi, vapPresPsi);

                // Get guessed api
                apiGuessed = api60 / CTPL;

                i++;
            }
            while (CTPL*Math.Abs(api-apiGuessed) > 0.1*uoms["api"].Precision);  // A little better than precision

            // Check ranges
            checkRange(api60, "api", grp);


			// Return reached at api60
			return api60;
		}
		
        // API 11.1. - Equation (7)
        public double GetDensityFromDensity60(COMMODITY_GROUP grp,double api60, double tempF,double presPsi=0,double vapPresPsi=0)
        {
            if (tempF==60.0 && presPsi==0.0)
                return api60;

            // Get CTPL
            double CTPL = GetCTPLFromApiDegFPsig(grp, api60, tempF, presPsi, vapPresPsi);

            // Use to computed API - Remember API is inverse of density
            return api60/CTPL;
        }

        // API 12.1 - Tank Roof Corrections
        public double GetBarrelsDueToTankRoof(COMMODITY_GROUP grp, double api60, double tempF, double presPsi=0, double vapPresPsi=0, double roofWgtLb = 0, double bblPerApi = 0, double refApi=0)
        {
            // Get CTPL
            double CTPL = GetCTPLFromApiDegFPsig(grp, api60, tempF, presPsi, vapPresPsi);

            return GetBarrelsDueToTankRoof(api60,CTPL,roofWgtLb,bblPerApi,refApi);
        }

		// API 12.1 - Tank Roof Corrections
		public double GetBarrelsDueToTankRoof(double api60, double CTPL, double roofWgtLb = 0, double bblPerApi = 0, double refApi = 0)
		{
			double bblTankRoof = 0.0;

			// Two cases: Straps include Roof Weight and Straps do not include tanks
			if (bblPerApi > 0) // Straps include Roog Weight
			{
				// Get actual api at observed conditions
				double api = api60 / CTPL;
				bblTankRoof = (refApi - api) * bblPerApi;
			}
			else
			{
				// Get density
				double dens60 = Conversions.APItoKgm3(api60);

				// Get volume from roof in m3
				double roofWgtKg = Conversions.LbToKg(roofWgtLb);
				double roofVolM3 = roofWgtKg / (dens60 * CTPL);

				// Convert to barrels to be substracted
				bblTankRoof = -Conversions.M3toBBL(roofVolM3);
			}

			return bblTankRoof;
		}

        public double GetVapPressPsia(double api60,double tempF)
        {
            double sg60 = Conversions.APItoSG(api60);

            // Check temperature is in range
            if(sg60>=0.425 && sg60<=0.676)
            {
                if (!(tempF >= -50 && tempF <= 140))
					throw (new ArgumentException("Temperature is not in the range of Vapor Pressure estimation")); ;
            }
            else if(sg60>=0.350 && sg60<0.425)
            {
                if(!(tempF>=-50 && tempF<=(695.51*sg60-155.51)))
					throw (new ArgumentException("Temperature is not in the range of Vapor Pressure estimation")); ;
			}
			else
				throw (new ArgumentException("Relative Density is not in the range of of Vapor Pressure estimation"));

            VapPressCorrParams prms = GetVapPressCorrParams(sg60);
            double A = prms.A0 + sg60 * prms.A1 + sg60 * sg60 * prms.A2;
            double B = prms.B0 + sg60 * prms.B1 + sg60 * sg60 * prms.B2;
            double vapPress = Math.Exp(A + B / (tempF + 443.0));

            return vapPress;
        }


		#endregion

		#region Private methods

		// Section 11.1.6.1  CTPL (commonly known as VCF)
		double GetCTPLFromApiDegFPsigNONLiqGas(COMMODITY_GROUP grp, double api60, double tempF, double presPsig = 0)
		{
			// Step 1 - Check range for density,temperature and pressure
			checkRange(api60, "API", grp);
			checkRange(tempF, "degF");
			if (presPsig < 0)
				presPsig = 0;
			checkRange(presPsig, "psig");

			KCoeffs coeffs = GetKCoeffs(grp, api60);

			// Step 2 and 3  - Get corrected temp and density at ITP68 basis
			double tempITPS68 = Conversions.TempITS90toITPS68(tempF, "degF");
			double densITSP68 = Conversions.Api60ITS90tokgm3ITPS68(api60, coeffs); // kg/m3

			// Step 4 - Compute coefficient of thermal expansion
			double thermExpCoeff60 = GetThermExpCoeff60(densITSP68, coeffs);

			// Step 5 - Compute temperature correction factor
			double CTL = GetCTL(thermExpCoeff60, tempITPS68);

			double CPL = 1.0; // No compensations
							  // If not ATM pressure correct for pressure
			if (presPsig > 0)
			{
				// Step 6 - Compute compressibility factor
				double Fp = GetCompressFactor(densITSP68, tempITPS68);

				// Step 7 - Compute pressure correction
				CPL = GetCPL(Fp, presPsig);
			}

			// Step 8
			double CTPL = CTL * CPL;

			return RoundUpAPI11_1(CTPL, "CTPL");
		}


		// API 12.2.2 - Section 11  CTPL (commonly known as VCF)
		double GetCTPLFromAPIDegFPsigLiqGas(double api60, double tempF, double pressPsig, double vapPressPsig)
		{
			// Fix press to vapPress if higher
			if (pressPsig < vapPressPsig)
				pressPsig = vapPressPsig;

			// Determine CTL - Compute temperature correction factor
			double CTL = GetCTLLiqGas(tempF, api60);

			// Determine F - Compute compressibility factor
			double Fp = GetCompressFactorLiqGas(tempF, api60, pressPsig, vapPressPsig, out double A, out double B);

			// Determine CPL - Compute pressure correction
			double CPL = GetCPLLiquidGas(Fp, pressPsig, vapPressPsig);

			// Determine Combined coefficient
			double CTPL = CTL * CPL;

			return RoundUp(CTPL, -5);
		}



		VapPressCorrParams GetVapPressCorrParams(double sg60)
        {
            VapPressCorrParams ret = null;
            foreach(var vpCorrParm in vpCorrParms)
            {
                if(vpCorrParm.RelDensityRange.ValueIsWithin(sg60))
                {
					// Found it!
                    ret = vpCorrParm;
					break;
                }
            }
			if (ret == null)
				throw (new ArgumentException("Relative Density is not in the range of of Vapor Pressure estimation"));
            return ret;
        }

        COMMODITY_GROUP GetKCoeffsGroup(COMMODITY_GROUP grp,double api60)
        {
            if (grp != COMMODITY_GROUP.GENERALIZED_REFINED_PRODUCT)
                return grp;
            // Try to match density range
            var uom = uoms["api"];
            COMMODITY_GROUP ret = COMMODITY_GROUP.ANY;
            foreach(var limKv in uom.Limits)
            {
                if(((int)limKv.Key)>=100)  // Limit to refined product subgroups
                {
                    if(limKv.Value.ValueIsWithin(api60))
                    {
						// Found it!
						ret = limKv.Key;
						break;
					}
                }
            }

            if (ret == COMMODITY_GROUP.ANY)
                throw (new ArgumentException("Density is not in the range of any of the Generalized Refined Products"));
            return ret;
        }

		double getSatDensityAtTemp(double tempK, LiqGasProperties props)
        {
			double tau = 1 - tempK;
			double tau35 = Math.Pow(tau, 0.35);
			double tau2 = tau * tau;
			double tau3 = tau * tau2;
			double tau65 = Math.Pow(tau, 0.65);
            double dens = props.densCrit * (1 + (props.k1 * tau35 + props.k3 * tau2 + props.k4 * tau3) / (1 + props.k2 * tau65));
            return dens;
        }

        void loadUoMs()
        {
            uoms = RulesLoader.GetUoMs();
        }

		void loadKCoeffs()
		{
            kCoeffs = RulesLoader.GetKCoeffs();
		}

		void loadLiqGasProperties()
		{
            lgProps = RulesLoader.GetLiqGasProperties();
		}

        void loadVapCorrParams()
        {
            vpCorrParms = RulesLoader.GetVapPressCorrParams();
        }


        double getCriticalTemperature(double api60)
        {
            double tc = 621.418 - 822.686 * api60 + 1737.86 * api60 * api60;
            return tc;
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

            bool isLiquidGas = (grp == COMMODITY_GROUP.LPG_NGL);
            bool isGeneralizedRefinedProduct = ((int)grp >= 100);
            // Ignore commodity if limits are for ANY commodity
            if(!isLiquidGas && uom.Limits.ContainsKey(COMMODITY_GROUP.ANY))
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
            var type = grp;
            if (isGeneralizedRefinedProduct)
                type = COMMODITY_GROUP.GENERALIZED_REFINED_PRODUCT;   // Needed to allow density variations within refined products group
            if(!uom.Limits.TryGetValue(type,out limit))
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
