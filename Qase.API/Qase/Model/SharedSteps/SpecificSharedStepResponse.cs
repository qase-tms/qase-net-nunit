﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Qase.API.Qase.Model.SharedSteps
{
  public partial class SpecificSharedStepResponse : BaseResponse
  {
    [JsonProperty("result")]
    public List<SharedStep> Result { get; set; }
  }
}
