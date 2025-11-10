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

        private void SetupForm()
        {
            this.Text = "Municipal Services Application";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = AppPalette.Background;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblTitle = new Label();
            lblTitle.Text = "South African Municipal Services";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.ForeColor = AppPalette.TextPrimary;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(150, 50);
            this.Controls.Add(lblTitle);

            btnReportIssues = new Button();
            btnReportIssues.Text = "Report Issues";
            btnReportIssues.Size = new Size(200, 50);
            btnReportIssues.Location = new Point(200, 120);
            btnReportIssues.BackColor = AppPalette.AccentSecondary;
            btnReportIssues.FlatStyle = FlatStyle.Flat;
            btnReportIssues.FlatAppearance.BorderColor = AppPalette.Border;
            btnReportIssues.FlatAppearance.BorderSize = 1;
            btnReportIssues.ForeColor = AppPalette.TextOnAccent;
            btnReportIssues.Font = new Font("Segoe UI", 12);
            btnReportIssues.UseVisualStyleBackColor = false;
            btnReportIssues.Click += BtnReportIssues_Click;
            this.Controls.Add(btnReportIssues);

            btnLocalEvents = new Button();
            btnLocalEvents.Text = "Local Events and Announcements";
            btnLocalEvents.Size = new Size(200, 50);
            btnLocalEvents.Location = new Point(200, 180);
            btnLocalEvents.BackColor = AppPalette.AccentSecondary;
            btnLocalEvents.FlatStyle = FlatStyle.Flat;
            btnLocalEvents.FlatAppearance.BorderColor = AppPalette.Border;
            btnLocalEvents.FlatAppearance.BorderSize = 1;
            btnLocalEvents.ForeColor = AppPalette.TextOnAccent;
            btnLocalEvents.Font = new Font("Segoe UI", 10);
            btnLocalEvents.Enabled = true;
            btnLocalEvents.Click += BtnLocalEvents_Click;
            btnLocalEvents.UseVisualStyleBackColor = false;
            btnLocalEvents.AutoSize = false;
            btnLocalEvents.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(btnLocalEvents);

            btnServiceStatus = new Button();
            btnServiceStatus.Text = "Service Request Status";
            btnServiceStatus.Size = new Size(200, 50);
            btnServiceStatus.Location = new Point(200, 240);
            btnServiceStatus.BackColor = AppPalette.AccentSecondary;
            btnServiceStatus.FlatStyle = FlatStyle.Flat;
            btnServiceStatus.FlatAppearance.BorderColor = AppPalette.Border;
            btnServiceStatus.FlatAppearance.BorderSize = 1;
            btnServiceStatus.ForeColor = AppPalette.TextOnAccent;
            btnServiceStatus.Font = new Font("Segoe UI", 12);
            btnServiceStatus.Enabled = true;
            btnServiceStatus.Click += BtnServiceStatus_Click;
            btnServiceStatus.UseVisualStyleBackColor = false;
            this.Controls.Add(btnServiceStatus);
            
            btnGenerateTestData = new Button();
            btnGenerateTestData.Text = "Generate Test Data";
            btnGenerateTestData.Size = new Size(200, 40);
            btnGenerateTestData.Location = new Point(200, 305);
            btnGenerateTestData.BackColor = Color.FromArgb(100, 149, 237);
            btnGenerateTestData.FlatStyle = FlatStyle.Flat;
            btnGenerateTestData.FlatAppearance.BorderColor = AppPalette.Border;
            btnGenerateTestData.FlatAppearance.BorderSize = 1;
            btnGenerateTestData.ForeColor = Color.White;
            btnGenerateTestData.Font = new Font("Segoe UI", 9);
            btnGenerateTestData.UseVisualStyleBackColor = false;
            btnGenerateTestData.Click += BtnGenerateTestData_Click;
            this.Controls.Add(btnGenerateTestData);
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