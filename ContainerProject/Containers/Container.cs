namespace ContainerProject.Containers
{
    public abstract class Container
    {
        public string SerialNumber { get; }
        public double CargoMass { get; protected set; }
        public double TareWeight { get; }
        public double Height { get; }
        public double Depth { get; }
        public double MaxPayload { get; }
        private static int _counter;
        public Ship? CurrentShip { get; set; }

        protected Container(char typeCode, double tare, double height, double depth, double maxPayload)
        {
            SerialNumber = $"KON-{typeCode}-{++_counter}";
            
            TareWeight = tare;
            Height = height;
            Depth = depth;
            MaxPayload = maxPayload;
        }
        
        //loading container with a given mass of cargo
        public virtual void LoadCargo(double mass)
        {
            if (CargoMass + mass > MaxPayload)
                throw new OverfillException($"\n❌ Too heavy! Max: {MaxPayload}kg");
            CargoMass += mass;
        }
        
        //emptying cargo
        public virtual void Empty()
        {
            CargoMass = 0;
        }

        
    }
}