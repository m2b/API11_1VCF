using System;
using System.Collections.Generic;

namespace APIVCF
{
    public enum OPERATION { NOP, ADD, SUBSTRACT, MULTIPLE, DIVIDE };

    public class MeasureOperationToken
    {
        public OPERATION Operation { get; set; }
        public TapeMeasureFtInFract Measure { get; set; }
    }

    public interface ITapeMeasureAPI
    {
        IList<Tuple<bool, string>> AreValid(string[] measures);
        IList<TapeMeasureFtInFract> GetObjects(string[] measures);
        IList<TapeMeasureFtInFract> GetObjectsFromValues(double[] values);
        IList<double> GetDecimalValues(string[] measures);
        IList<string> GetStringsFromValues(double[] values);
        TapeMeasureFtInFract Add(List<MeasureOperationToken> tokens);
        TapeMeasureFtInFract Average(List<TapeMeasureFtInFract> measures);
    }

    public class TapeMeasureAPI : ITapeMeasureAPI
    {
        public TapeMeasureAPI() {; }

        #region ITapeMeasureAPI

        public TapeMeasureFtInFract Average(List<TapeMeasureFtInFract> measures)
        {
            if (measures == null || measures.Count < 1)
                return new TapeMeasureFtInFract();
            TapeMeasureFtInFract sum = new TapeMeasureFtInFract();
            var count = measures.Count;
            foreach (var measure in measures)
            {
                sum += measure;
            }
            var avg = TapeMeasureFtInFract.GetDecimalValue(sum) / count;
            return TapeMeasureFtInFract.GetFromDecimal(avg);    
        }

        public TapeMeasureFtInFract Add(List<MeasureOperationToken> tokens)
        {
            if (tokens == null || tokens.Count < 1)
                return new TapeMeasureFtInFract();

            List<TapeMeasureFtInFract> addOperands = new List<TapeMeasureFtInFract>();
            List<TapeMeasureFtInFract> substractOperands = new List<TapeMeasureFtInFract>();

            foreach (var token in tokens)
            {
                switch (token.Operation)
                {
					case OPERATION.ADD:
                    case OPERATION.NOP:
                        addOperands.Add(token.Measure);
                        break;
                    case OPERATION.SUBSTRACT:
                        substractOperands.Add(token.Measure);
                        break;
                    default:
                        throw (new ArgumentException(String.Format("Operation {0} not supported", token.Operation.ToString())));
                }
            }

            // Add all addends
            TapeMeasureFtInFract adds = new TapeMeasureFtInFract();
            foreach (var meas in addOperands)
            {
                adds = adds + meas;
            }
            // Add all substractions
            TapeMeasureFtInFract subs = new TapeMeasureFtInFract();
            foreach (var meas in substractOperands)
            {
                subs = subs + meas;
            }

            // Combine
            TapeMeasureFtInFract ret = adds - subs;
            return ret;
        }

        public IList<Tuple<bool, string>> AreValid(string[] measures)
        {
            List<Tuple<bool, string>> ret = new List<Tuple<bool, string>>();
            if (measures == null || measures.Length < 1)
            {
                ret.Add(Tuple.Create<bool, string>(false, "Empty measures list passed"));
            }
            else
            {
                foreach (var meas in measures)
                {
                    try
                    {
                        var obj = new TapeMeasureFtInFract(meas);
                        ret.Add(Tuple.Create<bool, string>(true, "Valid"));
                    }
                    catch (ArgumentException ae)
                    {
                        ret.Add(Tuple.Create<bool, string>(false, String.Format(ae.Message)));
                    }
                }
            }
            return ret;
        }

        public IList<double> GetDecimalValues(string[] measures)
        {
            List<double> ret = new List<double>();
            if (measures != null)
            {
                foreach (var meas in measures)
                {
                    ret.Add(TapeMeasureFtInFract.GetDecimalValue(new TapeMeasureFtInFract(meas)));
                }
            }
            return ret;
        }

        public IList<TapeMeasureFtInFract> GetObjects(string[] measures)
        {
            List<TapeMeasureFtInFract> ret = new List<TapeMeasureFtInFract>();
            if (measures != null)
            {
                foreach (var meas in measures)
                {
                    var obj = new TapeMeasureFtInFract(meas);
                    ret.Add(obj);
                }
            }
            return ret;
        }

        public IList<TapeMeasureFtInFract> GetObjectsFromValues(double[] values)
        {
            List<TapeMeasureFtInFract> ret = new List<TapeMeasureFtInFract>();
            if (values != null)
            {
                foreach (var val in values)
                {
                    ret.Add(TapeMeasureFtInFract.GetFromDecimal(val));
                }
            }
            return ret;
        }

        public IList<string> GetStringsFromValues(double[] values)
        {
            List<string> ret = new List<string>();
            if (values != null)
            {
                foreach (var val in values)
                {
                    var meas = TapeMeasureFtInFract.GetFromDecimal(val);
                    ret.Add(meas.ToString());
                }
            }
            return ret;
        }

        #endregion
    }
}

