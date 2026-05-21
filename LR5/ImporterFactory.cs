using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Import;
using LR5.Models;

namespace LR5
{
    internal class ImporterFactory
    {
        private readonly List<BaseImporter> _importers;

        public ImporterFactory(TransportFactoryRegistry factoryRegistry)
        {
            _importers = new List<BaseImporter>
        {
            new JsonImporter(factoryRegistry),
            new XmlImporter(factoryRegistry),
            new CsvImporter(factoryRegistry)
        };
        }

        public BaseImporter GetImporter(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();
            return _importers.FirstOrDefault(i => i.CanHandle(extension))
                ?? throw new NotSupportedException($"Format {extension} not supported");
        }

        public DeliveryRequest Import(string filePath)
        {
            var importer = GetImporter(filePath);
            return importer.Read(filePath);
        }
    }
}
