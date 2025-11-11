using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Sidequest_municiple_app {
    public partial class MainMenuForm : Form {
        private Button btnReportIssues;
        private Button btnLocalEvents;
        private Button btnServiceStatus;
        private Button btnGenerateTestData;
        private Label lblTitle;
        private LinkLabel lnkAuthor;
        private LinkLabel lnkOfficialGitHub;
        private LinkLabel lnkSubmissionGitHub;
        private LinkLabel lnkReadme;

        public MainMenuForm() {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm() {
            this.Text = "Municipal Services Application";
            this.Size = new Size(1000, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = AppPalette.Background;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblTitle = new Label();
            lblTitle.Text = "South African Municipal Services";
            lblTitle.Font = new Font("Segoe UI", 28, FontStyle.Bold);
            lblTitle.ForeColor = AppPalette.TextHeading;
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(900, 50);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Location = new Point(50, 40);
            this.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Select a service to continue";
            lblSubtitle.Font = new Font("Segoe UI", 12);
            lblSubtitle.ForeColor = AppPalette.TextSecondary;
            lblSubtitle.AutoSize = false;
            lblSubtitle.Size = new Size(900, 25);
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            lblSubtitle.Location = new Point(50, 95);
            this.Controls.Add(lblSubtitle);

            int cardWidth = 700;
            int cardLeft = (1000 - cardWidth) / 2;

            Panel pnlCard1 = new Panel();
            pnlCard1.Size = new Size(cardWidth, 100);
            pnlCard1.Location = new Point(cardLeft, 160);
            pnlCard1.BackColor = AppPalette.Surface;
            pnlCard1.BorderStyle = BorderStyle.None;
            this.Controls.Add(pnlCard1);

            btnReportIssues = new Button();
            btnReportIssues.Text = "Report Issues";
            btnReportIssues.Size = new Size(cardWidth - 20, 80);
            btnReportIssues.Location = new Point(10, 10);
            btnReportIssues.BackColor = AppPalette.Surface;
            btnReportIssues.FlatStyle = FlatStyle.Flat;
            btnReportIssues.FlatAppearance.BorderColor = AppPalette.Border;
            btnReportIssues.FlatAppearance.BorderSize = 2;
            btnReportIssues.ForeColor = AppPalette.TextPrimary;
            btnReportIssues.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btnReportIssues.UseVisualStyleBackColor = false;
            btnReportIssues.Click += BtnReportIssues_Click;
            btnReportIssues.MouseEnter += (s, e) => {
                btnReportIssues.BackColor = AppPalette.CodeBlock;
                btnReportIssues.FlatAppearance.BorderColor = AppPalette.TextSecondary;
            };
            btnReportIssues.MouseLeave += (s, e) => {
                btnReportIssues.BackColor = AppPalette.Surface;
                btnReportIssues.FlatAppearance.BorderColor = AppPalette.Border;
            };
            pnlCard1.Controls.Add(btnReportIssues);

            Panel pnlCard2 = new Panel();
            pnlCard2.Size = new Size(cardWidth, 100);
            pnlCard2.Location = new Point(cardLeft, 280);
            pnlCard2.BackColor = AppPalette.Surface;
            pnlCard2.BorderStyle = BorderStyle.None;
            this.Controls.Add(pnlCard2);

            btnLocalEvents = new Button();
            btnLocalEvents.Text = "Local Events and Announcements";
            btnLocalEvents.Size = new Size(cardWidth - 20, 80);
            btnLocalEvents.Location = new Point(10, 10);
            btnLocalEvents.BackColor = AppPalette.Surface;
            btnLocalEvents.FlatStyle = FlatStyle.Flat;
            btnLocalEvents.FlatAppearance.BorderColor = AppPalette.Border;
            btnLocalEvents.FlatAppearance.BorderSize = 2;
            btnLocalEvents.ForeColor = AppPalette.TextPrimary;
            btnLocalEvents.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btnLocalEvents.Enabled = true;
            btnLocalEvents.Click += BtnLocalEvents_Click;
            btnLocalEvents.UseVisualStyleBackColor = false;
            btnLocalEvents.MouseEnter += (s, e) => {
                btnLocalEvents.BackColor = AppPalette.CodeBlock;
                btnLocalEvents.FlatAppearance.BorderColor = AppPalette.TextSecondary;
            };
            btnLocalEvents.MouseLeave += (s, e) => {
                btnLocalEvents.BackColor = AppPalette.Surface;
                btnLocalEvents.FlatAppearance.BorderColor = AppPalette.Border;
            };
            pnlCard2.Controls.Add(btnLocalEvents);

            Panel pnlCard3 = new Panel();
            pnlCard3.Size = new Size(cardWidth, 100);
            pnlCard3.Location = new Point(cardLeft, 400);
            pnlCard3.BackColor = AppPalette.Surface;
            pnlCard3.BorderStyle = BorderStyle.None;
            this.Controls.Add(pnlCard3);

            btnServiceStatus = new Button();
            btnServiceStatus.Text = "Service Request Status";
            btnServiceStatus.Size = new Size(cardWidth - 20, 80);
            btnServiceStatus.Location = new Point(10, 10);
            btnServiceStatus.BackColor = AppPalette.Surface;
            btnServiceStatus.FlatStyle = FlatStyle.Flat;
            btnServiceStatus.FlatAppearance.BorderColor = AppPalette.Border;
            btnServiceStatus.FlatAppearance.BorderSize = 2;
            btnServiceStatus.ForeColor = AppPalette.TextPrimary;
            btnServiceStatus.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btnServiceStatus.Enabled = true;
            btnServiceStatus.Click += BtnServiceStatus_Click;
            btnServiceStatus.UseVisualStyleBackColor = false;
            btnServiceStatus.MouseEnter += (s, e) => {
                btnServiceStatus.BackColor = AppPalette.CodeBlock;
                btnServiceStatus.FlatAppearance.BorderColor = AppPalette.TextSecondary;
            };
            btnServiceStatus.MouseLeave += (s, e) => {
                btnServiceStatus.BackColor = AppPalette.Surface;
                btnServiceStatus.FlatAppearance.BorderColor = AppPalette.Border;
            };
            pnlCard3.Controls.Add(btnServiceStatus);
            
            btnGenerateTestData = new Button();
            btnGenerateTestData.Text = "Generate Test Data";
            btnGenerateTestData.Size = new Size(200, 45);
            btnGenerateTestData.Location = new Point(270, 530);
            btnGenerateTestData.BackColor = AppPalette.Surface;
            btnGenerateTestData.FlatStyle = FlatStyle.Flat;
            btnGenerateTestData.FlatAppearance.BorderColor = AppPalette.Border;
            btnGenerateTestData.FlatAppearance.BorderSize = 1;
            btnGenerateTestData.ForeColor = AppPalette.TextSecondary;
            btnGenerateTestData.Font = new Font("Segoe UI", 9);
            btnGenerateTestData.UseVisualStyleBackColor = false;
            btnGenerateTestData.Click += BtnGenerateTestData_Click;
            this.Controls.Add(btnGenerateTestData);

            Panel pnlFooter = new Panel();
            pnlFooter.Size = new Size(600, 120);
            pnlFooter.Location = new Point(270, 550);
            pnlFooter.BackColor = AppPalette.Background;
            this.Controls.Add(pnlFooter);

            Label lblMadeBy = new Label();
            lblMadeBy.Text = "Made by";
            lblMadeBy.Font = new Font("Segoe UI", 9);
            lblMadeBy.ForeColor = AppPalette.TextMuted;
            lblMadeBy.AutoSize = true;
            lblMadeBy.Location = new Point(0, 5);
            pnlFooter.Controls.Add(lblMadeBy);

            lnkAuthor = new LinkLabel();
            lnkAuthor.Text = "Max Day";
            lnkAuthor.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lnkAuthor.LinkColor = AppPalette.TextPrimary;
            lnkAuthor.ActiveLinkColor = AppPalette.AccentPrimary;
            lnkAuthor.VisitedLinkColor = AppPalette.TextPrimary;
            lnkAuthor.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkAuthor.AutoSize = true;
            lnkAuthor.Location = new Point(60, 3);
            lnkAuthor.LinkClicked += (s, e) => Process.Start("https://github.com/Max0xDay");
            pnlFooter.Controls.Add(lnkAuthor);

            lnkReadme = new LinkLabel();
            lnkReadme.Text = "Read the README";
            lnkReadme.Font = new Font("Segoe UI", 9);
            lnkReadme.LinkColor = AppPalette.TextSecondary;
            lnkReadme.ActiveLinkColor = AppPalette.AccentPrimary;
            lnkReadme.VisitedLinkColor = AppPalette.TextSecondary;
            lnkReadme.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkReadme.AutoSize = true;
            lnkReadme.Location = new Point(0, 35);
            lnkReadme.LinkClicked += LnkReadme_LinkClicked;
            pnlFooter.Controls.Add(lnkReadme);

            Label lblGitHub = new Label();
            lblGitHub.Text = "GitHub:";
            lblGitHub.Font = new Font("Segoe UI", 9);
            lblGitHub.ForeColor = AppPalette.TextMuted;
            lblGitHub.AutoSize = true;
            lblGitHub.Location = new Point(0, 65);
            pnlFooter.Controls.Add(lblGitHub);

            lnkOfficialGitHub = new LinkLabel();
            lnkOfficialGitHub.Text = "Official Repository";
            lnkOfficialGitHub.Font = new Font("Segoe UI", 9);
            lnkOfficialGitHub.LinkColor = AppPalette.TextSecondary;
            lnkOfficialGitHub.ActiveLinkColor = AppPalette.AccentPrimary;
            lnkOfficialGitHub.VisitedLinkColor = AppPalette.TextSecondary;
            lnkOfficialGitHub.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkOfficialGitHub.AutoSize = true;
            lnkOfficialGitHub.Location = new Point(55, 65);
            lnkOfficialGitHub.LinkClicked += (s, e) => Process.Start("https://github.com/Max0xDay/MunicipalApp");
            pnlFooter.Controls.Add(lnkOfficialGitHub);

            Label lblSeparator = new Label();
            lblSeparator.Text = "|";
            lblSeparator.Font = new Font("Segoe UI", 9);
            lblSeparator.ForeColor = AppPalette.TextMuted;
            lblSeparator.AutoSize = true;
            lblSeparator.Location = new Point(165, 65);
            pnlFooter.Controls.Add(lblSeparator);

            lnkSubmissionGitHub = new LinkLabel();
            lnkSubmissionGitHub.Text = "Submission Repository";
            lnkSubmissionGitHub.Font = new Font("Segoe UI", 9);
            lnkSubmissionGitHub.LinkColor = AppPalette.TextSecondary;
            lnkSubmissionGitHub.ActiveLinkColor = AppPalette.AccentPrimary;
            lnkSubmissionGitHub.VisitedLinkColor = AppPalette.TextSecondary;
            lnkSubmissionGitHub.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkSubmissionGitHub.AutoSize = true;
            lnkSubmissionGitHub.Location = new Point(180, 65);
            lnkSubmissionGitHub.LinkClicked += LnkSubmissionGitHub_LinkClicked;
            pnlFooter.Controls.Add(lnkSubmissionGitHub);

            Label lblNote = new Label();
            lblNote.Text = "Please read the README for important information about this application";
            lblNote.Font = new Font("Segoe UI", 8, FontStyle.Italic);
            lblNote.ForeColor = AppPalette.TextMuted;
            lblNote.AutoSize = true;
            lblNote.Location = new Point(0, 95);
            pnlFooter.Controls.Add(lblNote);
        }

        private void LnkReadme_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            try {
                string readmePath = System.IO.Path.Combine(Application.StartupPath, "..", "..", "README.md");
                if (System.IO.File.Exists(readmePath))
                    Process.Start("notepad.exe", readmePath);
                else
                    MessageBox.Show("README.md not found in project directory.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex) {
                MessageBox.Show("Unable to open README: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LnkSubmissionGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            MessageBox.Show("Submission GitHub repository URL will be provided by instructor.", "Submission Repository", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnReportIssues_Click(object sender, EventArgs e) {
            ReportIssuesForm reportForm = new ReportIssuesForm();
            reportForm.ShowDialog();
        }

        private void BtnLocalEvents_Click(object sender, EventArgs e) {
            try {
                LocalEventsForm eventsForm = new LocalEventsForm();
                eventsForm.ShowDialog();
            } catch (Exception ex) {
                MessageBox.Show("Unable to open Local Events: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnServiceStatus_Click(object sender, EventArgs e) {
            try {
                ServiceRequestStatusForm statusForm = new ServiceRequestStatusForm();
                statusForm.ShowDialog();
            } catch (Exception ex) {
                MessageBox.Show("Unable to open Service Request Status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnGenerateTestData_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show(
                "This will generate test data. Choose:\n\n" +
                "YES - Generate 100 records (comprehensive test)\n" +
                "NO - Generate 25 records (quick test)\n" +
                "CANCEL - Skip generation",
                "Generate Test Data",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Cancel)
                return;

            int recordCount = result == DialogResult.Yes ? 100 : 25;

            try {
                DataSeeder seeder = new DataSeeder();
                
                DialogResult clearResult = MessageBox.Show(
                    "Clear existing data first?",
                    "Clear Data",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (clearResult == DialogResult.Yes)
                    seeder.ClearAllData();

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
            } catch (Exception ex) {
                MessageBox.Show("Error generating test data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent() {
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