using Elias.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.DAL.Annotations
{
    /// <summary>
    /// Attribute used to indicate that a string property is used for phone strings
    /// </summary>
    public class SCIPhoneAttribute : SCIRegularExpressionAttribute
    {
        public SCIPhoneAttribute()
            : base(SCIRegexLibrary.PHONE)
        {

        }

        public override string GetErrorMessage(string name)
        {
            return !string.IsNullOrWhiteSpace(this.ErrorMessage)
                ? this.ErrorMessage
                : $"The '{name}' field is not in valid phone format.";
        }
    }
}
