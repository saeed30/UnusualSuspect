using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common;

namespace UnusualSuspect.Services;

public class UnusualSuspectErrorResult : ErrorResult<LogicErrorCode>
{
	public UnusualSuspectErrorResult() { }
	public UnusualSuspectErrorResult(LogicErrorCode error)
	{
		this.Type = error;
	}
	public override string ToString()
	{
		return ((int)Type).ToString();
	}
}

public class UnusualSuspectErrorResult<TError> : UnusualSuspectErrorResult
{
	public UnusualSuspectErrorResult(LogicErrorCode error, TError value)
	{
		this.Type = error;
		Value = value;
	}

	public TError Value { get; set; }
}
