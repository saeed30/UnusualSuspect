using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Game;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Mapping
{
	public static class GameTypeMapper
	{
		public static GameTypeDto ToGameTypeDto(this GameType value)
		{
			return new GameTypeDto()
			{
				Id = value.Id,
				Name = value.Name,
				Title = value.Title,
				NumberOfPlayers = value.NumberOfPlayers
			};
		}
		public static IEnumerable<GameTypeDto> ToGameTypeDto(this IEnumerable<GameType> value)
		{
			return value.Select(x => x.ToGameTypeDto());
		}

	}
}
