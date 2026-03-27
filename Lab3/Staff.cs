
// =========================================================
// Файл: Staff.cs
// Описание: Система управления персоналом склада.
// =========================================================

namespace Orders;

interface IWarehouseWorker
{
    void ProcessOrder();
    void AttendMeeting();
    void GetRest();
    void SwingingTheLead();
}

/// <summary>
/// HumanManager - Человек.
/// </summary>
class HumanManager : IWarehouseWorker
{
    public void ProcessOrder()
    {
        Console.WriteLine("Manager is processing logic...");
    }

    public void AttendMeeting()
    {
        Console.WriteLine("Manager is boring at the meeting...");
    }

    public void GetRest()
    {
        Console.WriteLine("Manager is taking a break...");
    }

    public void SwingingTheLead()
    {
        Console.WriteLine("Manager is watching reels...");
    }
}


/// <summary>
/// RobotPacker - Робот.
/// </summary>
class RobotPacker : IWarehouseWorker
{
    public required string Model { get; init; }

    public void ProcessOrder()
    {
        Console.WriteLine("Robot " + Model + " is packing boxes...");
    }

    public void GetRest()
    {
        Console.WriteLine("Robot was taken for maintenance");
    }

    public void AttendMeeting()
    {
        Console.WriteLine("ERROR: Robot cannot attend meetings");
    }

    public void SwingingTheLead()
    {
        throw new InvalidOperationException("CRITICAL ERROR: Robot cannot waste our money (we hope so)");
    }
}

static class Warehouse
{
    /// <summary>
    /// ManageWarehouse - функция, которая работает со списком работников.
    /// </summary>
    /// <param name="workers"></param>
    public static void ManageWarehouse(IWarehouseWorker[] workers)
    {
        Console.WriteLine("\n--- Warehouse Shift Started ---");

        foreach (var worker in workers)
        {
            worker.ProcessOrder();
            worker.AttendMeeting();
            worker.GetRest();
            worker.SwingingTheLead();
        }
    }
}