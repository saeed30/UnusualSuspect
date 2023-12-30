using ElmahCore;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.DataLayer;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Mapping;

namespace UnusualSuspect.Services.Services;

public class PreGameService(IUnitOfWork uow,
    IPreGameGroupRepository preGameGroupRepository,
    IJoinedPreGameRepository joinedPreGameRepository,
    IApplicationUserManager applicationUserManager,
    IGameTypeRepository gameTypeRepository,
    IGameRepository gameRepository,
    IParticipateRepository participateRepository,
    ICharacterCardRepository characterCardRepository,
    ICharacterCardGameRepository characterCardGameRepository,
    INotificationService notificationService,
    IQuestionRepository questionRepository,
    IQuestionGameRepository questionGameRepository,
    ITurnOfPlayService turnOfPlayService)
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
      return new UnusualSuspectServiceResult<PreGameGroup>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameTypeId));
    if (await gameRepository.UserIsInActiveGameAsync(userId, cancellationToken))
      return new UnusualSuspectServiceResult<PreGameGroup>(new UnusualSuspectErrorResult(LogicErrorCode.UserIsInActiveGame));
    var oldPreGames = await joinedPreGameRepository.JoinedPreGameOfUserAsync(userId, cancellationToken);
    if (oldPreGames.Any())
    {
      JoinedPreGame? firstOwnGame = oldPreGames.FirstOrDefault(x => x.IsOwnerOfPreGroup && x.PreGameGroup.GameTypeId == gameTypeId);
      if (firstOwnGame != null)
      {
        var preGame = await preGameGroupRepository.GetByIdAsync(firstOwnGame.PreGameGroupId, cancellationToken);
        if (preGame == null)
          throw new Exception("wrong refrence to preGameGroup: " + firstOwnGame.PreGameGroupId);
        return new UnusualSuspectServiceResult<PreGameGroup>(preGame);
      }
      await RemoveFromAllUserPreGames(oldPreGames.ToList(), userId, cancellationToken);
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
      ReadyToGameStatusId = (int)ReadyToGameStatusEnum.Ready
    };
    joinedPreGameRepository.Add(joinedPreGame);
    return new UnusualSuspectServiceResult<PreGameGroup>(group);
  }

  public async Task RemoveFromAllUserPreGames(List<JoinedPreGame> joinedPreGame, int userId, CancellationToken cancellationToken = default)
  {
    if (!joinedPreGame.Any())
      return;
    for (int i = 0; i < joinedPreGame.Count; i++)
      await RemoveUserFromPreGameGroup(joinedPreGame[i], userId, cancellationToken);
  }

  public async Task RemoveUserFromPreGameGroup(JoinedPreGame joinedPreGame, int userId, CancellationToken cancellationToken = default)
  {
    await joinedPreGameRepository.ExecuteDeleteUserJoinedPreGameGroupAsync(userId, joinedPreGame.PreGameGroupId, cancellationToken);
    if (joinedPreGame.IsOwnerOfPreGroup || !await joinedPreGameRepository.ExistsInPreGameGroupExceptUserAsync(userId, joinedPreGame.PreGameGroupId, cancellationToken))
      await RemovePreGameGroup(joinedPreGame.PreGameGroupId, cancellationToken);
    else
      await RecalculatePreGameGroupUsers(joinedPreGame.PreGameGroupId, -1, cancellationToken);
  }
  public async Task RecalculatePreGameGroupUsers(int preGameGroupId, short changeOnThisTransaction = 0, CancellationToken cancellationToken = default)
  {
    var preGame = await preGameGroupRepository.GetByIdAsync(preGameGroupId, cancellationToken);
    if (preGame == null)
      throw new Exception("preGameGroup not found. preGameGroupId: " + preGameGroupId);
    int count = await joinedPreGameRepository.UserCountJoinedPreGameGroupAsync(preGameGroupId, cancellationToken);
    if (count > 32000)
      throw new Exception("invalid user count. preGameGroupId: " + preGameGroupId);
    preGame.CalculatedJoinedUsers = (short)(count + changeOnThisTransaction);
  }

  public async Task RemovePreGameGroup(int preGameGroupId, CancellationToken cancellationToken = default)
  {
    await joinedPreGameRepository.ExecuteDeleteAllJoinedPreGameGroupAsync(preGameGroupId, cancellationToken);
    await preGameGroupRepository.ExecuteDeleteByIdAsync(preGameGroupId, cancellationToken);
  }
  public async Task<UnusualSuspectServiceResult<bool>> ChangeUserReadyStatus(int userId, int preGameGroupId,
    ReadyToGameStatusEnum readyToGameStatusEnum, CancellationToken cancellationToken = default)
  {
    var preGameGroup = await preGameGroupRepository.GetByIdAsync(preGameGroupId, cancellationToken);
    if (preGameGroup == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidPreGameGroupId));
    if (preGameGroup.PreGameGroupStatusId != (int)PreGameGroupStatusEnum.NotReady)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidPreGameGroupStatusId));
    var joinedPreGame = await joinedPreGameRepository.GetByUserIdPreGameGroupIdAsync(userId, preGameGroupId, cancellationToken);
    if (joinedPreGame == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.UserJoinedPreGameGroupNotFound));
    joinedPreGame.ReadyToGameStatusId = (short)readyToGameStatusEnum;
    return new UnusualSuspectServiceResult<bool>(true);
  }
  public async Task<UnusualSuspectServiceResult<bool>> StartPreGameGroup(int preGameGroupId)
  {
    if (!await joinedPreGameRepository.AllJoinedPreGameGroupUsersAreReadyAsync(preGameGroupId))
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.ThereIsUnreadyUserInGroup));
    return new UnusualSuspectServiceResult<bool>(true);
  }
  public async Task<UnusualSuspectServiceResult<JoinedPreGame>> AddUserToPreGameGroup(int addingUserId, string username, int preGameGroupId, CancellationToken cancellationToken = default)
  {
    var user = await applicationUserManager.FindByNameAsync(username);
    if (user == null)
      return new UnusualSuspectServiceResult<JoinedPreGame>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidUsername));
    if (user.Id == addingUserId)
      return new UnusualSuspectServiceResult<JoinedPreGame>(new UnusualSuspectErrorResult(LogicErrorCode.CanNotAddOwnToPreGameGroup));
    return await AddUserToPreGameGroup(addingUserId, user.Id, preGameGroupId, cancellationToken);
  }
  public async Task<UnusualSuspectServiceResult<JoinedPreGame>> AddUserToPreGameGroup(int addingUserId, int userId, int preGameGroupId, CancellationToken cancellationToken = default)
  {
    if (await gameRepository.UserIsInActiveGameAsync(userId, cancellationToken))
      return new UnusualSuspectServiceResult<JoinedPreGame>(new UnusualSuspectErrorResult(LogicErrorCode.UserIsInActiveGame));
    PreGameGroup? preGameGroup = await preGameGroupRepository.GetByIdAsync(preGameGroupId, cancellationToken);
    if (preGameGroup == null)
      return new UnusualSuspectServiceResult<JoinedPreGame>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidPreGameGroupId));
    bool ownsTheGroup = await CheckUserOwnsThePreGameGroup(addingUserId, preGameGroupId, cancellationToken);
    if (!ownsTheGroup)
      return new UnusualSuspectServiceResult<JoinedPreGame>(new UnusualSuspectErrorResult(LogicErrorCode.UserDoNotOwnTheGroup));
    bool alreadyJoinedPreGameGroup = await CheckUserJoinedPreGameGroup(userId, preGameGroupId, cancellationToken);
    if (alreadyJoinedPreGameGroup)
      return new UnusualSuspectServiceResult<JoinedPreGame>(new UnusualSuspectErrorResult(LogicErrorCode.UserAlreadyJoinedPreGameGroup));
    JoinedPreGame joinedPreGame = new JoinedPreGame()
    {
      UserId = userId,
      IsOwnerOfPreGroup = false,
      JoinTime = DateTime.Now,
      PreGameGroup = preGameGroup,
      ReadyToGameStatusId = (int)ReadyToGameStatusEnum.Notified
    };
    joinedPreGameRepository.Add(joinedPreGame);
    await RecalculatePreGameGroupUsers(preGameGroupId, 1, cancellationToken);
    await notificationService.SendSignalToUser(userId, SignalCommands.NewUserAdded, userId);
    return new UnusualSuspectServiceResult<JoinedPreGame>(joinedPreGame);
  }


  public async Task CombineGroupsToStartGames(CancellationToken cancellationToken = default)
  {
    var gameTypes = await gameTypeRepository.GetActiveGameTypesAsync(cancellationToken);
    foreach (var gameType in gameTypes)
    {
      await CombineGroupsToStartGamesByGameTypeAsync(gameType, cancellationToken);
    }
  }

  public async Task<UnusualSuspectServiceResult<bool>> PreGameGroupChangeReadyToPlayAsync(int preGameGroupId, PreGameGroupStatusEnum preGameGroupStatusEnum, CancellationToken cancellationToken = default)
  {
    PreGameGroup? preGameGroup =
      await preGameGroupRepository.GetByIdWithJoinedPreGameAsync(preGameGroupId, cancellationToken);
    if (preGameGroup == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidPreGameGroupId));
    if (preGameGroup.PreGameGroupStatusId == (short)preGameGroupStatusEnum)
      return new UnusualSuspectServiceResult<bool>(false);
    if (preGameGroup.PreGameGroupStatusId == (short)PreGameGroupStatusEnum.InGame)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.PreGameGroupIsInGame));
    if (preGameGroupStatusEnum == PreGameGroupStatusEnum.Ready)
    {
      if (preGameGroup.PreGameGroupStatusId != (int)PreGameGroupStatusEnum.NotReady)
        return new UnusualSuspectServiceResult<bool>(
          new UnusualSuspectErrorResult(LogicErrorCode.PreGameGroupHasNoJoinedPreGame));
      if (!preGameGroup.JoinedPreGames.Any())
        return new UnusualSuspectServiceResult<bool>(
          new UnusualSuspectErrorResult(LogicErrorCode.PreGameGroupHasNoJoinedPreGame));
      if (preGameGroup.JoinedPreGames.Any(x => x.ReadyToGameStatusId != (int)ReadyToGameStatusEnum.Ready))
        return new UnusualSuspectServiceResult<bool>(
          new UnusualSuspectErrorResult(LogicErrorCode.ThereIsUnreadyUserInGroup));
      List<int> inGameUserIds = CheckNoJoinedUsersAreInGameAndDeleteInactiveJoinedPreGames(preGameGroup);
      if (inGameUserIds.Any())
        return new UnusualSuspectServiceResult<bool>(
          new UnusualSuspectErrorResult(LogicErrorCode.CurrentGroupUsersAreInGame));
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

  public async Task<UnusualSuspectServiceResult<PreGameGroupGetResponse>> GetPreGameGroupDetail(
    int preGameGroupId, int userId, CancellationToken cancellationToken = default)
  {
    if (!await joinedPreGameRepository.UserExistsInPreGameGroupAsync(userId, preGameGroupId, cancellationToken))
      return new UnusualSuspectServiceResult<PreGameGroupGetResponse>(
        new UnusualSuspectErrorResult(LogicErrorCode.UserNotMemberOfPreGameGroup));
    var result = await preGameGroupRepository.GetByIdWithJoinedPreGameAsync(preGameGroupId, cancellationToken);
    if (result == null)
      return new UnusualSuspectServiceResult<PreGameGroupGetResponse>(
        new UnusualSuspectErrorResult(LogicErrorCode.InvalidPreGameGroupId));
    return new UnusualSuspectServiceResult<PreGameGroupGetResponse>(result.ToGetPreGameGroupDetailResponse());
  }

  public async Task<UnusualSuspectServiceResult<MyPreGameGroupsResponse>> GetPreGameGroupByUserId(int userId)
  {
    var result = await preGameGroupRepository.GetByUserIdWithJoinedPreGameAsync(userId);
    return new UnusualSuspectServiceResult<MyPreGameGroupsResponse>(result.ToMyPreGameGroupsResponse());
  }


  private List<int> CheckNoJoinedUsersAreInGameAndDeleteInactiveJoinedPreGames(PreGameGroup preGameGroup)
  {
    //throw new NotImplementedException();
    return new List<int>();
  }

  #region PrivateMethods
  private async Task CombineGroupsToStartGamesByGameTypeAsync(GameType gameType, CancellationToken cancellationToken)
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
      int currentGameUserCount = 0;
      List<PreGameGroup> currentGamePreGameGroups = new List<PreGameGroup>();
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

      if (currentGameUserCount == gameType.NumberOfPlayers)
      {
        Game game = await CreateGameWithSelectedPreGameGroupAsync(currentGamePreGameGroups, gameType, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        Game? gameWithDetails = await gameRepository.GetGameWithDetailsAsync(game.Id, cancellationToken);
        if (gameWithDetails == null)
        {
          ElmahExtensions.RaiseError(new Exception("Game not available after creation! id: " + game.Id));
          return;
        }
        await turnOfPlayService.StartTurnOfPlayAsync(gameWithDetails.Id);
        await notificationService.NotifyOnGameStart(new GameGetResponse(gameWithDetails.ToGameBaseDto(),
          gameWithDetails.ToGameFlowDto()));
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

  private async Task<Game> CreateGameWithSelectedPreGameGroupAsync(List<PreGameGroup> preGameGroups, GameType gameType,
    CancellationToken cancellationToken = default)
  {
    Game game = gameRepository.Add(new Game()
    {
      CreateTime = DateTime.Now,
      FinishedTime = null,
      GameType = gameType,
      GameStatusId = (short)GameStatusEnum.WaitingForPlayers
    });
    await AddGameParticipants(preGameGroups, game, cancellationToken);
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

  private async Task AddGameParticipants(List<PreGameGroup> preGameGroups, Game game, CancellationToken cancellationToken = default)
  {
    short counter = 1;
    for (int i = 0; i < preGameGroups.Count; i++)
    {
      List<JoinedPreGame> joined =
        await joinedPreGameRepository.JoinedPreGameOfPreGameGroupAsync(preGameGroups[i].Id, cancellationToken);
      List<int> selectedNumbers = RandomHelper.GetUniqueRandomNumbers(0, joined.Count - 1, 3);
      for (int j = 0; j < joined.Count; j++)
      {
        //random role selection
        RoleCardEnum role;
        if (selectedNumbers[0] == j)
          role = RoleCardEnum.Accomplice;
        else if (selectedNumbers[1] == j)
          role = RoleCardEnum.MainDetective;
        else if (selectedNumbers[2] == j)
          role = RoleCardEnum.Witness;
        else
          role = RoleCardEnum.Detective;

        participateRepository.Add(new Participate()
        {
          UserId = joined[j].UserId,
          Game = game,
          IsActive = true,
          OrderOfParticipation = counter++,
          RoleCardId = (short)role
        });
      }

      preGameGroups[i].PreGameGroupStatusId = (short)PreGameGroupStatusEnum.InGame;
      preGameGroups[i].Game = game;
    }
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