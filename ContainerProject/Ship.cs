using ContainerProject.Containers;

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

        //load a container onto a ship
        public void AddContainer(Container container)
        {
            if (container.CurrentShip != null)
                throw new InvalidOperationException($"\n❌ Container {container.SerialNumber} is already on another ship!");
            
            ValidateCapacity(container);
            
            Containers.Add(container);
            
            container.CurrentShip = this;

            Console.WriteLine($"\n✅ Container {container.SerialNumber} added successfully to the ship!");
        }

        //load a list of containers onto a ship
        public void AddContainers(List<Container> containers)
        {
            foreach (var container in containers)
            {
                if (container.CurrentShip != null)
                    throw new InvalidOperationException(
                        $"\n❌ Container {container.SerialNumber} is already on another ship!"
                    );
            
                ValidateCapacity(container);
            }
            
            foreach (var container in containers)
            {
                Containers.Add(container);
                container.CurrentShip = this;
            }
        }

        //remove container by serial number
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
                throw new ArgumentException($"\n❌ Container {serialNumber} not found!");
            
            Containers.Remove(toRemove);
            
            toRemove.CurrentShip = null;

            Console.WriteLine($"\n✅ Container {serialNumber} removed successfully from the ship!");
        }

        //replace a container on the ship with a given number with another container
        public void ReplaceContainer(string oldSerialNum, Container newContainer)
        {
            if (newContainer.CurrentShip != null)
                throw new InvalidOperationException($"\n❌ Container {newContainer.SerialNumber} is already on another ship!");
    
            ValidateCapacity(newContainer); 
            RemoveContainer(oldSerialNum);
            
            Containers.Add(newContainer);
            newContainer.CurrentShip = this;
        }

        //transfer between ships
        public static void TransferContainer(Ship source, Ship destination, string serialNumber)
        {
            Console.WriteLine($"\nAttempting to transfer container {serialNumber}...");

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
                throw new ArgumentException($"\n❌ Container {serialNumber} not found in source ship!");

            Console.WriteLine($"Container found: {toTransfer.SerialNumber}, CurrentShip: {toTransfer.CurrentShip}");

            try
            {
                destination.ValidateCapacity(toTransfer);

                Console.WriteLine("Removing container from source ship...");
                source.RemoveContainer(serialNumber);

                Console.WriteLine("Adding container to destination ship...");
                destination.AddContainer(toTransfer);

                Console.WriteLine($"\n✅ Container {serialNumber} transferred successfully!");
            }
            catch (OverfillException ex)
            {
                throw new InvalidOperationException($"\n❌ Transfer failed: {ex.Message}");
            }
        }
        
        //helper functions
        private void ValidateCapacity(Container container)
        {
            if (Containers.Count >= MaxContainerCount)
                throw new OverfillException(
                    $"❌ Ship can't carry more than {MaxContainerCount} containers " +
                    $"(Current: {Containers.Count})");

            double newTotal = GetTotalWeight() + container.TareWeight + container.CargoMass;
            if (newTotal > MaxWeightKg)
                throw new OverfillException(
                    $"❌ Total weight would be {newTotal/1000:F1}t " +
                    $"(Max: {MaxWeightKg/1000:F1}t)");
        }

        public double GetTotalWeight()
        {
            double total = 0;
            foreach (var container in Containers)
            {
                total += container.TareWeight + container.CargoMass;
            }
            return total;
        }
        
    }
}