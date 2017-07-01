using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Elias.Web.Code
{
    public class SCIModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var result = base.BindModel(controllerContext, bindingContext);
            if (result != null)
            {
                // No need to continue if the binding is valid
                return result;
            }

            // Gets the attempted value
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueResult == null)
            {
                return result;
            }

            // Gets the model state for the specific model
            var modelState = bindingContext.ModelState[bindingContext.ModelName];
            if (modelState == null)
            {
                return result;
            }

            // Gets the errors, for the specific model, that contain an exception and replaces them with a validation error
            var errors = modelState.Errors.Where(w => w.Exception != null).ToList();
            if (!errors.Any())
            {
                return result;
            }

            foreach (var error in errors)
            {
                modelState.Errors.Remove(error);
            }

            var propertyName = !string.IsNullOrEmpty(bindingContext.ModelMetadata.DisplayName)
                ? bindingContext.ModelMetadata.DisplayName
                : bindingContext.ModelMetadata.PropertyName;

            // Enters the new validation error
            modelState.Errors.Add($"The value '{valueResult.AttemptedValue}' is not valid for the '{propertyName}' field.");

            return result;
        }
    }
}