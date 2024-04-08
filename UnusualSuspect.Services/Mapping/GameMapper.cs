using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels;
using UnusualSuspect.ApiViewModels.InnerModels.Game;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Mapping;

public static class GameMapper
{
  public static GameGetResponse ToGameGetResponse(this Game value, List<int> onlineUserIds, int timeToTalkInSeconds, int userId)
  {
    return new GameGetResponse(value.ToGameBaseDto(), value.ToGameFlowDto(timeToTalkInSeconds), onlineUserIds, value.GameStatusId, value.ToPrivateInfoDto(userId));
  }

  public static IEnumerable<GameGetResponse> ToGameGetResponse(this IEnumerable<Game> value, List<int> onlineUserIds, int timeToTalkInSeconds, int userId)
  {
    return value.Select(x => x.ToGameGetResponse(onlineUserIds, timeToTalkInSeconds, userId));
  }

  public static TurnOfPlayTalkingState? ToTurnOfPlayTalkingState(this Game value, int timeToTalkInSeconds)
  {
    if (!value.OrderOfParticipationTurnToTalk.HasValue ||
        !value.OrderOfParticipationTalkBeginner.HasValue ||
        !value.TalkingTurnStartedTime.HasValue ||
        !value.CurrentUserTurnStartedTime.HasValue)
      return null;
    return new TurnOfPlayTalkingState()
    {
      TimeToTalkInSeconds = timeToTalkInSeconds,
      OrderOfParticipationTurnToTalk = value.OrderOfParticipationTurnToTalk.Value,
      CurrentUserTurnStartedTime = value.CurrentUserTurnStartedTime.Value,
      OrderOfParticipationTalkBeginner = value.OrderOfParticipationTalkBeginner.Value,
      TalkingTurnStartedTime = value.TalkingTurnStartedTime.Value
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
    return new GameFlowDto()
    {
      Id = value.Id,
      ActiveCharacterIds = value.CharacterCardGames.Where(x => x.IsActive).Select(x => x.CharacterCardId).ToList(),
      TurnOfPlayTalkingState = value.GameStatusId == (short)GameStatusEnum.Talking ? value.ToTurnOfPlayTalkingState(timeToTalkInSeconds) : null,
      WitnessLastAnswer = (value.WitnessLastAnswer.HasValue ? (value.WitnessLastAnswer.Value ? WitnessAnswer.Yes : WitnessAnswer.No) : WitnessAnswer.NoAnswer),
      CandidateCard = candidates
    };
  }

  public static PrivateInfoDto ToPrivateInfoDto(this Game game, int? userId)
  {
    if (!userId.HasValue)
      return null;
    GameRole role = (GameRole)game.Participates.Single(x => x.UserId == userId).RoleCardId;
    return new PrivateInfoDto()
    {
      GameRole = role,
      MurdererId = role == GameRole.Accomplice || role == GameRole.Witness ? game.CharacterCardGames.Single(x => x.IsMurderer).Id : -1
    };
  }

  public static FinishedResponse ToFinishedResponse(this Game game)
  {
    bool won;
    if (game.GameStatusId == (short)GameStatusEnum.FinishedAndLostTheGame)
      won = false;
    else if (game.GameStatusId == (short)GameStatusEnum.FinishedAndWonTheGame)
      won = true;
    else
      throw new Exception("Invalid game status: " + game.GameStatusId);
    List<FinishedParticipantDto> parDtos = new List<FinishedParticipantDto>();
    foreach (Participate participate in game.Participates)
    {
      parDtos.Add(new FinishedParticipantDto()
      {
        GameRole = (GameRole)participate.RoleCardId,
        Coins = GetCoinByWinCondition((GameRole)participate.RoleCardId, won),
        Cups = GetCupByWinCondition((GameRole)participate.RoleCardId, won),
        UserDto = new UserDto()
        {
          AvatarId = participate.ApplicationUser.AvatarId,
          Id = participate.ApplicationUser.Id,
          NickName = participate.ApplicationUser.NickName,
          Username = participate.ApplicationUser.UserName
        }
      });
    }
    return new FinishedResponse()
    {
      SessionTimeSpan = game.FinishedTime.HasValue ? game.FinishedTime.Value.Subtract(game.CreateTime) : DateTime.Now.Subtract(game.CreateTime),
      MurdererId = game.CharacterCardGames.Single(x => x.IsMurderer && x.IsActive).CharacterCardId,
      FinishedParticipantDtos = parDtos
    };
  }

  private static int GetCupByWinCondition(GameRole gameRole, bool won)
  {
    switch (gameRole)
    {
      case GameRole.Detective:
      case GameRole.MainDetective:
        return won ? 1 : 0;
      case GameRole.Witness:
        return won ? 1 : 0;
      case GameRole.Accomplice:
        return won ? 0 : 1;
      default:
        throw new ArgumentOutOfRangeException(nameof(gameRole), gameRole, null);
    }
  }

  private static int GetCoinByWinCondition(GameRole gameRole, bool won)
  {
    switch (gameRole)
    {
      case GameRole.Detective:
      case GameRole.MainDetective:
        return won ? 10 : 1;
      case GameRole.Witness:
        return won ? 10 : 2;
      case GameRole.Accomplice:
        return won ? 1 : 10;
      default:
        throw new ArgumentOutOfRangeException(nameof(gameRole), gameRole, null);
    }
  }
}