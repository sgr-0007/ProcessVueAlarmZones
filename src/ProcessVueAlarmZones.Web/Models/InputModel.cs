using System.ComponentModel.DataAnnotations;
namespace ProcessVueAlarmZones.Web.Models;

public sealed class InputModel
{
    [Display(Name = "Average Alarm Rate")]
    [Range(0, double.MaxValue, ErrorMessage = "Value must be ≥ 0")]
    public double AverageAlarmRate { get; set; }

    [Display(Name = "% Time Outside Target")]
    [Range(0, 100, ErrorMessage = "Value must be between 0 and 100")]
    public double PercentOutsideTarget { get; set; }
}