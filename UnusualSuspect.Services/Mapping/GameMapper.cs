using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Game;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Mapping
{
	public static class GameMapper
	{
		public static GameGetResponse ToGameGetResponse(this Game value)
		{
			return new GameGetResponse(value.ToGameBaseDto(), value.ToGameFlowDto());
		}

		public static IEnumerable<GameGetResponse> ToGameGetResponse(this IEnumerable<Game> value)
		{
			return value.Select(x => x.ToGameGetResponse());
		}

		public static GameBaseDto ToGameBaseDto(this Game value)
		{
			return new GameBaseDto()
			{
				Id = value.Id,
				GameCharacterDtos = value.CharacterCardGames.ToList().ToGameCharacterDto(),
				GameParticipantDto = value.Participates.ToList().ToGameParticipantDto(),
				GameTypeDto = value.GameType.ToGameTypeDto()
			};
		}
		public static GameFlowDto ToGameFlowDto(this Game value)
		{
			return new GameFlowDto()
			{
				Id = value.Id,
				ActiveCharacterIds = value.CharacterCardGames.Where(x=>x.RemovedTurn == null).Select(x=>x.Id).ToList()
			};
		}
	}
}
