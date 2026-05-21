using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Import
{
    class JsonImporter : BaseImporter
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

            return CreateRequest(data!);
        }
    }
}
