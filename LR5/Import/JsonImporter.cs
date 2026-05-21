using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Import
{
    public class JsonImporter : BaseImporter
    {
        public JsonImporter(TransportFactoryRegistry factoryRegistry) : base(factoryRegistry) {}

        public override bool CanHandle(string path)
        {
            return Path.GetExtension(path).ToLower() == ".json";
        }

        public override DeliveryRequest Read(string path)
        {
            string json = File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<ImportData>(json);

            Console.WriteLine($"Cargos count: {data?.Cargos?.Count ?? 0}"); // отладка
            Console.WriteLine($"TransportType: {data?.TransportType}"); // отладка
            Console.WriteLine($"Distance: {data?.Distance}"); // отладка

            return CreateRequest(data!);
        }
    }
}
