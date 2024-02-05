namespace UnusualSuspect.Common;

public abstract class ServiceResult<TResult, TError, TErrorType> where TError : ErrorResult<TErrorType>
{
  public TResult Result { get; set; }
  public bool Success { get; set; }
  public IEnumerable<TError> Errors { get; set; }
  public TError MainError
  {
    get
    {
      return Errors?.FirstOrDefault();
    }
  }
  public ServiceResult(TResult result)
    : this(success: true, result: result, errors: null)
  {

  }
  public ServiceResult(TError error)
    : this(success: false, result: default(TResult), errors: new List<TError>() { error })
  {

  }
  public ServiceResult(IEnumerable<TError> errors)
    : this(success: false, result: default(TResult), errors: errors)
  {

  }
  public ServiceResult(bool success, TResult result, IEnumerable<TError> errors)
  {
    this.Success = success;
    this.Result = result;
    this.Errors = errors;
  }
}