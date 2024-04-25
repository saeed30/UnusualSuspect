using System.ComponentModel.DataAnnotations.Schema;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.Entities.GameModels;
public class QuestionCharacterCardDefaultAnswer : BaseEntity
{
  public short QuestionId { get; set; }
  [ForeignKey("QuestionId")]
  public virtual Question Question { get; set; }
  public short CharacterCardId { get; set; }
  [ForeignKey("CharacterCardId")]
  public virtual CharacterCard CharacterCard { get; set; }
  public bool DefaultAnswer { get; set; }
  public DateTime DateTimeAdded { get; set; }
  [NotMapped]
  public string DateTimeAddedPersian => DateTimeAdded.ToString();
}
