using Microsoft.Extensions.Options;
using ProcessVueAlarmZones.Application.Config;
using ProcessVueAlarmZones.Application.Interface;
using ProcessVueAlarmZones.Domain.Domain;

namespace ProcessVueAlarmZones.Application.Service;

/// <summary>
/// Geometry-accurate EEMUA 191 Rev 3 classifier:
///  - 3 vertical cutoffs at x=1,2,10
///  - 3 sloped boundaries: (0,25)-(1,10), (1,50)-(2,25), (2,50)-(10,25)
/// </summary>
public sealed class EemuaZoneClassifier : IEemuaZoneClassifier
{
    private readonly EemuaGeometry _g;
    public EemuaZoneClassifier(IOptions<EemuaGeometry> options) => _g = options.Value;

    public Zone Classify(double x, double y)
    {
        if (x < 0) throw new ArgumentOutOfRangeException(nameof(x));
        if (y < 0 || y > 100) throw new ArgumentOutOfRangeException(nameof(y));

        // Far right: x > X3 → Overloaded
        if (x > _g.X3) return Zone.Overloaded;

        // Segment A: 0 ≤ x ≤ 1 (Robust wedge vs Stable)
        if (x <= _g.X1)
        {
            var roof = EvalLine(_g.Robust_X0, _g.Robust_Y0, _g.Robust_X1, _g.Robust_Y1, x);
            return y <= roof ? Zone.Robust : (y >= _g.YTop ? Zone.Stable : Zone.Stable);
        }

        // Segment B: 1 < x ≤ 2 (Stable/Reactive diagonal)
        if (x <= _g.X2)
        {
            var sr = EvalLine(_g.SR_X0, _g.SR_Y0, _g.SR_X1, _g.SR_Y1, x);
            return y > sr ? Zone.Reactive : Zone.Stable;
        }

        // Segment C: 2 < x ≤ 10 (Reactive/Overloaded diagonal)
        {
            var ro = EvalLine(_g.RO_X0, _g.RO_Y0, _g.RO_X1, _g.RO_Y1, x);
            return y > ro ? Zone.Overloaded : Zone.Reactive;
        }
    }

    private static double EvalLine(double x0, double y0, double x1, double y1, double x)
    {
        var m = (y1 - y0) / (x1 - x0);
        var b = y0 - m * x0;
        return m * x + b;
    }
}