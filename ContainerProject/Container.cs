namespace ContainerProject
{
    public abstract class Container
    {
        public string SerialNumber { get; }
        public double CargoMass { get; set; }
        public double TareWeight { get; }
        public double Height { get; }
        protected double Depth { get; }
        public double MaxPayload { get; }
        private static readonly Dictionary<char, int> TypeCounters = new();

        protected Container(char typeCode, double tare, double height, double depth, double maxPayload)
        {
            TypeCounters.TryGetValue(typeCode, out var count);
            TypeCounters[typeCode] = ++count;
            SerialNumber = $"KON-{typeCode}-{count}";
            
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