using UnusualSuspect.ApiViewModels.Enums;
using Newtonsoft.Json;
using System;

namespace UnusualSuspect.ApiViewModels
{
	[Serializable]
	public class ApiResult
	{
		public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, string? message = null)
		{
			IsSuccess = isSuccess;
			StatusCode = statusCode;
			Message = message;
		}
		public bool IsSuccess { get; set; }
		public ApiResultStatusCode StatusCode { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string? Message { get; set; }

	}
	[Serializable]
	public class ApiResult<TData> : ApiResult
		where TData : class
	{
		public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, TData data, string? message = null)
			: base(isSuccess, statusCode, message)
		{
			Data = data;
		}
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public TData Data { get; set; }

	}
}
