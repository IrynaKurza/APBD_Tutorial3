namespace ContainerProject
{
    public class Ship
    {
        public List<Container> Containers { get; } = new();
        public double MaxSpeed { get; }
        public int MaxContainerCount { get; }
        public double MaxWeightKg { get; }

        public Ship(double maxSpeed, int maxContainerCount, double maxWeightTons)
        {
            MaxSpeed = maxSpeed;
            MaxContainerCount = maxContainerCount;
            MaxWeightKg = maxWeightTons * 1000;
        }

        // Basic container loading
        public void AddContainer(Container container)
        {
            ValidateCapacity(container);
            Containers.Add(container);
        }

        // Load multiple containers
        public void AddContainers(List<Container> containers)
        {
            foreach (var container in containers)
            {
                ValidateCapacity(container);
                Containers.Add(container);
            }
        }

        // Remove container by serial number
        public void RemoveContainer(string serialNumber)
        {
            Container? toRemove = null;
            foreach (var container in Containers)
            {
                if (container.SerialNumber == serialNumber)
                {
                    toRemove = container;
                    break;
                }
            }

            if (toRemove == null)
                throw new ArgumentException($"Container {serialNumber} not found!");
            
            Containers.Remove(toRemove);
        }

        // Replace container
        public void ReplaceContainer(string oldSerial, Container newContainer)
        {
            RemoveContainer(oldSerial);
            AddContainer(newContainer);
        }

        // Transfer between ships
        public static void TransferContainer(Ship source, Ship destination, string serialNumber)
        {
            // Find container in source ship
            Container? toTransfer = null;
            foreach (var container in source.Containers)
            {
                if (container.SerialNumber == serialNumber)
                {
                    toTransfer = container;
                    break;
                }
            }

            if (toTransfer == null)
                throw new ArgumentException($"Container {serialNumber} not found in source ship!");

            try
            {
                destination.AddContainer(toTransfer);
                source.RemoveContainer(serialNumber);
            }
            catch (OverfillException ex)
            {
                throw new InvalidOperationException($"Transfer failed: {ex.Message}");
            }
        }

        // Print ship info
        public void PrintShipInfo()
        {
            Console.WriteLine($"Ship [Max Speed: {MaxSpeed} knots]");
            Console.WriteLine($"Capacity: {Containers.Count}/{MaxContainerCount} containers");
            Console.WriteLine($"Weight: {GetTotalWeight()/1000:F1}t/{MaxWeightKg/1000:F1}t");
            Console.WriteLine("Containers onboard:");
            foreach (var container in Containers)
            {
                Console.WriteLine($"- {container.SerialNumber}");
            }
        }

        // Print container info
        public void PrintContainerInfo(string serialNumber)
        {
            foreach (var container in Containers)
            {
                if (container.SerialNumber == serialNumber)
                {
                    Console.WriteLine($"Container {serialNumber}");
                    Console.WriteLine($"Type: {GetContainerType(serialNumber)}");
                    Console.WriteLine($"Cargo Mass: {container.CargoMass}kg");
                    Console.WriteLine($"Max Payload: {container.MaxPayload}kg");
            
                    // Add refrigerated container details
                    if (container is RefrigeratedContainer rc)
                        Console.WriteLine($"Product: {rc.StoredProductType}, Temp: {rc.Temperature}°C");
            
                    return;
                }
            }
            throw new ArgumentException($"Container {serialNumber} not found in this ship!");
        }

        private void ValidateCapacity(Container container)
        {
            if (Containers.Count >= MaxContainerCount)
                throw new OverfillException(
                    $"Ship can only carry {MaxContainerCount} containers!");

            if (GetTotalWeight() + container.TareWeight + container.CargoMass > MaxWeightKg)
                throw new OverfillException(
                    $"Total weight would exceed {(MaxWeightKg/1000):F1}t limit!");
        }

        private double GetTotalWeight()
        {
            double total = 0;
            foreach (var container in Containers)
            {
                total += container.TareWeight + container.CargoMass;
            }
            return total;
        }

        private static string GetContainerType(string serialNumber)
        {
            var parts = serialNumber.Split('-');
            return parts[1] switch
            {
                "L" => "Liquid",
                "G" => "Gas",
                "C" => "Refrigerated",
                _ => "Unknown"
            };
        }
    }
}