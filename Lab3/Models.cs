
// =========================================================
// Файл: Models.cs
// Описание: Модели данных системы.
// =========================================================

namespace Orders;

/// <summary>
/// Item - товар в заказе.
/// </summary>
class Item
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required double Price { get; set; }
}

/// <summary>
/// Address - адрес доставки.
/// </summary>
class Address
{
    public required string City { get; set; }
    public required string Street { get; set; }
    public required string Zip { get; set; }
}

/// <summary>
/// Order - заказ.
/// </summary>
class Order
{
    public required string Id { get; set; }
    public required Item[] Items { get; set; }
    public required string Type { get; set; } // "Standard", "Premium", "Budget", "International"
    public required DiscountCard Card { get; set; }
    public required string ClientEmail { get; set; }
    public required Address Destination { get; set; }
}

/// <summary>
/// DiscountCard - Дисконтная карта
/// </summary>

class DiscountCard
{
    public DiscountCard(DiscountCardTypes type)
    {
        if (type == DiscountCardTypes.Gold)
        {
            Discount = 0.85;
        } 
        else if (type == DiscountCardTypes.Silver)
        {
            Discount = 0.9;
        } else if (type == DiscountCardTypes.Newbie)
        {
            Discount = 1;
        }
    }
    public double Discount { get; }
}

enum DiscountCardTypes
{
    Newbie,
    Silver,
    Gold
}