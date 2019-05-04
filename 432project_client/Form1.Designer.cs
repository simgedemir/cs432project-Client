namespace _432project_client
{
    partial class Form1
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
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.ipBox = new System.Windows.Forms.TextBox();
            this.portBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.messageBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(83, 129);
            this.passwordBox.Margin = new System.Windows.Forms.Padding(2);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(119, 20);
            this.passwordBox.TabIndex = 0;
            // 
            // usernameBox
            // 
            this.usernameBox.Location = new System.Drawing.Point(83, 91);
            this.usernameBox.Margin = new System.Windows.Forms.Padding(2);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(119, 20);
            this.usernameBox.TabIndex = 1;
            this.usernameBox.TextChanged += new System.EventHandler(this.usernameBox_TextChanged);
            // 
            // ipBox
            // 
            this.ipBox.Location = new System.Drawing.Point(83, 21);
            this.ipBox.Margin = new System.Windows.Forms.Padding(2);
            this.ipBox.Name = "ipBox";
            this.ipBox.Size = new System.Drawing.Size(119, 20);
            this.ipBox.TabIndex = 2;
            this.ipBox.Text = "127.0.0.1";
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(83, 55);
            this.portBox.Margin = new System.Windows.Forms.Padding(2);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(119, 20);
            this.portBox.TabIndex = 3;
            this.portBox.Text = "1234";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 94);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Username:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 130);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(59, 24);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "IP:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 58);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Port:";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(43, 172);
            this.connectButton.Margin = new System.Windows.Forms.Padding(2);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(72, 30);
            this.connectButton.TabIndex = 9;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(125, 172);
            this.disconnectButton.Margin = new System.Windows.Forms.Padding(2);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(73, 30);
            this.disconnectButton.TabIndex = 10;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(221, 11);
            this.logs.Margin = new System.Windows.Forms.Padding(2);
            this.logs.Name = "logs";
            this.logs.Size = new System.Drawing.Size(245, 365);
            this.logs.TabIndex = 11;
            this.logs.Text = "";
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(43, 207);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(155, 23);
            this.loginButton.TabIndex = 12;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.SeaShell;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.messageBox);
            this.panel1.Location = new System.Drawing.Point(11, 246);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(191, 129);
            this.panel1.TabIndex = 13;
            // 
            // messageBox
            // 
            this.messageBox.Location = new System.Drawing.Point(15, 26);
            this.messageBox.Multiline = true;
            this.messageBox.Name = "messageBox";
            this.messageBox.Size = new System.Drawing.Size(163, 62);
            this.messageBox.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(103, 94);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Broadcast Message:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 387);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.portBox);
            this.Controls.Add(this.ipBox);
            this.Controls.Add(this.usernameBox);
            this.Controls.Add(this.passwordBox);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ipBox;
        private System.Windows.Forms.TextBox portBox;
        private System.Windows.Forms.TextBox usernameBox;
        private System.Windows.Forms.TextBox passwordBox;   
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox messageBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
    }
}

