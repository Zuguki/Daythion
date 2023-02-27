using System.Xml;

namespace Game;

public enum WeakDay
{
    Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday, Random
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

