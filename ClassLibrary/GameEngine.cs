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
using Engine;
using System.Text;

namespace ClassLibrary
{
    public class GameEngine 
    {
        private TelegramBotClient _bot;
        //private IGameGUI gameGUI;
        private List<Person> _gameParticipants = new List<Person>();
        private long _chatId;
        private System.Timers.Timer _timer;
        private int _time;
        CancellationToken cancellationToken;

        public delegate void PrintResult(string[] result);
        public event PrintResult OnPrintResult;

        public GameEngine(string token) {
            _bot = new TelegramBotClient(token);   
        }
        
        public void StartBot()
        {
            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            _bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken: cts.Token);
        }

        public string GetlistParticipants() 
        {
            string listParticipant = "";
            var number = new StringBuilder();

            for (int i = 0; i < _gameParticipants.Count; i++)
            {
                number.Clear();
                listParticipant += "\n " + number.Insert(0, i + 1) + ". " + _gameParticipants[i].Surname + " " + _gameParticipants[i].Name;
            }

            if (_gameParticipants.Count == 0)
            {
                listParticipant = "Ще немає жодного гравця!";
            }

            return listParticipant;
        }

        public  async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {

            if (update.Type == UpdateType.Message)
            {
                if (update.Message!.Type == MessageType.Text)
                {
                    _chatId = update.Message.Chat.Id;
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

                         await _bot.SendTextMessageAsync(chatId: _chatId, text: answer, replyMarkup: inlineKeyBoard);      
                    }
                }
            }
            else if (update.Type == UpdateType.CallbackQuery)
                
                if (update.CallbackQuery.Data.ToString() == "Так")
                {
                    string name = update.CallbackQuery.Message.Chat.FirstName;
                    string surname = update.CallbackQuery.Message.Chat.LastName;
                    _chatId = update.CallbackQuery.Message.Chat.Id;
                    bool personRegistered = false;

                    foreach (var person in _gameParticipants)
                    {
                        if (person.ChatId == _chatId)
                        {
                            personRegistered = true;
                        }
                    }

                    if (personRegistered == false)
                    {
                        _gameParticipants.Add(new Person(name, surname, _chatId, 0));
                        await _bot.SendTextMessageAsync(chatId: _chatId, text: "Вам прийде сповіщення про початок гри!", cancellationToken: cancellationToken);

                    }

                }

                else if (update.CallbackQuery.Data.ToString() == "Тисни")
                {
                    _chatId = update.CallbackQuery.Message.Chat.Id;

                    foreach (var person in _gameParticipants)
                    {
                        if (person.ChatId == _chatId && _time != 10)
                        {
                            person.Point++;
                        }
                    }
                }
                else
                {
                    return;
                }
        }

        public Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            return Task.CompletedTask;
        }

        public async Task SendGameStartNotification() 
        {
            string note = "Жми кнопку протягом 10 с.";
            var inlineKeyBoard = new InlineKeyboardMarkup(new[]
            {
                new[]{
                    InlineKeyboardButton.WithCallbackData("Тисни")
                }
            });

            foreach (Person obg in _gameParticipants)
            {
                await _bot.SendTextMessageAsync(chatId: obg.ChatId, text: "Гра почнеться через:", cancellationToken: cancellationToken);
                
                for (int i = 3; i != 0; --i)
                await _bot.SendTextMessageAsync(chatId: obg.ChatId, text: i.ToString(), cancellationToken: cancellationToken);
                
                await _bot.SendTextMessageAsync(chatId: obg.ChatId, text: note, replyMarkup: inlineKeyBoard);

            }
            
            _timer = new System.Timers.Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            
        }

        private async void  OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            string[] result = new string[_gameParticipants.Count];
           
            string message = "Результат гри: ";
            if (_time == 10)
            {
                _timer.Enabled = false;

                _gameParticipants.OrderBy(n => n.Point); ;

                for (int i = 0; i < _gameParticipants.Count; i++)
                {
                    message += "\n " + (i + 1) + ". " + _gameParticipants[i].Surname + " " + _gameParticipants[i].Name + " , кількість балів: " + _gameParticipants[i].Point;
                    result[i] = message;
                    
                }
                foreach (var person in _gameParticipants)
                await _bot.SendTextMessageAsync(chatId: person.ChatId, text: message);

                if (OnPrintResult != null)
                    OnPrintResult(result);

                _gameParticipants.Clear();
                return;
            }

            _time++;
        }

        

    }
}

