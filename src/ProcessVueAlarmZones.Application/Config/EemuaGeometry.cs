namespace ProcessVueAlarmZones.Application.Config
{
    /// <summary>
    /// Defines the vertical cutoffs and diagonal boundaries that divide
    /// Robust, Stable, Reactive, and Overloaded zones.
    ///
    /// Vertical cutoffs:
    ///   - X1 = 1  → End of Robust wedge
    ///   - X2 = 2  → End of Stable band
    ///   - X3 = 10 → End of Reactive band (beyond = Overloaded)
    ///
    /// Top reference:
    ///   - YTop = 50% → Chart’s top band cutoff
    ///
    /// Sloped boundaries:
    ///   - Robust roof line: (Robust_X0,Robust_Y0) → (Robust_X1,Robust_Y1)
    ///       Separates Robust vs Stable (0,25) → (1,10)
    ///
    ///   - Stable/Reactive boundary: (SR_X0,SR_Y0) → (SR_X1,SR_Y1)
    ///       Separates Stable vs Reactive (1,50) → (2,25)
    ///
    ///   - Reactive/Overloaded line: (RO_X0,RO_Y0) → (RO_X1,RO_Y1)
    ///       Separates Reactive vs Overloaded (2,50) → (10,25)
    ///
    /// These values can be overridden in appsettings.json.
    /// </summary>
    
    public sealed class EemuaGeometry
    {
        // Vertical regime cutoffs
        public double X1 { get; set; } = 1.0;   // end of Robust segment
        public double X2 { get; set; } = 2.0;   // end of Stable segment
        public double X3 { get; set; } = 10.0;  // end of Reactive segment

        // Top band reference (chart top)
        public double YTop { get; set; } = 50.0;

        // Robust roof: (0,25) → (1,10)
        public double Robust_X0 { get; set; } = 0.0;
        public double Robust_Y0 { get; set; } = 25.0;
        public double Robust_X1 { get; set; } = 1.0;
        public double Robust_Y1 { get; set; } = 10.0;

        // Stable/Reactive: (1,50) → (2,25)
        public double SR_X0 { get; set; } = 1.0;
        public double SR_Y0 { get; set; } = 50.0;
        public double SR_X1 { get; set; } = 2.0;
        public double SR_Y1 { get; set; } = 25.0;

        // Reactive/Overloaded: (2,50) → (10,25)
        public double RO_X0 { get; set; } = 2.0;
        public double RO_Y0 { get; set; } = 50.0;
        public double RO_X1 { get; set; } = 10.0;
        public double RO_Y1 { get; set; } = 25.0;

    }
}