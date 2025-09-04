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
            this.mainLayout = new TableLayoutPanel();
            this.headerPanel = new Panel();
            this.lblTitle = new Label();
            this.lblSubtitle = new Label();
            this.buttonPanel = new Panel();
            this.cardReportIssues = new Panel();
            this.btnReportIssues = new Button();
            this.lblReportDesc = new Label();
            this.cardLocalEvents = new Panel();
            this.btnLocalEvents = new Button();
            this.lblEventsDesc = new Label();
            this.cardServiceStatus = new Panel();
            this.btnServiceStatus = new Button();
            this.lblServiceDesc = new Label();
            this.footerPanel = new Panel();
            this.lblVersion = new Label();
            this.SuspendLayout();
            
            // Main Layout
            this.mainLayout.Dock = DockStyle.Fill;
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.mainLayout.RowCount = 3;
            this.mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 65F));
            this.mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Padding = new Padding(40, 20, 40, 20);
            
            // Header Panel
            this.headerPanel.Dock = DockStyle.Fill;
            this.headerPanel.BackColor = Color.Transparent;
            
            // lblTitle
            this.lblTitle.Dock = DockStyle.Fill;
            this.lblTitle.Font = new Font("Segoe UI", 28F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblTitle.ForeColor = Color.FromArgb(33, 37, 41);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "Municipal Services";
            this.lblTitle.TextAlign = ContentAlignment.BottomCenter;
            
            // lblSubtitle
            this.lblSubtitle.Dock = DockStyle.Bottom;
            this.lblSubtitle.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblSubtitle.ForeColor = Color.FromArgb(108, 117, 125);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new Size(720, 30);
            this.lblSubtitle.Text = "Your Gateway to City Services";
            this.lblSubtitle.TextAlign = ContentAlignment.TopCenter;
            
            this.headerPanel.Controls.Add(this.lblTitle);
            this.headerPanel.Controls.Add(this.lblSubtitle);
            
            // Button Panel
            this.buttonPanel.Dock = DockStyle.Fill;
            this.buttonPanel.BackColor = Color.Transparent;
            this.buttonPanel.Padding = new Padding(50, 20, 50, 20);
            
            // Card - Report Issues
            this.cardReportIssues.BackColor = Color.White;
            this.cardReportIssues.Location = new Point(50, 30);
            this.cardReportIssues.Name = "cardReportIssues";
            this.cardReportIssues.Size = new Size(620, 80);
            this.cardReportIssues.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.cardReportIssues.Padding = new Padding(20, 15, 20, 15);
            
            this.btnReportIssues.BackColor = Color.FromArgb(76, 175, 80);
            this.btnReportIssues.FlatAppearance.BorderSize = 0;
            this.btnReportIssues.FlatStyle = FlatStyle.Flat;
            this.btnReportIssues.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnReportIssues.ForeColor = Color.White;
            this.btnReportIssues.Dock = DockStyle.Left;
            this.btnReportIssues.Name = "btnReportIssues";
            this.btnReportIssues.Size = new Size(180, 50);
            this.btnReportIssues.Text = "Report Issues";
            this.btnReportIssues.UseVisualStyleBackColor = false;
            this.btnReportIssues.Cursor = Cursors.Hand;
            this.btnReportIssues.Click += new EventHandler(this.btnReportIssues_Click);
            
            this.lblReportDesc.Dock = DockStyle.Fill;
            this.lblReportDesc.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblReportDesc.ForeColor = Color.FromArgb(73, 80, 87);
            this.lblReportDesc.Name = "lblReportDesc";
            this.lblReportDesc.Text = "Submit municipal issues like potholes, broken streetlights, and service requests with photo attachments.";
            this.lblReportDesc.TextAlign = ContentAlignment.MiddleLeft;
            this.lblReportDesc.Padding = new Padding(20, 0, 0, 0);
            
            this.cardReportIssues.Controls.Add(this.lblReportDesc);
            this.cardReportIssues.Controls.Add(this.btnReportIssues);
            
            // Card - Local Events
            this.cardLocalEvents.BackColor = Color.White;
            this.cardLocalEvents.Location = new Point(50, 130);
            this.cardLocalEvents.Name = "cardLocalEvents";
            this.cardLocalEvents.Size = new Size(620, 80);
            this.cardLocalEvents.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.cardLocalEvents.Padding = new Padding(20, 15, 20, 15);
            
            this.btnLocalEvents.BackColor = Color.FromArgb(33, 150, 243);
            this.btnLocalEvents.FlatAppearance.BorderSize = 0;
            this.btnLocalEvents.FlatStyle = FlatStyle.Flat;
            this.btnLocalEvents.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnLocalEvents.ForeColor = Color.White;
            this.btnLocalEvents.Dock = DockStyle.Left;
            this.btnLocalEvents.Name = "btnLocalEvents";
            this.btnLocalEvents.Size = new Size(180, 50);
            this.btnLocalEvents.Text = "Local Events";
            this.btnLocalEvents.UseVisualStyleBackColor = false;
            this.btnLocalEvents.Enabled = true;
            this.btnLocalEvents.Cursor = Cursors.Hand;
            this.btnLocalEvents.Click += new EventHandler(this.btnLocalEvents_Click);
            
            this.lblEventsDesc.Dock = DockStyle.Fill;
            this.lblEventsDesc.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblEventsDesc.ForeColor = Color.FromArgb(73, 80, 87);
            this.lblEventsDesc.Name = "lblEventsDesc";
            this.lblEventsDesc.Text = "Browse community events, municipal announcements, and local activities with smart recommendations.";
            this.lblEventsDesc.TextAlign = ContentAlignment.MiddleLeft;
            this.lblEventsDesc.Padding = new Padding(20, 0, 0, 0);
            
            this.cardLocalEvents.Controls.Add(this.lblEventsDesc);
            this.cardLocalEvents.Controls.Add(this.btnLocalEvents);
            
            // Card - Service Status
            this.cardServiceStatus.BackColor = Color.White;
            this.cardServiceStatus.Location = new Point(50, 230);
            this.cardServiceStatus.Name = "cardServiceStatus";
            this.cardServiceStatus.Size = new Size(620, 80);
            this.cardServiceStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.cardServiceStatus.Padding = new Padding(20, 15, 20, 15);
            
            this.btnServiceStatus.BackColor = Color.FromArgb(158, 158, 158);
            this.btnServiceStatus.FlatAppearance.BorderSize = 0;
            this.btnServiceStatus.FlatStyle = FlatStyle.Flat;
            this.btnServiceStatus.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnServiceStatus.ForeColor = Color.White;
            this.btnServiceStatus.Dock = DockStyle.Left;
            this.btnServiceStatus.Name = "btnServiceStatus";
            this.btnServiceStatus.Size = new Size(180, 50);
            this.btnServiceStatus.Text = "Service Status";
            this.btnServiceStatus.UseVisualStyleBackColor = false;
            this.btnServiceStatus.Enabled = false;
            this.btnServiceStatus.Click += new EventHandler(this.btnServiceStatus_Click);
            
            this.lblServiceDesc.Dock = DockStyle.Fill;
            this.lblServiceDesc.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblServiceDesc.ForeColor = Color.FromArgb(158, 158, 158);
            this.lblServiceDesc.Name = "lblServiceDesc";
            this.lblServiceDesc.Text = "Track your service requests, view status updates, and manage your submissions. (Coming in Part 3)";
            this.lblServiceDesc.TextAlign = ContentAlignment.MiddleLeft;
            this.lblServiceDesc.Padding = new Padding(20, 0, 0, 0);
            
            this.cardServiceStatus.Controls.Add(this.lblServiceDesc);
            this.cardServiceStatus.Controls.Add(this.btnServiceStatus);
            
            this.buttonPanel.Controls.Add(this.cardServiceStatus);
            this.buttonPanel.Controls.Add(this.cardLocalEvents);
            this.buttonPanel.Controls.Add(this.cardReportIssues);
            
            // Footer Panel
            this.footerPanel.Dock = DockStyle.Fill;
            this.footerPanel.BackColor = Color.Transparent;
            
            this.lblVersion.Dock = DockStyle.Fill;
            this.lblVersion.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblVersion.ForeColor = Color.FromArgb(134, 142, 150);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Text = "Municipal Services App v2.0 - Part 1 & 2 Complete";
            this.lblVersion.TextAlign = ContentAlignment.BottomCenter;
            
            this.footerPanel.Controls.Add(this.lblVersion);
            
            // Add all panels to main layout
            this.mainLayout.Controls.Add(this.headerPanel, 0, 0);
            this.mainLayout.Controls.Add(this.buttonPanel, 0, 1);
            this.mainLayout.Controls.Add(this.footerPanel, 0, 2);
            
            // MainMenuForm
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new Size(800, 600);
            this.Controls.Add(this.mainLayout);
            this.Name = "MainMenuForm";
            this.ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel mainLayout;
        private Panel headerPanel;
        private Label lblTitle;
        private Label lblSubtitle;
        private Panel buttonPanel;
        private Panel cardReportIssues;
        private Button btnReportIssues;
        private Label lblReportDesc;
        private Panel cardLocalEvents;
        private Button btnLocalEvents;
        private Label lblEventsDesc;
        private Panel cardServiceStatus;
        private Button btnServiceStatus;
        private Label lblServiceDesc;
        private Panel footerPanel;
        private Label lblVersion;
    }
}