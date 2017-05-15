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
			var densKgM3Limits = new Dictionary<COMMODITY_GROUP, ValueLimit>();
            densKgM3Limits.Add(densKgM3Limit1.Group, densKgM3Limit1);
			densKgM3Limits.Add(densKgM3Limit2.Group, densKgM3Limit2);
			densKgM3Limits.Add(densKgM3Limit3.Group, densKgM3Limit3);
			densKgM3Limits.Add(densKgM3Limit4.Group, densKgM3Limit4);
			densKgM3Limits.Add(densKgM3Limit5.Group, densKgM3Limit5);
			densKgM3Limits.Add(densKgM3Limit6.Group, densKgM3Limit6);
			densKgM3Limits.Add(densKgM3Limit7.Group, densKgM3Limit7);
			UnitOfMeasure den1 = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.DENSITY, Name = "kg/m3", Precision = 0.1 ,Limits=densKgM3Limits};
            // Since API is the inverse of density... limits are swaped Max=Min and Min=Max... Min is now to be compared for <= as well
            var densAPILimit1 = new ValueLimit() { Group = COMMODITY_GROUP.CRUDE_OIL, Min = Conversions.Kgm3toAPI(densKgM3Limit1.Max), Max = Conversions.Kgm3toAPI(densKgM3Limit1.Min), MinCompare = COMPARE.INSIDE };
			var densAPILimit2 = new ValueLimit() { Group = COMMODITY_GROUP.FUEL_OILS, Min = Conversions.Kgm3toAPI(densKgM3Limit2.Max), Max = Conversions.Kgm3toAPI(densKgM3Limit2.Min) };
			var densAPILimit3 = new ValueLimit() { Group = COMMODITY_GROUP.JET_FUELS, Min = Conversions.Kgm3toAPI(densKgM3Limit3.Max), Max = Conversions.Kgm3toAPI(densKgM3Limit3.Min), MinCompare = COMPARE.INSIDE };
			var densAPILimit4 = new ValueLimit() { Group = COMMODITY_GROUP.TRANSITION_ZONE, Min = Conversions.Kgm3toAPI(densKgM3Limit4.Max), Max = Conversions.Kgm3toAPI(densKgM3Limit4.Min), MinCompare = COMPARE.INSIDE };
			var densAPILimit5 = new ValueLimit() { Group = COMMODITY_GROUP.GASOLINES, Min = Conversions.Kgm3toAPI(densKgM3Limit5.Max), Max = Conversions.Kgm3toAPI(densKgM3Limit5.Min), MinCompare = COMPARE.INSIDE };
			var densAPILimit6 = new ValueLimit() { Group = COMMODITY_GROUP.LUBRICATING_OIL, Min = Conversions.Kgm3toAPI(densKgM3Limit6.Max), Max = Conversions.Kgm3toAPI(densKgM3Limit6.Min), MinCompare = COMPARE.INSIDE };
            var densAPILimit7 = new ValueLimit() { Group = COMMODITY_GROUP.GENERALIZED_REFINED_PRODUCT,Min = Conversions.Kgm3toAPI(densKgM3Limit7.Max), Max = Conversions.Kgm3toAPI(densKgM3Limit7.Min)};
			var densAPILimits = new Dictionary<COMMODITY_GROUP, ValueLimit>();
			densAPILimits.Add(densAPILimit1.Group, densAPILimit1);
			densAPILimits.Add(densAPILimit2.Group, densAPILimit2);
			densAPILimits.Add(densAPILimit3.Group, densAPILimit3);
			densAPILimits.Add(densAPILimit4.Group, densAPILimit4);
			densAPILimits.Add(densAPILimit5.Group, densAPILimit5);
			densAPILimits.Add(densAPILimit6.Group, densAPILimit6);
			densAPILimits.Add(densAPILimit7.Group, densAPILimit7);
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
            UnitOfMeasure den3 = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.DENSITY, Name = "Relative Density", Precision = 0.001,Limits=densSGLimits };
			uoms.Add(den1.Name.ToLower(), den1);
			uoms.Add(den2.Name.ToLower(), den2);
			uoms.Add(den3.Name.ToLower(), den3);

			// Temperature
			var tempFLimit = new ValueLimit() { Group = COMMODITY_GROUP.ANY, Min = -58.0, Max = 302.0 };
			var tempFLimits = new Dictionary<COMMODITY_GROUP, ValueLimit>();
            tempFLimits.Add(tempFLimit.Group, tempFLimit);
            UnitOfMeasure temp1 = new UnitOfMeasure() { VariableType = VARIABLE_TYPE.TEMPERATURE, Name = "degF", Precision = 0.1,Limits=tempFLimits };
            var tempCLimit = new ValueLimit() { Group = COMMODITY_GROUP.ANY, Min = Conversions.DegFToDegC(tempFLimit.Min), Max = Conversions.DegFToDegC(tempFLimit.Max) };
			var tempCLimits = new Dictionary<COMMODITY_GROUP, ValueLimit>();
            tempCLimits.Add(tempCLimit.Group, tempCLimit);
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
    }
}
