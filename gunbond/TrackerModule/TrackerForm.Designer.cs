namespace TrackerModule
{
    partial class TrackerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelListenIP = new System.Windows.Forms.Label();
            this.textListenIP = new System.Windows.Forms.TextBox();
            this.labelListPeer = new System.Windows.Forms.Label();
            this.lbListPeer = new System.Windows.Forms.ListBox();
            this.labelListRoom = new System.Windows.Forms.Label();
            this.lbListRoom = new System.Windows.Forms.ListBox();
            this.labelMessagesSent = new System.Windows.Forms.Label();
            this.textMessagesSent = new System.Windows.Forms.TextBox();
            this.labelMessagesReceived = new System.Windows.Forms.Label();
            this.textMessagesReceived = new System.Windows.Forms.TextBox();
            this.labelCommandLine = new System.Windows.Forms.Label();
            this.textCommand = new System.Windows.Forms.TextBox();
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelListenIP
            // 
            this.labelListenIP.AutoSize = true;
            this.labelListenIP.Location = new System.Drawing.Point(13, 13);
            this.labelListenIP.Name = "labelListenIP";
            this.labelListenIP.Size = new System.Drawing.Size(62, 13);
            this.labelListenIP.TabIndex = 0;
            this.labelListenIP.Text = "Listening IP";
            // 
            // textListenIP
            // 
            this.textListenIP.BackColor = System.Drawing.Color.White;
            this.textListenIP.Location = new System.Drawing.Point(13, 40);
            this.textListenIP.Multiline = true;
            this.textListenIP.Name = "textListenIP";
            this.textListenIP.Size = new System.Drawing.Size(193, 53);
            this.textListenIP.TabIndex = 1;
            // 
            // labelListPeer
            // 
            this.labelListPeer.AutoSize = true;
            this.labelListPeer.Location = new System.Drawing.Point(16, 112);
            this.labelListPeer.Name = "labelListPeer";
            this.labelListPeer.Size = new System.Drawing.Size(48, 13);
            this.labelListPeer.TabIndex = 2;
            this.labelListPeer.Text = "List Peer";
            // 
            // lbListPeer
            // 
            this.lbListPeer.FormattingEnabled = true;
            this.lbListPeer.Location = new System.Drawing.Point(19, 138);
            this.lbListPeer.Name = "lbListPeer";
            this.lbListPeer.Size = new System.Drawing.Size(187, 95);
            this.lbListPeer.TabIndex = 3;
            // 
            // labelListRoom
            // 
            this.labelListRoom.AutoSize = true;
            this.labelListRoom.Location = new System.Drawing.Point(243, 112);
            this.labelListRoom.Name = "labelListRoom";
            this.labelListRoom.Size = new System.Drawing.Size(54, 13);
            this.labelListRoom.TabIndex = 4;
            this.labelListRoom.Text = "List Room";
            // 
            // lbListRoom
            // 
            this.lbListRoom.FormattingEnabled = true;
            this.lbListRoom.Location = new System.Drawing.Point(246, 138);
            this.lbListRoom.Name = "lbListRoom";
            this.lbListRoom.Size = new System.Drawing.Size(216, 95);
            this.lbListRoom.TabIndex = 5;
            // 
            // labelMessagesSent
            // 
            this.labelMessagesSent.AutoSize = true;
            this.labelMessagesSent.Location = new System.Drawing.Point(19, 250);
            this.labelMessagesSent.Name = "labelMessagesSent";
            this.labelMessagesSent.Size = new System.Drawing.Size(80, 13);
            this.labelMessagesSent.TabIndex = 6;
            this.labelMessagesSent.Text = "Messages Sent";
            // 
            // textMessagesSent
            // 
            this.textMessagesSent.BackColor = System.Drawing.Color.White;
            this.textMessagesSent.Location = new System.Drawing.Point(22, 276);
            this.textMessagesSent.Multiline = true;
            this.textMessagesSent.Name = "textMessagesSent";
            this.textMessagesSent.ReadOnly = true;
            this.textMessagesSent.Size = new System.Drawing.Size(440, 84);
            this.textMessagesSent.TabIndex = 7;
            // 
            // labelMessagesReceived
            // 
            this.labelMessagesReceived.AutoSize = true;
            this.labelMessagesReceived.Location = new System.Drawing.Point(22, 376);
            this.labelMessagesReceived.Name = "labelMessagesReceived";
            this.labelMessagesReceived.Size = new System.Drawing.Size(104, 13);
            this.labelMessagesReceived.TabIndex = 8;
            this.labelMessagesReceived.Text = "Messages Received";
            // 
            // textMessagesReceived
            // 
            this.textMessagesReceived.BackColor = System.Drawing.Color.White;
            this.textMessagesReceived.Location = new System.Drawing.Point(25, 407);
            this.textMessagesReceived.Multiline = true;
            this.textMessagesReceived.Name = "textMessagesReceived";
            this.textMessagesReceived.ReadOnly = true;
            this.textMessagesReceived.Size = new System.Drawing.Size(437, 81);
            this.textMessagesReceived.TabIndex = 9;
            // 
            // labelCommandLine
            // 
            this.labelCommandLine.AutoSize = true;
            this.labelCommandLine.Location = new System.Drawing.Point(246, 13);
            this.labelCommandLine.Name = "labelCommandLine";
            this.labelCommandLine.Size = new System.Drawing.Size(77, 13);
            this.labelCommandLine.TabIndex = 10;
            this.labelCommandLine.Text = "Command Line";
            // 
            // textCommand
            // 
            this.textCommand.Location = new System.Drawing.Point(249, 40);
            this.textCommand.Name = "textCommand";
            this.textCommand.Size = new System.Drawing.Size(213, 20);
            this.textCommand.TabIndex = 11;
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Location = new System.Drawing.Point(249, 70);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(75, 23);
            this.buttonSubmit.TabIndex = 12;
            this.buttonSubmit.Text = "Submit";
            this.buttonSubmit.UseVisualStyleBackColor = true;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(131, 99);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 13;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // TrackerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 536);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.buttonSubmit);
            this.Controls.Add(this.textCommand);
            this.Controls.Add(this.labelCommandLine);
            this.Controls.Add(this.textMessagesReceived);
            this.Controls.Add(this.labelMessagesReceived);
            this.Controls.Add(this.textMessagesSent);
            this.Controls.Add(this.labelMessagesSent);
            this.Controls.Add(this.lbListRoom);
            this.Controls.Add(this.labelListRoom);
            this.Controls.Add(this.lbListPeer);
            this.Controls.Add(this.labelListPeer);
            this.Controls.Add(this.textListenIP);
            this.Controls.Add(this.labelListenIP);
            this.Name = "TrackerForm";
            this.Text = "TrackerForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TrackerForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelListenIP;
        public System.Windows.Forms.TextBox textListenIP;
        private System.Windows.Forms.Label labelListPeer;
        private System.Windows.Forms.ListBox lbListPeer;
        private System.Windows.Forms.Label labelListRoom;
        private System.Windows.Forms.ListBox lbListRoom;
        private System.Windows.Forms.Label labelMessagesSent;
        private System.Windows.Forms.TextBox textMessagesSent;
        private System.Windows.Forms.Label labelMessagesReceived;
        private System.Windows.Forms.TextBox textMessagesReceived;
        private System.Windows.Forms.Label labelCommandLine;
        private System.Windows.Forms.TextBox textCommand;
        private System.Windows.Forms.Button buttonSubmit;
        private System.Windows.Forms.Button buttonConnect;
    }
}