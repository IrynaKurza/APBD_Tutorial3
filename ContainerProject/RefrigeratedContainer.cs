namespace ContainerProject
{
    public class RefrigeratedContainer : Container
    {
        private static readonly Dictionary<string, double> ProductMinTemperatures = new()
        {
            {"Bananas", 13.3},    {"Chocolate", 18},   {"Fish", 2},
            {"Meat", -15},        {"Ice cream", -18},  {"Frozen pizza", -30},
            {"Cheese", 7.2},     {"Sausages", 5},     {"Butter", 20.5},
            {"Eggs", 19}
        };
        
        private string StoredProductType { get; }
        public double Temperature { get; private set; }

        public RefrigeratedContainer(double tare, double height, double depth, double maxPayload, string productType, double initialTemperature)
            : base('C', tare, height, depth, maxPayload)
        {
            if (!ProductMinTemperatures.ContainsKey(productType))
                throw new ArgumentException($"Invalid product type: {productType}");
            
            if (initialTemperature < ProductMinTemperatures[productType])
                throw new ArgumentException(
                    $"Temperature too low for {productType}. " +
                    $"Minimum: {ProductMinTemperatures[productType]}°C");

            StoredProductType = productType;
            Temperature = initialTemperature;
        }

        public void SetTemperature(double newTemperature)
        {
            if (newTemperature < ProductMinTemperatures[StoredProductType])
                throw new ArgumentException(
                    $"Temperature {newTemperature}°C is too low for {StoredProductType}. " +
                    $"Minimum: {ProductMinTemperatures[StoredProductType]}°C");

            Temperature = newTemperature;
        }
        
    }
}
