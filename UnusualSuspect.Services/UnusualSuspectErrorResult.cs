using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.Common;
using UnusualSuspect.Common.Enums;

namespace UnusualSuspect.Services;

public class UnusualSuspectErrorResult : ErrorResult<ErrorType>
{
	public UnusualSuspectErrorResult() { }
	public UnusualSuspectErrorResult(ErrorType error)
	{
		this.Type = error;
	}
}

public class UnusualSuspectErrorResult<TError> : UnusualSuspectErrorResult
{
	public UnusualSuspectErrorResult(ErrorType error, TError value)
	{
		this.Type = error;
		Value = value;
	}

	public TError Value { get; set; }
}
