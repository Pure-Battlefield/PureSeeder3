namespace PureSeeder.Forms
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
            this.label5 = new System.Windows.Forms.Label();
            this.currentLoggedInUser = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SeedingMinPlayers = new System.Windows.Forms.TextBox();
            this.SeedingMaxPlayers = new System.Windows.Forms.TextBox();
            this.saveSettings = new System.Windows.Forms.Button();
            this.seedingEnabled = new System.Windows.Forms.CheckBox();
            this.logging = new System.Windows.Forms.CheckBox();
            this.joinServerButton = new System.Windows.Forms.Button();
            this.refreshInterval = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.refresh = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimizeWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimizeToTray = new System.Windows.Forms.CheckBox();
            this.password = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSettingsDialog = new System.Windows.Forms.SaveFileDialog();
            this.importSettingsDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.webControlBindingSource)).BeginInit();
            this.browserPanel.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
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
            // browserPanel
            // 
            this.browserPanel.Controls.Add(this.geckoWebBrowser1);
            this.browserPanel.Location = new System.Drawing.Point(12, 174);
            this.browserPanel.Name = "browserPanel";
            this.browserPanel.Size = new System.Drawing.Size(1100, 500);
            this.browserPanel.TabIndex = 3;
            // 
            // geckoWebBrowser1
            // 
            this.geckoWebBrowser1.Location = new System.Drawing.Point(3, 3);
            this.geckoWebBrowser1.Name = "geckoWebBrowser1";
            this.geckoWebBrowser1.Size = new System.Drawing.Size(1100, 500);
            this.geckoWebBrowser1.TabIndex = 0;
            this.geckoWebBrowser1.UseHttpActivityObserver = false;
            this.geckoWebBrowser1.DomContentChanged += new System.EventHandler<Gecko.DomEventArgs>(this.geckoWebBrowser1_DomContentChanged);
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
            this.username.Location = new System.Drawing.Point(18, 148);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(144, 20);
            this.username.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Seeder Username";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(435, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 20);
            this.label5.TabIndex = 14;
            this.label5.Text = "Current User:";
            // 
            // currentLoggedInUser
            // 
            this.currentLoggedInUser.AutoSize = true;
            this.currentLoggedInUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentLoggedInUser.Location = new System.Drawing.Point(562, 49);
            this.currentLoggedInUser.Name = "currentLoggedInUser";
            this.currentLoggedInUser.Size = new System.Drawing.Size(47, 20);
            this.currentLoggedInUser.TabIndex = 15;
            this.currentLoggedInUser.Text = "None";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Seeding Min Players:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Seeding Max Players:";
            // 
            // SeedingMinPlayers
            // 
            this.SeedingMinPlayers.Location = new System.Drawing.Point(127, 55);
            this.SeedingMinPlayers.Name = "SeedingMinPlayers";
            this.SeedingMinPlayers.Size = new System.Drawing.Size(100, 20);
            this.SeedingMinPlayers.TabIndex = 18;
            // 
            // SeedingMaxPlayers
            // 
            this.SeedingMaxPlayers.Location = new System.Drawing.Point(127, 78);
            this.SeedingMaxPlayers.Name = "SeedingMaxPlayers";
            this.SeedingMaxPlayers.Size = new System.Drawing.Size(100, 20);
            this.SeedingMaxPlayers.TabIndex = 19;
            // 
            // saveSettings
            // 
            this.saveSettings.Location = new System.Drawing.Point(968, 145);
            this.saveSettings.Name = "saveSettings";
            this.saveSettings.Size = new System.Drawing.Size(134, 23);
            this.saveSettings.TabIndex = 20;
            this.saveSettings.Text = "Save Settings";
            this.saveSettings.UseVisualStyleBackColor = true;
            this.saveSettings.Click += new System.EventHandler(this.saveSettings_Click);
            // 
            // seedingEnabled
            // 
            this.seedingEnabled.AutoSize = true;
            this.seedingEnabled.Location = new System.Drawing.Point(968, 29);
            this.seedingEnabled.Name = "seedingEnabled";
            this.seedingEnabled.Size = new System.Drawing.Size(107, 17);
            this.seedingEnabled.TabIndex = 23;
            this.seedingEnabled.Text = "Seeding Enabled";
            this.seedingEnabled.UseVisualStyleBackColor = true;
            // 
            // logging
            // 
            this.logging.AutoSize = true;
            this.logging.Enabled = false;
            this.logging.Location = new System.Drawing.Point(968, 75);
            this.logging.Name = "logging";
            this.logging.Size = new System.Drawing.Size(64, 17);
            this.logging.TabIndex = 22;
            this.logging.Text = "Logging";
            this.logging.UseVisualStyleBackColor = true;
            // 
            // joinServerButton
            // 
            this.joinServerButton.Location = new System.Drawing.Point(439, 145);
            this.joinServerButton.Name = "joinServerButton";
            this.joinServerButton.Size = new System.Drawing.Size(217, 23);
            this.joinServerButton.TabIndex = 24;
            this.joinServerButton.Text = "Seed Now";
            this.joinServerButton.UseVisualStyleBackColor = true;
            this.joinServerButton.Click += new System.EventHandler(this.joinServerButton_Click);
            // 
            // refreshInterval
            // 
            this.refreshInterval.Location = new System.Drawing.Point(566, 71);
            this.refreshInterval.Name = "refreshInterval";
            this.refreshInterval.Size = new System.Drawing.Size(43, 20);
            this.refreshInterval.TabIndex = 25;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(435, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(126, 20);
            this.label8.TabIndex = 26;
            this.label8.Text = "Refresh Interval:";
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(615, 69);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(75, 23);
            this.refresh.TabIndex = 27;
            this.refresh.Text = "Refresh";
            this.refresh.UseVisualStyleBackColor = true;
            this.refresh.Click += new System.EventHandler(this.refresh_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showWindowToolStripMenuItem,
            this.minimizeWindowToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(171, 48);
            // 
            // showWindowToolStripMenuItem
            // 
            this.showWindowToolStripMenuItem.Name = "showWindowToolStripMenuItem";
            this.showWindowToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.showWindowToolStripMenuItem.Text = "Show Window";
            this.showWindowToolStripMenuItem.Click += new System.EventHandler(this.showWindowToolStripMenuItem_Click);
            // 
            // minimizeWindowToolStripMenuItem
            // 
            this.minimizeWindowToolStripMenuItem.Name = "minimizeWindowToolStripMenuItem";
            this.minimizeWindowToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.minimizeWindowToolStripMenuItem.Text = "Minimize Window";
            this.minimizeWindowToolStripMenuItem.Click += new System.EventHandler(this.minimizeWindowToolStripMenuItem_Click);
            // 
            // minimizeToTray
            // 
            this.minimizeToTray.AutoSize = true;
            this.minimizeToTray.Location = new System.Drawing.Point(968, 52);
            this.minimizeToTray.Name = "minimizeToTray";
            this.minimizeToTray.Size = new System.Drawing.Size(106, 17);
            this.minimizeToTray.TabIndex = 28;
            this.minimizeToTray.Text = "Minimize To Tray";
            this.minimizeToTray.UseVisualStyleBackColor = true;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(178, 148);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(144, 20);
            this.password.TabIndex = 29;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(175, 132);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 13);
            this.label9.TabIndex = 30;
            this.label9.Text = "Seeder Password";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1126, 24);
            this.menuStrip1.TabIndex = 31;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // saveSettingsDialog
            // 
            this.saveSettingsDialog.DefaultExt = "json";
            this.saveSettingsDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            this.saveSettingsDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveSettingsDialog_FileOk);
            // 
            // importSettingsDialog
            // 
            this.importSettingsDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            this.importSettingsDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.importSettingsDialog_FileOk);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1126, 694);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.password);
            this.Controls.Add(this.minimizeToTray);
            this.Controls.Add(this.refresh);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.refreshInterval);
            this.Controls.Add(this.joinServerButton);
            this.Controls.Add(this.seedingEnabled);
            this.Controls.Add(this.saveSettings);
            this.Controls.Add(this.logging);
            this.Controls.Add(this.SeedingMaxPlayers);
            this.Controls.Add(this.SeedingMinPlayers);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.currentLoggedInUser);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.username);
            this.Controls.Add(this.maxPlayers);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.curPlayers);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.browserPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.serverSelector);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1142, 733);
            this.Name = "MainForm";
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.webControlBindingSource)).EndInit();
            this.browserPanel.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label currentLoggedInUser;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox SeedingMinPlayers;
        private System.Windows.Forms.TextBox SeedingMaxPlayers;
        private System.Windows.Forms.Button saveSettings;
        private System.Windows.Forms.CheckBox seedingEnabled;
        private System.Windows.Forms.CheckBox logging;
        private System.Windows.Forms.Button joinServerButton;
        private System.Windows.Forms.TextBox refreshInterval;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button refresh;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.CheckBox minimizeToTray;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minimizeWindowToolStripMenuItem;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveSettingsDialog;
        private System.Windows.Forms.OpenFileDialog importSettingsDialog;
    }
}

