
// =========================================================
// Файл: Infrastructure.cs
// Описание: Имитация работы с БД и внешними сервисами.
// =========================================================

using System.Runtime.CompilerServices;

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

interface IMailer
{
    public void Send(string to, string subject, string body);
}

/// <summary>
/// SmtpMailer - имитация почтового сервиса. 
/// </summary>
class SmtpMailer : IMailer
{
    public required string Server { get; set; }

    public void Send(string to, string subject, string body)
    {
        Console.WriteLine($">> Connecting to SMTP server {Server}...");
        Console.WriteLine($">> Sending EMAIL to {to}\n   Subject: {subject}\n   Body: {body}");
    }
}

class TelegramMailer : IMailer
{
    public void Send(string to, string subject, string body)
    {
        Console.WriteLine($">> Connecting to Telegram server");
        Console.WriteLine($">> Sending message to manager\n" +
            $"Subject: {subject}\n" +
            $"Body: {body}");
    }
}

class Mailer : IMailer
{
    public Mailer(string server)
    {
        smtpmailer = new() { Server = server };
        telegrammailer = new();
    }

    private readonly SmtpMailer smtpmailer;
    private readonly TelegramMailer telegrammailer;

    public void Send(string to, string subject, string body)
    {
        smtpmailer.Send(to, subject, body);
        telegrammailer.Send(to, subject, body);
    }
}