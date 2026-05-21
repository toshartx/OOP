using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Export
{
    public class JsonExporter : IExporter
    {
        public void Export(List<DeliveryResult> results, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(results, options);
            File.WriteAllText(filePath, json);
        }
    }
}
