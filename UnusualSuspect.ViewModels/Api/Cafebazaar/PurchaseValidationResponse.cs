using Newtonsoft.Json;

namespace UnusualSuspect.ViewModels.Api.Cafebazaar;
public class PurchaseValidationResponse
{
    [JsonProperty("consumptionState")]
    public int ConsumptionState { get; set; }

    [JsonProperty("purchaseState")]
    public int PurchaseState { get; set; }

    [JsonProperty("kind")]
    public string Kind { get; set; }

    [JsonProperty("developerPayload")]
    public string DeveloperPayload { get; set; }

    [JsonProperty("purchaseTime")]
    public long PurchaseTime { get; set; }
}
