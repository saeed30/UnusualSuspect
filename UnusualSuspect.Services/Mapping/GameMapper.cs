using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels.Game;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Mapping;

public static class GameMapper
{
  public static GameGetResponse ToGameGetResponse(this Game value, List<int> onlineUserIds, int timeToTalkInSeconds)
  {
    return new GameGetResponse(value.ToGameBaseDto(), value.ToGameFlowDto(timeToTalkInSeconds), onlineUserIds, value.GameStatusId);
  }

  public static IEnumerable<GameGetResponse> ToGameGetResponse(this IEnumerable<Game> value, List<int> onlineUserIds, int timeToTalkInSeconds)
  {
    return value.Select(x => x.ToGameGetResponse(onlineUserIds, timeToTalkInSeconds));
  }

  public static TurnOfPlayTalkingState? ToTurnOfPlayTalkingState(this Game value, int timeToTalkInSeconds)
  {
    if (!value.OrderOfParticipationTurnToTalk.HasValue ||
        !value.OrderOfParticipationTalkBeginner.HasValue ||
        !value.TalkingTurnStartedTime.HasValue ||
        !value.CurrentUserTurnStartedTime.HasValue)
      return null;
    List<CandidateCardDto> candidates = new List<CandidateCardDto>();
    foreach (var item in value.GameCandidates)
    {
      if (candidates.Any(x => x.UserId == item.UserId))
        continue;
      candidates.Add(new CandidateCardDto()
      {
        UserId = item.UserId,
        CharacterCardId = item.CharacterCardId
      });
    }
    return new TurnOfPlayTalkingState()
    {
      TimeToTalkInSeconds = timeToTalkInSeconds,
      OrderOfParticipationTurnToTalk = value.OrderOfParticipationTurnToTalk.Value,
      CurrentUserTurnStartedTime = value.CurrentUserTurnStartedTime.Value,
      OrderOfParticipationTalkBeginner = value.OrderOfParticipationTalkBeginner.Value,
      TalkingTurnStartedTime = value.TalkingTurnStartedTime.Value,
      CandidateCard = candidates
    };
  }
  public static TurnOfPlayGetResponse ToTurnOfPlayGetResponse(this Game value, int timeToTalkInSeconds)
  {
    if (value.GameStatusId != (short)GameStatusEnum.Talking)
      return new TurnOfPlayGetResponse();
    return new TurnOfPlayGetResponse(value.ToTurnOfPlayTalkingState(timeToTalkInSeconds));
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
  public static GameFlowDto ToGameFlowDto(this Game value, int timeToTalkInSeconds)
  {
    return new GameFlowDto()
    {
      Id = value.Id,
      ActiveCharacterIds = value.CharacterCardGames.Where(x => x.IsActive).Select(x => x.CharacterCardId).ToList(),
      TurnOfPlayTalkingState = value.GameStatusId == (short)GameStatusEnum.Talking ? value.ToTurnOfPlayTalkingState(timeToTalkInSeconds) : null,
      WitnessLastAnswer = (value.WitnessLastAnswer.HasValue ? (value.WitnessLastAnswer.Value ? WitnessAnswer.Yes : WitnessAnswer.No) : WitnessAnswer.NoAnswer)
    };
  }
}