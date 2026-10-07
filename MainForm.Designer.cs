namespace STALZONERegionSwitcher
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblRegionCaption = new System.Windows.Forms.Label();
            this.lblRegion = new System.Windows.Forms.Label();
            this.panelFolder = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblFolderPath = new System.Windows.Forms.Label();
            this.btnChangeFolder = new System.Windows.Forms.Button();
            this.btnChangeRegion = new System.Windows.Forms.Button();
            this.btnLaunch = new System.Windows.Forms.Button();
            this.lblSignature = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelFolder.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblRegionCaption
            // 
            this.lblRegionCaption.AutoSize = true;
            this.lblRegionCaption.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblRegionCaption.Location = new System.Drawing.Point(120, 30);
            this.lblRegionCaption.Name = "lblRegionCaption";
            this.lblRegionCaption.Size = new System.Drawing.Size(170, 25);
            this.lblRegionCaption.TabIndex = 0;
            this.lblRegionCaption.Text = "Текущий регион: ";
            // 
            // lblRegion
            // 
            this.lblRegion.AutoSize = true;
            this.lblRegion.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblRegion.Location = new System.Drawing.Point(290, 30);
            this.lblRegion.Name = "lblRegion";
            this.lblRegion.Size = new System.Drawing.Size(0, 25);
            this.lblRegion.TabIndex = 1;
            // 
            // panelFolder
            // 
            this.panelFolder.Controls.Add(this.lblTitle);
            this.panelFolder.Controls.Add(this.lblFolderPath);
            this.panelFolder.Controls.Add(this.btnChangeFolder);
            this.panelFolder.Location = new System.Drawing.Point(40, 160);
            this.panelFolder.Name = "panelFolder";
            this.panelFolder.Size = new System.Drawing.Size(480, 100);
            this.panelFolder.TabIndex = 2;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(16, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(105, 21);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Папка игры";
            // 
            // lblFolderPath
            // 
            this.lblFolderPath.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFolderPath.Location = new System.Drawing.Point(16, 42);
            this.lblFolderPath.Name = "lblFolderPath";
            this.lblFolderPath.Size = new System.Drawing.Size(320, 45);
            this.lblFolderPath.TabIndex = 1;
            this.lblFolderPath.Text = "...";
            // 
            // btnChangeFolder
            // 
            this.btnChangeFolder.Location = new System.Drawing.Point(350, 42);
            this.btnChangeFolder.Name = "btnChangeFolder";
            this.btnChangeFolder.Size = new System.Drawing.Size(110, 36);
            this.btnChangeFolder.TabIndex = 2;
            this.btnChangeFolder.Text = "Изменить";
            this.btnChangeFolder.UseVisualStyleBackColor = false;
            this.btnChangeFolder.Click += new System.EventHandler(this.btnChangeFolder_Click);
            // 
            // btnChangeRegion
            // 
            this.btnChangeRegion.Location = new System.Drawing.Point(80, 90);
            this.btnChangeRegion.Name = "btnChangeRegion";
            this.btnChangeRegion.Size = new System.Drawing.Size(180, 50);
            this.btnChangeRegion.TabIndex = 3;
            this.btnChangeRegion.Text = "Выбрать регион";
            this.btnChangeRegion.UseVisualStyleBackColor = false;
            this.btnChangeRegion.Click += new System.EventHandler(this.btnChangeRegion_Click);
            // 
            // btnLaunch
            // 
            this.btnLaunch.Location = new System.Drawing.Point(300, 90);
            this.btnLaunch.Name = "btnLaunch";
            this.btnLaunch.Size = new System.Drawing.Size(180, 50);
            this.btnLaunch.TabIndex = 4;
            this.btnLaunch.Text = "Запустить игру";
            this.btnLaunch.UseVisualStyleBackColor = false;
            this.btnLaunch.Click += new System.EventHandler(this.btnLaunch_Click);
            // 
            // lblSignature
            // 
            this.lblSignature.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblSignature.Location = new System.Drawing.Point(40, 280);
            this.lblSignature.Name = "lblSignature";
            this.lblSignature.Size = new System.Drawing.Size(480, 20);
            this.lblSignature.TabIndex = 5;
            this.lblSignature.Text = "насрано с помощью нейросети";
            this.lblSignature.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 320);
            this.Controls.Add(this.lblSignature);
            this.Controls.Add(this.btnLaunch);
            this.Controls.Add(this.btnChangeRegion);
            this.Controls.Add(this.panelFolder);
            this.Controls.Add(this.lblRegion);
            this.Controls.Add(this.lblRegionCaption);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "STALZONE Region Switcher";
            this.panelFolder.ResumeLayout(false);
            this.panelFolder.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblRegionCaption;
        private System.Windows.Forms.Label lblRegion;
        private System.Windows.Forms.Panel panelFolder;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblFolderPath;
        private System.Windows.Forms.Button btnChangeFolder;
        private System.Windows.Forms.Button btnChangeRegion;
        private System.Windows.Forms.Button btnLaunch;
        private System.Windows.Forms.Label lblSignature;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
