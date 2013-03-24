namespace gunbond
{
    partial class MainForm
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
            this.buttonTracker = new System.Windows.Forms.Button();
            this.buttonPeer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonTracker
            // 
            this.buttonTracker.Location = new System.Drawing.Point(45, 30);
            this.buttonTracker.Name = "buttonTracker";
            this.buttonTracker.Size = new System.Drawing.Size(184, 53);
            this.buttonTracker.TabIndex = 0;
            this.buttonTracker.Text = "Run Tracker";
            this.buttonTracker.UseVisualStyleBackColor = true;
            this.buttonTracker.Click += new System.EventHandler(this.buttonTracker_Click);
            // 
            // buttonPeer
            // 
            this.buttonPeer.Location = new System.Drawing.Point(45, 149);
            this.buttonPeer.Name = "buttonPeer";
            this.buttonPeer.Size = new System.Drawing.Size(184, 53);
            this.buttonPeer.TabIndex = 1;
            this.buttonPeer.Text = "Run Peer";
            this.buttonPeer.UseVisualStyleBackColor = true;
            this.buttonPeer.Click += new System.EventHandler(this.buttonPeer_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.buttonPeer);
            this.Controls.Add(this.buttonTracker);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonTracker;
        private System.Windows.Forms.Button buttonPeer;
    }
}