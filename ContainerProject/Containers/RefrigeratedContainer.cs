namespace ContainerProject.Containers
{
    public class RefrigeratedContainer : Container
    {
        public static readonly Dictionary<string, double> ProductMinTemperatures = new()
        {
            {"Bananas", 13.3},    {"Chocolate", 18},   {"Fish", 2},
            {"Meat", -15},        {"Ice cream", -18},  {"Frozen pizza", -30},
            {"Cheese", 7.2},     {"Sausages", 5},     {"Butter", 20.5},
            {"Eggs", 19}
        };
        
        public string StoredProductType { get; }
        public double Temperature { get; private set; }

        public RefrigeratedContainer(double tare, double height, double depth, double maxPayload, string productType, double initialTemperature)
            : base('C', tare, height, depth, maxPayload)
        {
            if (!ProductMinTemperatures.TryGetValue(productType, out var minTemp))
                throw new ArgumentException($"Invalid product: {productType}");

            //temperature of container can't be lower than temperature required by a given type of product
            if (initialTemperature < minTemp)
                throw new ArgumentException(
                    $"Temperature {initialTemperature}°C is too low for {productType}. " +
                    $"Minimum: {minTemp}°C");

            StoredProductType = productType;
            Temperature = initialTemperature;
        }

        
        //if container's temperature will change later on
        public void SetTemperature(double newTemperature)
        {
            if (newTemperature < ProductMinTemperatures[StoredProductType])
                throw new ArgumentException(
                    $"Temperature {newTemperature}°C is too low for {StoredProductType}. " +
                    $"Minimum: {ProductMinTemperatures[StoredProductType]}°C");

            Temperature = newTemperature;
        }
        
        //loading cargo
        public void LoadCargo(double mass, string productType)
        {
            if (productType != StoredProductType)
                throw new ArgumentException($"This container is for {StoredProductType}, not {productType}!");

            if (CargoMass + mass > MaxPayload)
                throw new OverfillException($"Too heavy! Max: {MaxPayload}kg");

            CargoMass += mass;
        }
        
        
        
    }
}
