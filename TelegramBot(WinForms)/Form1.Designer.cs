
namespace TelegramBot_WinForms_
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.StartGame = new System.Windows.Forms.Button();
            this.GetlistParticipants = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.listParticipants = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // StartGame
            // 
            this.StartGame.Location = new System.Drawing.Point(51, 349);
            this.StartGame.Name = "StartGame";
            this.StartGame.Size = new System.Drawing.Size(393, 42);
            this.StartGame.TabIndex = 0;
            this.StartGame.Text = "start";
            this.StartGame.UseVisualStyleBackColor = true;
            this.StartGame.Click += new System.EventHandler(this.StartGame_Click);
            // 
            // GetlistParticipants
            // 
            this.GetlistParticipants.Location = new System.Drawing.Point(51, 54);
            this.GetlistParticipants.Name = "GetlistParticipants";
            this.GetlistParticipants.Size = new System.Drawing.Size(393, 42);
            this.GetlistParticipants.TabIndex = 5;
            this.GetlistParticipants.Text = "get game participants";
            this.GetlistParticipants.UseVisualStyleBackColor = true;
            this.GetlistParticipants.Click += new System.EventHandler(this.GetlistParticipants_Click);
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            // 
            // listParticipants
            // 
            this.listParticipants.FormattingEnabled = true;
            this.listParticipants.ItemHeight = 20;
            this.listParticipants.Location = new System.Drawing.Point(51, 126);
            this.listParticipants.Name = "listParticipants";
            this.listParticipants.Size = new System.Drawing.Size(393, 184);
            this.listParticipants.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 413);
            this.Controls.Add(this.listParticipants);
            this.Controls.Add(this.GetlistParticipants);
            this.Controls.Add(this.StartGame);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StartGame;
        private System.Windows.Forms.Button GetlistParticipants;
        private System.Windows.Forms.Timer timer;
        public System.Windows.Forms.ListBox listParticipants;
    }
}

