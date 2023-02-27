using System.Xml;

namespace Game;

public enum WeakDay
{
    Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday, Random
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

