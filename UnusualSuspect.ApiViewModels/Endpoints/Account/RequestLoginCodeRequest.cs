using System;
using UnusualSuspect.ApiViewModels.InputParameters;

namespace UnusualSuspect.ApiViewModels.Endpoints.Account
{
	[Serializable]
	public sealed class RequestLoginCodeRequest : ValueParameters<string>
	{
	}
}
