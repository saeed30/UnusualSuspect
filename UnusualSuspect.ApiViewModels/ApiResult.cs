using UnusualSuspect.ApiViewModels.Enums;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace UnusualSuspect.ApiViewModels
{
	[Serializable]
	public class ApiResult
	{
		[SerializeField]
		private string? message;
		[SerializeField]
		private ApiResultStatusCode statusCode;
		[SerializeField]
		private bool isSuccess;

		public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, string? message = null)
		{
			IsSuccess = isSuccess;
			StatusCode = statusCode;
			Message = message;
		}
		public bool IsSuccess { get => isSuccess; set => isSuccess = value; }
		public ApiResultStatusCode StatusCode { get => statusCode; set => statusCode = value; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string? Message { get => message; set => message = value; }

	}
	[Serializable]
	public class ApiResult<TData> : ApiResult
		where TData : class
	{
		[SerializeField]
		private TData? data;

		public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, TData? data, string? message = null)
			: base(isSuccess, statusCode, message)
		{
			Data = data;
		}
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public TData? Data { get => data; set => data = value; }

	}
}
