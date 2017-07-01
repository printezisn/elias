using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Elias.DAL.Annotations
{
    /// <summary>
    /// Attribute used to set range limits for properties
    /// </summary>
    public class SCIRangeAttribute : RangeAttribute, IClientValidatable
    {
        public SCIRangeAttribute(int minimum, int maximum)
            : base(minimum, maximum)
        {

        }

        public SCIRangeAttribute(double minimum, double maximum)
            : base(minimum, maximum)
        {

        }

        public SCIRangeAttribute(Type type, string minimum, string maximum)
            : base(type, minimum, maximum)
        {

        }

        public virtual string GetErrorMessage(string name)
        {
            return !string.IsNullOrWhiteSpace(this.ErrorMessage)
                ? this.ErrorMessage
                : $"The value of '{name}' field must be from {this.Minimum} to {this.Maximum}.";
        }

        public override string FormatErrorMessage(string name)
        {
            return GetErrorMessage(name);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new ModelClientValidationRule[] { new ModelClientValidationRangeRule(GetErrorMessage(metadata.DisplayName ?? metadata.PropertyName), this.Minimum, this.Maximum) };
        }
    }
}