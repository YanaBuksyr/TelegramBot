using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot_WinForms_
{

    public partial class Form1 : Form
    {
        public static TelegramBotClient bot = new TelegramBotClient("5027258622:AAHruPQHbExVgHS_N_QV4sDc1KkJsxw5yzY");
        public static long chatId;
        public static List<Person> gameParticipants = new List<Person>();
        private static int aTimer = 0; 

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
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

            //cts.Cancel();

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

                    foreach (var person in gameParticipants )
                        {
                        if (person.chatId == chatId && aTimer != 10) {
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

      

        private async void  button_Click(object sender, EventArgs e)
        {
            
            CancellationToken cancellationToken;
            
            string answer = "Жми кнопку протягом 10 с.";
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

                Telegram.Bot.Types.Message MessageButton = await bot.SendTextMessageAsync(chatId: obg.chatId, text: answer, replyMarkup: inlineKeyBoard);

            }


            timer.Enabled = true;

        }


        private void Timer_Click(object sender, EventArgs e)
        {
            aTimer++;
            label1.Text = aTimer.ToString();
            if (aTimer == 10) {
                timer.Enabled = false;
                
                listBox1.Items.Clear();

                var istеPeople = gameParticipants.OrderBy(n => n.point);
                int i = 1;
                foreach (var person in gameParticipants)
                {
                    listBox1.Items.Add(i.ToString() + ". " +person.surname + " " + person.name + " , кількість балів: " + person.point);
                    i++;
                }
                gameParticipants.Clear();
            }
            
        }

        public class Person {

            public string name;
            public string surname;
            public long chatId;
            public int point;


            public Person(string name, string surname, long chatId, int point) {
                this.name = name;
                this.surname = surname;
                this.chatId = chatId;
                this.point = point;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            aTimer = 0;
            label1.Text = "0";
            //gameParticipants.Clear();
            listBox1.Items.Clear();

            foreach (Person obg in gameParticipants)
            {
                listBox1.Items.Add(obg.name + " " + obg.surname);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            foreach (var person in gameParticipants)
            {
                listBox1.Items.Add(person.surname + " " + person.name + " , " + person.point);

            }
        }
    }
}




