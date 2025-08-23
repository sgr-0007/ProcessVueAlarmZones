using ProcessVueAlarmZones.Domain.Domain;

namespace ProcessVueAlarmZones.Web.Models;

public sealed class IndexViewModel
{
    public InputModel Input { get; set; } = new();
    public Zone? Result { get; set; }
}