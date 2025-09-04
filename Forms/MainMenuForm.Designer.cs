namespace MunicipalApp.Forms
{
    partial class MainMenuForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.lblSubtitle = new Label();
            this.btnReportIssues = new Button();
            this.btnLocalEvents = new Button();
            this.btnServiceStatus = new Button();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblTitle.ForeColor = Color.FromArgb(25, 118, 210);
            this.lblTitle.Location = new Point(120, 50);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(360, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Municipal Services Hub";
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            
            // lblSubtitle
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblSubtitle.ForeColor = Color.FromArgb(66, 66, 66);
            this.lblSubtitle.Location = new Point(180, 95);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new Size(240, 21);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Connecting Citizens to Services";
            this.lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            
            // btnReportIssues
            this.btnReportIssues.BackColor = Color.FromArgb(76, 175, 80);
            this.btnReportIssues.FlatAppearance.BorderSize = 0;
            this.btnReportIssues.FlatStyle = FlatStyle.Flat;
            this.btnReportIssues.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnReportIssues.ForeColor = Color.White;
            this.btnReportIssues.Location = new Point(200, 150);
            this.btnReportIssues.Name = "btnReportIssues";
            this.btnReportIssues.Size = new Size(200, 50);
            this.btnReportIssues.TabIndex = 2;
            this.btnReportIssues.Text = "Report Issues";
            this.btnReportIssues.UseVisualStyleBackColor = false;
            this.btnReportIssues.Click += new EventHandler(this.btnReportIssues_Click);
            
            // btnLocalEvents
            this.btnLocalEvents.BackColor = Color.FromArgb(158, 158, 158);
            this.btnLocalEvents.FlatAppearance.BorderSize = 0;
            this.btnLocalEvents.FlatStyle = FlatStyle.Flat;
            this.btnLocalEvents.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnLocalEvents.ForeColor = Color.White;
            this.btnLocalEvents.Location = new Point(200, 220);
            this.btnLocalEvents.Name = "btnLocalEvents";
            this.btnLocalEvents.Size = new Size(200, 50);
            this.btnLocalEvents.TabIndex = 3;
            this.btnLocalEvents.Text = "Local Events (Part 2)";
            this.btnLocalEvents.UseVisualStyleBackColor = false;
            this.btnLocalEvents.Enabled = false;
            this.btnLocalEvents.Click += new EventHandler(this.btnLocalEvents_Click);
            
            // btnServiceStatus
            this.btnServiceStatus.BackColor = Color.FromArgb(158, 158, 158);
            this.btnServiceStatus.FlatAppearance.BorderSize = 0;
            this.btnServiceStatus.FlatStyle = FlatStyle.Flat;
            this.btnServiceStatus.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnServiceStatus.ForeColor = Color.White;
            this.btnServiceStatus.Location = new Point(200, 290);
            this.btnServiceStatus.Name = "btnServiceStatus";
            this.btnServiceStatus.Size = new Size(200, 50);
            this.btnServiceStatus.TabIndex = 4;
            this.btnServiceStatus.Text = "Service Status (Part 3)";
            this.btnServiceStatus.UseVisualStyleBackColor = false;
            this.btnServiceStatus.Enabled = false;
            this.btnServiceStatus.Click += new EventHandler(this.btnServiceStatus_Click);
            
            // MainMenuForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(600, 400);
            this.Controls.Add(this.btnServiceStatus);
            this.Controls.Add(this.btnLocalEvents);
            this.Controls.Add(this.btnReportIssues);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.lblTitle);
            this.Name = "MainMenuForm";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblSubtitle;
        private Button btnReportIssues;
        private Button btnLocalEvents;
        private Button btnServiceStatus;
    }
}