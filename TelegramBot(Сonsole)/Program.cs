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
            GameEngineConsole.OnMessagePrint += OnMessagePrint;

            GameEngineConsole.StartBot();

            Console.WriteLine("Для того щоб отримати список учасників гри натисніть 1");
            int answer = int.Parse(Console.ReadLine());
            if (answer == 1)
            {
               
                Console.WriteLine(GameEngineConsole.GetlistParticipants());
            }

            Console.WriteLine("Для того щоб почати гру введіть 'start'");
            string startGame = Console.ReadLine();
            if (startGame == "start")
            {
                GameEngineConsole.SendGameStartNotification();

            }

            Console.ReadLine();
        }

        private static void OnMessagePrint(string[] result)
        {
            
            foreach (var message in result)
                Console.WriteLine(message);
        }

        

    }
}
