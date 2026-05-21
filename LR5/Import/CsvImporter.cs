using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Import
{
    class CsvImporter : BaseImporter
    {
        public CsvImporter(TransportFactoryRegistry factoryRegistry) : base(factoryRegistry) { }

        public override bool CanHandle(string extension) => extension == ".csv";

        public override DeliveryRequest Read(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var dto = new ImportData { Cargos = [] };

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
                }
            }

            return CreateRequest(dto);
        }
    }
}
