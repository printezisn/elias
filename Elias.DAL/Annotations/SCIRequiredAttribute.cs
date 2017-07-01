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
    /// Attribute used to indicate that a property is required
    /// </summary>
    public class SCIRequiredAttribute : RequiredAttribute, IClientValidatable
    {
        public SCIRequiredAttribute()
        {

        }

        public virtual string GetErrorMessage(string name)
        {
            return !string.IsNullOrWhiteSpace(this.ErrorMessage)
                ? this.ErrorMessage
                : $"The '{name}' field is required.";
        }

        public override string FormatErrorMessage(string name)
        {
            return GetErrorMessage(name);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new ModelClientValidationRule[] { new ModelClientValidationRequiredRule(GetErrorMessage(metadata.DisplayName ?? metadata.PropertyName)) };
        }
    }
}
