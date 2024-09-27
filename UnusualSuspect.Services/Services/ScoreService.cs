using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.Services.Services;

public sealed class ScoreService(IScoreRepository scoreRepository,
  IApplicationUserManager applicationUserManager,
  ISoftSettingService softSettingService,
  ICoinPackageUserRepository coinPackageUserRepository) : IScoreService
{
  public async Task<UnusualSuspectServiceResult<bool>> SetGameFinishedScoresAsync(
    int gameId, bool won, List<Participate> participates, CancellationToken cancellationToken = default)
  {
    SoftSetting softSetting = await softSettingService.GetSoftSettingAsync(false, cancellationToken);
    foreach (Participate participate in participates)
    {
      GameRole gameRole = (GameRole)participate.RoleCardId;

      int scoreChange = GetScoreByWinCondition(gameRole, won, softSetting);
      int coinChange = GetCoinByWinCondition(gameRole, won, softSetting);
      scoreRepository.Add(new Score()
      {
        Amount = scoreChange,
        GameId = gameId,
        TimeAdded = DateTime.Now,
        ScoreTypeId = (short)GetScoreTypeEnumByWinCondition(gameRole, won),
        UserId = participate.UserId
      });
      coinPackageUserRepository.Add(new CoinPackageUser()
      {
        UserId = participate.UserId,
        Amount = coinChange,
        CoinPackageId = (short)BaseCoinPackageEnum.GameAward,
        TimeAdded = DateTime.Now
      });
      ApplicationUser? user = await applicationUserManager.FindByIdAsync(participate.UserId.ToString());
      if (user != null)
      {
        user.CalculatedScore += scoreChange;
        user.CalculatedCoins += coinChange;
      }
    }

    return new UnusualSuspectServiceResult<bool>(true);
  }

  private ScoreTypeEnum GetScoreTypeEnumByWinCondition(GameRole gameRole, bool won)
  {
    switch (gameRole)
    {
      case GameRole.Detective:
        return won ? ScoreTypeEnum.GameWonAsDetective : ScoreTypeEnum.GameLostAsDetective;
      case GameRole.MainDetective:
        return won ? ScoreTypeEnum.GameWonAsMainDetective : ScoreTypeEnum.GameLostAsMainDetective;
      case GameRole.Witness:
        return won ? ScoreTypeEnum.GameWonAsWitness : ScoreTypeEnum.GameLostAsWitness;
      case GameRole.Accomplice:
        return won ? ScoreTypeEnum.GameLostAsAccomplice : ScoreTypeEnum.GameWonAsAccomplice;
      default:
        throw new ArgumentOutOfRangeException(nameof(gameRole), gameRole, null);
    }
  }

  private static int GetScoreByWinCondition(GameRole gameRole, bool won, SoftSetting softSetting)
  {
    switch (gameRole)
    {
      case GameRole.Detective:
      case GameRole.MainDetective:
        return won ? softSetting.DetectiveWinScore : softSetting.DetectiveLooseScore;
      case GameRole.Witness:
        return won ? softSetting.WitnessWinScore : softSetting.WitnessLooseScore;
      case GameRole.Accomplice:
        return won ? softSetting.AccompliceLooseScore: softSetting.AccompliceWinScore;
      default:
        throw new ArgumentOutOfRangeException(nameof(gameRole), gameRole, null);
    }
  }
  private static int GetCoinByWinCondition(GameRole gameRole, bool won, SoftSetting softSetting)
  {
    switch (gameRole)
    {
      case GameRole.Detective:
      case GameRole.MainDetective:
        return won ? softSetting.DetectiveWinCoin : softSetting.DetectiveLooseCoin;
      case GameRole.Witness:
        return won ? softSetting.WitnessWinCoin : softSetting.WitnessLooseCoin;
      case GameRole.Accomplice:
        return won ? softSetting.AccompliceLooseCoin : softSetting.AccompliceWinCoin;
      default:
        throw new ArgumentOutOfRangeException(nameof(gameRole), gameRole, null);
    }
  }

}