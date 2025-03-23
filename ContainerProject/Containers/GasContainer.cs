namespace ContainerProject.Containers
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
            //exceeds payload -> returns an error
            if (CargoMass + mass > MaxPayload)
            {
                NotifyHazard($"Gas overfill in {SerialNumber}");
                throw new OverfillException($"Max {MaxPayload}kg allowed");
            }
    
            base.LoadCargo(mass); 
        }

        //when emptying - leave 5% of cargo inside the container
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