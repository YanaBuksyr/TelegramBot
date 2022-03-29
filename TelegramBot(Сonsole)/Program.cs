using System;
using System.Timers;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;
using System.Linq;

namespace TelegramBot_Сonsole_
{
    class Program
    {
        public static TelegramBotClient bot = new TelegramBotClient("5027258622:AAHruPQHbExVgHS_N_QV4sDc1KkJsxw5yzY");
        public static long chatId;
        public static List<Person> gameParticipants = new List<Person>();
        private static System.Timers.Timer aTimer;
        private static int seconds;

        static async Task Main(string[] args)
        {
            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken: cts.Token);

            
            Console.WriteLine("Для того щоб отримати список учасників гри натисніть 1");
            int answer = int.Parse(Console.ReadLine());
            if (answer == 1) {
                foreach (var person in gameParticipants)
                {
                    Console.WriteLine(person.surname + " " + person.name);

                }
            }

            Console.WriteLine("Для того щоб почати гру введіть 'start'");
            string startGame = Console.ReadLine();
            if (startGame == "start")
            {
                CancellationToken cancellationToken;

                string note = "Жми кнопку протягом 10 с.";
                var inlineKeyBoard = new InlineKeyboardMarkup(new[]
                {
                    new[]{
                        InlineKeyboardButton.WithCallbackData("Тисни")
                     }
                 });

                foreach (Person obg in gameParticipants)
                {

                    Telegram.Bot.Types.Message sentMessage = await bot.SendTextMessageAsync(chatId: obg.chatId, text: "Гра почнеться через:", cancellationToken: cancellationToken);
                    for (int i = 3; i != 0; --i)
                    {
                        Telegram.Bot.Types.Message Message = await bot.SendTextMessageAsync(chatId: obg.chatId, text: i.ToString(), cancellationToken: cancellationToken);
                    }

                    Telegram.Bot.Types.Message MessageButton = await bot.SendTextMessageAsync(chatId: obg.chatId, text: note, replyMarkup: inlineKeyBoard);

                }

                aTimer = new System.Timers.Timer();
                aTimer.Interval = 1000;
                aTimer.Elapsed += OnTimedEvent;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;



            }

            Console.ReadLine();
            cts.Cancel();
        }

        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (seconds == 10)
            {
                aTimer.Enabled = false;
                var istеPeople = gameParticipants.OrderBy(n => n.point);
                int i = 1;
                foreach (var person in gameParticipants)
                {
                    Console.WriteLine(i.ToString() + ". " + person.surname + " " + person.name + " , кількість балів: " + person.point);
                    i++;
                }
                gameParticipants.Clear();
                return;
            }

            seconds++;
        }


        public static async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {

            if (update.Type == UpdateType.Message)
            {
                if (update.Message!.Type == MessageType.Text)
                {
                    chatId = update.Message.Chat.Id;
                    var messageText = update.Message.Text;

                    switch (messageText)
                    {
                        case "/start":
                            string answer = "Ви хочете зіграти гру?";
                            var inlineKeyBoard = new InlineKeyboardMarkup(new[]
                            {
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Так"),
                                    InlineKeyboardButton.WithCallbackData("Ні")
                                }
                            });

                            Telegram.Bot.Types.Message sentMessage = await bot.SendTextMessageAsync(chatId: chatId, text: answer, replyMarkup: inlineKeyBoard);

                            break;

                        case "так":

                            break;
                        default:
                            break;
                    }
                }
            }
            else if (update.Type == UpdateType.CallbackQuery)
                if (update.CallbackQuery.Data.ToString() == "Так")
                {
                    string name = update.CallbackQuery.Message.Chat.FirstName;
                    string surname = update.CallbackQuery.Message.Chat.LastName;
                    long chstId = update.CallbackQuery.Message.Chat.Id;
                    bool personRegistered = false;

                    foreach (var person in gameParticipants)
                    {
                        if (person.chatId == chatId)
                        {
                            personRegistered = true;
                            ;
                        }
                    }
                    if (personRegistered == false)
                    {
                        gameParticipants.Add(new Person(name, surname, chstId, 0));
                        Telegram.Bot.Types.Message sentMessage = await bot.SendTextMessageAsync(chatId: chatId, text: "Вам прийде сповіщення про початок гри!", cancellationToken: cancellationToken);

                    }

                }

                else if (update.CallbackQuery.Data.ToString() == "Тисни")
                {
                    chatId = update.CallbackQuery.Message.Chat.Id;

                    foreach (var person in gameParticipants)
                    {
                        if (person.chatId == chatId && seconds != 10)
                        {
                            person.point++;
                        }
                    }
                }
                else
                {
                    return;
                }
        }

        public static Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            return Task.CompletedTask;
        }
        
        public class Person
        {

            public string name;
            public string surname;
            public long chatId;
            public int point;


            public Person(string name, string surname, long chatId, int point)
            {
                this.name = name;
                this.surname = surname;
                this.chatId = chatId;
                this.point = point;
            }
        }
    }
}
