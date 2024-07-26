using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.Common.Attribute;

namespace UnusualSuspect.Api.Endpoints;

[LocalRequestOnly]
public static class MyBaseEndpointLocal
{
  #region inhert from MyBaseEndpointAuthenticated
  public static class WithRequest<TRequest>
  {
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class WithResult<TResponse> : MyBaseEndpointAuthenticated
          .WithRequest<TRequest>
          .WithResult<TResponse>
    {
    }
    //[ApiExplorerSettings(IgnoreApi = true)]
    //public abstract class WithoutResult : MyBaseEndpointAuthenticated
    //  .WithRequest<TRequest>
    //  .WithoutResult
    //{
    //}

    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class WithActionResult<TResponse> : MyBaseEndpointAuthenticated
          .WithRequest<TRequest>
          .WithActionResult<TResponse>
    {
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class WithActionResult : MyBaseEndpointAuthenticated
          .WithRequest<TRequest>
          .WithActionResult
    {
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class WithAsyncEnumerableResult<T> : MyBaseEndpointAuthenticated
          .WithRequest<TRequest>
          .WithAsyncEnumerableResult<T>
    {
    }
  }

  public static class WithoutRequest
  {
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class WithResult<TResponse> : MyBaseEndpointAuthenticated
          .WithoutRequest
          .WithResult<TResponse>
    {
    }

    //[ApiExplorerSettings(IgnoreApi = true)]
    //public abstract class WithoutResult : MyBaseEndpointAuthenticated
    //  .WithoutRequest
    //  .WithoutResult
    //{
    //}

    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class WithActionResult<TResponse> : MyBaseEndpointAuthenticated
          .WithoutRequest
          .WithActionResult<TResponse>
    {
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class WithActionResult : MyBaseEndpointAuthenticated
          .WithoutRequest
          .WithActionResult
    {
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class WithAsyncEnumerableResult<T> : MyBaseEndpointAuthenticated
          .WithoutRequest
          .WithAsyncEnumerableResult<T>
    {
    }
  }
  #endregion inhert from MyBaseEndpointAuthenticated

}