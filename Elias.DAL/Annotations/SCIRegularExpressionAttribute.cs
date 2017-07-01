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
    /// Attribute used to validate a property based on a regular expression
    /// </summary>
    public class SCIRegularExpressionAttribute : RegularExpressionAttribute, IClientValidatable
    {
        public SCIRegularExpressionAttribute(string pattern)
            : base(pattern)
        {

        }

        public virtual string GetErrorMessage(string name)
        {
            return !string.IsNullOrWhiteSpace(this.ErrorMessage)
                ? this.ErrorMessage
                : $"The '{name}' field is not in valid format.";
        }

        public override string FormatErrorMessage(string name)
        {
            return GetErrorMessage(name);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new ModelClientValidationRule[] { new ModelClientValidationRegexRule(GetErrorMessage(metadata.DisplayName ?? metadata.PropertyName), this.Pattern) };
        }
    }
}