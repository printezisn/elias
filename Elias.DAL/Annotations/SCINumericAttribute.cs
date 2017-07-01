using Elias.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.DAL.Annotations
{
    public class SCNumericAttribute : SCIRegularExpressionAttribute
    {
        public SCNumericAttribute()
            : base(SCIRegexLibrary.NUMERIC)
        {

        }

        public override string GetErrorMessage(string name)
        {
            return !string.IsNullOrWhiteSpace(this.ErrorMessage)
                ? this.ErrorMessage
                : $"The '{name}' field must contain only digits.";
        }
    }
}
