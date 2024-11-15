using Aspose.Cells;
using ElmahCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.AccessControl;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.DataLayer;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Mapping;
using UnusualSuspect.ViewModels.Dto;
using UnusualSuspect.ViewModels.PreGame;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Services.Services;

public sealed class PreGameService(IUnitOfWork uow,
    IPreGameGroupRepository preGameGroupRepository,
    IJoinedPreGameRepository joinedPreGameRepository,
    IApplicationUserManager applicationUserManager,
    IGameTypeRepository gameTypeRepository,
    IGameRepository gameRepository,
    IParticipateRepository participateRepository,
    ISoftSettingService softSetting,
    ICharacterCardRepository characterCardRepository,
    ICharacterCardGameRepository characterCardGameRepository,
    INotificationService notificationService,
    IQuestionRepository questionRepository,
    IQuestionGameRepository questionGameRepository,
    IMemoryCacheService memoryCacheService,
    ISoftSettingService softSettingService,
    ICoinUsedUserRepository coinUsedUserRepository,
    ICoinUsedService coinUsedService,
    IOptionsSnapshot<ProjectSetting> setting,
    ILogger<PreGameService> logger)
  : IPreGameService
{
  public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    return await uow.SaveChangesAsync(cancellationToken);
  }
  public async Task<UnusualSuspectServiceResult<PreGameGroup>> CreatePreGameGroup(int userId, short gameTypeId, CancellationToken cancellationToken = default)
  {
    GameType? gameType = await gameTypeRepository.GetByIdAsync(gameTypeId, cancellationToken);
    if (gameType == null)
      return LogicErrorCode.InvalidGameTypeId;
    if (await gameRepository.UserIsInActiveGameAsync(userId, cancellationToken))
      return LogicErrorCode.UserIsInActiveGame;
    var user = await applicationUserManager.FindByIdAsync(userId.ToString());
    if (user == null)
      return LogicErrorCode.InvalidUserId;
    //var oldPreGames = await joinedPreGameRepository.JoinedPreGameOfUserAsync(userId, cancellationToken);
    //if (oldPreGames.Any())
    //{
    //  JoinedPreGame? firstOwnGame = oldPreGames.FirstOrDefault(x => x.IsOwnerOfPreGroup && x.PreGameGroup.GameTypeId == gameTypeId);
    //  if (firstOwnGame != null)
    //  {
    //    var preGame = await preGameGroupRepository.GetByIdAsync(firstOwnGame.PreGameGroupId, cancellationToken);
    //    if (preGame == null)
    //      throw new Exception("wrong refrence to preGameGroup: " + firstOwnGame.PreGameGroupId);
    //    return new UnusualSuspectServiceResult<PreGameGroup>(preGame);
    //  }
    //  await RemoveFromAllUserPreGames(oldPreGames.ToList(), userId, cancellationToken);
    //}
    await UnreadyAllUserReadyPreGameGroups(userId, cancellationToken);
    var softSetting = await softSettingService.GetSoftSettingAsync(false, cancellationToken);
    Guid newGuid = Guid.NewGuid();
    if (softSetting.CoinCostToEnterPreGameForHost > 0 && gameType.AllowUserToAddOtherUsers)
    {
      var payResult = coinUsedService.PayIfHasEnough(softSetting.CoinCostToEnterPreGameForHost, user, PriceTypeEnum.PreGame, newGuid);
      if (!payResult.Success)
        return payResult.Errors.ToList();
      if (!payResult.Result)
        return LogicErrorCode.DoNotHaveEnoughToPay;
    }
    PreGameGroup group = new PreGameGroup()
    {
      CalculatedJoinedUsers = 1,
      CreatedTime = DateTime.Now,
      GameType = gameType,
      ReadyToGameTime = null,
      PreGameGroupStatusId = (int)PreGameGroupStatusEnum.NotReady
    };
    preGameGroupRepository.Add(group);
    JoinedPreGame joinedPreGame = new JoinedPreGame()
    {
      UserId = userId,
      IsOwnerOfPreGroup = true,
      JoinTime = DateTime.Now,
      PreGameGroup = group,
      ReadyToGameStatusId = (int)ReadyToGameStatusEnum.Ready,
      Guid = newGuid
    };
    joinedPreGameRepository.Add(joinedPreGame);
    return new UnusualSuspectServiceResult<PreGameGroup>(group);
  }

  public async Task RecalculatePreGameGroupUsers(int preGameGroupId, short changeOnThisTransaction = 0,
    CancellationToken cancellationToken = default)
  {
    PreGameGroup? preGame = await preGameGroupRepository.GetByIdAsync(preGameGroupId, cancellationToken);
    if (preGame == null)
      throw new Exception("preGameGroup not found. preGameGroupId: " + preGameGroupId);
    await RecalculatePreGameGroupUsers(preGame, changeOnThisTransaction, cancellationToken);
  }
  public async Task RecalculatePreGameGroupUsers(PreGameGroup preGameGroup, short changeOnThisTransaction = 0, CancellationToken cancellationToken = default)
  {
    int count = await joinedPreGameRepository.UserCountJoinedPreGameGroupAsync(preGameGroup.Id, cancellationToken);
    if (count > 32000)
      throw new Exception("invalid user count. preGameGroupId: " + preGameGroup.Id);
    preGameGroup.CalculatedJoinedUsers = (short)(count + changeOnThisTransaction);
  }

  public async Task<UnusualSuspectServiceResult<bool>> UnreadyAllUserReadyPreGameGroups(int userId, CancellationToken cancellationToken = default)
  {
    List<JoinedPreGame> groups = await joinedPreGameRepository.GetByUserIdAsync(userId, ReadyToGameStatusEnum.Ready, cancellationToken);
    if (!groups.Any())
      return new UnusualSuspectServiceResult<bool>(false);
    foreach (JoinedPreGame joinedPreGame in groups)
      await ChangeUserReadyStatusAsync(joinedPreGame, ReadyToGameStatusEnum.NotReady, cancellationToken);
    return new UnusualSuspectServiceResult<bool>(true);
  }

  public async Task<UnusualSuspectServiceResult<bool>> ChangeUserReadyStatusAsync(int userId, int preGameGroupId,
    ReadyToGameStatusEnum readyToGameStatusEnum, CancellationToken cancellationToken = default)
  {
    var preGameGroup = await preGameGroupRepository.GetByIdAsync(preGameGroupId, cancellationToken);
    if (preGameGroup == null)
      return LogicErrorCode.InvalidPreGameGroupId;
    return await ChangeUserReadyStatusAsync(userId, preGameGroup, readyToGameStatusEnum, cancellationToken);
  }

  public async Task<UnusualSuspectServiceResult<bool>> ChangeUserReadyStatusAsync(JoinedPreGame joinedPreGame,
    ReadyToGameStatusEnum readyToGameStatusEnum, CancellationToken cancellationToken = default)
  {
    var preGameGroup = await preGameGroupRepository.GetByIdAsync(joinedPreGame.PreGameGroupId, cancellationToken);
    if (preGameGroup == null)
      return LogicErrorCode.InvalidPreGameGroupId;
    return await ChangeUserReadyStatusAsync(joinedPreGame, preGameGroup, readyToGameStatusEnum, cancellationToken);
  }

  public async Task<UnusualSuspectServiceResult<bool>> ChangeUserReadyStatusAsync(int userId, PreGameGroup preGameGroup,
    ReadyToGameStatusEnum readyToGameStatusEnum, CancellationToken cancellationToken = default)
  {
    var joinedPreGame = await joinedPreGameRepository.GetByUserIdPreGameGroupIdAsync(userId, preGameGroup.Id, cancellationToken);
    if (joinedPreGame == null)
      return LogicErrorCode.UserJoinedPreGameGroupNotFound;
    return await ChangeUserReadyStatusAsync(joinedPreGame, preGameGroup, readyToGameStatusEnum, cancellationToken);
  }
  public async Task<UnusualSuspectServiceResult<bool>> ChangeUserReadyStatusAsync(JoinedPreGame joinedPreGame, PreGameGroup preGameGroup,
    ReadyToGameStatusEnum readyToGameStatusEnum, CancellationToken cancellationToken = default)
  {
    if (readyToGameStatusEnum != ReadyToGameStatusEnum.Notified)
    {
      if (readyToGameStatusEnum == ReadyToGameStatusEnum.Ready)
      {
        if (await gameRepository.UserIsInActiveGameAsync(joinedPreGame.UserId, cancellationToken))
          return LogicErrorCode.UserIsInActiveGame;
      }
      var softSetting = await softSettingService.GetSoftSettingAsync(false, cancellationToken);
      if (softSetting.CoinCostToEnterPreGame > 0)
      {
        ApplicationUser? user = applicationUserManager.FindById(joinedPreGame.UserId);
        if (user == null)
          return LogicErrorCode.InvalidUserId;
        if (readyToGameStatusEnum == ReadyToGameStatusEnum.Ready)
        {
          if (!coinUsedUserRepository.Search(new CoinUsedUserSearchFilterDto()
          {
            UserId = user.Id,
            ReferenceGuid = joinedPreGame.Guid,
            UsedForPriceType = PriceTypeEnum.PreGame
          }).Any())
          {
            var payResult = coinUsedService.PayIfHasEnough(
              joinedPreGame.IsOwnerOfPreGroup ? softSetting.CoinCostToEnterPreGameForHost : softSetting.CoinCostToEnterPreGame,
              user, PriceTypeEnum.PreGame, joinedPreGame.Guid);
            if (!payResult.Success)
              return payResult.Errors.ToList();
            if (!payResult.Result)
              return LogicErrorCode.DoNotHaveEnoughToPay;
          }
        }
        else if (readyToGameStatusEnum == ReadyToGameStatusEnum.NotReady)
        {
          await coinUsedService.DeletePaymentIfExistsAsync(user, joinedPreGame.Guid, PriceTypeEnum.PreGame, cancellationToken);
        }
      }
    }

    if (joinedPreGame.ReadyToGameStatusId != (short)ReadyToGameStatusEnum.Ready && preGameGroup.PreGameGroupStatusId == (short)PreGameGroupStatusEnum.Ready)
      preGameGroup.PreGameGroupStatusId = (short)PreGameGroupStatusEnum.NotReady;
    joinedPreGame.ReadyToGameStatusId = (short)readyToGameStatusEnum;
    return new UnusualSuspectServiceResult<bool>(true);
  }
  public async Task<UnusualSuspectServiceResult<bool>> StartPreGameGroup(int preGameGroupId)
  {
    if (!await joinedPreGameRepository.AllJoinedPreGameGroupUsersAreReadyAsync(preGameGroupId))
      return LogicErrorCode.ThereIsUnreadyUserInGroup;
    return new UnusualSuspectServiceResult<bool>(true);
  }
  public async Task<UnusualSuspectServiceResult<JoinedPreGame>> AddUserToPreGameGroup(int addingUserId, string username, int preGameGroupId, CancellationToken cancellationToken = default)
  {
    var user = await applicationUserManager.FindByNameAsync(username);
    if (user == null)
      return LogicErrorCode.InvalidUsername;
    if (user.Id == addingUserId)
      return LogicErrorCode.CanNotAddOwnToPreGameGroup;
    return await AddUserToPreGameGroup(addingUserId, user.Id, preGameGroupId, cancellationToken);
  }
  public async Task<UnusualSuspectServiceResult<JoinedPreGame>> AddUserToPreGameGroup(int addingUserId, int userId, int preGameGroupId, CancellationToken cancellationToken = default)
  {
    if (await gameRepository.UserIsInActiveGameAsync(userId, cancellationToken))
      return LogicErrorCode.UserIsInActiveGame;
    PreGameGroup? preGameGroup = await preGameGroupRepository.GetByIdWithGameTypeAsync(preGameGroupId, cancellationToken);
    if (preGameGroup == null)
      return LogicErrorCode.InvalidPreGameGroupId;
    if (preGameGroup.CalculatedJoinedUsers >= preGameGroup.GameType.NumberOfPlayers)
    {
      if (preGameGroup.CalculatedJoinedUsers > preGameGroup.GameType.NumberOfPlayers)
      {
        //saeed do some action to fix
        logger.LogEvent(SystemEventType.CalculatedJoinedUsersMoreThanTypeNumberOfPlayers, preGameGroupId,
          $"calculatedJoinedUsers ({preGameGroup.CalculatedJoinedUsers}) - userId ({userId})",
          logLevel: LogLevel.Critical);
      }
      return LogicErrorCode.PreGameGroupIsFull;
    }
    bool ownsTheGroup = await CheckUserOwnsThePreGameGroup(addingUserId, preGameGroupId, cancellationToken);
    if (!ownsTheGroup)
      return LogicErrorCode.UserDoNotOwnTheGroup;
    bool alreadyJoinedPreGameGroup = await CheckUserJoinedPreGameGroup(userId, preGameGroupId, cancellationToken);
    if (alreadyJoinedPreGameGroup)
      return LogicErrorCode.UserAlreadyJoinedPreGameGroup;
    var user = await applicationUserManager.FindByIdAsync(userId.ToString());
    if (user == null)
      return LogicErrorCode.InvalidUserId;

    //List<JoinedPreGame> preGameGroupsOwned = (await joinedPreGameRepository.GetAllOwnedByUserId(userId, cancellationToken)).ToList();
    //if (preGameGroupsOwned.Any())
    //  return LogicErrorCode.UserOwnsAnotherPregameGroup;

    Guid newGuid = Guid.NewGuid();
    JoinedPreGame joinedPreGame = new JoinedPreGame()
    {
      UserId = userId,
      IsOwnerOfPreGroup = false,
      JoinTime = DateTime.Now,
      PreGameGroup = preGameGroup,
      ReadyToGameStatusId = (int)ReadyToGameStatusEnum.Notified,
      Guid = newGuid
    };
    joinedPreGameRepository.Add(joinedPreGame);
    await RecalculatePreGameGroupUsers(preGameGroupId, 1, cancellationToken);
    await notificationService.SendSignalToUser(userId, SignalCommands.NewUserAdded, userId);
    return joinedPreGame;
  }


  public async Task CombineGroupsToStartGames(CancellationToken cancellationToken = default)
  {
    var gameTypes = await gameTypeRepository.GetActiveGameTypesAsync(cancellationToken);
    var setting = await softSettingService.GetSoftSettingAsync(false, cancellationToken);
    foreach (var gameType in gameTypes)
    {
      await CombineGroupsToStartGamesByGameTypeAsync(gameType, setting, cancellationToken);
    }
  }

  public async Task<UnusualSuspectServiceResult<bool>> PreGameGroupChangeReadyToPlayAsync(int preGameGroupId, PreGameGroupStatusEnum preGameGroupStatusEnum, CancellationToken cancellationToken = default)
  {
    PreGameGroup? preGameGroup =
      await preGameGroupRepository.GetByIdWithDetailAsync(preGameGroupId, cancellationToken);
    if (preGameGroup == null)
      return LogicErrorCode.InvalidPreGameGroupId;
    if (preGameGroup.PreGameGroupStatusId == (short)preGameGroupStatusEnum)
      return new UnusualSuspectServiceResult<bool>(false);
    if (preGameGroup.PreGameGroupStatusId == (short)PreGameGroupStatusEnum.InGame)
      return LogicErrorCode.PreGameGroupIsInGame;
    if (preGameGroupStatusEnum == PreGameGroupStatusEnum.Ready)
    {
      if (preGameGroup.PreGameGroupStatusId != (int)PreGameGroupStatusEnum.NotReady)
        return LogicErrorCode.PreGameGroupHasNoJoinedPreGame;
      if (!preGameGroup.JoinedPreGames.Any())
        return LogicErrorCode.PreGameGroupHasNoJoinedPreGame;
      if (preGameGroup.JoinedPreGames.Any(x => x.ReadyToGameStatusId != (int)ReadyToGameStatusEnum.Ready))
        return LogicErrorCode.ThereIsUnreadyUserInGroup;
      IQueryable<int> gameIds = participateRepository.GetUsersGameIds(preGameGroup.JoinedPreGames.Select(x => x.UserId).ToList());
      if (gameIds.Any())
        return LogicErrorCode.CurrentGroupUsersAreInGame;
      preGameGroup.PreGameGroupStatusId = (int)PreGameGroupStatusEnum.Ready;
      preGameGroup.ReadyToGameTime = DateTime.Now;
    }
    else if (preGameGroupStatusEnum == PreGameGroupStatusEnum.NotReady)
    {
      preGameGroup.PreGameGroupStatusId = (int)PreGameGroupStatusEnum.NotReady;
      preGameGroup.ReadyToGameTime = null;
    }
    return new UnusualSuspectServiceResult<bool>(true);
  }

  public IQueryable<PreGameGroup> GetAllPreGameGroupsWithDetailsWaitingForGame()
  {
    return preGameGroupRepository.GetAllPreGameGroupsWithDetailsWaitingForGame();
  }

  public async Task<UnusualSuspectServiceResult<PreGameDetailsViewModel>> GetPreGameGroupViewModel(int preGameGroupId,
    CancellationToken cancellationToken = default)
  {
    var data = await GetPreGameGroupDetail(preGameGroupId, cancellationToken);
    if (!data.Success)
      return new UnusualSuspectServiceResult<PreGameDetailsViewModel>(data.Errors);
    return new UnusualSuspectServiceResult<PreGameDetailsViewModel>(data.Result.ToPreGameDetailsViewModel());
  }
  public async Task<UnusualSuspectServiceResult<PreGameGroup>> GetPreGameGroupDetail(int preGameGroupId, CancellationToken cancellationToken = default)
  {
    var result = await preGameGroupRepository.GetByIdWithDetailAsync(preGameGroupId, cancellationToken);
    if (result == null)
      return LogicErrorCode.InvalidPreGameGroupId;
    return new UnusualSuspectServiceResult<PreGameGroup>(result);
  }

  public async Task<UnusualSuspectServiceResult<PreGameGroupGetResponse>> GetPreGameGroupResponseDetail(
    int preGameGroupId, int callerUserId, CancellationToken cancellationToken = default)
  {
    if (!await joinedPreGameRepository.UserExistsInPreGameGroupAsync(callerUserId, preGameGroupId, cancellationToken))
      return LogicErrorCode.UserNotMemberOfPreGameGroup;
    return await GetPreGameGroupResponseDetail(preGameGroupId, cancellationToken);
  }
  public async Task<UnusualSuspectServiceResult<PreGameGroupGetResponse>> GetPreGameGroupResponseDetail(
    int preGameGroupId, CancellationToken cancellationToken = default)
  {
    var result = await preGameGroupRepository.GetByIdWithDetailAsync(preGameGroupId, cancellationToken);
    if (result == null)
      return LogicErrorCode.InvalidPreGameGroupId;
    return new UnusualSuspectServiceResult<PreGameGroupGetResponse>(result.ToPreGameGroupDetailResponse());
  }

  public async Task<UnusualSuspectServiceResult<MyPreGameGroupsResponse>> GetPreGameGroupByUserId(int userId)
  {
    var result = await preGameGroupRepository.GetByUserIdWithJoinedPreGameAsync(userId);
    return new UnusualSuspectServiceResult<MyPreGameGroupsResponse>(result.ToMyPreGameGroupsResponse());
  }

  public async Task<UnusualSuspectServiceResult<bool>> ExitFromPreGameGroup(int preGameGroupId, int? userIdToExit, int currentUserId,
    CancellationToken cancellationToken = default)
  {
    JoinedPreGame? joinedPreGame = await joinedPreGameRepository.GetByUserIdPreGameGroupIdAsync(
      currentUserId, preGameGroupId, cancellationToken);
    if (joinedPreGame == null)
      return LogicErrorCode.InvalidPreGameGroupId;
    if (userIdToExit.HasValue && currentUserId != userIdToExit.Value)
    {
      if (!joinedPreGame.IsOwnerOfPreGroup)
        return LogicErrorCode.OnlyGroupOwnerCanRemoveOtherUsersFromGroup;
      joinedPreGame = await joinedPreGameRepository.GetByUserIdPreGameGroupIdAsync(userIdToExit.Value, preGameGroupId, cancellationToken);
      if (joinedPreGame == null)
        return LogicErrorCode.UserJoinedPreGameGroupNotFound;
    }
    if (!userIdToExit.HasValue)
      userIdToExit = currentUserId;
    var user = await applicationUserManager.FindByIdAsync(userIdToExit.Value.ToString());
    if (user == null)
      return LogicErrorCode.InvalidUserId;
    int groupCount = await joinedPreGameRepository.UserCountJoinedPreGameGroupAsync(preGameGroupId, cancellationToken);
    joinedPreGameRepository.Delete(joinedPreGame);
    await coinUsedService.DeletePaymentIfExistsAsync(user, joinedPreGame.Guid, PriceTypeEnum.PreGame, cancellationToken);
    if (groupCount <= 1)
      preGameGroupRepository.DeleteById(preGameGroupId);
    else
    {
      await RecalculatePreGameGroupUsers(preGameGroupId, -1, cancellationToken);
      if (joinedPreGame.IsOwnerOfPreGroup)
      {
        List<JoinedPreGame> members = (await joinedPreGameRepository.JoinedPreGameOfPreGameGroupAsync(preGameGroupId, cancellationToken))
          .Where(x => x.UserId != userIdToExit.Value).ToList();
        if (!members.Any())
          return LogicErrorCode.PreGameGroupHasNoJoinedPreGame;
        var nextOwner = members.First();
        nextOwner.IsOwnerOfPreGroup = true;
      }
      await notificationService.SendSignalToPreGameGroup(preGameGroupId,
        SignalCommands.UserWasRemovedFromPreGameGroup, preGameGroupId);
    }
    return new UnusualSuspectServiceResult<bool>(true);
  }

  public async Task<UnusualSuspectServiceResult<bool>> IsMemberOfPregameGroup(int userId, int preGameGroupId, CancellationToken cancellationToken = default)
  {
    bool result = await joinedPreGameRepository.IsGroupMember(userId, preGameGroupId, cancellationToken);
    return new UnusualSuspectServiceResult<bool>(result);
  }

  public async Task<UnusualSuspectServiceResult<bool>> DeleteAsync(PreGameGroup group)
  {
    if (group.JoinedPreGames != null && group.JoinedPreGames.Any())
    {
      foreach (JoinedPreGame groupJoinedPreGame in group.JoinedPreGames)
      {
        ApplicationUser? user = await applicationUserManager.FindByNameAsync(groupJoinedPreGame.Id.ToString());
        if (user == null)
          continue;
        await coinUsedService.DeletePaymentIfExistsAsync(user, groupJoinedPreGame.Guid, PriceTypeEnum.PreGame);
      }

      joinedPreGameRepository.DeleteRange(group.JoinedPreGames.ToList());
    }
    preGameGroupRepository.Delete(group);
    return true;
  }

  public async Task DeleteExpiredPregameGroupsAsync(CancellationToken cancellationToken)
  {
    int expireMinutes = (await softSetting.GetSoftSettingAsync(false, cancellationToken)).PreGameGroupExpiresInMinutes;
    if (expireMinutes <= 0)
      return;
    PreGameGroup? group = await preGameGroupRepository.GetFirstExpiredPregameGroupWithDetailsAsync(expireMinutes, cancellationToken);
    while (group != null)
    {
      UnusualSuspectServiceResult<bool> result = await DeleteAsync(group);
      if (result.Success && result.Result)
      {
        await uow.SaveChangesAsync(cancellationToken);
        await notificationService.SendSignalToPreGameGroup(group.Id, SignalCommands.PregameGroupWasRemoved);
        logger.LogEvent(SystemEventType.PregameGroupExpiredAndRemoved, group.Id);
      }
      group = await preGameGroupRepository.GetFirstExpiredPregameGroupWithDetailsAsync(expireMinutes, cancellationToken);
    }

  }

  #region PrivateMethods
  private async Task CombineGroupsToStartGamesByGameTypeAsync(GameType gameType, Entities.Models.SoftSetting softSetting, CancellationToken cancellationToken)
  {
    bool needToRefill = true;
    List<PreGameGroup> topGroups = new List<PreGameGroup>();
    //Greedy algorithm with some changes to cover more combinations
    while (true)// while has chance for more games
    {
      if (needToRefill)
      {
        topGroups = await preGameGroupRepository.GetTopPreGameGroupByReadyTimeAsync(gameType, 50, cancellationToken);
        needToRefill = false;
      }
      if (!topGroups.Any())
        return;
      int totalPlayersInQueue = topGroups.Sum(x => x.CalculatedJoinedUsers);
      if (totalPlayersInQueue < 2)//min required
        return;
      int currentGameUserCount = 0;
      List<PreGameGroup> currentGamePreGameGroups = new List<PreGameGroup>();
      List<ApplicationUser> botUsers = new List<ApplicationUser>();
      if (totalPlayersInQueue < gameType.NumberOfPlayers && softSetting.WaitTimeToAddEachBotInSec > 0)//not enough players to create game
      {
        double mostWaitTime = (DateTime.Now - topGroups.Min(x => x.ReadyToGameTime.Value)).TotalSeconds;
        int numberOfPlayersCanBeAdded = (int)Math.Floor(mostWaitTime / softSetting.WaitTimeToAddEachBotInSec);
        if (numberOfPlayersCanBeAdded + totalPlayersInQueue >= gameType.NumberOfPlayers)//can start game using bots
        {
          currentGamePreGameGroups.AddRange(topGroups);
          currentGameUserCount = totalPlayersInQueue;
          botUsers = await GetFreeBotUsersAsync(gameType.NumberOfPlayers - currentGameUserCount, cancellationToken);
          if (botUsers == null || botUsers.Count != gameType.NumberOfPlayers - currentGameUserCount)
            return; //could not get free bots
          currentGameUserCount += botUsers.Count;
        }
        else
          return;//total number of players in queue and the bots are lower that required number of players
      }
      else
      {
        for (int i = 0; i < topGroups.Count; i++)
        {
          if (topGroups[i].CalculatedJoinedUsers > gameType.NumberOfPlayers)
            throw new Exception($"invalid group CalculatedJoinedUsers: {topGroups[i].Id} - CalculatedJoinedUsers: {topGroups[i].CalculatedJoinedUsers}");
          if (topGroups[i].CalculatedJoinedUsers + currentGameUserCount <= gameType.NumberOfPlayers)
          {
            currentGamePreGameGroups.Add(topGroups[i]);
            currentGameUserCount += topGroups[i].CalculatedJoinedUsers;
          }
          if (currentGameUserCount >= gameType.NumberOfPlayers)
            break;
        }
      }
      if (currentGameUserCount == gameType.NumberOfPlayers)
      {
        Game? game = await CreateGameWithSelectedPreGameGroupAsync(currentGamePreGameGroups, gameType, botUsers, softSetting, cancellationToken);
        if (game == null)
          return;
        await SaveChangesAsync(cancellationToken);
        Game? gameWithDetails = await gameRepository.GetGameWithDetailsAsync(game.Id, cancellationToken);
        if (gameWithDetails == null)
        {
          ElmahExtensions.RaiseError(new Exception("Game not available after creation! id: " + game.Id));
          return;
        }
        await notificationService.NotifyOnGameStart(new GameGetResponse(gameWithDetails.ToGameBaseDto(),
          gameWithDetails.ToGameFlowDto(),
          await memoryCacheService.GetSignalRGroupOnlineUsers(game.Id.ToString()), gameWithDetails.GameStatusId,
          null, gameWithDetails.CachedTime.ToString(), setting.Value.GameSetting.TimeToTalkInSeconds));
        needToRefill = true;
      }
      else
        topGroups.RemoveAt(0);//گروه اول که به اجبار در لیست قرار داده شده بود حذف شد تا از گروه دوم شروع شود
    }
    //dynamic programming solution to cover all possible combinations
    //chatgtp:
    /*
     public class Group
       {
       public int Id { get; set; }
       public int Priority { get; set; }
       public List<string> Members { get; set; }
       }

       public class SelectedGroupsResult
       {
       public List<Group> SelectedGroups { get; set; }
       }

       public class GroupSelector
       {
       public SelectedGroupsResult SelectGroups(List<Group> allGroups)
       {
       var groupsCount = allGroups.Count;
       var maxMembers = 12;

       // Create a 2D array to store the maximum priority achievable with the first i groups and j members
       var dp = new int[groupsCount + 1, maxMembers + 1];

       // Build the dynamic programming table
       for (var i = 1; i <= groupsCount; i++)
       {
       for (var j = 0; j <= maxMembers; j++)
       {
       var currentGroup = allGroups[i - 1];
       if (currentGroup.Members.Count > j)
       {
       dp[i, j] = dp[i - 1, j];
       }
       else
       {
       dp[i, j] = Math.Max(dp[i - 1, j], dp[i - 1, j - currentGroup.Members.Count] + currentGroup.Priority);
       }
       }
       }

       // Reconstruct the selected groups
       var selectedGroups = new List<Group>();
       var remainingMembers = maxMembers;

       for (var i = groupsCount; i > 0 && remainingMembers > 0; i--)
       {
       if (dp[i, remainingMembers] != dp[i - 1, remainingMembers])
       {
       var currentGroup = allGroups[i - 1];
       selectedGroups.Add(currentGroup);
       remainingMembers -= currentGroup.Members.Count;
       }
       }

       selectedGroups.Reverse(); // Reversing because we built the list in reverse order

       return new SelectedGroupsResult { SelectedGroups = selectedGroups };
       }
       }
     */
  }

  private async Task<List<ApplicationUser>> GetFreeBotUsersAsync(int numberOfBots, CancellationToken cancellationToken = default)
  {
    return await applicationUserManager.GetFreeBotUsersAsync(numberOfBots, cancellationToken);
  }

  private async Task<Game?> CreateGameWithSelectedPreGameGroupAsync(List<PreGameGroup> preGameGroups, GameType gameType,
    List<ApplicationUser> botUsers, SoftSetting softSetting, CancellationToken cancellationToken = default)
  {
    Game game = gameRepository.Add(new Game()
    {
      CreateTime = DateTime.Now,
      FinishedTime = null,
      GameType = gameType,
      GameStatusId = (short)GameStatusEnum.WaitingForPlayers
    });
    bool done = await AddGameParticipants(preGameGroups, game, gameType, botUsers, softSetting, cancellationToken);
    if (!done)
      return null;
    await Add12RandomCharactersToGame(game, cancellationToken);
    await Add11RandomQuestionsToGame(game, cancellationToken);
    return game;
  }

  private async Task Add11RandomQuestionsToGame(Game game, CancellationToken cancellationToken = default)
  {
    List<Question> activeQuestions = await questionRepository.GetRandomActiveQuestionsAsync(11, cancellationToken);
    for (short i = 0; i < activeQuestions.Count; i++)
    {
      questionGameRepository.Add(new QuestionGame()
      {
        Game = game,
        Question = activeQuestions[i],
        Turn = (short)(i + 1)
      });
    }
  }

  private async Task<bool> AddGameParticipants(List<PreGameGroup> preGameGroups, Game game, GameType gameType,
    List<ApplicationUser> botUsers, SoftSetting softSetting, CancellationToken cancellationToken = default)
  {
    short counter = 1;
    int humanPlayerCount = gameType.NumberOfPlayers - 1 - botUsers.Count;
    if (humanPlayerCount < 2)
      return false;
    List<int> selectedNumbers = RandomHelper.GetUniqueRandomNumbers(0, humanPlayerCount, humanPlayerCount >= 3 ? 3 : 2);
    int usersAddedCounter = 0;
    for (int i = 0; i < preGameGroups.Count; i++)
    {
      List<JoinedPreGame> joined =
        await joinedPreGameRepository.JoinedPreGameOfPreGameGroupAsync(preGameGroups[i].Id, cancellationToken);
      for (int j = 0; j < joined.Count; j++)
      {
        if (usersAddedCounter > gameType.NumberOfPlayers)
        {
          logger.LogEvent(SystemEventType.GameAddedParticipantsMoreThanTypeNumberOfPlayers, game.Id,
            $"PreGameGroupIds ({string.Join(", ", preGameGroups.Select(x => x.Id))}) - userId ({joined[j].UserId})",
            logLevel: LogLevel.Critical);
          break;
        }
        //random role selection
        RoleCardEnum role;
        if (selectedNumbers[0] == usersAddedCounter)
          role = RoleCardEnum.Witness;
        else if (selectedNumbers[1] == usersAddedCounter)
          role = RoleCardEnum.MainDetective;
        else if (selectedNumbers.Count > 2 && selectedNumbers[2] == usersAddedCounter)
          role = RoleCardEnum.Accomplice;
        else
          role = RoleCardEnum.Detective;
        Guid newGuid = Guid.NewGuid();
        participateRepository.Add(new Participate()
        {
          UserId = joined[j].UserId,
          Game = game,
          IsActive = true,
          OrderOfParticipation = counter++,
          RoleCardId = (short)role,
          Guid = newGuid
        });
        if (softSetting.CoinCostToEnterPreGame > 0)
        {
          CoinUsedUser? coinUsedUser = await coinUsedUserRepository.GetPreGameSavePaymentAsync(joined[j].Guid, cancellationToken);
          if (coinUsedUser == null)
            logger.LogEvent(SystemEventType.PreGamePaymentNotFoundToWhileChangingToGame, joined[j].UserId,
              $"preGameGroupId ({joined[j].PreGameGroupId}) - guid ({joined[j].Guid})", logLevel: LogLevel.Critical);
          else
          {
            if (coinUsedUser.Amount != softSetting.CoinCostToEnterPreGame)
              logger.LogEvent(SystemEventType.CoinUsedUserAmountNoEqualToCoinCostToEnterPreGame, joined[j].UserId,
                $"preGameGroupId ({joined[j].PreGameGroupId}) - guid ({joined[j].Guid})", logLevel: LogLevel.Warning);
            coinUsedUser.UsedForPriceTypeId = (int)PriceTypeEnum.Game;
            coinUsedUser.ReferenceGuid = newGuid;
          }
        }
        usersAddedCounter++;
      }
      preGameGroups[i].PreGameGroupStatusId = (short)PreGameGroupStatusEnum.InGame;
      preGameGroups[i].Game = game;
    }
    if(botUsers.Any())
    {
      for (int i = 0; i < botUsers.Count; i++)
      {
        RoleCardEnum role;
        if (humanPlayerCount == 2 && i == 0)
          role = RoleCardEnum.Accomplice;
        else
          role = RoleCardEnum.Detective;
        Guid newGuid = Guid.NewGuid();
        participateRepository.Add(new Participate()
        {
          UserId = botUsers[i].Id,
          Game = game,
          IsActive = true,
          OrderOfParticipation = counter++,
          RoleCardId = (short)role,
          Guid = newGuid
        });
      }
      logger.LogEvent(SystemEventType.BotAddedToTheGame, preGameGroups[0].Id, "Bot count: " + botUsers.Count);
    }
    return true;
  }

  private async Task Add12RandomCharactersToGame(Game game, CancellationToken cancellationToken = default)
  {
    var activeCards = await characterCardRepository.GetRandomActiveCharacterCardsAsync(12, cancellationToken);
    Random rnd = new Random();
    int murdererIndex = rnd.Next(0, 11);
    for (int i = 0; i < activeCards.Count; i++)
    {
      bool isMurderer = i == murdererIndex;
      characterCardGameRepository.Add(new CharacterCardGame()
      {
        CharacterCard = activeCards[i],
        Game = game,
        IsActive = true,
        IsMurderer = isMurderer
      });
    }

  }

  private async Task<bool> CheckUserOwnsThePreGameGroup(int userId, int preGameGroupId, CancellationToken cancellationToken = default)
  {
    JoinedPreGame? joinedPreGame = await joinedPreGameRepository.GetByUserIdPreGameGroupIdAsync(userId, preGameGroupId, cancellationToken);
    if (joinedPreGame == null)
      return false;
    return joinedPreGame.IsOwnerOfPreGroup;
  }
  private async Task<bool> CheckUserJoinedPreGameGroup(int userId, int preGameGroupId, CancellationToken cancellationToken = default)
  {
    return await joinedPreGameRepository.UserExistsInPreGameGroupAsync(userId, preGameGroupId, cancellationToken);
  }

  #endregion PrivateMethods

}