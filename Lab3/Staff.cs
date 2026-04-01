
// =========================================================
// Файл: Staff.cs
// Описание: Система управления персоналом склада.
// =========================================================

namespace Orders;

interface IWorkExecutor
{
    void ProcessOrder();
}
interface IMeetingAttendee
{
    void AttendMeeting();
}
interface IRestCapable
{
    void GetRest();
}
interface ILeadSwingingCapable
{
    void SwingingTheLead();
}
interface IWarehouseWorker : IWorkExecutor {}

/// <summary>
/// HumanManager - Человек.
/// </summary>
class HumanManager : IWorkExecutor, ILeadSwingingCapable, IRestCapable, IMeetingAttendee
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
class RobotPacker : IWorkExecutor, IRestCapable
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
            if (worker is IMeetingAttendee attendee)
            {
                attendee.AttendMeeting();
            }
            if (worker is IRestCapable restCapable) 
            {
                restCapable.GetRest();
            }
            if (worker is ILeadSwingingCapable leadSwingingCapable)
            {
                leadSwingingCapable.SwingingTheLead();
            }
        }
    }
}