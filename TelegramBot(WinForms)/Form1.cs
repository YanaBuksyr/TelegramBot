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
            GameEngineWinForm.OnMessagePrint += OnMessagePrint;
            GameEngineWinForm.StartBot();
            
        }

       
        private  void OnMessagePrint(string[] result)
        {

            Invoke(new Action(() =>
            {

              
                listBox1.Items.Clear();
                foreach(var message in result)
                listBox1.Items.Add(message);

            }));
        }

        private void  button_Click(object sender, EventArgs e)
        {

            GameEngineWinForm.SendGameStartNotification();
          
        }


        private void button2_Click(object sender, EventArgs e)
        {
           
           listBox1.Items.Clear();
           listBox1.Items.Add(GameEngineWinForm.GetlistParticipants());
        
        }

       

        
    }
}




