namespace PureSeeder.Forms
{
    partial class ServerEditor
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
            this.serverList = new System.Windows.Forms.ListBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.closeDialog = new System.Windows.Forms.Button();
            this.addServer = new System.Windows.Forms.Button();
            this.deleteServer = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // serverList
            // 
            this.serverList.FormattingEnabled = true;
            this.serverList.Location = new System.Drawing.Point(12, 12);
            this.serverList.Name = "serverList";
            this.serverList.Size = new System.Drawing.Size(351, 225);
            this.serverList.TabIndex = 0;
            this.serverList.SelectedIndexChanged += new System.EventHandler(this.serverList_SelectedIndexChanged);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(407, 12);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertyGrid1.Size = new System.Drawing.Size(450, 224);
            this.propertyGrid1.TabIndex = 1;
            // 
            // closeDialog
            // 
            this.closeDialog.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.closeDialog.Location = new System.Drawing.Point(782, 243);
            this.closeDialog.Name = "closeDialog";
            this.closeDialog.Size = new System.Drawing.Size(75, 23);
            this.closeDialog.TabIndex = 2;
            this.closeDialog.Text = "OK";
            this.closeDialog.UseVisualStyleBackColor = true;
            this.closeDialog.Click += new System.EventHandler(this.closeDialog_Click);
            // 
            // addServer
            // 
            this.addServer.Location = new System.Drawing.Point(207, 243);
            this.addServer.Name = "addServer";
            this.addServer.Size = new System.Drawing.Size(75, 23);
            this.addServer.TabIndex = 3;
            this.addServer.Text = "Add";
            this.addServer.UseVisualStyleBackColor = true;
            this.addServer.Click += new System.EventHandler(this.addServer_Click);
            // 
            // deleteServer
            // 
            this.deleteServer.Location = new System.Drawing.Point(288, 243);
            this.deleteServer.Name = "deleteServer";
            this.deleteServer.Size = new System.Drawing.Size(75, 23);
            this.deleteServer.TabIndex = 4;
            this.deleteServer.Text = "Delete";
            this.deleteServer.UseVisualStyleBackColor = true;
            this.deleteServer.Click += new System.EventHandler(this.deleteServer_Click);
            // 
            // upButton
            // 
            this.upButton.Image = global::PureSeeder.Forms.Properties.Resources.arrow_Up_16xMD;
            this.upButton.Location = new System.Drawing.Point(369, 12);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(31, 23);
            this.upButton.TabIndex = 5;
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // downButton
            // 
            this.downButton.Image = global::PureSeeder.Forms.Properties.Resources.arrow_Down_16xMD;
            this.downButton.Location = new System.Drawing.Point(369, 41);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(31, 23);
            this.downButton.TabIndex = 6;
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // ServerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 273);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.deleteServer);
            this.Controls.Add(this.addServer);
            this.Controls.Add(this.closeDialog);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.serverList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ServerEditor";
            this.Text = "Servers";
            this.Load += new System.EventHandler(this.ServerEditor_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox serverList;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button closeDialog;
        private System.Windows.Forms.Button addServer;
        private System.Windows.Forms.Button deleteServer;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button downButton;
    }
}