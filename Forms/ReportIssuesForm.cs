using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Sidequest_municiple_app {
    public partial class ReportIssuesForm : Form {
        private TextBox txtLocation;
        private ComboBox cmbCategory;
        private RichTextBox txtDescription;
        private Button btnAttachment;
        private Button btnSubmit;
        private Button btnAdmin;
        private ProgressBar progressBar;
        private Label lblProgress;
        private Label lblLocation;
        private Label lblCategory;
        private Label lblDescription;
        private Label lblAttachment;
        private Panel panelSocialMedia;
        private Button btnWhatsApp;
        private Button btnEmail;
        private Button btnSMS;

        private string attachmentPath = "";
        private DatabaseHelper dbHelper;
        private List<Issue> issues;

        public ReportIssuesForm() {
            InitializeComponent();
            SetupForm();
            dbHelper = new DatabaseHelper();
            issues = new List<Issue>();
            UpdateProgress();
        }

        private void SetupForm() {
            Text = "Report Municipal Issues";
            Size = new Size(700, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = AppPalette.Background;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            lblLocation = new Label();
            lblLocation.Text = "Location:";
            lblLocation.Location = new Point(30, 30);
            lblLocation.Size = new Size(80, 23);
            lblLocation.Font = new Font("Segoe UI", 10);
            lblLocation.ForeColor = AppPalette.TextPrimary;
            Controls.Add(lblLocation);

            txtLocation = new TextBox();
            txtLocation.Location = new Point(120, 30);
            txtLocation.Size = new Size(300, 25);
            txtLocation.Font = new Font("Segoe UI", 10);
            txtLocation.BackColor = AppPalette.CodeBlock;
            txtLocation.ForeColor = AppPalette.TextPrimary;
            txtLocation.BorderStyle = BorderStyle.FixedSingle;
            txtLocation.TextChanged += Input_Changed;
            Controls.Add(txtLocation);

            lblCategory = new Label();
            lblCategory.Text = "Category:";
            lblCategory.Location = new Point(30, 80);
            lblCategory.Size = new Size(80, 23);
            lblCategory.Font = new Font("Segoe UI", 10);
            lblCategory.ForeColor = AppPalette.TextPrimary;
            Controls.Add(lblCategory);

            cmbCategory = new ComboBox();
            cmbCategory.Location = new Point(120, 80);
            cmbCategory.Size = new Size(300, 25);
            cmbCategory.Font = new Font("Segoe UI", 10);
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.BackColor = AppPalette.CodeBlock;
            cmbCategory.ForeColor = AppPalette.TextPrimary;
            cmbCategory.FlatStyle = FlatStyle.Flat;
            cmbCategory.Items.AddRange(new object[] { "Sanitation", "Roads", "Utilities", "Water", "Electricity" });
            cmbCategory.SelectedIndexChanged += Input_Changed;
            Controls.Add(cmbCategory);

            lblDescription = new Label();
            lblDescription.Text = "Description:";
            lblDescription.Location = new Point(30, 130);
            lblDescription.Size = new Size(80, 23);
            lblDescription.Font = new Font("Segoe UI", 10);
            lblDescription.ForeColor = AppPalette.TextPrimary;
            Controls.Add(lblDescription);

            txtDescription = new RichTextBox();
            txtDescription.Location = new Point(120, 130);
            txtDescription.Size = new Size(400, 120);
            txtDescription.Font = new Font("Segoe UI", 10);
            txtDescription.BackColor = AppPalette.CodeBlock;
            txtDescription.ForeColor = AppPalette.TextPrimary;
            txtDescription.TextChanged += Input_Changed;
            Controls.Add(txtDescription);

            lblAttachment = new Label();
            lblAttachment.Text = "Attachment:";
            lblAttachment.Location = new Point(30, 270);
            lblAttachment.Size = new Size(80, 23);
            lblAttachment.Font = new Font("Segoe UI", 10);
            lblAttachment.ForeColor = AppPalette.TextPrimary;
            Controls.Add(lblAttachment);

            btnAttachment = new Button();
            btnAttachment.Text = "Browse Files...";
            btnAttachment.Location = new Point(120, 270);
            btnAttachment.Size = new Size(120, 30);
            btnAttachment.BackColor = AppPalette.Surface;
            btnAttachment.FlatStyle = FlatStyle.Flat;
            btnAttachment.FlatAppearance.BorderColor = AppPalette.Border;
            btnAttachment.FlatAppearance.BorderSize = 2;
            btnAttachment.ForeColor = AppPalette.TextPrimary;
            btnAttachment.Font = new Font("Segoe UI", 9);
            btnAttachment.UseVisualStyleBackColor = false;
            btnAttachment.Click += BtnAttachment_Click;
            Controls.Add(btnAttachment);

            btnSubmit = new Button();
            btnSubmit.Text = "Submit Report";
            btnSubmit.Location = new Point(120, 320);
            btnSubmit.Size = new Size(120, 35);
            btnSubmit.BackColor = AppPalette.AccentPrimary;
            btnSubmit.FlatStyle = FlatStyle.Flat;
            btnSubmit.FlatAppearance.BorderSize = 0;
            btnSubmit.ForeColor = AppPalette.TextOnAccent;
            btnSubmit.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnSubmit.UseVisualStyleBackColor = false;
            btnSubmit.Click += BtnSubmit_Click;
            Controls.Add(btnSubmit);

            btnAdmin = new Button();
            btnAdmin.Text = "Admin";
            btnAdmin.Location = new Point(580, 520);
            btnAdmin.Size = new Size(80, 30);
            btnAdmin.BackColor = AppPalette.AccentPrimary;
            btnAdmin.FlatStyle = FlatStyle.Flat;
            btnAdmin.FlatAppearance.BorderSize = 0;
            btnAdmin.ForeColor = AppPalette.TextOnAccent;
            btnAdmin.Font = new Font("Segoe UI", 9);
            btnAdmin.UseVisualStyleBackColor = false;
            btnAdmin.Click += BtnAdmin_Click;
            Controls.Add(btnAdmin);

            SetupSocialMediaPanel();
            SetupProgressBar();
        }

        private void SetupSocialMediaPanel() {
            panelSocialMedia = new Panel();
            panelSocialMedia.Location = new Point(30, 370);
            panelSocialMedia.Size = new Size(400, 80);
            panelSocialMedia.BorderStyle = BorderStyle.FixedSingle;
            panelSocialMedia.BackColor = AppPalette.Surface;

            Label lblShare = new Label();
            lblShare.Text = "Share on Social Media:";
            lblShare.Location = new Point(10, 10);
            lblShare.Size = new Size(150, 20);
            lblShare.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblShare.ForeColor = AppPalette.TextPrimary;
            panelSocialMedia.Controls.Add(lblShare);

            btnWhatsApp = new Button();
            btnWhatsApp.Text = "WhatsApp";
            btnWhatsApp.Location = new Point(10, 40);
            btnWhatsApp.Size = new Size(80, 25);
            btnWhatsApp.BackColor = AppPalette.AccentPrimary;
            btnWhatsApp.Font = new Font("Segoe UI", 8);
            btnWhatsApp.UseVisualStyleBackColor = false;
            btnWhatsApp.ForeColor = AppPalette.TextOnAccent;
            btnWhatsApp.FlatStyle = FlatStyle.Flat;
            btnWhatsApp.FlatAppearance.BorderSize = 0;
            btnWhatsApp.Click += BtnShare_Click;
            panelSocialMedia.Controls.Add(btnWhatsApp);

            btnEmail = new Button();
            btnEmail.Text = "Email";
            btnEmail.Location = new Point(100, 40);
            btnEmail.Size = new Size(80, 25);
            btnEmail.BackColor = AppPalette.AccentPrimary;
            btnEmail.Font = new Font("Segoe UI", 8);
            btnEmail.UseVisualStyleBackColor = false;
            btnEmail.ForeColor = AppPalette.TextOnAccent;
            btnEmail.FlatStyle = FlatStyle.Flat;
            btnEmail.FlatAppearance.BorderSize = 0;
            btnEmail.Click += BtnShare_Click;
            panelSocialMedia.Controls.Add(btnEmail);

            btnSMS = new Button();
            btnSMS.Text = "SMS";
            btnSMS.Location = new Point(190, 40);
            btnSMS.Size = new Size(80, 25);
            btnSMS.BackColor = AppPalette.Surface;
            btnSMS.Font = new Font("Segoe UI", 8);
            btnSMS.UseVisualStyleBackColor = false;
            btnSMS.ForeColor = AppPalette.TextPrimary;
            btnSMS.FlatStyle = FlatStyle.Flat;
            btnSMS.FlatAppearance.BorderColor = AppPalette.Border;
            btnSMS.FlatAppearance.BorderSize = 2;
            btnSMS.Click += BtnShare_Click;
            panelSocialMedia.Controls.Add(btnSMS);

            Controls.Add(panelSocialMedia);
        }

        private void SetupProgressBar() {
            progressBar = new ProgressBar();
            progressBar.Location = new Point(30, 520);
            progressBar.Size = new Size(400, 20);
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 0;
            Controls.Add(progressBar);

            lblProgress = new Label();
            lblProgress.Text = "Progress: 0%";
            lblProgress.Location = new Point(30, 490);
            lblProgress.Size = new Size(100, 20);
            lblProgress.Font = new Font("Segoe UI", 9);
            lblProgress.ForeColor = AppPalette.TextPrimary;
            Controls.Add(lblProgress);
        }

        private void Input_Changed(object sender, EventArgs e) {
            UpdateProgress();
        }

        private void UpdateProgress() {
            int progress = 0;

            if (!string.IsNullOrWhiteSpace(txtLocation.Text)) progress += 25;
            if (cmbCategory.SelectedIndex >= 0) progress += 25;
            if (!string.IsNullOrWhiteSpace(txtDescription.Text)) progress += 25;
            if (!string.IsNullOrEmpty(attachmentPath)) progress += 25;

            progressBar.Value = progress;
            lblProgress.Text = $"Progress: {progress}%";
        }

        private void BtnAttachment_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif|Document Files|*.pdf;*.doc;*.docx|All Files|*.*";
            openFileDialog.Title = "Select Attachment";

            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                attachmentPath = openFileDialog.FileName;
                btnAttachment.Text = Path.GetFileName(attachmentPath);
                UpdateProgress();
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e) {
            if (ValidateInput()) {
                Issue issue = new Issue(
                    txtLocation.Text,
                    cmbCategory.SelectedItem.ToString(),
                    txtDescription.Text,
                    attachmentPath
                );
                issue.Priority = DeterminePriority(issue.Category);

                try {
                    int issueId = dbHelper.SaveIssue(issue);
                    issues.Add(issue);

                    MessageBox.Show($"Issue reported successfully! Reference ID: {issueId:D6}",
                                  "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearForm();
                }
                catch (Exception ex) {
                    MessageBox.Show("Error saving issue: " + ex.Message,
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateInput() {
            if (string.IsNullOrWhiteSpace(txtLocation.Text)) {
                MessageBox.Show("Please enter a location.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbCategory.SelectedIndex < 0) {
                MessageBox.Show("Please select a category.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text)) {
                MessageBox.Show("Please provide a description.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ClearForm() {
            txtLocation.Clear();
            cmbCategory.SelectedIndex = -1;
            txtDescription.Clear();
            attachmentPath = "";
            btnAttachment.Text = "Browse Files...";
            UpdateProgress();
        }

        private void BtnAdmin_Click(object sender, EventArgs e) {
            AdminForm adminForm = new AdminForm();
            adminForm.ShowDialog();
        }

        private void BtnShare_Click(object sender, EventArgs e) {
            MessageBox.Show("Social media sharing feature is not yet implemented.",
                "Feature Not Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private ServiceRequestPriority DeterminePriority(string category) {
            if (string.IsNullOrWhiteSpace(category)) {
                return ServiceRequestPriority.Medium;
            }

            switch (category.ToLowerInvariant()) {
                case "water":
                case "electricity":
                    return ServiceRequestPriority.Urgent;
                case "sanitation":
                    return ServiceRequestPriority.High;
                case "roads":
                    return ServiceRequestPriority.Medium;
                default:
                    return ServiceRequestPriority.Low;
            }
        }

        private void InitializeComponent() {
            SuspendLayout();
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 261);
            Name = "ReportIssuesForm";
            Text = "ReportIssuesForm";
            ResumeLayout(false);
        }
    }
}