using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels;

public class Game : BaseEntity
{
	public DateTime CreateTime { get; set; }
	public DateTime? FinishedTime { get; set; }
	public short GameTypeId { get; set; }
  [ForeignKey("GameTypeId")]
	public virtual GameType GameType { get; set; }

  [DefaultValue((short)GameStatusEnum.WaitingForPlayers)]
  public short GameStatusId { get; set; }
  [ForeignKey("GameStatusId")]
  public virtual GameStatus GameStatus { get; set; }

  public short? OrderOfParticipationTalkBeginner { get; set; }
  public short? OrderOfParticipationTurnToTalk { get; set; }
  public DateTime? TalkingTurnStartedTime { get; set; }
  public DateTime? CurrentUserTurnStartedTime { get; set; }

  public virtual ICollection<CharacterCardGame> CharacterCardGames { get; set; }
	public virtual ICollection<Participate> Participates { get; set; }
	public virtual ICollection<QuestionGame> QuestionGames { get; set; }
	public virtual ICollection<GameCandidate> GameCandidates { get; set; }

	[NotMapped]
  public string CreateTimePersian => CreateTime.ToString();
	[NotMapped]
  public string FinishedTimePersian => FinishedTime == null ? "" : FinishedTime.ToString();
}