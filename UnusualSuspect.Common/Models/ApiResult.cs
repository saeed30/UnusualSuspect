using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Extensions;

namespace UnusualSuspect.Common.Models;

public class ApiResultCommon : ApiResult
{
  public ApiResultCommon(bool isSuccess, ApiResultStatusCode statusCode, string? message = null)
     : base(isSuccess, statusCode, message ?? statusCode.ToDisplay())
  {
  }
  #region Implicit Operators
  public static implicit operator ApiResultCommon(OkResult result)
  {
    return new ApiResultCommon(true, ApiResultStatusCode.Success);
  }

  public static implicit operator ApiResultCommon(BadRequestResult result)
  {
    return new ApiResultCommon(false, ApiResultStatusCode.BadRequest);
  }

  public static implicit operator ApiResultCommon(BadRequestObjectResult result)
  {
    var message = result.Value?.ToString();
    if (result.Value is SerializableError errors)
    {
      var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
      message = string.Join(" | ", errorMessages);
    }
    return new ApiResultCommon(false, ApiResultStatusCode.BadRequest, message);
  }

  public static implicit operator ApiResultCommon(ContentResult result)
  {
    return new ApiResultCommon(true, ApiResultStatusCode.Success, result.Content);
  }

  public static implicit operator ApiResultCommon(NotFoundResult result)
  {
    return new ApiResultCommon(false, ApiResultStatusCode.NotFound);
  }
  #endregion
}

public class ApiResultCommon<TData> : ApiViewModels.ApiResult<TData>
    where TData : class
{
  public ApiResultCommon(bool isSuccess, ApiResultStatusCode statusCode, TData? data = null, string? message = null)
  : base(isSuccess, statusCode, data, message ?? statusCode.ToDisplay())
  {
  }

  #region Implicit Operators
  public static implicit operator ApiResultCommon<TData>(TData data)
  {
    return new ApiResultCommon<TData>(true, ApiResultStatusCode.Success, data);
  }

  public static implicit operator ApiResultCommon<TData>(OkResult result)
  {
    return new ApiResultCommon<TData>(true, ApiResultStatusCode.Success, null);
  }

  public static implicit operator ApiResultCommon<TData>(OkObjectResult result)
  {
    return new ApiResultCommon<TData>(true, ApiResultStatusCode.Success, (TData)result.Value);
  }

  public static implicit operator ApiResultCommon<TData>(BadRequestResult result)
  {
    return new ApiResultCommon<TData>(false, ApiResultStatusCode.BadRequest, null);
  }

  public static implicit operator ApiResultCommon<TData>(BadRequestObjectResult result)
  {
    var message = result.Value?.ToString();
    if (result.Value is SerializableError errors)
    {
      var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
      message = string.Join(" | ", errorMessages);
    }
    return new ApiResultCommon<TData>(false, ApiResultStatusCode.BadRequest, null, message);
  }

  public static implicit operator ApiResultCommon<TData>(ContentResult result)
  {
    return new ApiResultCommon<TData>(true, ApiResultStatusCode.Success, null, result.Content);
  }

  public static implicit operator ApiResultCommon<TData>(NotFoundResult result)
  {
    return new ApiResultCommon<TData>(false, ApiResultStatusCode.NotFound, null);
  }

  public static implicit operator ApiResultCommon<TData>(NotFoundObjectResult result)
  {
    return new ApiResultCommon<TData>(false, ApiResultStatusCode.NotFound, (TData?)result.Value);
  }
  #endregion
}
