using System;

namespace MeasureTapeUtils.WebApi.Model
{
    public class ValidateDto
    {
        public string Measure { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }
}
