using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary
{
    public class GameEngine 
    {
        public TelegramBotClient bot;
        public IGameGUI gameGUI;
        public List<Person> gameParticipants = new List<Person>();
        public long chatId;
        public System.Timers.Timer timer;
        public int time;
        CancellationToken cancellationToken;

        public delegate void PrintResult(string[] result);
        public event PrintResult OnPrintResult;

        public GameEngine(string token) {
            this.bot = new TelegramBotClient(token);   
        }

        public void StartBot()
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
        }

        public string GetlistParticipants() 
        {
            string listParticipant = "";
            
            for (int i = 0; i < gameParticipants.Count; i++)
            listParticipant += "\n " + i+1.ToString() + ". " + gameParticipants[i].surname + " " + gameParticipants[i].name;

            return listParticipant;
        }

        public  async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {

            if (update.Type == UpdateType.Message)
            {
                if (update.Message!.Type == MessageType.Text)
                {
                    chatId = update.Message.Chat.Id;
                    var messageText = update.Message.Text;
                    
                    if (messageText == "/start") 
                    { 
                        string answer = "Ви хочете зіграти гру?";
                        var inlineKeyBoard = new InlineKeyboardMarkup( new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Так"),
                                InlineKeyboardButton.WithCallbackData("Ні")
                             }
                         });

                         await bot.SendTextMessageAsync(chatId: chatId, text: answer, replyMarkup: inlineKeyBoard);      
                    }
                }
            }
            else if (update.Type == UpdateType.CallbackQuery)
                
                if (update.CallbackQuery.Data.ToString() == "Так")
                {
                    string name = update.CallbackQuery.Message.Chat.FirstName;
                    string surname = update.CallbackQuery.Message.Chat.LastName;
                    chatId = update.CallbackQuery.Message.Chat.Id;
                    bool personRegistered = false;

                    foreach (var person in gameParticipants)
                    {
                        if (person.chatId == chatId)
                        {
                            personRegistered = true;
                        }
                    }

                    if (personRegistered == false)
                    {
                        gameParticipants.Add(new Person(name, surname, chatId, 0));
                        await bot.SendTextMessageAsync(chatId: chatId, text: "Вам прийде сповіщення про початок гри!", cancellationToken: cancellationToken);

                    }

                }

                else if (update.CallbackQuery.Data.ToString() == "Тисни")
                {
                    chatId = update.CallbackQuery.Message.Chat.Id;

                    foreach (var person in gameParticipants)
                    {
                        if (person.chatId == chatId && time != 10)
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

        public async void SendGameStartNotification() 
        {
            string note = "Жми кнопку протягом 10 с.";
            var inlineKeyBoard = new InlineKeyboardMarkup(new[]
            {
                new[]{
                    InlineKeyboardButton.WithCallbackData("Тисни")
                }
            });

            foreach (Person obg in gameParticipants)
            {
                await bot.SendTextMessageAsync(chatId: obg.chatId, text: "Гра почнеться через:", cancellationToken: cancellationToken);
                
                for (int i = 3; i != 0; --i)
                await bot.SendTextMessageAsync(chatId: obg.chatId, text: i.ToString(), cancellationToken: cancellationToken);
                
                await bot.SendTextMessageAsync(chatId: obg.chatId, text: note, replyMarkup: inlineKeyBoard);

            }
            
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private async void  OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            string[] result = new string[gameParticipants.Count];
           
            string message = "Результат гри: ";
            if (time == 10)
            {
                timer.Enabled = false;

                gameParticipants.OrderBy(n => n.point); ;

                for (int i = 0; i < gameParticipants.Count; i++)
                {
                    message += "\n " + (i + 1) + ". " + gameParticipants[i].surname + " " + gameParticipants[i].name + " , кількість балів: " + gameParticipants[i].point;
                    result[i] = message;
                    
                }
                foreach (var person in gameParticipants)
                await bot.SendTextMessageAsync(chatId: person.chatId, text: message);

                if (OnPrintResult != null)
                    OnPrintResult(result);

                gameParticipants.Clear();
                return;
            }

            time++;
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

