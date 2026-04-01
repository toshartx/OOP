
// =========================================================
// Файл: Infrastructure.cs
// Описание: Имитация работы с БД и внешними сервисами.
// =========================================================

using System.Runtime.CompilerServices;

namespace Orders;

/// <summary>
/// Интерфейс для проверки наличия в кэше
/// </summary>
interface ICacheReader
{
    bool Contains(string id);
}

/// <summary>
/// Интерфейс для добавления в кэш
/// </summary>
interface ICacheWriter
{
    void Add(string id);
}

/// <summary>
/// Полный интерфейс кэш-хранилища
/// </summary>
interface ICacheStore : ICacheReader, ICacheWriter
{
    int Count { get; }
    void Clear();
}

/// <summary>
/// Хранилище кэша - отвечает только за хранение и базовые операции
/// </summary>
class MemoryCacheStore : ICacheStore
{
    private readonly HashSet<string> _cache = [];

    public int Count => _cache.Count;

    public bool Contains(string id)
    {
        return _cache.Contains(id);
    }

    public void Add(string id)
    {
        _cache.Add(id);
    }

    public void Clear()
    {
        _cache.Clear();
    }
}

/// <summary>
/// RandomSQLDatabase - имитация тяжелой базы данных.
/// </summary>
class RandomSQLDatabase
{
    public string ConnectionString { get; set; } = "random://root:password@localhost:228/shop";
    private readonly ICacheStore _cacheStore;

    public RandomSQLDatabase(ICacheStore cacheStore)
    {
        _cacheStore = cacheStore;
    }
    public void SaveOrder(Order order, double total)
    {
        if (_cacheStore.Contains(order.Id))
        {
            Console.WriteLine($"The order is already in the cache.");
            return;
        }
        
        Console.WriteLine($"Connecting to RandomSQL at {ConnectionString} ...");
        Thread.Sleep(500);

        using var file = new StreamWriter("../../../orders_db.txt", new FileStreamOptions()
        {
            Access = FileAccess.Write,
            Mode = FileMode.Append
        });
        
        var @record = $"[{DateTime.Now:yyyy-MM-ddTHH:mm:ssK}] ID: {order.Id} | Type: {order.Type} | Total: {total:F2}\n";
        file.Write(@record);
        _cacheStore.Add(order.Id);
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