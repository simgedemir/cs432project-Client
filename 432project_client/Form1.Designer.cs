﻿namespace _432project_client
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
            this.components = new System.ComponentModel.Container();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.ipBox = new System.Windows.Forms.TextBox();
            this.portBox = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(156, 292);
            this.passwordBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(186, 31);
            this.passwordBox.TabIndex = 0;
            // 
            // usernameBox
            // 
            this.usernameBox.Location = new System.Drawing.Point(156, 219);
            this.usernameBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(186, 31);
            this.usernameBox.TabIndex = 1;
            this.usernameBox.TextChanged += new System.EventHandler(this.usernameBox_TextChanged);
            // 
            // ipBox
            // 
            this.ipBox.Location = new System.Drawing.Point(156, 85);
            this.ipBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ipBox.Name = "ipBox";
            this.ipBox.Size = new System.Drawing.Size(186, 31);
            this.ipBox.TabIndex = 2;
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(156, 150);
            this.portBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(186, 31);
            this.portBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 225);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Username:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 294);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(108, 90);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 25);
            this.label3.TabIndex = 6;
            this.label3.Text = "IP:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(90, 156);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 25);
            this.label4.TabIndex = 7;
            this.label4.Text = "Port:";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(60, 377);
            this.connectButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(144, 58);
            this.connectButton.TabIndex = 9;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(224, 377);
            this.disconnectButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(146, 58);
            this.disconnectButton.TabIndex = 10;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(424, 67);
            this.logs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.logs.Name = "logs";
            this.logs.Size = new System.Drawing.Size(300, 467);
            this.logs.TabIndex = 11;
            this.logs.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 560);
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
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.TextBox usernameBox;
        private System.Windows.Forms.TextBox ipBox;
        private System.Windows.Forms.TextBox portBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.RichTextBox logs;
    }
}

