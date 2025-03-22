namespace ContainerProject
{
    public abstract class Container
    {
        private static int _counter = 1;
        public string SerialNumber { get; }
        public double CargoMass { get; set; }
        public double TareWeight { get; }
        public double Height { get; }
        public double Depth { get; }
        public double MaxPayload { get; }

        protected Container(char typeCode, double tare, double height, double depth, double maxPayload)
        {
            SerialNumber = $"KON-{typeCode}-{_counter++}";
            TareWeight = tare;
            Height = height;
            Depth = depth;
            MaxPayload = maxPayload;
        }
        
        public virtual void LoadCargo(double mass)
        {
            if (CargoMass + mass > MaxPayload)
                throw new OverfillException($"Too heavy! Max: {MaxPayload}kg");
            CargoMass += mass;
        }

        public virtual void Empty()
        {
            CargoMass = 0;
        }

        
    }
}