using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sidequest_municiple_app
{
    public partial class MainMenuForm : Form
    {
        private Button btnReportIssues;
        private Button btnLocalEvents;
        private Button btnServiceStatus;
        private Button btnGenerateTestData;
        private Label lblTitle;

        public MainMenuForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm() {
            this.Text = "Municipal Services Application";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = AppPalette.Background;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Panel pnlSidebar = new Panel();
            pnlSidebar.Size = new Size(280, 600);
            pnlSidebar.Location = new Point(0, 0);
            pnlSidebar.BackColor = AppPalette.Sidebar;
            pnlSidebar.Dock = DockStyle.Left;
            this.Controls.Add(pnlSidebar);

            Label lblSidebarTitle = new Label();
            lblSidebarTitle.Text = "Municipal Services";
            lblSidebarTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblSidebarTitle.ForeColor = AppPalette.TextHeading;
            lblSidebarTitle.AutoSize = false;
            lblSidebarTitle.Size = new Size(240, 60);
            lblSidebarTitle.Location = new Point(20, 40);
            lblSidebarTitle.TextAlign = ContentAlignment.MiddleLeft;
            pnlSidebar.Controls.Add(lblSidebarTitle);

            Label lblSidebarSubtitle = new Label();
            lblSidebarSubtitle.Text = "South Africa";
            lblSidebarSubtitle.Font = new Font("Segoe UI", 12);
            lblSidebarSubtitle.ForeColor = AppPalette.TextSecondary;
            lblSidebarSubtitle.AutoSize = true;
            lblSidebarSubtitle.Location = new Point(20, 95);
            pnlSidebar.Controls.Add(lblSidebarSubtitle);

            lblTitle = new Label();
            lblTitle.Text = "Welcome to Municipal Services";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = AppPalette.TextHeading;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(330, 50);
            this.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Select a service to continue";
            lblSubtitle.Font = new Font("Segoe UI", 11);
            lblSubtitle.ForeColor = AppPalette.TextSecondary;
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(330, 90);
            this.Controls.Add(lblSubtitle);

            Panel pnlCard1 = new Panel();
            pnlCard1.Size = new Size(520, 80);
            pnlCard1.Location = new Point(330, 150);
            pnlCard1.BackColor = AppPalette.Surface;
            pnlCard1.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(pnlCard1);

            btnReportIssues = new Button();
            btnReportIssues.Text = "Report Issues";
            btnReportIssues.Size = new Size(480, 60);
            btnReportIssues.Location = new Point(10, 10);
            btnReportIssues.BackColor = AppPalette.AccentPrimary;
            btnReportIssues.FlatStyle = FlatStyle.Flat;
            btnReportIssues.FlatAppearance.BorderSize = 0;
            btnReportIssues.ForeColor = AppPalette.TextOnAccent;
            btnReportIssues.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            btnReportIssues.UseVisualStyleBackColor = false;
            btnReportIssues.Click += BtnReportIssues_Click;
            btnReportIssues.MouseEnter += (s, e) => btnReportIssues.BackColor = AppPalette.AccentHover;
            btnReportIssues.MouseLeave += (s, e) => btnReportIssues.BackColor = AppPalette.AccentPrimary;
            pnlCard1.Controls.Add(btnReportIssues);

            Panel pnlCard2 = new Panel();
            pnlCard2.Size = new Size(520, 80);
            pnlCard2.Location = new Point(330, 250);
            pnlCard2.BackColor = AppPalette.Surface;
            pnlCard2.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(pnlCard2);

            btnLocalEvents = new Button();
            btnLocalEvents.Text = "Local Events and Announcements";
            btnLocalEvents.Size = new Size(480, 60);
            btnLocalEvents.Location = new Point(10, 10);
            btnLocalEvents.BackColor = AppPalette.AccentPrimary;
            btnLocalEvents.FlatStyle = FlatStyle.Flat;
            btnLocalEvents.FlatAppearance.BorderSize = 0;
            btnLocalEvents.ForeColor = AppPalette.TextOnAccent;
            btnLocalEvents.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            btnLocalEvents.Enabled = true;
            btnLocalEvents.Click += BtnLocalEvents_Click;
            btnLocalEvents.UseVisualStyleBackColor = false;
            btnLocalEvents.MouseEnter += (s, e) => btnLocalEvents.BackColor = AppPalette.AccentHover;
            btnLocalEvents.MouseLeave += (s, e) => btnLocalEvents.BackColor = AppPalette.AccentPrimary;
            pnlCard2.Controls.Add(btnLocalEvents);

            Panel pnlCard3 = new Panel();
            pnlCard3.Size = new Size(520, 80);
            pnlCard3.Location = new Point(330, 350);
            pnlCard3.BackColor = AppPalette.Surface;
            pnlCard3.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(pnlCard3);

            btnServiceStatus = new Button();
            btnServiceStatus.Text = "Service Request Status";
            btnServiceStatus.Size = new Size(480, 60);
            btnServiceStatus.Location = new Point(10, 10);
            btnServiceStatus.BackColor = AppPalette.AccentPrimary;
            btnServiceStatus.FlatStyle = FlatStyle.Flat;
            btnServiceStatus.FlatAppearance.BorderSize = 0;
            btnServiceStatus.ForeColor = AppPalette.TextOnAccent;
            btnServiceStatus.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            btnServiceStatus.Enabled = true;
            btnServiceStatus.Click += BtnServiceStatus_Click;
            btnServiceStatus.UseVisualStyleBackColor = false;
            btnServiceStatus.MouseEnter += (s, e) => btnServiceStatus.BackColor = AppPalette.AccentHover;
            btnServiceStatus.MouseLeave += (s, e) => btnServiceStatus.BackColor = AppPalette.AccentPrimary;
            pnlCard3.Controls.Add(btnServiceStatus);
            
            btnGenerateTestData = new Button();
            btnGenerateTestData.Text = "Generate Test Data";
            btnGenerateTestData.Size = new Size(240, 45);
            btnGenerateTestData.Location = new Point(20, 500);
            btnGenerateTestData.BackColor = AppPalette.Surface;
            btnGenerateTestData.FlatStyle = FlatStyle.Flat;
            btnGenerateTestData.FlatAppearance.BorderColor = AppPalette.Border;
            btnGenerateTestData.FlatAppearance.BorderSize = 2;
            btnGenerateTestData.ForeColor = AppPalette.TextPrimary;
            btnGenerateTestData.Font = new Font("Segoe UI", 10);
            btnGenerateTestData.UseVisualStyleBackColor = false;
            btnGenerateTestData.Click += BtnGenerateTestData_Click;
            pnlSidebar.Controls.Add(btnGenerateTestData);
        }

        private void BtnReportIssues_Click(object sender, EventArgs e)
        {
            ReportIssuesForm reportForm = new ReportIssuesForm();
            reportForm.ShowDialog();
        }

        private void BtnLocalEvents_Click(object sender, EventArgs e)
        {
            try
            {
                LocalEventsForm eventsForm = new LocalEventsForm();
                eventsForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open Local Events: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnServiceStatus_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceRequestStatusForm statusForm = new ServiceRequestStatusForm();
                statusForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open Service Request Status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnGenerateTestData_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "This will generate test data. Choose:\n\n" +
                "YES - Generate 100 records (comprehensive test)\n" +
                "NO - Generate 25 records (quick test)\n" +
                "CANCEL - Skip generation",
                "Generate Test Data",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Cancel)
            {
                return;
            }

            int recordCount = result == DialogResult.Yes ? 100 : 25;

            try
            {
                DataSeeder seeder = new DataSeeder();
                
                DialogResult clearResult = MessageBox.Show(
                    "Clear existing data first?",
                    "Clear Data",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (clearResult == DialogResult.Yes)
                {
                    seeder.ClearAllData();
                }

                seeder.SeedData(recordCount);
                seeder.SeedRelatedIssues(10, 5);

                MessageBox.Show(
                    string.Format("Successfully generated {0} test records plus 50 related cluster records!\n\n" +
                    "Data includes:\n" +
                    "- Mixed categories (Water, Electricity, Sanitation, Roads, Utilities)\n" +
                    "- Varied locations across 30 streets\n" +
                    "- Random dates over 6 months\n" +
                    "- Related issue clusters for graph testing\n\n" +
                    "Open Service Request Status to see all data structures in action!", 
                    recordCount),
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating test data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(284, 261);
            this.Name = "MainMenuForm";
            this.Text = "MainMenuForm";
            this.ResumeLayout(false);
        }
    }
}