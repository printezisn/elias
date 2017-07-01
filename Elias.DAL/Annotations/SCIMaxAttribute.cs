using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.DAL.Annotations
{
    /// <summary>
    /// Attribute used to set range maximum value for properties
    /// </summary>
    public class SCIMaxAttribute : SCIRangeAttribute
    {
        public SCIMaxAttribute(int maximum)
            : base(int.MinValue, maximum)
        {

        }

        public SCIMaxAttribute(double maximum)
            : base(double.MinValue, maximum)
        {

        }

        public override string GetErrorMessage(string name)
        {
            return !string.IsNullOrWhiteSpace(this.ErrorMessage)
                ? this.ErrorMessage
                : $"The value of '{name}' field must be maximum {this.Maximum}.";
        }
    }
}
