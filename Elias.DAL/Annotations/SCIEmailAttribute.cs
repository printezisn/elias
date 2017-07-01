using Elias.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.DAL.Annotations
{
    /// <summary>
    /// Attribute used to indicate that a string property is used for email strings
    /// </summary>
    public class SCIEmailAttribute : SCIRegularExpressionAttribute
    {
        public SCIEmailAttribute()
            : base(SCIRegexLibrary.EMAIL)
        {

        }

        public override string GetErrorMessage(string name)
        {
            return !string.IsNullOrWhiteSpace(this.ErrorMessage)
                ? this.ErrorMessage
                : $"The '{name}' field is not in valid email format.";
        }
    }
}
