namespace PeerModule
{
    partial class PeerForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.textIPServer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonR = new System.Windows.Forms.Button();
            this.buttonJ = new System.Windows.Forms.Button();
            this.connect = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textPeerId = new System.Windows.Forms.TextBox();
            this.lbRoom = new System.Windows.Forms.ListBox();
            this.textMessages = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.createRoom = new System.Windows.Forms.Button();
            this.roomName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textPlayerNum = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonRun = new System.Windows.Forms.Button();
            this.lbRoomPeers = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lbTeam1 = new System.Windows.Forms.ListBox();
            this.lbTeam2 = new System.Windows.Forms.ListBox();
            this.buttonJoinTeam1 = new System.Windows.Forms.Button();
            this.buttonJoinTeam2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP SERVER";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // textIPServer
            // 
            this.textIPServer.Location = new System.Drawing.Point(94, 43);
            this.textIPServer.Name = "textIPServer";
            this.textIPServer.Size = new System.Drawing.Size(255, 20);
            this.textIPServer.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 379);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "ROOM LIST";
            // 
            // buttonR
            // 
            this.buttonR.Location = new System.Drawing.Point(193, 483);
            this.buttonR.Name = "buttonR";
            this.buttonR.Size = new System.Drawing.Size(75, 23);
            this.buttonR.TabIndex = 4;
            this.buttonR.Text = "refresh";
            this.buttonR.UseVisualStyleBackColor = true;
            this.buttonR.Click += new System.EventHandler(this.buttonR_Click_1);
            // 
            // buttonJ
            // 
            this.buttonJ.Location = new System.Drawing.Point(274, 483);
            this.buttonJ.Name = "buttonJ";
            this.buttonJ.Size = new System.Drawing.Size(75, 23);
            this.buttonJ.TabIndex = 6;
            this.buttonJ.Text = "Join";
            this.buttonJ.UseVisualStyleBackColor = true;
            this.buttonJ.Click += new System.EventHandler(this.buttonJ_Click_1);
            // 
            // connect
            // 
            this.connect.Location = new System.Drawing.Point(248, 69);
            this.connect.Name = "connect";
            this.connect.Size = new System.Drawing.Size(75, 23);
            this.connect.TabIndex = 7;
            this.connect.Text = "Connect";
            this.connect.UseVisualStyleBackColor = true;
            this.connect.Click += new System.EventHandler(this.connect_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "PEER ID";
            // 
            // textPeerId
            // 
            this.textPeerId.Location = new System.Drawing.Point(94, 13);
            this.textPeerId.Name = "textPeerId";
            this.textPeerId.Size = new System.Drawing.Size(255, 20);
            this.textPeerId.TabIndex = 9;
            // 
            // lbRoom
            // 
            this.lbRoom.FormattingEnabled = true;
            this.lbRoom.Location = new System.Drawing.Point(17, 395);
            this.lbRoom.Name = "lbRoom";
            this.lbRoom.Size = new System.Drawing.Size(334, 82);
            this.lbRoom.TabIndex = 3;
            this.lbRoom.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // textMessages
            // 
            this.textMessages.Location = new System.Drawing.Point(16, 123);
            this.textMessages.Multiline = true;
            this.textMessages.Name = "textMessages";
            this.textMessages.Size = new System.Drawing.Size(330, 157);
            this.textMessages.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Messages Received";
            // 
            // createRoom
            // 
            this.createRoom.Location = new System.Drawing.Point(265, 331);
            this.createRoom.Name = "createRoom";
            this.createRoom.Size = new System.Drawing.Size(84, 23);
            this.createRoom.TabIndex = 12;
            this.createRoom.Text = "Create Room";
            this.createRoom.UseVisualStyleBackColor = true;
            this.createRoom.Click += new System.EventHandler(this.createRoom_Click);
            // 
            // roomName
            // 
            this.roomName.Location = new System.Drawing.Point(119, 305);
            this.roomName.Name = "roomName";
            this.roomName.Size = new System.Drawing.Size(230, 20);
            this.roomName.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 306);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "New Room Name";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // textPlayerNum
            // 
            this.textPlayerNum.Location = new System.Drawing.Point(119, 333);
            this.textPlayerNum.Name = "textPlayerNum";
            this.textPlayerNum.Size = new System.Drawing.Size(100, 20);
            this.textPlayerNum.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 340);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Max Players";
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(24, 483);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(75, 23);
            this.buttonRun.TabIndex = 17;
            this.buttonRun.Text = "Run Game";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // lbRoomPeers
            // 
            this.lbRoomPeers.FormattingEnabled = true;
            this.lbRoomPeers.Location = new System.Drawing.Point(400, 45);
            this.lbRoomPeers.Name = "lbRoomPeers";
            this.lbRoomPeers.Size = new System.Drawing.Size(327, 147);
            this.lbRoomPeers.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(400, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Room Peers";
            // 
            // lbTeam1
            // 
            this.lbTeam1.FormattingEnabled = true;
            this.lbTeam1.Location = new System.Drawing.Point(397, 247);
            this.lbTeam1.Name = "lbTeam1";
            this.lbTeam1.Size = new System.Drawing.Size(324, 95);
            this.lbTeam1.TabIndex = 20;
            // 
            // lbTeam2
            // 
            this.lbTeam2.FormattingEnabled = true;
            this.lbTeam2.Location = new System.Drawing.Point(397, 395);
            this.lbTeam2.Name = "lbTeam2";
            this.lbTeam2.Size = new System.Drawing.Size(324, 95);
            this.lbTeam2.TabIndex = 21;
            // 
            // buttonJoinTeam1
            // 
            this.buttonJoinTeam1.Location = new System.Drawing.Point(435, 199);
            this.buttonJoinTeam1.Name = "buttonJoinTeam1";
            this.buttonJoinTeam1.Size = new System.Drawing.Size(75, 23);
            this.buttonJoinTeam1.TabIndex = 22;
            this.buttonJoinTeam1.Text = "Join Team 1";
            this.buttonJoinTeam1.UseVisualStyleBackColor = true;
            this.buttonJoinTeam1.Click += new System.EventHandler(this.buttonJoinTeam1_Click);
            // 
            // buttonJoinTeam2
            // 
            this.buttonJoinTeam2.Location = new System.Drawing.Point(601, 198);
            this.buttonJoinTeam2.Name = "buttonJoinTeam2";
            this.buttonJoinTeam2.Size = new System.Drawing.Size(75, 23);
            this.buttonJoinTeam2.TabIndex = 23;
            this.buttonJoinTeam2.Text = "Join Team 2";
            this.buttonJoinTeam2.UseVisualStyleBackColor = true;
            this.buttonJoinTeam2.Click += new System.EventHandler(this.buttonJoinTeam2_Click);
            // 
            // PeerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 518);
            this.Controls.Add(this.buttonJoinTeam2);
            this.Controls.Add(this.buttonJoinTeam1);
            this.Controls.Add(this.lbTeam2);
            this.Controls.Add(this.lbTeam1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lbRoomPeers);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textPlayerNum);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.roomName);
            this.Controls.Add(this.createRoom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textMessages);
            this.Controls.Add(this.textPeerId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.connect);
            this.Controls.Add(this.buttonJ);
            this.Controls.Add(this.buttonR);
            this.Controls.Add(this.lbRoom);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textIPServer);
            this.Controls.Add(this.label1);
            this.Name = "PeerForm";
            this.Text = "Peer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PeerForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textIPServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonR;
        public System.Windows.Forms.Button buttonJ;
        private System.Windows.Forms.Button connect;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox textPeerId;
        public System.Windows.Forms.ListBox lbRoom;
        private System.Windows.Forms.TextBox textMessages;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Button createRoom;
        private System.Windows.Forms.TextBox roomName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textPlayerNum;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.ListBox lbRoomPeers;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListBox lbTeam1;
        private System.Windows.Forms.ListBox lbTeam2;
        private System.Windows.Forms.Button buttonJoinTeam1;
        private System.Windows.Forms.Button buttonJoinTeam2;
    }
}