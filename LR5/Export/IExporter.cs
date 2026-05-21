using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Export
{
    public interface IExporter
    {
        void Export(List<DeliveryResult> results, string filePath);
    }
}
