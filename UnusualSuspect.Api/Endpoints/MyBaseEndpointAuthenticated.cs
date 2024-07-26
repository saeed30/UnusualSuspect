using Ardalis.ApiEndpoints;
using UnusualSuspect.ViewModels.Identity;
using System.Security.Claims;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.Services;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;

namespace UnusualSuspect.Api.Endpoints;

public static class MyBaseEndpointAuthenticated
{
	#region inhert from EndpointBaseAsync
	public static class WithRequest<TRequest>
	{
		public abstract class WithResult<TResponse> : EndpointBaseAsync
			.WithRequest<TRequest>
			.WithResult<TResponse>
		{
			public CurrentUserViewModel CurrentUser => new CurrentUserViewModel(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToInt(), User.Identity.Name);
      protected ApiResultCommon<T> ReturnResult<T>(UnusualSuspectServiceResult<T> result) where T : class
      {
        if (!result.Success)
          return new ApiResultCommon<T>(false, ApiResultStatusCode.LogicError, null, result.MainError.ToString());
        return new ApiResultCommon<T>(true, ApiResultStatusCode.Success, result.Result);
      }
		}
  //  public abstract class WithoutResult : EndpointBaseAsync
		//	.WithRequest<TRequest>
		//	.WithoutResult
		//{
		//	public CurrentUserViewModel CurrentUser => new CurrentUserViewModel(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToInt(), User.Identity.Name);
		//}

		public abstract class WithActionResult<TResponse> : EndpointBaseAsync
			.WithRequest<TRequest>
			.WithActionResult<TResponse>
		{
      protected CurrentUserViewModel CurrentUser => new CurrentUserViewModel(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToInt(), User.Identity.Name);

      protected ApiResultCommon<T> ReturnResult<T>(UnusualSuspectServiceResult<T> result) where T : class
      {
        if (!result.Success)
          return new ApiResultCommon<T>(false, ApiResultStatusCode.LogicError, null, result.MainError.ToString());
        return new ApiResultCommon<T>(true, ApiResultStatusCode.Success, result.Result);
      }
    }

		public abstract class WithActionResult : EndpointBaseAsync
			.WithRequest<TRequest>
			.WithActionResult
		{
			public CurrentUserViewModel CurrentUser => new CurrentUserViewModel(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToInt(), User.Identity.Name);
		}

		public abstract class WithAsyncEnumerableResult<T> : EndpointBaseAsync
			.WithRequest<TRequest>
			.WithAsyncEnumerableResult<T>
		{
			public CurrentUserViewModel CurrentUser => new CurrentUserViewModel(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToInt(), User.Identity.Name);
		}
	}

	public static class WithoutRequest
	{
		public abstract class WithResult<TResponse> : EndpointBaseAsync
			.WithoutRequest
			.WithResult<TResponse>
		{
			public CurrentUserViewModel CurrentUser => new CurrentUserViewModel(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToInt(), User.Identity.Name);
      protected ApiResultCommon<T> ReturnResult<T>(UnusualSuspectServiceResult<T> result) where T : class
      {
        if (!result.Success)
          return new ApiResultCommon<T>(false, ApiResultStatusCode.LogicError, null, result.MainError.ToString());
        return new ApiResultCommon<T>(true, ApiResultStatusCode.Success, result.Result);
      }
		}

  //  public abstract class WithoutResult : EndpointBaseAsync
		//	.WithoutRequest
		//	.WithoutResult
		//{
		//	public CurrentUserViewModel CurrentUser => new CurrentUserViewModel(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToInt(), User.Identity.Name);
		//}

		public abstract class WithActionResult<TResponse> : EndpointBaseAsync
			.WithoutRequest
			.WithActionResult<TResponse>
		{
			public CurrentUserViewModel CurrentUser => new CurrentUserViewModel(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToInt(), User.Identity.Name);
      protected ApiResultCommon<T> ReturnResult<T>(UnusualSuspectServiceResult<T> result) where T : class
      {
        if (!result.Success)
          return new ApiResultCommon<T>(false, ApiResultStatusCode.LogicError, null, result.MainError.ToString());
        return new ApiResultCommon<T>(true, ApiResultStatusCode.Success, result.Result);
      }
		}

    public abstract class WithActionResult : EndpointBaseAsync
			.WithoutRequest
			.WithActionResult
		{
			public CurrentUserViewModel CurrentUser => new CurrentUserViewModel(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToInt(), User.Identity.Name);
		}

		public abstract class WithAsyncEnumerableResult<T> : EndpointBaseAsync
			.WithoutRequest
			.WithAsyncEnumerableResult<T>
		{
			public CurrentUserViewModel CurrentUser => new CurrentUserViewModel(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).ToInt(), User.Identity.Name);
		}
	}
	#endregion inhert from EndpointBaseAsync
}