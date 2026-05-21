using System.Text.Json;
using System.Xml.Linq;
using LR5.Export;
using LR5.Filter;
using LR5.Models;
using LR5.Sort;

namespace LR5;

class Program
{
    static void CreateTestFiles()
    {
        // JSON файл
        var jsonData = new
        {
            Cargos = new[] {
            new { Name = "Ноутбуки", Mass = 500.0, CostPerKg = 10.0 },
            new { Name = "Телефоны", Mass = 200.0, CostPerKg = 15.0 }
        },
            TransportType = "Самолёт",
            Distance = 1500.0
        };
        var json = JsonSerializer.Serialize(jsonData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("test.json", json);

        Console.WriteLine("   Создан test.json");

        // XML файл
        var xmlDoc = new XDocument(
            new XElement("DeliveryRequest",
                new XElement("Cargos",
                    new XElement("Cargo",
                        new XElement("Name", "Нефть"),
                        new XElement("Mass", 10000),
                        new XElement("CostPerKg", 0.5)
                    ),
                    new XElement("Cargo",
                        new XElement("Name", "Газ"),
                        new XElement("Mass", 5000),
                        new XElement("CostPerKg", 0.3)
                    )
                ),
                new XElement("Distance", 500)
            )
        );
        xmlDoc.Save("test.xml");
        Console.WriteLine("   Создан test.xml");

        // CSV файл
        var csvLines = new[] {
        "Name,Mass,CostPerKg,Distance",
        "Книги,200,2,31",
        "Мебель,800,3,42",
        "Игрушки,150,4,57"
    };
        File.WriteAllLines("test.csv", csvLines);
        Console.WriteLine("   Создан test.csv");
    }
    static void Main(string[] args)
    {
        CreateTestFiles();

        var registry = new TransportFactoryRegistry();
        var controller = new DeliveryController(registry);

        Console.WriteLine("1. JSON (Самолёт):");
        var jsonResults = controller.ProcessDelivery("test.json");
        foreach (var r in jsonResults)
            Console.WriteLine($"{r}");

        Console.WriteLine("\n2. XML (автовыбор):");
        var xmlResults = controller.ProcessDelivery("test.xml");
        foreach (var r in xmlResults)
            Console.WriteLine($"{r}");

        Console.WriteLine("\n3. CSV:");
        var csvResults = controller.ProcessDelivery("test.csv");
        foreach (var r in csvResults)
            Console.WriteLine($"{r}");

        // Собираем ВСЕ результаты вместе
        var allResults = new List<DeliveryResult>();
        allResults.AddRange(jsonResults);
        allResults.AddRange(xmlResults);
        allResults.AddRange(csvResults);

        // 4. Фильтрация (используем ТВОЙ CostFilter)
        Console.WriteLine("\n4. Фильтрация (стоимость <= 6000):");
        var costFilter = new CostFilter(3000, 10000);
        var filteredResults = new List<DeliveryResult>();
        foreach (var r in allResults)
        {
            if (costFilter.Apply(r))
                filteredResults.Add(r);
        }
        foreach (var r in filteredResults)
            Console.WriteLine($"{r}");
        if (filteredResults.Count == 0) Console.WriteLine("   (нет результатов)");

        // 5. Сортировка по стоимости (используем ТВОЙ SortByCost)
        Console.WriteLine("\n5. Сортировка по стоимости (возрастание):");
        var sortByCost = new SortByCost(); // по умолчанию false = возрастание
        var sortedResults = sortByCost.Sort(allResults);
        foreach (var r in sortedResults)
            Console.WriteLine($"{r}");

        // 6. Комбинированная сортировка (сначала по стоимости, потом по имени)
        Console.WriteLine("\n6. Комбинированная сортировка (стоимость + имя):");
        // Для комбинации используем ThenBy из твоего SortHandler
        var combinedSort = new SortByTransportName().ThenBy(new SortByCost(true));
        var combinedResults = combinedSort.Sort(allResults);
        foreach (var r in combinedResults)
            Console.WriteLine($"{r}");

        // 7. Экспорт в JSON
        Console.WriteLine("\n7. Экспорт в JSON:");
        var jsonExporter = new JsonExporter();
        jsonExporter.Export(allResults, "output.json");
        Console.WriteLine("   Сохранено output.json");

        // 8. Экспорт в CSV
        Console.WriteLine("\n8. Экспорт в CSV:");
        var csvExporter = new CsvExporter();
        csvExporter.Export(allResults, "output.csv");
        Console.WriteLine("   Сохранено output.csv");

        // 9. Экспорт с шифрованием
        Console.WriteLine("\n9. Экспорт с шифрованием:");
        var encryptedExporter = new EncryptionDecorator(new JsonExporter(), "secret123");
        encryptedExporter.Export(allResults, "output_encrypted.dat");
        Console.WriteLine("   Сохранено output_encrypted.dat");

        // 10. Экспорт со сжатием
        Console.WriteLine("\n10. Экспорт со сжатием (ZIP):");
        var zipPath = "output.zip";
        if (File.Exists(zipPath)) File.Delete(zipPath);
        var zipExporter = new ZipDecorator(new JsonExporter());
        zipExporter.Export(allResults, "output");
        Console.WriteLine("   Сохранено output.zip");

        // 11. Экспорт с шифрованием и сжатием
        Console.WriteLine("\n11. Экспорт с шифрованием и сжатием:");
        var bothPath = "output_both.zip";
        if (File.Exists(bothPath)) File.Delete(bothPath);
        var bothExporter = new ZipDecorator(new EncryptionDecorator(new JsonExporter(), "secret123"));
        bothExporter.Export(allResults, "output_both");
        Console.WriteLine("   Сохранено output_both.zip");

        Console.WriteLine("\n=== ТЕСТИРОВАНИЕ ЗАВЕРШЕНО ===");
    }
}
