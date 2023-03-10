using System;
using System.IO;
using System.Threading;
using Game;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;
using Task = System.Threading.Tasks.Task;

var token = File.ReadAllText("../../../token.txt");
var botClient = new TelegramBotClient(token);

using CancellationTokenSource cts = new ();

ReceiverOptions receiverOptions = new ()
{
    AllowedUpdates = Array.Empty<UpdateType>() 
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var user = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{user.Username}");
Console.ReadLine();

cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Message is not { } message)
        return;
    
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;
    var path = "users/" + chatId + ".json";
    Application? app;
    app = File.Exists(path) ? ReadFromJsonFile<Application>(path) : new Application();

    if (messageText == "Очистить задачи")
    {
        app.TaskManager.ClearTasks();
        await botClient.SendTextMessageAsync(
            chatId: chatId, 
            text:"Задачи удаленны!",
            cancellationToken: cancellationToken);
        
    }
    else if (messageText == "Добавить задачу️")
    {
        const string response = "Введите по шаблону: 'название задачи день недели (0-7)'\n" + 
                                "Введите 0 если без разницы в какой день недели ставить.";
        await botClient.SendTextMessageAsync(
            chatId: chatId, 
            text: response,
            cancellationToken: cancellationToken);
        // var items = Console.ReadLine()?.Split(" ");
        // app.TaskManager.Add(items?[0], int.Parse(items?[1]).GetWeakDay());
        // Console.WriteLine($"Задача: {items[0]} была добавленна!");
    }
    else if (messageText == "Удалить задачу по имени")
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId, 
            text: "Введите название задачи для удаления: ",
            cancellationToken: cancellationToken);
    }
    else if (messageText == "Посмотреть расписание")
        await botClient.SendTextMessageAsync(
            chatId: chatId, 
            text: app.ToString(),
            cancellationToken: cancellationToken);
    else if (messageText == "Получить новое расписание️")
    {
        app.RandomizeSchedule();
        await botClient.SendTextMessageAsync(
            chatId: chatId, 
            text: "Расписание обновлено",
            cancellationToken: cancellationToken);
    }
    else
    {
        var splitText = messageText.Split(" ");
        if (splitText.Length == 2)
        {
            app.TaskManager.Add(splitText[0], int.Parse(splitText[1]).GetWeakDay());
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Done",
                cancellationToken: cancellationToken);
        }
        else if (splitText.Length == 1)
        {
            app.TaskManager.RemoveByName(messageText);
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Done",
                cancellationToken: cancellationToken);
        }
        else
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Error",
                cancellationToken: cancellationToken);
    }

    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
    {
        new KeyboardButton[] { "Очистить задачи", "Добавить задачу️" },
        new KeyboardButton[] { "Удалить задачу по имени️", "Посмотреть расписание" },
        new KeyboardButton[] { "Получить новое расписание️" },
    }) 
    {
        ResizeKeyboard = true
    };

}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
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
