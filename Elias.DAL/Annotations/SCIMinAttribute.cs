using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.DAL.Annotations
{
    /// <summary>
    /// Attribute used to set range minimum value for properties
    /// </summary>
    public class SCIMinAttribute : SCIRangeAttribute
    {
        public SCIMinAttribute(int minimum)
            : base(minimum, int.MaxValue)
        {

        }

        public SCIMinAttribute(double minimum)
            : base(minimum, double.MaxValue)
        {

        }

        public override string GetErrorMessage(string name)
        {
            return !string.IsNullOrWhiteSpace(this.ErrorMessage)
                ? this.ErrorMessage
                : $"The value of '{name}' field must be at least {this.Minimum}.";
        }
    }
}
