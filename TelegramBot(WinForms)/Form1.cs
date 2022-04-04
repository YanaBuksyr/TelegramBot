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

        //public void PrintResult(string result)
        //{
        //    //string result = "hello";
        //    listBox1.Items.Add(result);

        //}

        //public class WinFormsGui : IGameGUI
        //{

        //    public ListBox listBox1;
        //    public WinFormsGui(ListBox listBox1)
        //    {
        //        this.listBox1 = listBox1;
        //    }

        //    public void PrintResult(string result)
        //    {

        //        listBox1.Items.Add(result);

        //    }
        //}
        

        private void Form1_Load(object sender, EventArgs e)
        {
            //WinFormsGui winFormsGui = new WinFormsGui(listBox1);
            GameEngineWinForm = new GameEngine("5027258622:AAHruPQHbExVgHS_N_QV4sDc1KkJsxw5yzY");
            GameEngineWinForm.OnMessagePrint += OnMessagePrint;
            GameEngineWinForm.StartBot();
            
        }

       
        private static void OnMessagePrint(string mes)
        {
           
            listBox1.Items.Add(mes);
        }

        private void  button_Click(object sender, EventArgs e)
        {

            GameEngineWinForm.SendGameStartNotification();
          
        }


        private void button2_Click(object sender, EventArgs e)
        {
           label1.Text = "0";
            listBox1.Items.Clear();
           listBox1.Items.Add(GameEngineWinForm.GetlistParticipants());
        
        }

       

        
    }
}




