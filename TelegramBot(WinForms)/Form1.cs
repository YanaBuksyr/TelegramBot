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
using ClassLibrary;


namespace TelegramBot_WinForms_
{

    public partial class Form1 : Form
    {
  
        public ClassLibrary.GameEngine GameEngineWinForm;

        public Form1()
        {
            InitializeComponent();     
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GameEngineWinForm = new GameEngine("5027258622:AAHruPQHbExVgHS_N_QV4sDc1KkJsxw5yzY");
            GameEngineWinForm.OnPrintResult += OnPrintResult;
            GameEngineWinForm.StartBot();  
        }

        private void GetlistParticipants_Click(object sender, EventArgs e)
        {
            listParticipants.Items.Clear();
            listParticipants.Items.Add(GameEngineWinForm.GetlistParticipants());
        }

        private void StartGame_Click(object sender, EventArgs e)
        {
            GameEngineWinForm.SendGameStartNotification().Wait();
        }

        private  void OnPrintResult(string[] result)
        {
            Invoke(new Action(() =>
            {
                listParticipants.Items.Clear();
                foreach(var message in result)
                listParticipants.Items.Add(message);

            }));
        }

    }
}




