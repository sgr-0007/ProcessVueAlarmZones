using Microsoft.Extensions.Options;
using ProcessVueAlarmZones.Application.Config;
using ProcessVueAlarmZones.Application.Service;
using ProcessVueAlarmZones.Domain.Domain;

namespace ProcessVueAlarmZones.Tests;

public class EemuaZoneClassifierTest
{
    private static EemuaZoneClassifier Make()
    => new(Options.Create(new EemuaGeometry()));


    [Fact] //Robust wedge: below roof
    public void Robust_WhenBelowRoofLine()
        => Assert.Equal(Zone.Robust, Make().Classify(0.5, 15));

    [Fact] //Stable: above roof but x<1
    public void Stable_WhenAboveRoofLine_LeftBand()
        => Assert.Equal(Zone.Stable, Make().Classify(0.5, 20));

    [Fact] //Stable: below SR line
    public void Stable_WhenBelowSRLine()
        => Assert.Equal(Zone.Stable, Make().Classify(1.5, 30));

    [Fact] //Reactive: above SR line
    public void Reactive_WhenAboveSRLine()
        => Assert.Equal(Zone.Reactive, Make().Classify(1.5, 40));

    [Fact] //Reactive: below RO line
    public void Reactive_WhenBelowROLine()
        => Assert.Equal(Zone.Reactive, Make().Classify(3.0, 20));

    [Fact] //Overloaded: above RO line
    public void Overloaded_WhenAboveROLine()
        => Assert.Equal(Zone.Overloaded, Make().Classify(3.0, 60));

    [Fact] //Reactive: at right edge on RO line
    public void Reactive_WhenOnROLine()
        => Assert.Equal(Zone.Reactive, Make().Classify(10.0, 25));

    [Fact] //Overloaded: far right cutoff
    public void Overloaded_WhenXGreaterThan10()
        => Assert.Equal(Zone.Overloaded, Make().Classify(11.0, 10));

    [Fact] //Invalid: negative x
    public void Throws_WhenXNegative()
        => Assert.Throws<ArgumentOutOfRangeException>(() => Make().Classify(-1, 10));

    [Fact] //Invalid: y out of range
    public void Throws_WhenYGreaterThan100()
        => Assert.Throws<ArgumentOutOfRangeException>(() => Make().Classify(1, 120));
}
