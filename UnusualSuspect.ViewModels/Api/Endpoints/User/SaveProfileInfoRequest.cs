using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.ViewModels.Api.Endpoints.User;

public sealed class SaveProfileInfoRequest
{
	public string? NickName { get; set; }
	public IFormFile UserImage { get; set; }
}
