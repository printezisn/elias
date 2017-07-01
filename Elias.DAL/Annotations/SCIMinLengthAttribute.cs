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
    /// Attribute used to indicate the minimum length of a string property
    /// </summary>
    public class SCIMinLengthAttribute : MinLengthAttribute, IClientValidatable
    {
        public SCIMinLengthAttribute(int length)
            : base(length)
        {

        }

        public virtual string GetErrorMessage(string name)
        {
            return !string.IsNullOrWhiteSpace(this.ErrorMessage)
                ? this.ErrorMessage
                : $"The '{name}' field must have at least {this.Length} characters.";
        }

        public override string FormatErrorMessage(string name)
        {
            return GetErrorMessage(name);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new ModelClientValidationRule[] { new ModelClientValidationMinLengthRule(GetErrorMessage(metadata.DisplayName ?? metadata.PropertyName), this.Length) };
        }
    }
}
