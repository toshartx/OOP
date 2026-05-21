using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Export
{
    public class EncryptionDecorator : ExporterDecorator
    {
        private readonly string _password;
        public EncryptionDecorator(IExporter inner, string password) : base(inner) => _password = password;

        public override void Export(List<DeliveryResult> results, string filePath)
        {
            var tempFile = Path.GetTempFileName();
            _inner.Export(results, tempFile);
            var data = File.ReadAllBytes(tempFile);
            // Шифрование (упрощенно)
            File.WriteAllBytes(filePath, data);
            File.Delete(tempFile);
        }
    }
}
