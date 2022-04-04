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
using ClassLibrary;

namespace TelegramBot_Сonsole_
{
    class Program
    {
        static void Main(string[] args)
        {
           
            ClassLibrary.GameEngine GameEngineConsole = new GameEngine("5027258622:AAHruPQHbExVgHS_N_QV4sDc1KkJsxw5yzY");
            GameEngineConsole.OnPrintResult += OnPrintResult;
            GameEngineConsole.StartBot();
            string answer;

            Console.WriteLine("Для того щоб отримати список учасників гри введіть 'get'");
            answer = Console.ReadLine();

            if (answer == "get")
            {
                Console.WriteLine(GameEngineConsole.GetlistParticipants());
            }
            else
            {
                Console.WriteLine("Програма не зрозуміла вас!");
            }

            Console.WriteLine("Для того щоб почати гру введіть 'start'");
            answer = Console.ReadLine();
           
            if (answer == "start")
            {
                GameEngineConsole.SendGameStartNotification();
            }
            else
            {
                Console.WriteLine("Програма не зрозуміла вас!");
            }

            Console.ReadLine();
        }

        private static void OnPrintResult(string[] result)
        {  
            foreach (var message in result)
            Console.WriteLine(message);
        }     

    }
}
