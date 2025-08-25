namespace WingetUpdater
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lvApps = new ListView();
            btnUpdate = new Button();
            btnCancel = new Button();
            lblStatus = new Label();
            statusStrip1 = new StatusStrip();
            stsbar = new ToolStripStatusLabel();
            toolStripProgressBar1 = new ToolStripProgressBar();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // lvApps
            // 
            lvApps.CheckBoxes = true;
            lvApps.Location = new Point(6, 12);
            lvApps.Name = "lvApps";
            lvApps.Size = new Size(782, 370);
            lvApps.TabIndex = 0;
            lvApps.UseCompatibleStateImageBehavior = false;
            lvApps.View = View.List;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(589, 394);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(94, 29);
            btnUpdate.TabIndex = 1;
            btnUpdate.Text = "Update";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(689, 394);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(12, 394);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 20);
            lblStatus.TabIndex = 3;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { stsbar, toolStripProgressBar1 });
            statusStrip1.Location = new Point(0, 426);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(792, 24);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // stsbar
            // 
            stsbar.Name = "stsbar";
            stsbar.Size = new Size(0, 18);
            // 
            // toolStripProgressBar1
            // 
            toolStripProgressBar1.Name = "toolStripProgressBar1";
            toolStripProgressBar1.Size = new Size(100, 16);
            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(792, 450);
            Controls.Add(statusStrip1);
            Controls.Add(lblStatus);
            Controls.Add(btnCancel);
            Controls.Add(btnUpdate);
            Controls.Add(lvApps);
            Name = "Form1";
            Text = "Winget Updater";
            Load += Form1_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView lvApps;
        private Button btnUpdate;
        private Button btnCancel;
        private Label lblStatus;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel stsbar;
        private ToolStripProgressBar toolStripProgressBar1;
    }
}
