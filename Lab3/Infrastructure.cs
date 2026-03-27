
// =========================================================
// Файл: Infrastructure.cs
// Описание: Имитация работы с БД и внешними сервисами.
// =========================================================

namespace Orders;

/// <summary>
/// RandomSQLDatabase - имитация тяжелой базы данных.
/// </summary>
class RandomSQLDatabase
{
    public string ConnectionString { get; set; } = "random://root:password@localhost:228/shop";

    public void SaveOrder(Order order, double total)
    {
        Console.WriteLine($"Connecting to RandomSQL at {ConnectionString} ...");
        Thread.Sleep(500);

        using var file = new StreamWriter("orders_db.txt", new FileStreamOptions()
        {
            Access = FileAccess.Write,
            Mode = FileMode.Append
        });
        var @record = $"[{DateTime.Now:yyyy-MM-ddTHH:mm:ssK}] ID: {order.Id} | Type: {order.Type} | Total: {total:F2}\n";
        file.Write(@record);

        Console.WriteLine("Order saved successfully.");
    }
}

/// <summary>
/// SmtpMailer - имитация почтового сервиса. 
/// </summary>
class SmtpMailer
{
    public required string Server { get; set; }

    public void SendHtmlEmail(string to, string subject, string body)
    {
        Console.WriteLine($">> Connecting to SMTP server {Server}...");
        Console.WriteLine($">> Sending EMAIL to {to}\n   Subject: {subject}\n   Body: {body}");
    }
}