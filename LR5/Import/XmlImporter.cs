using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;
using System.Xml.Serialization;

namespace LR5.Import
{
    class XmlImporter : BaseImporter
    {
        public XmlImporter(TransportFactoryRegistry factoryRegistry) : base(factoryRegistry) { }

        public override bool CanHandle(string extension) => extension == ".xml";

        public override DeliveryRequest Read(string filePath)
        {
            var xml = File.ReadAllText(filePath);
            var serializer = new XmlSerializer(typeof(ImportData));

            using var reader = new StringReader(xml);
            var dto = (ImportData)serializer.Deserialize(reader)!;

            return CreateRequest(dto!);
        }
    }
}
