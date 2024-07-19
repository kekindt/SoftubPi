using System.ComponentModel.DataAnnotations;

namespace softubWeb.Models.Views;
public class ControlIndex
{
    [Display(Name = "Target Tempurature")]
    public int TargetTemp { get; set; } = 100;
    public int FailSafeCold { get; set; } = 40;
    public int FailSafeHot { get; set; } = 110;
    [Display(Name = "Jets On")]
    public bool JetsOn { get; set; } = false;
    [Display(Name = "Lights On")]
    public bool LightsOn { get; set; } = false;
    [Display(Name = "Current Tempurature")]
    public int LastTemp { get; set; } = 0;
}