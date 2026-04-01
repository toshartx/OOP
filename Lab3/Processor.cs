// =========================================================
// Файл: Processor.cs
// Описание: Основная бизнес-логика.
// =========================================================

using Orders;

namespace LR3;

interface IOrderValidator
{
    void Validate(Order order);
}

class OrderValidator : IOrderValidator
{
    public OrderValidator() { }

    public void Validate(Order order)
    {
        if (order.Items.Length == 0)
        {
            throw new ArgumentException("order must have at least one item", nameof(order));
        }
        if (order.Destination.City == "")
        {
            throw new ArgumentException("destination city is required", nameof(order));
        }
    }
}

interface IOrderSummator 
{
    double Sum(Order order);
}

class StandardOrderSummator() : IOrderSummator
{
    public double Sum(Order order)
    {
        double total = 0;
        foreach (var item in order.Items)
        {
            total += item.Price * order.Card.Discount * 1.2;
        }
        return total;
    }
}

class PremiumOrderSummator() : IOrderSummator
{
    public double Sum(Order order)
    {
        double total = 0;
        foreach (var item in order.Items)
        {
            total += item.Price * order.Card.Discount * 0.9 * 1.2;
        }
        return total;
    }
}

class BudgetOrderSummator() : IOrderSummator
{
    public double Sum(Order order)
    {
        if (order.Items.Length > 3)
        {
            throw new Exception("Budget orders cannot have more than 3 items. Skipping.");
        }
        double total = 0;
        foreach (var item in order.Items)
        {
            total += item.Price * order.Card.Discount * 1.2;
        }
        return total;
    }
}

class InternationalOrderSummator() : IOrderSummator
{
    public double Sum(Order order)
    {
        double total = 0;
        foreach (var item in order.Items)
        {
            total += item.Price * order.Card.Discount * 1.5;
        }
        return total;
    }
}

interface IOrderSummatorFactory
{
    public IOrderSummator GetSummator(Order order);
}
class OrderSummatorFactory() : IOrderSummatorFactory
{
    public IOrderSummator GetSummator(Order order)
    {
        switch (order.Type) 
        {
            case "Standard":
                return new StandardOrderSummator();
            case "Premium":
                return new PremiumOrderSummator();
            case "Budget":
                return new BudgetOrderSummator();
            case "International":
                return new InternationalOrderSummator();
            default:
                throw new NotImplementedException();
        }
    }
}

class OrderProcessor
{
    private readonly RandomSQLDatabase database;
    private readonly IMailer mailer;
    private readonly IOrderValidator validator;
    private readonly IOrderSummatorFactory factory;
    public OrderProcessor(RandomSQLDatabase db)
    {
        database = db;
        mailer = new Mailer("google.com");
        validator = new OrderValidator();
        factory = new OrderSummatorFactory();
    }

    public void Process(Order order)
    {
        Console.WriteLine($"--- Processing Order {order.Id} ---");
        // 1. Логика валидации
        try
        {
            validator.Validate(order);
        }
        catch (Exception ex) 
        {
            Console.WriteLine(ex.Message);
        }

        // 2. Логика расчета суммы
        double total = 0;
        try 
        {
            var summator = factory.GetSummator(order);
            total = summator.Sum(order);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        // 4. Логика сохранения
        try
        {
            database.SaveOrder(order, total);
        }
        catch (Exception e)
        {
            Console.WriteLine($"database error: {e.Message}");
        }

        // 5. Логика уведомлений
        var emailBody = $"<h1>Your order {order.Id} is confirmed!</h1><p>Total: {total:F2}</p>";
        mailer.Send(order.ClientEmail, "Order confirmation", emailBody);
    }
}