using System.Xml;

namespace Game;

public enum WeakDay
{
    Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday, Random
}

public class Application
{
    private Dictionary<WeakDay, List<Task>> _schedule;
    private const int DaysInWeak = 7;

    public Application()
    {
        _schedule = new Dictionary<WeakDay, List<Task>>();
        TaskManager = new TaskManager();
    }
    
    public TaskManager TaskManager { get; private set; }

    public void RandomizeSchedule()
    {
        var rnd = new Random();
    }
}

public static class IntExt
{
    public static WeakDay GetWeakDay(this int number) =>
        number switch
        {
            1 => WeakDay.Monday,
            2 => WeakDay.Tuesday,
            3 => WeakDay.Wednesday,
            4 => WeakDay.Thursday,
            5 => WeakDay.Friday,
            6 => WeakDay.Saturday,
            7 => WeakDay.Sunday,
            _ => throw new ArgumentException()
        };
}

public class TaskManager
{
    public readonly List<Task> Tasks;

    public TaskManager()
    {
        Tasks = new List<Task>();
    }

    public TaskManager Add(string name, WeakDay weakDay = WeakDay.Random)
    {
        Tasks.Add(new Task(name, weakDay));
        return this;
    }

    public TaskManager RemoveByName(string name)
    {
        var task = GetTask(name);
        Tasks.Remove(task);
        return this;
    }

    private Task GetTask(string name)
    {
        var task = Tasks.FirstOrDefault(task => task.Name == name);
        if (task is null)
            throw new ArgumentException($"Task with {nameof(name)} {name} does't exist");

        return task;
    }
}

public class Task
{
    public Task(string name)
    {
        Name = name;
        WeakDay = WeakDay.Random;
    }

    public Task(string name, WeakDay weakDay)
    {
        Name = name;
        WeakDay = weakDay;
    }
    
    public string Name { get; private set; }
    public WeakDay WeakDay { get; set; }
}

