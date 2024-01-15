using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Entities.GameModels;

public class QuestionGame : BaseEntity
{
	public short QuestionId { get; set; }
	[ForeignKey("QuestionId")]
	public virtual Question Question { get; set; }
	public int GameId { get; set; }
	[ForeignKey("GameId")]
	public virtual Game Game { get; set; }
	public short Turn { get; set; }
	public bool? UserAnswer { get; set; }
	public int? AnswerUserId { get; set; }
	[ForeignKey("AnswerUserId")]
	public virtual ApplicationUser? AnswerUser { get; set; }
}