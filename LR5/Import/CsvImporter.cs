using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Import
{
    public class CsvImporter : BaseImporter
    {
        public CsvImporter(TransportFactoryRegistry factoryRegistry) : base(factoryRegistry) { }

        public override bool CanHandle(string path) => Path.GetExtension(path).ToLower() == ".csv";

        public override DeliveryRequest Read(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var dto = new ImportData { Cargos = [] };

            var header = lines[0].Split(',');
            bool hasDistance = header.Contains("Distance");

            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Split(',');
                if (parts.Length >= 3)
                {
                    dto.Cargos.Add(new Cargo
                    {
                        Name = parts[0],
                        Mass = double.Parse(parts[1]),
                        CostPerKg = double.Parse(parts[2])
                    });

                    if (i == 1 && hasDistance && parts.Length > 3)
                    {
                        dto.Distance = double.Parse(parts[3]);
                    }
                }
            }

            return CreateRequest(dto);
        }
    }
}
