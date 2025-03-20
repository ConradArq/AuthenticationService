using Microsoft.AspNetCore.Mvc.ModelBinding;
using AuthenticationService.Shared.Resources;

namespace AuthenticationService.API.ModelBinders
{
    /// <summary>
    /// A query string binder implemented to ensure that error messages are localized and consistent with 
    /// the application's standardized error handling format. 
    /// 
    /// Model binders run after authorization has completed and before action filters and attributes are executed, 
    /// ensuring that action method parameters are fully bound and ready for use during the subsequent pipeline stages.
    /// </summary>
    public class QueryStringBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var query = bindingContext.HttpContext.Request.Query;
            if (query.TryGetValue(bindingContext.FieldName, out var queryValue))
            {
                var value = queryValue.FirstOrDefault();
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        var converter = System.ComponentModel.TypeDescriptor.GetConverter(bindingContext.ModelType);
                        if (converter.CanConvertFrom(typeof(string)))
                        {
                            var convertedValue = converter.ConvertFromString(value);
                            bindingContext.Result = ModelBindingResult.Success(convertedValue);
                        }
                    }
                    catch
                    {
                        bindingContext.ModelState.AddModelError(bindingContext.FieldName, string.Format(ValidationMessages.InvalidQueryParameter, value, bindingContext.ModelType.Name));
                        bindingContext.Result = ModelBindingResult.Failed();
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
