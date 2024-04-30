using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ApiViewModels.Enums.BaseData
{
  public enum ScoreTypeEnum
  {
    [Display(Name = "برد با نقش کارآگاه")]
    GameWonAsDetective = 1,
    [Display(Name = "برد با نقش کارآگاه ستاره دار")]
    GameWonAsMainDetective = 2,
    [Display(Name = "برد با نقش شاهد")]
    GameWonAsWitness = 3,
    [Display(Name = "برد با نقش شریک جرم")]
    GameWonAsAccomplice = 4,
    [Display(Name = "باخت با نقش کارآگاه")]
    GameLostAsDetective = 5,
    [Display(Name = "باخت با نقش کارآگاه ستاره دار")]
    GameLostAsMainDetective = 6,
    [Display(Name = "باخت با نقش شاهد")]
    GameLostAsWitness = 7,
    [Display(Name = "باخت با نقش شریک جرم")]
    GameLostAsAccomplice = 8,
  }
}
