using LR3;
using Orders;
using System.Net;

namespace Lab3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MemoryCacheStore cacheStore = new MemoryCacheStore();
            RandomSQLDatabase db = new(cacheStore);
            var order = new Order
            {
                Id = "ORD-256-X",
                Type = "Premium",
                Items = [
                    new() { Id = "1", Name = "Thermal Clips", Price = 1500 },
                    new() { Id = "2", Name = "UNATCO Pass Card", Price = 50 },
                ],
                ClientEmail = "jeevacation@gmail.com",
                Destination = new Address() { City = "Agartha", Street = "33 Thomas Street", Zip = "[REDACTED]" },
                Card = new DiscountCard(DiscountCardTypes.Silver)
            };

            // 2. Инициализация процессора
            var processor = new OrderProcessor(db);

            // 3. Обработка заказа + обработка того же заказа
            try
            {
                processor.Process(order);
                processor.Process(order);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to process order: {e.Message}");
                throw;
            }

            

            // 4. Работа с обслуживанием
            Console.WriteLine("\nTesting Warehouse Stuff:");
            var workers = new IWarehouseWorker[]{
                         new HumanManager(),
                         new RobotPacker() { Model = "George Droid" },
            };

            Warehouse.ManageWarehouse(workers);
        }
    }
}
