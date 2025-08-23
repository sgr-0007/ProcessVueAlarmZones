using ProcessVueAlarmZones.Application.Config;
using ProcessVueAlarmZones.Application.Interface;
using ProcessVueAlarmZones.Domain.Domain;
using Microsoft.Extensions.Options;

namespace ProcessVueAlarmZones.Application.Service
{
/// <summary>
/// EEMUA 191 classifier using a banded rule lookup.
/// 
/// 1) Bucket the inputs into bands:
///    X (average alarm rate) → B0, B1, B2 based on cutoffs X1, X2, X3
///    Y (% time outside target) → C0, C1, C2 based on cutoffs Y1, Y2, Y3
/// 2) Then look up the (bandX, bandY) pair in a small lookup to get the Zone.
/// 
/// `xb`/`yb` are 0/1/2:
/// - They are indexes into the lookup table (B0=0, B1=1, B2=2; C0=0, C1=1, C2=2).
/// - The numeric thresholds (X1..X3, Y1..Y3) are used to calculate the band index.
///       (0,0) Robust | (0,1) Robust | (0,2) Stable
///       (1,0) Stable | (1,1) Stable | (1,2) Reactive
///       (2,0) Reactive | (2,1) Reactive | (2,2) Reactive
///
/// Global rules (fast paths):
/// - Overloaded if Y ≥ Y3 (e.g 50%) or X ≥ X3 (e.g 10)
/// - Reactive for 2 < X < 10 when Y < Y3
/// - At X == X2 intentionally keep it in band B1 (so (2, 20) == Stable).
/// </summary>
    public sealed class EemuaZoneClassifier : IEemuaZoneClassifier
    {
        private readonly EemuaThresholds _t;

        public EemuaZoneClassifier(IOptions<EemuaThresholds> options)
        {
            _t = options.Value;
        }

        public Zone Classify(double avg, double pct)
        {
            if (avg < 0) throw new ArgumentOutOfRangeException(nameof(avg), "Average alarm rate cannot be negative.");
            if (pct < 0 || pct > 100) throw new ArgumentOutOfRangeException(nameof(pct), "Percentage must be between 0 and 100.");

            // Global cutoffs
            if (pct >= _t.Y3) return Zone.Overloaded;
            if (avg >= _t.X3) return Zone.Overloaded;
            if (avg > _t.X2 && avg < _t.X3) return Zone.Reactive; // x==X2 handled below

            // X band (x==X2 inclusive -> band1)
            int xb = avg < _t.X1 ? 0 : (avg <= _t.X2 ? 1 : 2);
            // Y band (y<Y3 guaranteed here)
            int yb = pct < _t.Y1 ? 0 : (pct < _t.Y2 ? 1 : 2);

            return (xb, yb) switch
            {
                (0, 0) => Zone.Robust,
                (0, 1) => Zone.Robust,
                (0, 2) => Zone.Stable,

                (1, 0) => Zone.Stable,
                (1, 1) => Zone.Stable,
                (1, 2) => Zone.Reactive,

                (2, 0) => Zone.Reactive,
                (2, 1) => Zone.Reactive,
                (2, 2) => Zone.Reactive,

                _ => Zone.Reactive
            };
        }
    }
}
