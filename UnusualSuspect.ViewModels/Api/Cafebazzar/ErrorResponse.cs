using Newtonsoft.Json;

namespace UnusualSuspect.ViewModels.Api.Cafebazzar;
public class ErrorResponse
{
  [JsonProperty("error")]
  public string Error { get; set; }

  [JsonProperty("error_description")]
  public string ErrorDescription { get; set; }
}