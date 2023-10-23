
namespace UnusualSuspect.ApiViewModels.Endpoints.User
{
	public sealed class SaveProfileInfoRequest<T>
	{
		public string? NickName { get; set; }
		public T UserImage { get; set; }
	}
}