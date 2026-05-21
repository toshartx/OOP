using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Export
{
    internal class CsvExporter : IExporter
    {
        public void Export(List<DeliveryResult> results, string filePath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Transport,TotalCost,TotalTime,Distance,CargosCount");
            foreach (var r in results)
            {
                sb.AppendLine($"{r.Request.DelTransport?.Name},{r.TotalCost:F2},{r.TotalTime:F2},{r.Request.Distance},{r.Request.Cargos.Count}");
            }
            File.WriteAllText(filePath, sb.ToString());
        }
    }
}
