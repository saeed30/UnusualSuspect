using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Game;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Services.Mapping
{
	public static class ApplicationUserMapper
	{
		public static GameUserDto ToGameUserDto(this ApplicationUser value)
		{
			return new GameUserDto()
			{
				Id = value.Id,
				NickName = value.NickName,
				Username = value.UserName
			};
		}
		public static IEnumerable<GameUserDto> ToGameUserDto(this IEnumerable<ApplicationUser> value)
		{
			return value.Select(x => x.ToGameUserDto());
		}

	}
}
