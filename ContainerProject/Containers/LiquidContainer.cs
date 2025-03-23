namespace ContainerProject.Containers
{
    public class LiquidContainer : Container, IHazardNotifier
    {
        public bool IsHazardous { get; }

        public LiquidContainer(double tare, double height, double depth, double maxPayload, bool isHazardous) 
            : base('L', tare, height, depth, maxPayload)
        {
            IsHazardous = isHazardous;
        }

        public override void LoadCargo(double mass)
        {
            //different limit depending 
            double limit = IsHazardous ? MaxPayload * 0.5 : MaxPayload * 0.9;
            
            //report the attempt to perform a dangerous operation
            if (CargoMass + mass > limit)
            {
                NotifyHazard($"Tried to overload! Container: {SerialNumber}");
                throw new OverfillException($"Cannot load {mass}kg");
            }
        
            base.LoadCargo(mass); 
        }

        public void NotifyHazard(string message)
        {
            Console.WriteLine($"DANGER! {message}");
        }
        
    }
}

