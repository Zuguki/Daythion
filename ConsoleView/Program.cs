using System;
using System.IO;
using Game;
using Newtonsoft.Json;


var app = new Application();
app.TaskManager
    .Add("A")
    .Add("B")
    .Add("C");

WriteToJsonFile("test.json", app);
app = new Application();
Console.WriteLine($"Добро пожаловать в Daythion, User: {app.TaskManager.Tasks.Count}");
app = ReadFromJsonFile<Application>("test.json");
Console.WriteLine($"Добро пожаловать в Daythion, User: {app.TaskManager.Tasks.Count}");

while (true)
{
    Console.WriteLine("Что вы хотите сделать?");
    Console.WriteLine("0. Очистить задачи.");
    Console.WriteLine("1. Добавить задачу.");
    Console.WriteLine("2. Удалить задачу по имени.");
    Console.WriteLine("3. Посмотреть расписание.");
    Console.WriteLine("4. Получить новое расписание.");

    var number = int.Parse(Console.ReadLine() ?? string.Empty);
    if (number == 0)
    {
        app.TaskManager.ClearTasks();
        Console.WriteLine("Задачи удаленны!");
    }
    else if (number == 1)
    {
        Console.WriteLine("Введите по шаблону: 'название задачи день недели (0-7)'");
        Console.WriteLine("Введите 0 если без разницы в какой день недели ставить.");
        var items = Console.ReadLine()?.Split(" ");
        app.TaskManager.Add(items?[0], int.Parse(items?[1]).GetWeakDay());
        Console.WriteLine($"Задача: {items[0]} была добавленна!");
    }
    else if (number == 2)
    {
        Console.Write("Введите название задачи для удаления: ");
        var taskName = Console.ReadLine();
        try
        {
            app.TaskManager.RemoveByName(taskName);
            Console.WriteLine($"Задача: {taskName} была удаленна!");
        }
        catch
        {
            Console.WriteLine($"Задачи с именем: {taskName} не существует");
        }
    }
    else if (number == 3)
        Console.WriteLine(app);
    else if (number == 4)
    {
        app.RandomizeSchedule();
        Console.WriteLine("Расписание обновленно!");
    }
    else
        Console.WriteLine("Такой комманды нет:(");
}


static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
{
    TextWriter? writer = null;
    try
    {
        var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
        writer = new StreamWriter(filePath, append);
        writer.Write(contentsToWriteToFile);
    }
    finally
    {
        writer?.Close();
    }
}

static T? ReadFromJsonFile<T>(string filePath) where T : new()
{
    TextReader? reader = null;
    try
    {
        reader = new StreamReader(filePath);
        var fileContents = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<T>(fileContents);
    }
    finally
    {
        reader?.Close();
    }
}


