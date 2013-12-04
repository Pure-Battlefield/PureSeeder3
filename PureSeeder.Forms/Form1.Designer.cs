namespace PureSeeder.Forms
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
            this.serverSelector = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.webControlBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.browserPanel = new System.Windows.Forms.Panel();
            this.geckoWebBrowser1 = new Gecko.GeckoWebBrowser();
            this.label2 = new System.Windows.Forms.Label();
            this.curPlayers = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.maxPlayers = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.gameHangDetection = new System.Windows.Forms.CheckBox();
            this.logging = new System.Windows.Forms.CheckBox();
            this.seedingEnabled = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.webControlBindingSource)).BeginInit();
            this.browserPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // serverSelector
            // 
            this.serverSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serverSelector.FormattingEnabled = true;
            this.serverSelector.Location = new System.Drawing.Point(12, 29);
            this.serverSelector.Name = "serverSelector";
            this.serverSelector.Size = new System.Drawing.Size(310, 21);
            this.serverSelector.TabIndex = 0;
            this.serverSelector.SelectionChangeCommitted += new System.EventHandler(this.serverSelector_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server";
            // 
            // webControlBindingSource
            // 
            this.webControlBindingSource.DataSource = typeof(Awesomium.Core.IWebView);
            // 
            // browserPanel
            // 
            this.browserPanel.Controls.Add(this.geckoWebBrowser1);
            this.browserPanel.Location = new System.Drawing.Point(12, 124);
            this.browserPanel.Name = "browserPanel";
            this.browserPanel.Size = new System.Drawing.Size(1094, 609);
            this.browserPanel.TabIndex = 3;
            // 
            // geckoWebBrowser1
            // 
            this.geckoWebBrowser1.Location = new System.Drawing.Point(3, 3);
            this.geckoWebBrowser1.Name = "geckoWebBrowser1";
            this.geckoWebBrowser1.Size = new System.Drawing.Size(1088, 603);
            this.geckoWebBrowser1.TabIndex = 0;
            this.geckoWebBrowser1.UseHttpActivityObserver = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(435, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Current Players:";
            // 
            // curPlayers
            // 
            this.curPlayers.AutoSize = true;
            this.curPlayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.curPlayers.Location = new System.Drawing.Point(562, 30);
            this.curPlayers.Name = "curPlayers";
            this.curPlayers.Size = new System.Drawing.Size(31, 20);
            this.curPlayers.TabIndex = 5;
            this.curPlayers.Text = "cur";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(599, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "/";
            // 
            // maxPlayers
            // 
            this.maxPlayers.AutoSize = true;
            this.maxPlayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxPlayers.Location = new System.Drawing.Point(618, 30);
            this.maxPlayers.Name = "maxPlayers";
            this.maxPlayers.Size = new System.Drawing.Size(38, 20);
            this.maxPlayers.TabIndex = 7;
            this.maxPlayers.Text = "max";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(12, 79);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(200, 20);
            this.username.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Username";
            // 
            // gameHangDetection
            // 
            this.gameHangDetection.AutoSize = true;
            this.gameHangDetection.Location = new System.Drawing.Point(849, 58);
            this.gameHangDetection.Name = "gameHangDetection";
            this.gameHangDetection.Size = new System.Drawing.Size(134, 17);
            this.gameHangDetection.TabIndex = 11;
            this.gameHangDetection.Text = "Game Hang Protection";
            this.gameHangDetection.UseVisualStyleBackColor = true;
            // 
            // logging
            // 
            this.logging.AutoSize = true;
            this.logging.Location = new System.Drawing.Point(849, 81);
            this.logging.Name = "logging";
            this.logging.Size = new System.Drawing.Size(64, 17);
            this.logging.TabIndex = 12;
            this.logging.Text = "Logging";
            this.logging.UseVisualStyleBackColor = true;
            // 
            // seedingEnabled
            // 
            this.seedingEnabled.AutoSize = true;
            this.seedingEnabled.Location = new System.Drawing.Point(849, 32);
            this.seedingEnabled.Name = "seedingEnabled";
            this.seedingEnabled.Size = new System.Drawing.Size(107, 17);
            this.seedingEnabled.TabIndex = 13;
            this.seedingEnabled.Text = "Seeding Enabled";
            this.seedingEnabled.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 793);
            this.Controls.Add(this.seedingEnabled);
            this.Controls.Add(this.logging);
            this.Controls.Add(this.gameHangDetection);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.username);
            this.Controls.Add(this.maxPlayers);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.curPlayers);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.browserPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.serverSelector);
            this.Name = "Form1";
            this.Text = "Pure Seeder 3";
            ((System.ComponentModel.ISupportInitialize)(this.webControlBindingSource)).EndInit();
            this.browserPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox serverSelector;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource webControlBindingSource;
        private System.Windows.Forms.Panel browserPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label curPlayers;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label maxPlayers;
        private Gecko.GeckoWebBrowser geckoWebBrowser1;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox gameHangDetection;
        private System.Windows.Forms.CheckBox logging;
        private System.Windows.Forms.CheckBox seedingEnabled;
    }
}

