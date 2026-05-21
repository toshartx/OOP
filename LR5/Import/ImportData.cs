using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LR5.Models;

namespace LR5.Import
{
    [XmlRoot("DeliveryRequest")]
    public class ImportData
    {
        public List<Cargo>? Cargos { get; set; }
        public string? TransportType { get; set; }
        public double Distance { get; set; }
    }
}
