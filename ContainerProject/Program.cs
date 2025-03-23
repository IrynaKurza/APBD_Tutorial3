using System.Diagnostics;
using ContainerProject.Containers;

namespace ContainerProject
{
    class Program
    {
        private static readonly List<Ship> Ships = new List<Ship>();
        private static readonly List<Container> Containers = new List<Container>();

        
        //===MENU===
        static void Main()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Container Management System ===");
                Console.WriteLine("1. Create New Ship");
                Console.WriteLine("2. Create New Container");
                Console.WriteLine("3. Load Container to Ship");
                Console.WriteLine("4. Transfer Container Between Ships");
                Console.WriteLine("5. List All Ships");
                Console.WriteLine("6. List All Containers");
                Console.WriteLine("7. Show Ship Details");
                Console.WriteLine("8. Load Cargo into Container");
                Console.WriteLine("9. Empty Container");
                Console.WriteLine("10. Remove Container from Ship");
                Console.WriteLine("11. Show Container Details");
                Console.WriteLine("12. Load Multiple Containers to Ship");
                Console.WriteLine("13. Replace Container on Ship");
                Console.WriteLine("14. Exit\n");
                Console.Write("Select option: ");

                var input= Console.ReadLine()?.Trim() ?? "";
                
                try
                {
                    switch (input)
                    {
                        case "1": CreateShip(); break;
                        case "2": CreateContainer(); break;
                        case "3": LoadContainer(); break;
                        case "4": TransferContainer(); break;
                        case "5": ListShips(); break;
                        case "6": ListContainers(); break;
                        case "7": ShowShipDetails(); break;
                        case "8": LoadCargoIntoContainer(); break;
                        case "9": EmptyContainer(); break;
                        case "10": RemoveContainerFromShip(); break;
                        case "11": ShowContainerDetails(); break;
                        case "12": LoadContainers(); break;
                        case "13": ReplaceContainerOnShip(); break;
                        case "14": return; 
                        default: Console.WriteLine("Invalid option!"); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                
                
                Console.WriteLine("\n⭕ Press any key to continue...");
                Console.ReadKey();
            }
        }
        

        //===CREATING===
        
        //ship creation
        static void CreateShip()
        {
            Console.WriteLine("\n=== Create New Ship ===");
    
            //speed validation
            double speed = 0;
            bool validSpeed = false;
            
            while (!validSpeed)
            {
                speed = GetValidDouble("Enter max speed (in knots): ");
                
                if (speed > 0)
                {
                    validSpeed = true;
                }
                else
                {
                    Console.WriteLine("\n❌ Speed must be greater than 0!");
                }
            }

            //capacity validation
            int capacity = 0;
            bool validCapacity = false;
            
            while (!validCapacity)
            {
                capacity = GetValidInt("Enter max container capacity: ");
                if (capacity > 0)
                {
                    validCapacity = true;
                }
                else
                {
                    Console.WriteLine("\n❌ Capacity must be greater than 0!");
                }
            }

            //weight validation
            double weight = 0;
            bool validWeight = false;
            
            while (!validWeight)
            {
                weight = GetValidDouble("Enter max weight in tons: ");
                if (weight > 0)
                {
                    validWeight = true;
                }
                else
                {
                    Console.WriteLine("\n❌ Weight must be greater than 0!");
                }
            }

            Ships.Add(new Ship(speed, capacity, weight));
            Console.WriteLine("\n✅ Ship was created successfully!");
        }
        
        
        //container creation
        static void CreateContainer()
        {
            try
            {
                Console.WriteLine("\nSelect container type:");
                Console.WriteLine("1. Liquid");
                Console.WriteLine("2. Gas");
                Console.WriteLine("3. Refrigerated");
        
                string typeChoice;
                while (true)
                {
                    Console.Write("\nSelect container type: ");
                    typeChoice = Console.ReadLine()?.Trim() ?? "";
            
                    if (typeChoice == "1" || typeChoice == "2" || typeChoice == "3") break;
            
                    Console.Write("\n❌ Invalid choice! Please enter 1-3 ");
                }

                double tare = GetValidDouble("\nTare weight (kg): ");
                double height = GetValidDouble("Height (cm): ");
                double depth = GetValidDouble("Depth (cm): ");
                double payload = GetValidDouble("Max payload (kg): ");

                Container container = typeChoice switch
                {
                    "1" => CreateLiquidContainer(tare, height, depth, payload),
                    "2" => CreateGasContainer(tare, height, depth, payload),
                    "3" => CreateRefrigeratedContainer(tare, height, depth, payload),
                    _ => throw new UnreachableException()
                };

                Containers.Add(container);
                Console.WriteLine($"\n✅ Container was created successfully! Container: {container.SerialNumber}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }
        }
        
        //liquid container
        static LiquidContainer CreateLiquidContainer(double tare, double height, double depth, double payload)
        {
            bool hazardous = GetValidBool("Is hazardous (true/false): ");
            return new LiquidContainer(tare, height, depth, payload, hazardous);
        }

        //gas container
        static GasContainer CreateGasContainer(double tare, double height, double depth, double payload)
        {
            double pressure = GetValidDouble("Enter pressure (atm): ");
            return new GasContainer(tare, height, depth, payload, pressure);
        }

        //refrigerated container
        static RefrigeratedContainer CreateRefrigeratedContainer(double tare, double height, double depth, double payload)
        {
            string productType = GetProductTypeFromUser();
    
            double minTemp = RefrigeratedContainer.ProductMinTemperatures[productType];
            double temp = GetTemperatureForProduct(productType, minTemp);
    
            return new RefrigeratedContainer(tare, height, depth, payload, productType, temp);
        }

        
        //===CONTAINER FUNCTIONS===
        
        //load container
        static void LoadContainer()
        {
            try
            {
                var container = SelectContainer();
                var ship = SelectShip("Select ship to load to:");
                
                ship.AddContainer(container);
                Console.WriteLine("\n✅ Container loaded successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
        
        
        //load containers (in bulk)
        static void LoadContainers()
        {
            try
            {
                var ship = SelectShip("Select ship to load to:");
                var containers = SelectMultipleContainers();
                
                ship.AddContainers(containers);
                Console.WriteLine("\n✅ Containers loaded successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }
        }
        
        
        //replace container
        static void ReplaceContainerOnShip()
        {
            try
            {
                var ship = SelectShip();
                
                Console.WriteLine("\n=== Containers On Ship ===");
                if (ship.Containers.Count == 0)
                {
                    Console.WriteLine("\n❌ No containers on this ship!");
                    return;
                }
        
                for (int i = 0; i < ship.Containers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {ship.Containers[i].SerialNumber} ({GetContainerType(ship.Containers[i])})");
                }
        
                int oldIndex = GetValidInt($"Select container to replace (1-{ship.Containers.Count}): ") - 1;
                var oldContainer = ship.Containers[oldIndex];
                
                Console.WriteLine("\n=== Available Containers ===");
                List<Container> availableContainers = new List<Container>();
                foreach (var container in Containers)
                {
                    if (container.CurrentShip == null)
                    {
                        availableContainers.Add(container);
                    }
                }
        
                if (availableContainers.Count == 0)
                {
                    Console.WriteLine("\n❌ No available containers!");
                    return;
                }
        
                for (int i = 0; i < availableContainers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {availableContainers[i].SerialNumber} ({GetContainerType(availableContainers[i])})");
                }
        
                int newIndex = GetValidInt($"Select new container (1-{availableContainers.Count})") - 1;
                var newContainer = availableContainers[newIndex];
        
                ship.ReplaceContainer(oldContainer.SerialNumber, newContainer);
                Console.WriteLine("\n✅ Container replaced successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }
        }
        
        
        //transfer container
        static void TransferContainer()
        {
            var source = SelectShip("Select source ship:");
            var dest = SelectShip("Select destination ship:");
            var container = SelectContainer();

            Ship.TransferContainer(source, dest, container.SerialNumber);
            Console.WriteLine("\n✅ Transfer completed successfully!");
        }
        
        
        //load cargo into container
        static void LoadCargoIntoContainer()
        {
            try
            {
                var container = SelectContainer();
                double mass = GetValidDouble("Enter cargo mass (kg): ");

                if (container is RefrigeratedContainer rc)
                {
                    Console.WriteLine("Available products: " + 
                                      string.Join(", ", RefrigeratedContainer.ProductMinTemperatures.Keys));
                    string productType = GetProductTypeFromUser();
                    rc.LoadCargo(mass, productType);
                }
                else
                {
                    container.LoadCargo(mass); 
                }
        
                Console.WriteLine("\n✅ Cargo loaded successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }
        }

        
        //get product type for refrigerated container
        static string GetProductTypeFromUser()
        {
            var validProducts = RefrigeratedContainer.ProductMinTemperatures.Keys.ToList();
    
            Console.WriteLine("\nAvailable products:");
            foreach (var product in validProducts)
            {
                Console.WriteLine($"- {product}");
            }
            
            while (true)
            {
                Console.Write("Enter product name: ");
                string input = Console.ReadLine()?.Trim() ?? "";
        
                //case-insensitive
                foreach (var validProduct in validProducts)
                {
                    if (validProduct.Equals(input, StringComparison.OrdinalIgnoreCase))
                    {
                        return validProduct; 
                    }
                }
                
                Console.WriteLine($"\n❌ '{input}' is not a valid product. Please choose from the list.");
            }
        }
        
        //get temperature for refrigerated container
        static double GetTemperatureForProduct(string productType, double minTemperature)
        {
            while (true)
            {
                double temperature = GetValidDouble(
                    $"Enter temperature (°C) for {productType} (min {minTemperature}°C): ");
        
                if (temperature >= minTemperature)
                {
                    return temperature;
                }
        
                Console.WriteLine($"\n❌ Temperature must be at least {minTemperature}°C!");
            }
        }
        
        //empty container
        static void EmptyContainer()
        {
            var container = SelectContainer();
            container.Empty();
            Console.WriteLine("\n✅ Container emptied successfully!");
        }
        
        //remove container from ship
        static void RemoveContainerFromShip()
        {
            try
            {
                var ship = SelectShip("Select ship to remove from:");
        
                Console.WriteLine("\n=== Containers On Ship ===");
                if (ship.Containers.Count == 0)
                {
                    Console.WriteLine("\n❌ No containers on this ship!");
                    return;
                }
                
                for (int i = 0; i < ship.Containers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {ship.Containers[i].SerialNumber} ({GetContainerType(ship.Containers[i])})");
                }
        
                int index = GetValidInt($"Select container to remove (1-{ship.Containers.Count}): ") - 1;
                var container = ship.Containers[index];
        
                ship.RemoveContainer(container.SerialNumber);
                container.CurrentShip = null;
                Console.WriteLine("\n✅ Container removed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }
        }
        

        
        //===PRINTING DETAILS===
        
        //list ships
        static void ListShips()
        {
            Console.WriteLine("\n=== Registered Ships ===");
            for (int i = 0; i < Ships.Count; i++)
            {
                Console.WriteLine($"Ship {i + 1}:");
                Console.WriteLine($"- Speed: {Ships[i].MaxSpeed}kts");
                Console.WriteLine($"- Max Containers: {Ships[i].MaxContainerCount}");
                Console.WriteLine($"- Max Weight: {Ships[i].MaxWeightKg / 1000}t");
                Console.WriteLine($"- Current Containers: {Ships[i].Containers.Count}");
                Console.WriteLine();
            }
        }
        
        //list containers
        static void ListContainers()
        {
            Console.WriteLine("\n=== Available Containers ===");
            for (int i = 0; i < Containers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Containers[i].SerialNumber} - {GetContainerType(Containers[i])}");
            }
        }

        //ship details
        static void ShowShipDetails()
        {
            Ship ship = SelectShip();
            Console.WriteLine($"\nShip [Max Speed: {ship.MaxSpeed} knots]");
            Console.WriteLine($"Capacity: {ship.Containers.Count}/{ship.MaxContainerCount} containers");
            Console.WriteLine($"Weight: {ship.GetTotalWeight()/1000:F1}t/{ship.MaxWeightKg/1000:F1}t");
            Console.WriteLine("Containers onboard:");
    
            foreach (Container container in ship.Containers)
            {
                Console.WriteLine($"- {container.SerialNumber} ({GetContainerType(container)})");
            }
        }
        
        //container details
        static void ShowContainerDetails()
        {
            var container = SelectContainer();
            Console.WriteLine($"\n=== Container {container.SerialNumber} ===");
            Console.WriteLine($"Type: {GetContainerType(container)}");
            Console.WriteLine($"Cargo Mass: {container.CargoMass}kg");
            Console.WriteLine($"Max Payload: {container.MaxPayload}kg");
            Console.WriteLine($"Tare Weight: {container.TareWeight}kg");
            Console.WriteLine($"Total Weight: {container.TareWeight + container.CargoMass}kg");
            Console.WriteLine($"Height: {container.Height}cm");
            Console.WriteLine($"Depth: {container.Depth}cm");

            if (container is RefrigeratedContainer rc)
            {
                Console.WriteLine($"Product Type: {rc.StoredProductType}");
                Console.WriteLine($"Temperature: {rc.Temperature}°C");
            }
            else if (container is LiquidContainer lc)
            {
                Console.WriteLine($"Hazardous: {lc.IsHazardous}");
            }
            else if (container is GasContainer gc)
            {
                Console.WriteLine($"Pressure: {gc.Pressure} atm");
            }
        }
        
        
        //===SELECTING===
        
        //select container
        static Container SelectContainer()
        {
            if (Containers.Count == 0)
                throw new InvalidOperationException("No containers available!");
            
            Console.WriteLine("\n=== Available Containers ===");
            for (int i = 0; i < Containers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Containers[i].SerialNumber} " +
                                  $"({GetContainerType(Containers[i])})");
            }
    
            int index = GetValidInt($"Select container (1-{Containers.Count}): ") - 1;
            return Containers[index];
        }
        
        //select multiple containers
        static List<Container> SelectMultipleContainers()
        {
            List<Container> selected = new List<Container>();
            var availableContainers = new List<Container>();
            
            foreach (Container container in Containers)
            {
                if (container.CurrentShip == null)
                {
                    availableContainers.Add(container);
                }
            }

            if (availableContainers.Count == 0)
            {
                throw new InvalidOperationException("No available containers to load!");
            }

            Console.WriteLine("\n=== Available Containers ===");
            for (int i = 0; i < availableContainers.Count; i++)
            {
                Console.WriteLine(
                    $"{i + 1}. {availableContainers[i].SerialNumber} ({GetContainerType(availableContainers[i])})");
            }

            while (true)
            {
                Console.Write("\nSelected containers: ");
                foreach (Container container in selected)
                {
                    Console.Write(container.SerialNumber + " ");
                }

                Console.WriteLine();

                Console.Write("Enter container number (0 to finish): ");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == 0) break;
                    if (choice > 0 && choice <= availableContainers.Count)
                    {
                        Container container = availableContainers[choice - 1];
                        
                        bool alreadyAdded = false;
                        foreach (Container c in selected)
                        {
                            if (c == container)
                            {
                                alreadyAdded = true;
                                break;
                            }
                        }

                        if (alreadyAdded)
                        {
                            Console.WriteLine("❌ Container already selected!");
                        }
                        else
                        {
                            selected.Add(container);
                            Console.WriteLine($"✅ Added {container.SerialNumber}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("❌ Invalid selection");
                    }
                }
                else
                {
                    Console.WriteLine("❌ Invalid number");
                }
            }

            return selected;
        }


        //select ship
        static Ship SelectShip(string prompt = "Select ship:")
        {
            if (Ships.Count == 0)
                throw new InvalidOperationException("No ships available!");
            
            Console.WriteLine("\n=== Available Ships ===");
            for (int i = 0; i < Ships.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Ship {i + 1} [Speed: {Ships[i].MaxSpeed}kts, " +
                                  $"Max Containers: {Ships[i].MaxContainerCount}, " +
                                  $"Max Weight: {Ships[i].MaxWeightKg / 1000}t]");
            }
    
            int index = GetValidInt($"{prompt} (1-{Ships.Count})") - 1;
            return Ships[index];
        }

        //select container type
        static string GetContainerType(Container container)
        {
            return container switch
            {
                LiquidContainer => "Liquid",
                GasContainer => "Gas",
                RefrigeratedContainer => "Refrigerated",
                _ => "Unknown"
            };
        }
        
        
        //helper functions
        static double GetValidDouble(string prompt)
        {
            double result = 0;
            bool isValid = false;

            while (!isValid)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                
                if (double.TryParse(input, out result))
                {
                    isValid = true;
                }
                else
                {
                    Console.WriteLine("\n❌ Invalid input. Please enter a valid number (e.g., 123.45)");
                }
            }

            return result;
        }

        
        static int GetValidInt(string prompt)
        {
            int result = 0;
            bool isValid = false;

            while (!isValid)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                
                if (int.TryParse(input, out result))
                {
                    isValid = true; 
                }
                else
                {
                    Console.WriteLine("\n❌ Invalid input. Please enter a whole number (e.g., 5)");
                }
            }

            return result;
        }

        static bool GetValidBool(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = (Console.ReadLine()?.Trim() ?? "").ToLower();
        
                if (input == "true" || input == "1") return true;
                if (input == "false" || input == "0") return false;
        
                Console.WriteLine("\n❌ Invalid input. Please enter 'true' or 'false'");
            }
        }
        
        
        
    }
}