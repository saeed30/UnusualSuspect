
namespace UnusualSuspect.Common;

public abstract class ErrorResult<TErrorType>
{
  public TErrorType Type { get; set; }
}