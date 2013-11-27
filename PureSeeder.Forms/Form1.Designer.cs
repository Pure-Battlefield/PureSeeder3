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
            this.label2 = new System.Windows.Forms.Label();
            this.curPlayers = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.maxPlayers = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.geckoWebBrowser1 = new Gecko.GeckoWebBrowser();
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(897, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // geckoWebBrowser1
            // 
            this.geckoWebBrowser1.Location = new System.Drawing.Point(3, 3);
            this.geckoWebBrowser1.Name = "geckoWebBrowser1";
            this.geckoWebBrowser1.Size = new System.Drawing.Size(1088, 603);
            this.geckoWebBrowser1.TabIndex = 0;
            this.geckoWebBrowser1.UseHttpActivityObserver = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 793);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.maxPlayers);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.curPlayers);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.browserPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.serverSelector);
            this.Name = "Form1";
            this.Text = "Form1";
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
        private System.Windows.Forms.Button button1;
        private Gecko.GeckoWebBrowser geckoWebBrowser1;
    }
}

