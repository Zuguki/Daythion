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

Console.WriteLine("Hello, World!");