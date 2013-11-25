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
            this.webControl1 = new Awesomium.Windows.Forms.WebControl(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.webControlBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.webControlBindingSource)).BeginInit();
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
            this.serverSelector.SelectedIndexChanged += new System.EventHandler(this.serverSelector_SelectedIndexChanged);
            // 
            // webControl1
            // 
            this.webControl1.Location = new System.Drawing.Point(12, 69);
            this.webControl1.Size = new System.Drawing.Size(1094, 712);
            this.webControl1.Source = new System.Uri("about:blank", System.UriKind.Absolute);
            this.webControl1.TabIndex = 1;
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 793);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.webControl1);
            this.Controls.Add(this.serverSelector);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.webControlBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox serverSelector;
        private Awesomium.Windows.Forms.WebControl webControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource webControlBindingSource;
    }
}

