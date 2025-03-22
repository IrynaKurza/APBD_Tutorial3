
using ContainerProject;

class Program
{
    static void Main()
    {
        try
        {
            // ========== CONTAINER CREATION ==========
            Console.WriteLine("=== Creating Containers ===");
            
            // Liquid containers
            var hazardousLiquid = new LiquidContainer(1000, 200, 200, 5000, isHazardous: true);
            var milkContainer = new LiquidContainer(800, 180, 150, 4000, isHazardous: false);
            
            // Gas container
            var heliumContainer = new GasContainer(1500, 250, 200, 4000, pressure: 5);
            
            // Refrigerated containers
            var bananaContainer = new RefrigeratedContainer(2000, 300, 250, 10000, "Bananas", 14);
            var invalidFishContainer = new RefrigeratedContainer(1500, 250, 200, 8000, "Fish", 1.5);

            Console.WriteLine("Containers created successfully!\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Container creation failed: {ex.Message}\n");
        }

        // ========== CARGO OPERATIONS ==========
        Console.WriteLine("=== Cargo Operations ===");
        
        // Valid liquid container operations
        var fuelContainer = new LiquidContainer(1200, 220, 210, 6000, true);
        
        try
        {
            fuelContainer.LoadCargo(3000); // 50% of 6000 = 3000 (valid)
            Console.WriteLine("Fuel container loaded successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fuel container error: {ex.Message}");
        }

        
        // Invalid liquid container load
        try
        {
            fuelContainer.LoadCargo(3100); // Should exceed 50% limit
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fuel container overfill: {ex.Message}");
        }

        
        // Gas container operations
        var gasContainer = new GasContainer(1600, 260, 210, 5000, 6);
        try
        {
            gasContainer.LoadCargo(5001); // Should exceed max payload
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Gas container error: {ex.Message}");
        }

        
        
        // ========== SHIP OPERATIONS ==========
        Console.WriteLine("\n=== Ship Operations ===");
        var shipAlpha = new Ship(20, 5, 50); // 5 containers, 50 tons
        var shipBeta = new Ship(25, 10, 100); // 10 containers, 100 tons

        
        // Create valid containers
        var containers = new List<Container>
        {
            new LiquidContainer(1000, 200, 200, 5000, false),
            new GasContainer(1500, 250, 200, 4000, 5),
            new RefrigeratedContainer(2000, 300, 250, 10000, "Bananas", 14)
        };

        
        try
        {
            // Load multiple containers
            shipAlpha.AddContainers(containers);
            Console.WriteLine("3 containers loaded to Ship Alpha");

            // Try to exceed capacity
            for (int i = 0; i < 3; i++) // Already has 3, adding 3 more (total 6/5)
            {
                shipAlpha.AddContainer(new LiquidContainer(800, 180, 150, 3000, false));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ship Alpha error: {ex.Message}");
        }

        
        // Container transfer
        try
        {
            Console.WriteLine("\n=== Transferring Container ===");
            Ship.TransferContainer(shipAlpha, shipBeta, containers[0].SerialNumber);
            Console.WriteLine($"Transferred container {containers[0].SerialNumber} to Ship Beta");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Transfer failed: {ex.Message}");
        }

        
        // Print ship info
        Console.WriteLine("\n=== Ship Alpha Status ===");
        shipAlpha.PrintShipInfo();
        
        Console.WriteLine("\n=== Ship Beta Status ===");
        shipBeta.PrintShipInfo();

        
        // Container replacement
        try
        {
            Console.WriteLine("\n=== Replacing Container ===");
            var newContainer = new RefrigeratedContainer(1800, 280, 240, 9000, "Fish", 3);
            shipBeta.ReplaceContainer(containers[0].SerialNumber, newContainer);
            Console.WriteLine($"Replaced container {containers[0].SerialNumber}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Replacement failed: {ex.Message}");
        }

        
        // Temperature adjustment
        try
        {
            Console.WriteLine("\n=== Temperature Adjustment ===");
            var fishContainer = new RefrigeratedContainer(1500, 250, 200, 8000, "Fish", 3);
            fishContainer.SetTemperature(1); // Below minimum 2°C
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Temperature error: {ex.Message}");
        }

        
        // Print container info
        Console.WriteLine("\n=== Container Details ===");
        if (shipBeta.Containers.Count > 0)
            shipBeta.PrintContainerInfo(shipBeta.Containers[0].SerialNumber);
    }
}