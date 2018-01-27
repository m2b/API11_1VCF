using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using APIVCF;
using MeasureTapeUtils.WebApi.Model;

namespace MeasureTapeUtils.WebApi.Controllers
{
    public class TapeUtilsController : Controller
    {
		private readonly ILogger _logger;
        private readonly ITapeMeasureAPI _api;

        // Constructor
        public TapeUtilsController(ITapeMeasureAPI api,ILogger<TapeUtilsController> logger)
		{
            _api = api;
			_logger = logger;
		}


		// Post api/TapeUtils/validate
		[HttpPost]
		[Route("api/[controller]/validate")]
        public IList<ValidateDto> AreValid([FromBody] string[] measures)
		{
            var tuples = _api.AreValid(measures);
			// Marshall response
			List<ValidateDto> ret = new List<ValidateDto>();
			for (int i = 0;i< measures.Length;i++)
            {
                ValidateDto dto = new ValidateDto() { Measure = measures[i], IsValid = tuples[i].Item1, Message = tuples[i].Item2 };
                ret.Add(dto);
            }
            return ret;
		}


		// Post api/TapeUtils/objects
		[HttpPost]
		[Route("api/[controller]/objects")]
		public IList<TapeMeasureFtInFract> GetObjects([FromBody] string[] measures)
		{
            try
            {
                return _api.GetObjects(measures);
            }
            catch(Exception e)
            {
                _logger.LogError(0,e,"Error getting objects from measure strings");
                throw;
            }
		}

		// Post api/TapeUtils/objectsfrmvals
		[HttpPost]
		[Route("api/[controller]/objectsfrmvals")]
		public IList<TapeMeasureFtInFract> GetObjectsFromValues([FromBody] double[] values)
		{
			try
			{
				return _api.GetObjectsFromValues(values);
			}
			catch (Exception e)
			{
				_logger.LogError(0, e, "Error getting objects from measure double values");
				throw;
			}
		}

		// Post api/TapeUtils/decimalvals
		[HttpPost]
		[Route("api/[controller]/decimalvals")]
		public IList<double> GetDecimalValues([FromBody] string[] measures)
		{
			try
			{
				return _api.GetDecimalValues(measures);
			}
			catch (Exception e)
			{
				_logger.LogError(0, e, "Error getting decimal values from measure strings");
				throw;
			}
		}

		// Post api/TapeUtils/stringsfrmvals
		[HttpPost]
		[Route("api/[controller]/stringsfrmvals")]
		public IList<String> GetStringFromValues([FromBody] double[] values)
		{
			try
			{
				return _api.GetStringsFromValues(values);
			}
			catch (Exception e)
			{
				_logger.LogError(0, e, "Error getting strings from measure double values");
				throw;
			}
		}

		// Post api/TapeUtils/add
		[HttpPost]
		[Route("api/[controller]/add")]
        public TapeMeasureFtInFract Add([FromBody] List<MeasureOperationToken> tokens)
		{
			try
			{
				return _api.Add(tokens);
			}
			catch (Exception e)
			{
				_logger.LogError(0, e, "Error adding tape measures");
				throw;
			}
		}

		// Post api/TapeUtils/average
		[HttpPost]
		[Route("api/[controller]/average")]
        public TapeMeasureFtInFract Average([FromBody] List<TapeMeasureFtInFract> objects)
		{
			try
			{
                return _api.Average(objects);
			}
			catch (Exception e)
			{
				_logger.LogError(0, e, "Error averaging tape measures");
				throw;
			}
		}
    }
}
