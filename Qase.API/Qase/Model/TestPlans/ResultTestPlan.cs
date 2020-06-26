using Newtonsoft.Json;

namespace Qase.API.Qase.Model.TestPlans
{
  public partial class ResultTestPlan
  {
    [JsonProperty("id")]
    public int Id { get; set; }
  }
}
