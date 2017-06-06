using System;
using System.Collections.Generic;

namespace APIVCF
{
    public class RulesLoader
    {
		public static Dictionary<string, UnitOfMeasure> GetUoMs()
		{
            Dictionary<string, UnitOfMeasure> uoms = new Dictionary<string, UnitOfMeasure>();

			// Density
            var densKgM3Limit1 = new ValueLimit() { Group = COMMODITY_GROUP.CRUDE_OIL, Min = 610.6, Max = 1163.5 ,MaxCompare=COMPARE.INSIDE};
            var densKgM3Limit2 = new ValueLimit() { Group = COMMODITY_GROUP.FUEL_OILS, Min = 838.3127, Max = 1163.5 };
            var densKgM3Limit3 = new ValueLimit() { Group = COMMODITY_GROUP.JET_FUELS, Min = 787.5195, Max = 838.3127,MaxCompare=COMPARE.INSIDE };  
            var densKgM3Limit4 = new ValueLimit() { Group = COMMODITY_GROUP.TRANSITION_ZONE, Min = 770.3520, Max = 787.5195,MaxCompare=COMPARE.INSIDE };
            var densKgM3Limit5 = new ValueLimit() { Group = COMMODITY_GROUP.GASOLINES, Min = 610.6, Max = 770.3520 , MaxCompare=COMPARE.INSIDE};
            var densKgM3Limit6 = new ValueLimit() { Group = COMMODITY_GROUP.LUBRICATING_OIL, Min = 800.9, Max = 1163.5, MaxCompare = COMPARE.INSIDE };
			var densKgM3Limit7 = new ValueLimit() { Group = COMMODITY_GROUP.GENERALIZED_REFINED_PRODUCT, Min = 610.6, Max = 1163.5 };
            var densSGLimit8 = new ValueLimit() { Group = COMMODITY_GROUP.LPG_NGL, Min = 0.3500, Max = 0.6880 };
            var densKgM3Limit8 = new ValueLimit() { Group = COMMODITY_GROUP.LPG_NGL, Min = Conversions.SGtoKgm3(densSGLimit8.Min),Max = Conversions.SGtoKgm3(densSGLimit8.Max) };
            var densKgM3Limits = new Dictionary<COMMODITY_GROUP, ValueLimit>();
            densKgM3Limits.Add(densKgM3Limit1.Group, densKgM3Limit1);
			densKgM3Limits.Add(densKgM3Limit2.Group, densKgM3Limit2);
			densKgM3Limits.Add(densKgM3Limit3.Group, densKgM3Limit3);
			densKgM3Limits.Add(densKgM3Limit4.Group, densKgM3Limit4);
			densKgM3Limits.Add(densKgM3Limit5.Group, densKgM3Limit5);
			densKgM3Limits.Add(densKgM3Limit6.Group, densKgM3Limit6);
			densKgM3Limits.Add(densKgM3Limit7.Group, densKgM3Limit7);
			densKgM3Limits.Add(densKgM3Limit8.Group, densKgM3Limit8);
			UnitOfMeasure den1 = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.DENSITY, Name = "kg/m3", Precision = 0.1 ,Limits=densKgM3Limits};
            // Since API is the inverse of density... limits are swaped Max=Min and Min=Max... Min is now to be compared for <= as well
            var densAPILimit1 = new ValueLimit() { Group = COMMODITY_GROUP.CRUDE_OIL, Min = Conversions.Kgm3toAPI(densKgM3Limit1.Max), Max = Conversions.Kgm3toAPI(densKgM3Limit1.Min), MinCompare = COMPARE.INSIDE };
			var densAPILimit2 = new ValueLimit() { Group = COMMODITY_GROUP.FUEL_OILS, Min = Conversions.Kgm3toAPI(densKgM3Limit2.Max), Max = Conversions.Kgm3toAPI(densKgM3Limit2.Min) };
			var densAPILimit3 = new ValueLimit() { Group = COMMODITY_GROUP.JET_FUELS, Min = Conversions.Kgm3toAPI(densKgM3Limit3.Max), Max = Conversions.Kgm3toAPI(densKgM3Limit3.Min), MinCompare = COMPARE.INSIDE };
			var densAPILimit4 = new ValueLimit() { Group = COMMODITY_GROUP.TRANSITION_ZONE, Min = Conversions.Kgm3toAPI(densKgM3Limit4.Max), Max = Conversions.Kgm3toAPI(densKgM3Limit4.Min), MinCompare = COMPARE.INSIDE };
			var densAPILimit5 = new ValueLimit() { Group = COMMODITY_GROUP.GASOLINES, Min = Conversions.Kgm3toAPI(densKgM3Limit5.Max), Max = Conversions.Kgm3toAPI(densKgM3Limit5.Min), MinCompare = COMPARE.INSIDE };
			var densAPILimit6 = new ValueLimit() { Group = COMMODITY_GROUP.LUBRICATING_OIL, Min = Conversions.Kgm3toAPI(densKgM3Limit6.Max), Max = Conversions.Kgm3toAPI(densKgM3Limit6.Min), MinCompare = COMPARE.INSIDE };
            var densAPILimit7 = new ValueLimit() { Group = COMMODITY_GROUP.GENERALIZED_REFINED_PRODUCT,Min = Conversions.Kgm3toAPI(densKgM3Limit7.Max), Max = Conversions.Kgm3toAPI(densKgM3Limit7.Min)};
            var densAPILimit8 = new ValueLimit() { Group = COMMODITY_GROUP.LPG_NGL, Min = Conversions.Kgm3toAPI(densKgM3Limit8.Max), Max = Conversions.Kgm3toAPI(densKgM3Limit8.Min) };
			var densAPILimits = new Dictionary<COMMODITY_GROUP, ValueLimit>();
			densAPILimits.Add(densAPILimit1.Group, densAPILimit1);
			densAPILimits.Add(densAPILimit2.Group, densAPILimit2);
			densAPILimits.Add(densAPILimit3.Group, densAPILimit3);
			densAPILimits.Add(densAPILimit4.Group, densAPILimit4);
			densAPILimits.Add(densAPILimit5.Group, densAPILimit5);
			densAPILimits.Add(densAPILimit6.Group, densAPILimit6);
			densAPILimits.Add(densAPILimit7.Group, densAPILimit7);
            densAPILimits.Add(densAPILimit8.Group, densAPILimit8);
			UnitOfMeasure den2 = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.DENSITY, Name = "API", Precision = 0.1,Limits=densAPILimits };
            // Swap again limits since it is based on API
            var densSGLimit1 = new ValueLimit() { Group = COMMODITY_GROUP.CRUDE_OIL, Min = Conversions.APItoSG(densAPILimit1.Max), Max = Conversions.APItoSG(densAPILimit1.Min), MaxCompare = COMPARE.INSIDE };
			var densSGLimit2 = new ValueLimit() { Group = COMMODITY_GROUP.FUEL_OILS, Min = Conversions.APItoSG(densAPILimit2.Max), Max = Conversions.APItoSG(densAPILimit2.Min) };
			var densSGLimit3 = new ValueLimit() { Group = COMMODITY_GROUP.JET_FUELS, Min = Conversions.APItoSG(densAPILimit3.Max), Max = Conversions.APItoSG(densAPILimit3.Min), MaxCompare = COMPARE.INSIDE };
			var densSGLimit4 = new ValueLimit() { Group = COMMODITY_GROUP.TRANSITION_ZONE, Min = Conversions.APItoSG(densAPILimit4.Max), Max = Conversions.APItoSG(densAPILimit4.Min), MaxCompare = COMPARE.INSIDE };
			var densSGLimit5 = new ValueLimit() { Group = COMMODITY_GROUP.GASOLINES, Min = Conversions.APItoSG(densAPILimit5.Max), Max = Conversions.APItoSG(densAPILimit5.Min), MaxCompare = COMPARE.INSIDE };
			var densSGLimit6 = new ValueLimit() { Group = COMMODITY_GROUP.LUBRICATING_OIL, Min = Conversions.APItoSG(densAPILimit6.Max), Max = Conversions.APItoSG(densAPILimit6.Min), MaxCompare = COMPARE.INSIDE };
            var densSGLimit7 = new ValueLimit() { Group = COMMODITY_GROUP.GENERALIZED_REFINED_PRODUCT, Min = Conversions.APItoSG(densAPILimit7.Max), Max = Conversions.APItoSG(densAPILimit7.Min)};
			var densSGLimits = new Dictionary<COMMODITY_GROUP, ValueLimit>();
			densSGLimits.Add(densSGLimit1.Group, densSGLimit1);
			densSGLimits.Add(densSGLimit2.Group, densSGLimit2);
			densSGLimits.Add(densSGLimit3.Group, densSGLimit3);
			densSGLimits.Add(densSGLimit4.Group, densSGLimit4);
			densSGLimits.Add(densSGLimit5.Group, densSGLimit5);
			densSGLimits.Add(densSGLimit6.Group, densSGLimit6);
			densSGLimits.Add(densSGLimit7.Group, densSGLimit7);
			densSGLimits.Add(densSGLimit8.Group, densSGLimit8);
			UnitOfMeasure den3 = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.DENSITY, Name = "Relative Density", Precision = 0.001,Limits=densSGLimits };
			uoms.Add(den1.Name.ToLower(), den1);
			uoms.Add(den2.Name.ToLower(), den2);
			uoms.Add(den3.Name.ToLower(), den3);


			// Temperature
			var tempFLimit = new ValueLimit() { Group = COMMODITY_GROUP.ANY, Min = -58.0, Max = 302.0 };
            var tempFLimit2 = new ValueLimit() { Group = COMMODITY_GROUP.LPG_NGL, Min = Conversions.DegKtoDegF(227.15), Max = Conversions.DegKtoDegF(366.15) };
			var tempFLimits = new Dictionary<COMMODITY_GROUP, ValueLimit>();
            tempFLimits.Add(tempFLimit.Group, tempFLimit);
            tempFLimits.Add(tempFLimit2.Group, tempFLimit2);
            UnitOfMeasure temp1 = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.TEMPERATURE, Name = "degF", Precision = 0.1,Limits=tempFLimits };
            var tempCLimit = new ValueLimit() { Group = COMMODITY_GROUP.ANY, Min = Conversions.DegFtoDegC(tempFLimit.Min), Max = Conversions.DegFtoDegC(tempFLimit.Max) };
            var tempCLimit2 = new ValueLimit() { Group = COMMODITY_GROUP.LPG_NGL, Min = Conversions.DegFtoDegC(tempFLimit2.Min), Max = Conversions.DegFtoDegC(tempFLimit2.Max) };
			var tempCLimits = new Dictionary<COMMODITY_GROUP, ValueLimit>();
            tempCLimits.Add(tempCLimit.Group, tempCLimit);
			tempCLimits.Add(tempCLimit2.Group, tempCLimit2);
            UnitOfMeasure temp2 = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.TEMPERATURE, Name = "degC", Precision = 0.05,Limits=tempCLimits };
			uoms.Add(temp1.Name.ToLower(), temp1);
			uoms.Add(temp2.Name.ToLower(), temp2);

			// Pressure 
			var pressPsigLimit = new ValueLimit() { Group = COMMODITY_GROUP.ANY, Min = 0, Max = 1500 };
			var pressPsigLimits = new Dictionary<COMMODITY_GROUP, ValueLimit>();
			pressPsigLimits.Add(pressPsigLimit.Group, pressPsigLimit);
            UnitOfMeasure press1 = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.PRESSURE, Name = "psig", Precision = 1,Limits=pressPsigLimits };
            var presskPaLimit = new ValueLimit() { Group = COMMODITY_GROUP.ANY, Min = Conversions.PSItokPa(pressPsigLimit.Min), Max = Conversions.PSItokPa(pressPsigLimit.Max) };
			var presskPaLimits = new Dictionary<COMMODITY_GROUP, ValueLimit>();
			presskPaLimits.Add(presskPaLimit.Group, presskPaLimit);
            UnitOfMeasure press2 = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.PRESSURE, Name = "kPa", Precision = 5,Limits=presskPaLimits };
			var pressBarLimit = new ValueLimit() { Group = COMMODITY_GROUP.ANY, Min = Conversions.PSItoBar(pressPsigLimit.Min), Max = Conversions.PSItoBar(pressPsigLimit.Max) };
			var pressBarLimits = new Dictionary<COMMODITY_GROUP, ValueLimit>();
			pressBarLimits.Add(pressBarLimit.Group, pressBarLimit); 
            UnitOfMeasure press3 = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.PRESSURE, Name = "bar", Precision = 0.05,Limits=pressBarLimits };
			uoms.Add(press1.Name.ToLower(), press1);
			uoms.Add(press2.Name.ToLower(), press2);
			uoms.Add(press3.Name.ToLower(), press3);

			// Thermal Expansion Coefficient (alpha60)
			var expFLimit = new ValueLimit() { Group = COMMODITY_GROUP.ANY, Min = 230.0e-6, Max = 930.0e-6 };
			var expFLimits = new Dictionary<COMMODITY_GROUP, ValueLimit>();
			expFLimits.Add(expFLimit.Group, expFLimit);
            UnitOfMeasure exp1 = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.THERMAL_EXPANSION_COEFF, Name = "1/degF", Precision = 0.0000001,Limits=expFLimits };
            var expCLimit = new ValueLimit() { Group = COMMODITY_GROUP.ANY, Min = Conversions.CoeffThermExpFtoC(expFLimit.Min), Max =Conversions.CoeffThermExpFtoC(expFLimit.Max)};
			var expCLimits = new Dictionary<COMMODITY_GROUP, ValueLimit>();
			expCLimits.Add(expCLimit.Group, expCLimit);			
            UnitOfMeasure exp2 = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.THERMAL_EXPANSION_COEFF, Name = "1/degC", Precision = 0.0000002,Limits=expCLimits };
			uoms.Add(exp1.Name.ToLower(), exp1);
			uoms.Add(exp2.Name.ToLower(), exp2);

			// CTL
			UnitOfMeasure ctl = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.CTL, Name = "CTL", Precision = 0.00001 };
			uoms.Add(ctl.Name.ToLower(), ctl);

			// Scaled Compressibility Factor (Fp)
			UnitOfMeasure comp1 = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.SCALED_COMPRESSIBILITY_FACTOR, Name = "1/psi", Precision = 0.001 };
			UnitOfMeasure comp2 = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.SCALED_COMPRESSIBILITY_FACTOR, Name = "1/kPa", Precision = 0.0001 };
			UnitOfMeasure comp3 = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.SCALED_COMPRESSIBILITY_FACTOR, Name = "1/bar", Precision = 0.01 };
			uoms.Add(comp1.Name.ToLower(), comp1);
			uoms.Add(comp2.Name.ToLower(), comp2);
			uoms.Add(comp3.Name.ToLower(), comp3);

			// CPL
			UnitOfMeasure cpl = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.CPL, Name = "CPL", Precision = 0.00001 };
			uoms.Add(cpl.Name.ToLower(), cpl);

			// CTPL
			UnitOfMeasure ctpl = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.CTPL, Name = "CTPL", Precision = 0.00001 };
			uoms.Add(ctpl.Name.ToLower(), ctpl);

            return uoms;
		}

        public static Dictionary<COMMODITY_GROUP,KCoeffs> GetKCoeffs()
        {
            Dictionary<COMMODITY_GROUP, KCoeffs> kCoeffs = new Dictionary<COMMODITY_GROUP, KCoeffs>();

            // Crude Oil
            KCoeffs crude = new KCoeffs() { CommodityGroup = COMMODITY_GROUP.CRUDE_OIL, k0 = 341.0957, k1 = 0.0, k2 = 0.0 };
            kCoeffs.Add(crude.CommodityGroup, crude);
            // Fuel Oils
            KCoeffs fuel = new KCoeffs() { CommodityGroup = COMMODITY_GROUP.FUEL_OILS, k0 = 103.872, k1 = 0.2701, k2 = 0.0 };
			kCoeffs.Add(fuel.CommodityGroup, fuel);
			// Jet Fuels
            KCoeffs jet = new KCoeffs() { CommodityGroup = COMMODITY_GROUP.JET_FUELS, k0 = 330.301, k1 = 0.0, k2 = 0.0 };
			kCoeffs.Add(jet.CommodityGroup, jet);
			// Transition zone
            KCoeffs trans = new KCoeffs() { CommodityGroup = COMMODITY_GROUP.TRANSITION_ZONE, k0 = 1489.0670, k1 = 0.0, k2 = -0.00186840 };
			kCoeffs.Add(trans.CommodityGroup, trans);
			// Gasolines
            KCoeffs gasol = new KCoeffs() { CommodityGroup = COMMODITY_GROUP.GASOLINES, k0 = 192.4571, k1 = 0.2438, k2 = 0.0 };
			kCoeffs.Add(gasol.CommodityGroup, gasol);
			// Lubricating Oils
            KCoeffs lube = new KCoeffs() { CommodityGroup = COMMODITY_GROUP.LUBRICATING_OIL, k0 = 0.0, k1 = 0.34878, k2 = 0.0 };
            kCoeffs.Add(lube.CommodityGroup, lube);

            return kCoeffs;

		}

        public static Dictionary<LIQ_GAS_FLUID, LiqGasProperties> GetLiqGasProperties()
        {
            Dictionary<LIQ_GAS_FLUID, LiqGasProperties> props = new Dictionary<LIQ_GAS_FLUID, LiqGasProperties>();

            // EE(68/32)
            LiqGasProperties props1 = new LiqGasProperties()
            {
                Fluid = LIQ_GAS_FLUID.EE_68_32,
                relDens60 = 0.325022,
                tempCritK = 298.11,
                comprFactCrit = 0.27998,
                densCrit = 6.250,
                k1 = 2.54616855327,
                k2 = -0.058244177754,
                k3 = 0.803398090807,
                k4 = -0.745720314137
            };
            props.Add(props1.Fluid, props1);
            // Ethane
            LiqGasProperties props2 = new LiqGasProperties()
            {
                Fluid = LIQ_GAS_FLUID.ETHANE,
                relDens60 = 0.355994,
                tempCritK = 305.33,
                comprFactCrit = 0.28220,
                densCrit = 6.870,
                k1 = 1.89113042610,
                k2 = -0.370305782347,
                k3 = -0.544867288720,
                k4 = 0.337876634952
            };
            props.Add(props2.Fluid, props2);
            // EP(65/35)
            LiqGasProperties props3 = new LiqGasProperties()
            {
                Fluid = LIQ_GAS_FLUID.EP_65_35,
                relDens60 = 0.429277,
                tempCritK = 333.67,
                comprFactCrit = 0.28060,
                densCrit = 5.615,
                k1 = 2.20970078464,
                k2 = -0.294253708172,
                k3 = -0.405754420098,
                k4 = 0.319443433421
            };
            props.Add(props3.Fluid, props3);
            // EP(35/65)
            LiqGasProperties props4 = new LiqGasProperties()
            {
                Fluid = LIQ_GAS_FLUID.EP_35_65,
                relDens60 = 0.470381,
                tempCritK = 352.46,
                comprFactCrit = 0.27930,
                densCrit = 5.110,
                k1 = 2.25341981320,
                k2 = -0.266542138024,
                k3 = -0.372756711655,
                k4 = 0.384734185665
            };
            props.Add(props4.Fluid, props4);
            // Propane
            LiqGasProperties props5 = new LiqGasProperties()
            {
                Fluid = LIQ_GAS_FLUID.PROPANE,
                relDens60 = 0.507025,
                tempCritK = 369.78,
                comprFactCrit = 0.27626,
                densCrit = 5.000,
                k1 = 1.96568366933,
                k2 = -0.327662435541,
                k3 = -0.417979702538,
                k4 = 0.303271602831
            };
            props.Add(props5.Fluid, props5);
			// i-Butane
			LiqGasProperties props6 = new LiqGasProperties()
			{
                Fluid = LIQ_GAS_FLUID.iBUTANE,
				relDens60 = 0.562827,
				tempCritK = 407.85,
				comprFactCrit = 0.28326,
				densCrit = 3.860,
				k1 = 2.04748034410,
				k2 = -0.289734363425,
				k3 = -0.330345036434,
				k4 = 0.291757103132
			};
			props.Add(props6.Fluid, props6);
			// n-Butane
			LiqGasProperties props7 = new LiqGasProperties()
			{
                Fluid = LIQ_GAS_FLUID.nBUTANE,
				relDens60 = 0.584127,
				tempCritK = 425.16,
				comprFactCrit = 0.27536,
				densCrit = 3.920,
				k1 = 2.03734743118,
				k2 = -0.299059145695,
				k3 = -0.418883095671,
				k4 = 0.380367738748
			};
			props.Add(props7.Fluid, props7);
			// i-Pentane
			LiqGasProperties props8 = new LiqGasProperties()
			{
                Fluid = LIQ_GAS_FLUID.iPENTANE,
				relDens60 = 0.624285,
				tempCritK = 460.44,
				comprFactCrit = 0.27026,
				densCrit = 3.247,
				k1 = 2.06541640707,
				k2 = -0.238366208840,
				k3 = -0.161440492247,
				k4 = 0.258681568613
			};
			props.Add(props8.Fluid, props8);
			// n-Pentane
			LiqGasProperties props9 = new LiqGasProperties()
			{
                Fluid = LIQ_GAS_FLUID.nPENTANE,
				relDens60 = 0.631054,
				tempCritK = 469.65,
				comprFactCrit = 0.27235,
				densCrit = 3.200,
				k1 = 2.11263474494,
				k2 = -0.261269413560,
				k3 = -0.291923445075,
				k4 = 0.308344290017
			};
			props.Add(props9.Fluid, props9);
			// i-Hexane
			LiqGasProperties props10 = new LiqGasProperties()
			{
                Fluid = LIQ_GAS_FLUID.iHEXANE,
				relDens60 = 0.657167,
				tempCritK = 498.05,
				comprFactCrit = 0.26706,
				densCrit = 2.727,
				k1 = 2.02382197871,
				k2 = -0.423550090067,
				k3 = -1.152810982570,
				k4 = 0.950139001678
			};
			props.Add(props10.Fluid, props10);
			// n-Hexane
			LiqGasProperties props11 = new LiqGasProperties()
			{
                Fluid = LIQ_GAS_FLUID.nHEXANE,
				relDens60 = 0.664064,
				tempCritK = 507.35,
				comprFactCrit = 0.26762,
				densCrit = 2.704,
				k1 = 2.17134547773,
				k2 = -0.232997313405,
				k3 = -0.267019794036,
				k4 = 0.378629524102
			};
			props.Add(props11.Fluid, props11);
			// n-Heptane
			LiqGasProperties props12 = new LiqGasProperties()
			{
                Fluid = LIQ_GAS_FLUID.nHEPTANE,
				relDens60 = 0.688039,
				tempCritK = 540.15,
				comprFactCrit = 0.26312,
				densCrit = 2.315,
				k1 = 2.19773533433,
				k2 = -0.275056764147,
				k3 = -0.447144095029,
				k4 = 0.493770995799
			};
			props.Add(props12.Fluid, props12);

            return props;
        }

        public static List<VapPressCorrParams> GetVapPressCorrParams()
        {
            List<VapPressCorrParams> ret = new List<VapPressCorrParams>();

            VapPressCorrParams params1 = new VapPressCorrParams()
            {
                RelDensityRange = new ValueLimit() { Min=0.350,Max=0.450,MaxCompare=COMPARE.INSIDE},
                A0 = 17.5297,
                A1 = -24.604,
                A2=24.373,
                B0=-6567.6,
                B1=19322.0,
                B2=-24693.3
            };
            ret.Add(params1);

			VapPressCorrParams params2 = new VapPressCorrParams()
			{
				RelDensityRange = new ValueLimit() { Min = 0.450, Max = 0.490, MaxCompare = COMPARE.INSIDE },
				A0 = 7.9907,
				A1 = 7.562,
				A2 = 0.0,
				B0 = 1895.5,
				B1 = -10596.9,
				B2 = 0.0
			};
			ret.Add(params2);

			VapPressCorrParams params3 = new VapPressCorrParams()
			{
				RelDensityRange = new ValueLimit() { Min = 0.490, Max = 0.510, MaxCompare = COMPARE.INSIDE },
				A0 = -6.4747,
				A1 = 37.083,
				A2 = 0.0,
				B0 = 12038.0,
				B1 = -31296.5,
				B2 = 0.0
			};
			ret.Add(params3);

			VapPressCorrParams params4 = new VapPressCorrParams()
			{
				RelDensityRange = new ValueLimit() { Min = 0.510, Max = 0.560, MaxCompare = COMPARE.INSIDE },
				A0 = 11.5454,
				A1 = 1.749,
				A2 = 0.0,
				B0 = 1378.8,
				B1 = -10396.1,
				B2 = 0.0
			};
			ret.Add(params4);

			VapPressCorrParams params5 = new VapPressCorrParams()
			{
				RelDensityRange = new ValueLimit() { Min = 0.560, Max = 0.585, MaxCompare = COMPARE.INSIDE },
				A0 = 6.4827,
				A1 = 10.790,
				A2 = 0.0,
				B0 = 3721.5,
				B1 = -14579.5,
				B2 = 0.0
			};
			ret.Add(params5);

			VapPressCorrParams params6 = new VapPressCorrParams()
			{
				RelDensityRange = new ValueLimit() { Min = 0.585, Max = 0.625, MaxCompare = COMPARE.INSIDE },
				A0 = 6.5412,
				A1 = 10.690,
				A2 = 0.0,
				B0 = 6514.5,
				B1 = -19353.9,
				B2 = 0.0
			};
			ret.Add(params6);

			VapPressCorrParams params7 = new VapPressCorrParams()
			{
				RelDensityRange = new ValueLimit() { Min = 0.625, Max = 0.676 },
				A0 = 20.8537,
				A1 = -12.210,
				A2 = 0.0,
				B0 = -6765.6,
				B1 = 1894.3,
				B2 = 0.0
			};
			ret.Add(params7);

            return ret;
        }

    }
}
