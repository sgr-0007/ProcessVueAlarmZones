namespace ProcessVueAlarmZones.Application.Config
{
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