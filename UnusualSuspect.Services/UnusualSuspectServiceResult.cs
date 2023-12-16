using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common;

namespace UnusualSuspect.Services;

public class UnusualSuspectServiceResult<TResult> : ServiceResult<TResult, UnusualSuspectErrorResult, LogicErrorCode>
{
	public UnusualSuspectServiceResult(TResult result)
		: this(success: true, result: result, errors: null)
	{

	}
	public UnusualSuspectServiceResult(IEnumerable<UnusualSuspectErrorResult> errors)
		: this(success: false, result: default(TResult), errors: errors)
	{

	}


	public UnusualSuspectServiceResult(UnusualSuspectErrorResult error)
		: this(success: false, result: default(TResult), errors: new List<UnusualSuspectErrorResult>() { error })
	{

	}
	public UnusualSuspectServiceResult(bool success, TResult result, IEnumerable<UnusualSuspectErrorResult> errors)
		: base(success, result, errors)
	{

	}
}

public class UnusualSuspectServiceResult<TResult, TError> : ServiceResult<TResult, UnusualSuspectErrorResult<TError>, LogicErrorCode>
{
	public UnusualSuspectServiceResult(TResult result)
		: this(success: true, result: result, errors: null)
	{

	}
	public UnusualSuspectServiceResult(IEnumerable<UnusualSuspectErrorResult<TError>> errors)
		: this(success: false, result: default(TResult), errors: errors)
	{

	}
	public UnusualSuspectServiceResult(UnusualSuspectErrorResult<TError> error)
		: base(success: false, result: default(TResult), errors: new List<UnusualSuspectErrorResult<TError>>() { error })
	{

	}
	public UnusualSuspectServiceResult(bool success, TResult result, IEnumerable<UnusualSuspectErrorResult<TError>> errors)
		: base(success, result, errors)
	{

	}
}
