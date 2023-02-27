using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        _schedule = GetClearDictionary();
        TaskManager = new TaskManager();
    }
    
    public TaskManager TaskManager { get; private set; }

    public void RandomizeSchedule()
    {
        _schedule = GetClearDictionary();
        var rnd = new Random();
        var randomTasksCount = (decimal) TaskManager.Tasks.Count(item => item.WeakDay == WeakDay.Random);
        var randomTaskInDay = (int) Math.Ceiling(randomTasksCount / DaysInWeak);

        foreach (var task in TaskManager.Tasks)
        {
            var weakDay = task.WeakDay;
            if (weakDay is WeakDay.Random)
                weakDay = rnd.Next(1, 7).GetWeakDay();
            
            if (!_schedule.ContainsKey(weakDay))
                _schedule.Add(weakDay, new List<Task>());
            
            _schedule[weakDay].Add(task);
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var item in _schedule.Where(item => item.Value.Count != 0))
        {
            sb.Append(item.Key)
                .AppendLine(": ");
            foreach (var task in item.Value)
                sb.AppendLine($"    {task}");
            
            sb.AppendLine();
        }

        return sb.ToString();
    }

    private Dictionary<WeakDay, List<Task>> GetClearDictionary() =>
        new()
        {
            {WeakDay.Monday, new List<Task>()},
            {WeakDay.Tuesday, new List<Task>()},
            {WeakDay.Wednesday, new List<Task>()},
            {WeakDay.Thursday, new List<Task>()},
            {WeakDay.Friday, new List<Task>()},
            {WeakDay.Saturday, new List<Task>()},
            {WeakDay.Sunday, new List<Task>()},
        };
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

    public override string ToString() => Name;
}

