namespace ContainerProject
{
    public class GasContainer : Container, IHazardNotifier
    {
        public double Pressure { get; }

        public GasContainer(double tare, double height, double depth, double maxPayload, double pressure)
            : base('G', tare, height, depth, maxPayload)
        {
            Pressure = pressure;
        }
        
        public override void LoadCargo(double mass)
        {
            try
            {
                base.LoadCargo(mass); 
            }
            catch (OverfillException ex)
            {
                NotifyHazard($"Overfill in {SerialNumber}: {ex.Message}");
                throw; 
            }
        }

        public override void Empty()
        {
            CargoMass *= 0.05; 
        }

        public void NotifyHazard(string message)
        {
            Console.WriteLine($"GAS WARNING! {message}");
        }
        
    }
}