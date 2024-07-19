using System;
using System.Collections.Generic;

namespace softub.Models;

public partial class ConfigValue
{
    public int? TargetTemp { get; set; }

    public int? FailSafeHot { get; set; }

    public int? FailSafeCold { get; set; }

    public int? LastTemp { get; set; }

    public int? JetsOn { get; set; }

    public int? LightsOn { get; set; }

    public int ConfigVersion { get; set; }
}
