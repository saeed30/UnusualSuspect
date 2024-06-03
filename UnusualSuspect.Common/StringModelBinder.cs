using UnusualSuspect.Common.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UnusualSuspect.Common;

public class StringModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Metadata.IsComplexType)
        {
            return null;
        }

        var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();
        var fallbackBinder = new SimpleTypeModelBinder(context.Metadata.ModelType,loggerFactory);
        if (context.Metadata.ModelType == typeof(string))
        {
            return new StringModelBinder(fallbackBinder);
        }
        return fallbackBinder;
    }
}
public class StringModelBinder : IModelBinder
{
    private readonly IModelBinder _fallbackBinder;
    public StringModelBinder(IModelBinder fallbackBinder)
    {
        if (fallbackBinder == null)
        {
            throw new ArgumentNullException(nameof(fallbackBinder));
        }
        _fallbackBinder = fallbackBinder;
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        if (valueProviderResult != ValueProviderResult.None)
        {
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            var valueAsString = valueProviderResult.FirstValue;
            if (string.IsNullOrWhiteSpace(valueAsString))
            {
                return _fallbackBinder.BindModelAsync(bindingContext);
            }

            var model = valueAsString.ToString().Replace((char)1610, (char)1740).Replace((char)1603, (char)1705);
            model = model.ToEnglishNumber();
            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }

        return _fallbackBinder.BindModelAsync(bindingContext);
    }
}
public static class StringModelBinderExtensions
{
    public static MvcOptions UseStringModelBinder(this MvcOptions options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        var simpleTypeModelBinder = options.ModelBinderProviders.FirstOrDefault(x => x.GetType() == typeof(SimpleTypeModelBinderProvider));
        if (simpleTypeModelBinder == null)
        {
            return options;
        }

        var simpleTypeModelBinderIndex = options.ModelBinderProviders.IndexOf(simpleTypeModelBinder);
        options.ModelBinderProviders.Insert(simpleTypeModelBinderIndex, new StringModelBinderProvider());
        return options;
    }
}
