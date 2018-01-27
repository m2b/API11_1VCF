using System;
using ITVizion.VizionDI.Definitions;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;

namespace APIVCF
{
    public sealed class RdbmsTankDataRepository<TConn> : ITankDataApi
    where TConn : DbConnection
    {
        #region Private members

        RdbmsRepository<long, TConn> _repos;

		#endregion

		#region Constructors and initialization methods

		public RdbmsTankDataRepository(string configDirectory, ILoggingDelegate loggingDelegate = null)
		{
			if (String.IsNullOrEmpty(configDirectory))
				throw (new ArgumentNullException(nameof(configDirectory)));

			_repos = new RdbmsRepository<long, TConn>(configDirectory);  // Leaving key generation to the database

            // Setup validators
            Dictionary<Type, IValidator<Recordable<long>>> validators = new Dictionary<Type, IValidator<Recordable<long>>>();
            // Shell Materials
            validators.Add(typeof(ShellMaterial), new ShellMaterialValidator() as IValidator<Recordable<long>>);
            // VCGroyps
            validators.Add(typeof(VolumeCorrectionGroup), new VCGroupValidator() as IValidator<Recordable<long>>);
			// Tanks
            validators.Add(typeof(Tank), new TankValidator() as IValidator<Recordable<long>>);
			// Straps
            validators.Add(typeof(TankStrap), new StrapValidator() as IValidator<Recordable<long>>);
            // Stocks
            validators.Add(typeof(Stock), new StockValidator() as IValidator<Recordable<long>>);
            _repos.Validators = validators;

			LoadShellMaterials();

            LoadVCGroups();
		}

        #endregion

        #region Private methods

        private void LoadShellMaterials()
        {
            ShellMaterial sm1 = new ShellMaterial()
            {
                Name = "mild carbon steel",
                ThermalExpansionCoeff = 6.20E-06
			};
			ShellMaterial sm2 = new ShellMaterial()
			{
				Name = "304 stainless steel",
				ThermalExpansionCoeff = 9.60E-06
			};
			ShellMaterial sm3 = new ShellMaterial()
			{
				Name = "316 stainless steel",
				ThermalExpansionCoeff = 8.83E-06
			};
			ShellMaterial sm4 = new ShellMaterial()
			{
				Name = "17 - 4HP stainless steel",
				ThermalExpansionCoeff = 6.00E-06
			};
            var crudResults = _repos.AddOrUpdate(new List<ShellMaterial>{sm1,sm2,sm3,sm4});
            foreach(var res in crudResults)
            {
                if (res.Exception != null)
                    throw (new Exception("Exception creating Shell Materials", res.Exception));
            }
		}

        private void LoadVCGroups()
        {
			List<VolumeCorrectionGroup> groups = new List<VolumeCorrectionGroup>();

            // Create groups from enum values in the API library
            var enumType = typeof(COMMODITY_GROUP);
            Array vals = Enum.GetValues(enumType);
			foreach (var val in vals)
			{
				VolumeCorrectionGroup vcg= new VolumeCorrectionGroup()
				{
                    Name = Enum.GetName(enumType,val),
				};
				groups.Add(vcg);
			}
            var crudResults = AddOrUpdateVCGroups(groups);
			foreach (var res in crudResults)
			{
				if (res.Exception != null)
					throw (new Exception("Exception creating Volume Correction Factor Commodity Groups", res.Exception));
			}
		}


        #endregion

        #region ITankDataApi implementation

        public IList<CRUDResult<Stock>> AddOrUpdateStocks(List<Stock> stocks)
        {
            return _repos.AddOrUpdate(stocks);
        }

        public IList<CRUDResult<TankStrap>> AddOrUpdateStraps(List<TankStrap> straps)
        {
            return _repos.AddOrUpdate(straps);
        }

        public IList<CRUDResult<Tank>> AddOrUpdateTanks(List<Tank> tanks)
        {
            return _repos.AddOrUpdate(tanks);
        }

        public IList<CRUDResult<VolumeCorrectionGroup>> AddOrUpdateVCGroups(List<VolumeCorrectionGroup> groups)
        {
			return _repos.AddOrUpdate(groups);
        }

        public CRUDResult<Stock> DeleteStock(long id)
        {
			if (id <= 0)
				throw (new ArgumentException("invalid id"));
			Stock item = new Stock()
			{
				ID = id
			};
			List<Stock> items = new List<Stock>();
			items.Add(item);
			return _repos.Delete(items).First();
        }

        public CRUDResult<TankStrap> DeleteStrap(long id)
        {
			if (id <= 0)
				throw (new ArgumentException("invalid id"));
			TankStrap item = new TankStrap()
			{
				ID = id
			};
            List<TankStrap> items = new List<TankStrap>();
			items.Add(item);
			return _repos.Delete(items).First();
        }

        public CRUDResult<Tank> DeleteTank(long id)
        {
			if (id <= 0)
				throw (new ArgumentException("invalid id"));
			Tank item = new Tank()
			{
				ID = id
			};
			List<Tank> items = new List<Tank>();
			items.Add(item);
			return _repos.Delete(items).First();        
        }

        public CRUDResult<VolumeCorrectionGroup> DeleteVCGroup(long id)
        {
            if (id <= 0)
                throw (new ArgumentException("invalid id"));
            VolumeCorrectionGroup item = new VolumeCorrectionGroup()
			{
				ID = id
			};
            List<VolumeCorrectionGroup> items = new List<VolumeCorrectionGroup>();
			items.Add(item);
			return _repos.Delete(items).First();
        }

        public IList<Stock> GetStocks(string filter)
        {
            return _repos.GetByFilter<Stock>(filter);
        }

        public IList<TankStrap> GetStraps(string filter)
        {
			return _repos.GetByFilter<TankStrap>(filter);
        }

        public IList<Tank> GetTanks(string filter)
        {
			return _repos.GetByFilter<Tank>(filter);
        }

        public IList<VolumeCorrectionGroup> GetVCGroups(string filter)
        {
            return _repos.GetByFilter<VolumeCorrectionGroup>(filter);
        }

        #endregion
    }
}
