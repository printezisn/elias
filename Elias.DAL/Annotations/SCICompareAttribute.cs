using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.DAL.Annotations
{
    /// <summary>
    /// Attribute used to compare a property with another property in the model
    /// </summary>
    public class SCICompareAttribute : CompareAttribute
    {
        public SCICompareAttribute(string otherProperty)
            : base(otherProperty)
        {

        }

        public virtual string GetErrorMessage(string name)
        {
            return !string.IsNullOrWhiteSpace(this.ErrorMessage)
                ? this.ErrorMessage
                : $"The value of '{name}' field must be the same with the value of '{this.OtherProperty}' field.";
        }

        public override string FormatErrorMessage(string name)
        {
            return GetErrorMessage(name);
        }
    }
}
