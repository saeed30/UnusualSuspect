using UnusualSuspect.Common.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace UnusualSuspect.Services.Config;

public class DateTimeModelBinder : IModelBinder
{
    public static readonly Type[] SUPPORTED_TYPES = new Type[] { typeof(DateTime), typeof(DateTime?) };

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        if (!SUPPORTED_TYPES.Contains(bindingContext.ModelType))
        {
            return Task.CompletedTask;
        }

        var modelName = GetModelName(bindingContext);

        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var dateToParse = valueProviderResult.FirstValue;

        if (string.IsNullOrEmpty(dateToParse))
        {
            return Task.CompletedTask;
        }

        var dateTime = ParseDate(bindingContext, dateToParse);

        bindingContext.Result = ModelBindingResult.Success(dateTime);

        return Task.CompletedTask;
    }

    private DateTime? ParseDate(ModelBindingContext bindingContext, string dateToParse)
    {
        return ParseDateTime(dateToParse);
    }
    public DateTime? ParseDateTime(string faDate)
    {
        try
        {

            faDate = faDate.Fa2En().ToString();
            string[] YMD = faDate.Substring(0, 10).Split('/');
            string[] HMS = new string[3];
            if (faDate.Length > 11)
            {
                HMS = faDate.Substring(10, 8).Split(':');
                return new PersianDateTime(int.Parse(YMD[0]), int.Parse(YMD[1]), int.Parse(YMD[2]), int.Parse(HMS[0]), int.Parse(HMS[1]), int.Parse(HMS[2])).ToDateTime();
            }
            else
            {
                return new PersianDateTime(int.Parse(YMD[0]), int.Parse(YMD[1]), int.Parse(YMD[2])).ToDateTime();
            }
        }
        catch
        {
            return null;
        }
    }

    private string GetModelName(ModelBindingContext bindingContext)
    {
        // The "Name" property of the ModelBinder attribute can be used to specify the
        // route parameter name when the action parameter name is different from the route parameter name.
        // For instance, when the route is /api/{birthDate} and the action parameter name is "date".
        // We can add this attribute with a Name property [DateTimeModelBinder(Name ="birthDate")]
        // Now bindingContext.BinderModelName will be "birthDate" and bindingContext.ModelName will be "date"
        if (!string.IsNullOrEmpty(bindingContext.BinderModelName))
        {
            return bindingContext.BinderModelName;
        }

        return bindingContext.ModelName;
    }
}

public class DateTimeModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (DateTimeModelBinder.SUPPORTED_TYPES.Contains(context.Metadata.ModelType))
        {
            return new BinderTypeModelBinder(typeof(DateTimeModelBinder));
        }

        return null;
    }
}
