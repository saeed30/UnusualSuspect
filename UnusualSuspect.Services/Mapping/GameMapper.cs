using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels.Game;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Mapping;

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

  public static TurnOfPlayTalkingState? ToTurnOfPlayTalkingState(this Game value)
  {
    if (!value.OrderOfParticipationTurnToTalk.HasValue ||
        !value.OrderOfParticipationTalkBeginner.HasValue ||
        !value.TalkingTurnStartedTime.HasValue ||
        !value.CurrentUserTurnStartedTime.HasValue)
      return null;
    Dictionary<int, short> candidates = new Dictionary<int, short>();
    foreach (var item in value.GameCandidates)
    {
      if(!candidates.ContainsKey(item.UserId))
        continue;
      candidates.Add(item.UserId, item.CharacterCardId);
    }
    return new TurnOfPlayTalkingState()
    {
      OrderOfParticipationTurnToTalk = value.OrderOfParticipationTurnToTalk.Value,
      CurrentUserTurnStartedTime = value.CurrentUserTurnStartedTime.Value,
      OrderOfParticipationTalkBeginner = value.OrderOfParticipationTalkBeginner.Value,
      TalkingTurnStartedTime = value.TalkingTurnStartedTime.Value,
      CandidateCard = candidates
    };
  }
  public static TurnOfPlayGetResponse ToTurnOfPlayGetResponse(this Game value)
  {
    if (value.GameStatusId != (short)GameStatusEnum.Talking)
      return new TurnOfPlayGetResponse();
    return new TurnOfPlayGetResponse(value.ToTurnOfPlayTalkingState());
  }
  public static GameBaseDto ToGameBaseDto(this Game value)
  {
    return new GameBaseDto()
    {
      Id = value.Id,
      GameCharacterDtos = value.CharacterCardGames.ToGameCharacterDto(),
      GameParticipantDto = value.Participates.ToGameParticipantDto(),
      GameTypeDto = value.GameType.ToGameTypeDto(),
      QuestionGameDtos = value.QuestionGames.ToQuestionGameDto()
    };
  }
  public static GameFlowDto ToGameFlowDto(this Game value)
  {
    return new GameFlowDto()
    {
      Id = value.Id,
      ActiveCharacterIds = value.CharacterCardGames.Where(x => x.IsActive).Select(x => x.CharacterCardId).ToList()
    };
  }
}