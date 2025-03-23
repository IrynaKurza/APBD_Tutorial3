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
                throw new InvalidOperationException($"Container {container.SerialNumber} is already on another ship!");
        
            ValidateCapacity(container);
            Containers.Add(container);
            container.CurrentShip = this;
        }

        //load a list of containers onto a ship
        public void AddContainers(List<Container> containers)
        {
            foreach (var container in containers)
            {
                if (container.CurrentShip != null)
                    throw new InvalidOperationException(
                        $"Container {container.SerialNumber} is already on another ship!"
                    );
            
                ValidateCapacity(container);
                Containers.Add(container);
                container.CurrentShip = this;
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

        //replace a container on the ship with a given number with another container
        public void ReplaceContainer(string oldSerialNum, Container newContainer)
        {
            if (newContainer.CurrentShip != null)
                throw new InvalidOperationException($"Container {newContainer.SerialNumber} is already on another ship!");
    
            ValidateCapacity(newContainer); 
            RemoveContainer(oldSerialNum);
            
            Containers.Add(newContainer);
            newContainer.CurrentShip = this;
        }

        //transfer between ships
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
        
        //helper functions
        private void ValidateCapacity(Container container)
        {
            if (Containers.Count >= MaxContainerCount)
                throw new OverfillException(
                    $"Ship can only carry {MaxContainerCount} containers!");

            if (GetTotalWeight() + container.TareWeight + container.CargoMass > MaxWeightKg)
                throw new OverfillException(
                    $"Total weight would exceed {(MaxWeightKg/1000):F1}t limit!");
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