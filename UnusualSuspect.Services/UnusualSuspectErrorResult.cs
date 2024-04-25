using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common;
using UnusualSuspect.Common.Extensions;

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
  public string GetDisplay()
  {
    return Type.ToDisplay();
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
