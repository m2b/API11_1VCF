using System;
using System.Collections.Generic;
using ITVizion.VizionDI.Definitions;


namespace APIVCF
{
    public class VolumeCorrectionGroup:Recordable<long>
    {
        public string Name { get; set; }
    }

    public class VCGroupValidator : IValidator<VolumeCorrectionGroup>
    {
        public bool IsValid(VolumeCorrectionGroup item, out string validatioMessage)
        {
            if (item == null)
            {
                validatioMessage = @"Null group passed";
                return false;
            }
            if(String.IsNullOrEmpty(item.Name))
            {
				validatioMessage = @"Group name cannot be null or empty";
                return false;
			}
            validatioMessage = @"OK";
            return true;
        }
    }

    public class TankStrap:Recordable<long>
    {
        public long TankID { get; set; }
        public double Level { get; set; }
        public double Volume { get; set; }
    }

	public class StrapValidator : IValidator<TankStrap>
	{
		public bool IsValid(TankStrap item, out string validatioMessage)
		{
			if (item == null)
			{
				validatioMessage = @"Null strap passed";
				return false;
			}
            if(item.TankID<=0)
            {
				validatioMessage = @"Tank id reference is missing";
				return false;                
            }
            if (item.Level<0)
			{
				validatioMessage = @"Strap level cannot be less than zero";
				return false;
			}
			if (item.Volume < 0)
			{
				validatioMessage = @"Strap volume cannot be less than zero";
				return false;
			}
			validatioMessage = @"OK";
			return true;
		}
	}

    public class Stock:Recordable<long>
    {
        public long VolCorrGroupId { get; set; }
        public string Abbreviation { get; set; }
        public string Name { get; set; }
    }

	public class StockValidator : IValidator<Stock>
	{
		public bool IsValid(Stock item, out string validatioMessage)
		{
			if (item == null)
			{
				validatioMessage = @"Null stock passed";
				return false;
			}
            if (item.VolCorrGroupId <= 0)
			{
				validatioMessage = @"VC Group id reference is missing";
				return false;
			}
            if (String.IsNullOrEmpty(item.Abbreviation))
			{
				validatioMessage = @"Stock abbreviation cannot be null or empty";
				return false;
			}
			if (String.IsNullOrEmpty(item.Name))
			{
				validatioMessage = @"Stock name cannot be null or empty";
				return false;
			}
			validatioMessage = @"OK";
			return true;
		}
	}

	public class ShellMaterial : Recordable<long>
    {
        public string Name { get; set; }
        public double ThermalExpansionCoeff { get; set; }
    }

    public class ShellMaterialValidator : IValidator<ShellMaterial>
	{
        public bool IsValid(ShellMaterial item, out string validatioMessage)
		{
			if (item == null)
			{
				validatioMessage = @"Null shell material passed";
				return false;
			}
			if (String.IsNullOrEmpty(item.Name))
			{
				validatioMessage = @"Stock name cannot be null or empty";
				return false;
			}
            if (item.ThermalExpansionCoeff<= 0)
			{
				validatioMessage = @"Thermal expansion coefficient must be greater than zero";
				return false;
			}
			validatioMessage = @"OK";
			return true;
		}
	}

	public class Tank:Recordable<long>
    {
        public long DefaultStockID { get; set; }
        public Stock DefaultStock { get; set; }  // To be hydrated on select
        public long ShellMaterialID { get; set; }
        public ShellMaterial ShellMaterial { get; set; } // To be hydrated on select
        public IList<TankStrap> Straps { get; set; } // To be hydrated on select
        public string Name { get; set; }
        bool _inService = true; // Default
        public bool InService 
        { 
            get
            {
                return _inService;
            }
            set
            {
                _inService = value;
            } 
        }
        public bool IsInsulated { get; set; }
        public bool HasFloatingRoof { get; set; }
        public double MinOperLevel { get; set; }
        public double MaxOperLevel { get; set; }
        public double MaxGaugeLevel { get; set; }
        public double RoofRestLevel { get; set; }
        public double RoofFloatLevel { get; set; }
        public double RoofWeight { get; set; }
        public double StrapCalibTemp { get; set; }
        public double StrapCalibDensity { get; set; }
        public double StrapFRAVolCorrectPerUnitDensity { get; set; }
    }

	public class TankValidator : IValidator<Tank>
	{
		public bool IsValid(Tank item, out string validatioMessage)
		{
			if (item == null)
			{
				validatioMessage = @"Null tank passed";
				return false;
			}
			if (String.IsNullOrEmpty(item.Name))
			{
				validatioMessage = @"Tank name cannot be null or empty";
				return false;
			}
            if (item.MinOperLevel <= 0)
			{
				validatioMessage = @"Min operating level must be greater than zero";
				return false;
			}
			if (item.MaxOperLevel <= 0)
			{
				validatioMessage = @"Max operating level must be greater than zero";
				return false;
			}
            if(item.MaxOperLevel<=item.MinOperLevel)
            {
				validatioMessage = @"Max operating level must be greater than Min operating level";
				return false;
			}
			if (item.MaxGaugeLevel <= 0)
			{
				validatioMessage = @"Max gaugelevel must be greater than zero";
				return false;
			}
			if (item.MaxGaugeLevel < item.MaxOperLevel)
			{
				validatioMessage = @"Max gauge level must be greater than or equal to Max operating level";
				return false;
			}
            validatioMessage = @"OK";
			return true;
		}
	}

    public interface ITankDataApi
    {
		// VCGroup Management
        IList<CRUDResult<VolumeCorrectionGroup>> AddOrUpdateVCGroups(List<VolumeCorrectionGroup> groups); 
        CRUDResult<VolumeCorrectionGroup> DeleteVCGroup(long id);
		IList<VolumeCorrectionGroup> GetVCGroups(string filter);

		// Strap Management
        IList<CRUDResult<TankStrap>> AddOrUpdateStraps(List<TankStrap> straps);
        CRUDResult<TankStrap> DeleteStrap(long id);
		IList<TankStrap> GetStraps(string filter);

		// Stock Management
        IList<CRUDResult<Stock>> AddOrUpdateStocks(List<Stock> stocks);
        CRUDResult<Stock> DeleteStock(long id);
		IList<Stock> GetStocks(string filter);

		// Tank Data Management
		IList<CRUDResult<Tank>> AddOrUpdateTanks(List<Tank> tanks);
        CRUDResult<Tank> DeleteTank(long id);
		IList<Tank> GetTanks(string filter);
    }

}
