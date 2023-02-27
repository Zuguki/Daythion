using System;
using Game;

var app = new Application();
app.TaskManager
    .Add("Зал", WeakDay.Monday)
    .Add("Зал", WeakDay.Wednesday)
    .Add("Зал", WeakDay.Friday)
    .Add("Дискретка")
    .Add("Физика")
    .Add("Читать")
    .Add("Проект");

Console.WriteLine(1);
app.RandomizeSchedule();
Console.WriteLine(app);

Console.WriteLine(2);
app.RandomizeSchedule();
Console.WriteLine(app);

Console.WriteLine(3);
app.RandomizeSchedule();
Console.WriteLine(app);