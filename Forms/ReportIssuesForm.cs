using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Sidequest_municiple_app {
    public partial class ReportIssuesForm : Form {
        private TextBox txtTitle;
        private TextBox txtLocation;
        private ComboBox cmbCategory;
        private RichTextBox txtDescription;
        private Button btnAttachment;
        private Button btnSubmit;
        private Button btnAdmin;
        private Button btnBack;
        private ProgressBar progressBar;
        private Label lblProgress;
        private Label lblProgressMessage;
        private Label lblTitle;
        private Label lblTitleField;
        private Label lblLocation;
        private Label lblCategory;
        private Label lblDescription;
        private Label lblAttachment;
        private Label lblSuggestion;
        private Panel pnlHeader;
        private Panel pnlForm;
        private Panel pnlSocialMedia;
        private Panel pnlProgress;
        private Panel pnlAttachments;
        private FlowLayoutPanel flowAttachments;
        private Button btnWhatsApp;
        private Button btnFacebook;
        private Button btnTwitter;
        private Button btnEmail;

        private List<string> attachmentPaths;
        private DatabaseHelper dbHelper;
        private List<Issue> issues;
        private IssuePredictionTrie predictionTrie;
        private string currentSuggestion;

        public ReportIssuesForm() {
            InitializeComponent();
            SetupForm();
            dbHelper = new DatabaseHelper();
            issues = new List<Issue>();
            attachmentPaths = new List<string>();
            predictionTrie = new IssuePredictionTrie();
            currentSuggestion = string.Empty;
            UpdateProgress();
        }

        private void SetupForm() {
            Text = "Report Municipal Issues";
            Size = new Size(1250, 850);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = AppPalette.Background;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(1250, 850);

            lblTitle = new Label {
                Text = "Report Municipal Issues",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = AppPalette.TextHeading,
                AutoSize = true,
                Location = new Point(50, 30)
            };
            Controls.Add(lblTitle);

            pnlHeader = new Panel {
                Location = new Point(50, 70),
                Size = new Size(950, 40),
                BackColor = AppPalette.CodeBlock,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(pnlHeader);

            Label lblSubtitle = new Label {
                Text = "Help us improve your community - every report counts",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = AppPalette.TextHeading,
                AutoSize = true,
                Location = new Point(20, 10)
            };
            pnlHeader.Controls.Add(lblSubtitle);

            Panel pnlTopButtons = new Panel {
                Location = new Point(1010, 70),
                Size = new Size(158, 40),
                BackColor = AppPalette.Surface,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(pnlTopButtons);

            btnBack = new Button {
                Text = "Back",
                Size = new Size(140, 30),
                Location = new Point(5, 5),
                BackColor = AppPalette.Surface,
                FlatStyle = FlatStyle.Flat,
                ForeColor = AppPalette.TextPrimary,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBack.FlatAppearance.BorderColor = AppPalette.Border;
            btnBack.FlatAppearance.BorderSize = 2;
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += (s, e) => Close();
            pnlTopButtons.Controls.Add(btnBack);

            pnlForm = new Panel {
                Location = new Point(50, 125),
                Size = new Size(1118, 480),
                BackColor = AppPalette.Surface,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = false
            };
            Controls.Add(pnlForm);

            lblTitleField = new Label {
                Text = "Issue Title:",
                Location = new Point(30, 25),
                Size = new Size(150, 23),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = AppPalette.TextPrimary
            };
            pnlForm.Controls.Add(lblTitleField);

            txtTitle = new TextBox {
                Location = new Point(30, 50),
                Size = new Size(800, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = AppPalette.CodeBlock,
                ForeColor = AppPalette.TextPrimary,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtTitle.TextChanged += TxtTitle_TextChanged;
            txtTitle.KeyDown += TxtTitle_KeyDown;
            pnlForm.Controls.Add(txtTitle);

            lblSuggestion = new Label {
                Text = string.Empty,
                Location = new Point(35, 55),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = AppPalette.TextMuted,
                BackColor = Color.Transparent,
                Visible = false
            };
            pnlForm.Controls.Add(lblSuggestion);
            lblSuggestion.BringToFront();

            Label lblTitleHint = new Label {
                Text = "Start typing and press SPACE to accept suggestions",
                Location = new Point(30, 85),
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = AppPalette.TextMuted
            };
            pnlForm.Controls.Add(lblTitleHint);

            lblLocation = new Label {
                Text = "Location of Issue:",
                Location = new Point(30, 120),
                Size = new Size(150, 23),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = AppPalette.TextPrimary
            };
            pnlForm.Controls.Add(lblLocation);

            txtLocation = new TextBox {
                Location = new Point(30, 145),
                Size = new Size(500, 30),
                Font = new Font("Segoe UI", 11),
                BackColor = AppPalette.CodeBlock,
                ForeColor = AppPalette.TextPrimary,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtLocation.TextChanged += Input_Changed;
            pnlForm.Controls.Add(txtLocation);

            Label lblLocationHint = new Label {
                Text = "e.g., 123 Main Street, Ward 5, or Near Central Park",
                Location = new Point(30, 180),
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = AppPalette.TextMuted
            };
            pnlForm.Controls.Add(lblLocationHint);

            lblCategory = new Label {
                Text = "Issue Category:",
                Location = new Point(580, 120),
                Size = new Size(150, 23),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = AppPalette.TextPrimary
            };
            pnlForm.Controls.Add(lblCategory);

            cmbCategory = new ComboBox {
                Location = new Point(580, 145),
                Size = new Size(250, 30),
                Font = new Font("Segoe UI", 11),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = AppPalette.CodeBlock,
                ForeColor = AppPalette.TextPrimary,
                FlatStyle = FlatStyle.Flat
            };
            cmbCategory.Items.AddRange(new object[] { 
                "Sanitation", 
                "Roads", 
                "Utilities", 
                "Water", 
                "Electricity" 
            });
            cmbCategory.SelectedIndexChanged += Input_Changed;
            pnlForm.Controls.Add(cmbCategory);

            Label lblCategoryHint = new Label {
                Text = "Select the type of issue you're reporting",
                Location = new Point(580, 180),
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = AppPalette.TextMuted
            };
            pnlForm.Controls.Add(lblCategoryHint);

            lblDescription = new Label {
                Text = "Detailed Description:",
                Location = new Point(30, 210),
                Size = new Size(200, 23),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = AppPalette.TextPrimary
            };
            pnlForm.Controls.Add(lblDescription);

            txtDescription = new RichTextBox {
                Location = new Point(30, 235),
                Size = new Size(800, 90),
                Font = new Font("Segoe UI", 10),
                BackColor = AppPalette.CodeBlock,
                ForeColor = AppPalette.TextPrimary,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtDescription.TextChanged += Input_Changed;
            pnlForm.Controls.Add(txtDescription);

            Label lblDescriptionHint = new Label {
                Text = "Provide additional details to help us address the issue",
                Location = new Point(30, 330),
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = AppPalette.TextMuted
            };
            pnlForm.Controls.Add(lblDescriptionHint);

            lblAttachment = new Label {
                Text = "Attach Photos or Documents:",
                Location = new Point(30, 300),
                Size = new Size(250, 23),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = AppPalette.TextPrimary
            };
            pnlForm.Controls.Add(lblAttachment);

            btnAttachment = new Button {
                Text = "+ Add Files",
                Location = new Point(30, 325),
                Size = new Size(150, 35),
                BackColor = AppPalette.AccentPrimary,
                FlatStyle = FlatStyle.Flat,
                ForeColor = AppPalette.TextOnAccent,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAttachment.FlatAppearance.BorderSize = 0;
            btnAttachment.UseVisualStyleBackColor = false;
            btnAttachment.Click += BtnAttachment_Click;
            btnAttachment.MouseEnter += (s, e) => btnAttachment.BackColor = AppPalette.AccentHover;
            btnAttachment.MouseLeave += (s, e) => btnAttachment.BackColor = AppPalette.AccentPrimary;
            pnlForm.Controls.Add(btnAttachment);

            pnlAttachments = new Panel {
                Location = new Point(30, 370),
                Size = new Size(800, 90),
                BackColor = AppPalette.CodeBlock,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true
            };
            pnlForm.Controls.Add(pnlAttachments);

            flowAttachments = new FlowLayoutPanel {
                Location = new Point(5, 5),
                Size = new Size(785, 80),
                AutoScroll = false,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight
            };
            pnlAttachments.Controls.Add(flowAttachments);

            Label lblAttachmentHint = new Label {
                Text = "Images help us understand and resolve issues faster (Optional)",
                Location = new Point(190, 335),
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = AppPalette.TextMuted
            };
            pnlForm.Controls.Add(lblAttachmentHint);

            btnSubmit = new Button {
                Text = "Submit Report",
                Location = new Point(870, 325),
                Size = new Size(200, 45),
                BackColor = AppPalette.AccentPrimary,
                FlatStyle = FlatStyle.Flat,
                ForeColor = AppPalette.TextOnAccent,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSubmit.FlatAppearance.BorderSize = 0;
            btnSubmit.UseVisualStyleBackColor = false;
            btnSubmit.Click += BtnSubmit_Click;
            btnSubmit.MouseEnter += (s, e) => btnSubmit.BackColor = AppPalette.AccentHover;
            btnSubmit.MouseLeave += (s, e) => btnSubmit.BackColor = AppPalette.AccentPrimary;
            pnlForm.Controls.Add(btnSubmit);

            btnAdmin = new Button {
                Text = "View All Reports",
                Location = new Point(870, 380),
                Size = new Size(200, 35),
                BackColor = AppPalette.Surface,
                FlatStyle = FlatStyle.Flat,
                ForeColor = AppPalette.TextPrimary,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAdmin.FlatAppearance.BorderColor = AppPalette.Border;
            btnAdmin.FlatAppearance.BorderSize = 2;
            btnAdmin.UseVisualStyleBackColor = false;
            btnAdmin.Click += BtnAdmin_Click;
            pnlForm.Controls.Add(btnAdmin);

            SetupProgressBar();
            SetupSocialMediaPanel();
        }

        private void SetupSocialMediaPanel() {
            pnlSocialMedia = new Panel {
                Location = new Point(50, 620),
                Size = new Size(1118, 90),
                BackColor = AppPalette.Surface,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(pnlSocialMedia);

            Label lblShare = new Label {
                Text = "Share Your Report:",
                Location = new Point(20, 15),
                Size = new Size(180, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = AppPalette.TextHeading
            };
            pnlSocialMedia.Controls.Add(lblShare);

            Label lblShareHint = new Label {
                Text = "Let your community know about local issues",
                Location = new Point(20, 40),
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = AppPalette.TextMuted
            };
            pnlSocialMedia.Controls.Add(lblShareHint);

            btnWhatsApp = new Button {
                Text = "WhatsApp",
                Location = new Point(220, 20),
                Size = new Size(110, 45),
                BackColor = Color.FromArgb(37, 211, 102),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnWhatsApp.FlatAppearance.BorderSize = 0;
            btnWhatsApp.Click += BtnShare_Click;
            btnWhatsApp.MouseEnter += (s, e) => btnWhatsApp.BackColor = Color.FromArgb(32, 183, 89);
            btnWhatsApp.MouseLeave += (s, e) => btnWhatsApp.BackColor = Color.FromArgb(37, 211, 102);
            pnlSocialMedia.Controls.Add(btnWhatsApp);

            btnFacebook = new Button {
                Text = "Facebook",
                Location = new Point(345, 20),
                Size = new Size(110, 45),
                BackColor = Color.FromArgb(24, 119, 242),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnFacebook.FlatAppearance.BorderSize = 0;
            btnFacebook.Click += BtnShare_Click;
            btnFacebook.MouseEnter += (s, e) => btnFacebook.BackColor = Color.FromArgb(20, 100, 205);
            btnFacebook.MouseLeave += (s, e) => btnFacebook.BackColor = Color.FromArgb(24, 119, 242);
            pnlSocialMedia.Controls.Add(btnFacebook);

            btnTwitter = new Button {
                Text = "Twitter",
                Location = new Point(470, 20),
                Size = new Size(110, 45),
                BackColor = Color.FromArgb(29, 161, 242),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnTwitter.FlatAppearance.BorderSize = 0;
            btnTwitter.Click += BtnShare_Click;
            btnTwitter.MouseEnter += (s, e) => btnTwitter.BackColor = Color.FromArgb(24, 135, 204);
            btnTwitter.MouseLeave += (s, e) => btnTwitter.BackColor = Color.FromArgb(29, 161, 242);
            pnlSocialMedia.Controls.Add(btnTwitter);

            btnEmail = new Button {
                Text = "Email",
                Location = new Point(595, 20),
                Size = new Size(110, 45),
                BackColor = Color.FromArgb(234, 67, 53),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEmail.FlatAppearance.BorderSize = 0;
            btnEmail.Click += BtnShare_Click;
            btnEmail.MouseEnter += (s, e) => btnEmail.BackColor = Color.FromArgb(197, 57, 45);
            btnEmail.MouseLeave += (s, e) => btnEmail.BackColor = Color.FromArgb(234, 67, 53);
            pnlSocialMedia.Controls.Add(btnEmail);
        }

        private void SetupProgressBar() {
            pnlProgress = new Panel {
                Location = new Point(50, 725),
                Size = new Size(1118, 70),
                BackColor = AppPalette.CodeBlock,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(pnlProgress);

            lblProgress = new Label {
                Text = "Progress: 0%",
                Location = new Point(20, 10),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = AppPalette.TextHeading
            };
            pnlProgress.Controls.Add(lblProgress);

            lblProgressMessage = new Label {
                Text = "Start by entering a title for the issue",
                Location = new Point(230, 11),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = AppPalette.TextSecondary
            };
            pnlProgress.Controls.Add(lblProgressMessage);

            progressBar = new ProgressBar {
                Location = new Point(20, 38),
                Size = new Size(1078, 20),
                Minimum = 0,
                Maximum = 100,
                Value = 0,
                Style = ProgressBarStyle.Continuous
            };
            pnlProgress.Controls.Add(progressBar);
        }

        private void Input_Changed(object sender, EventArgs e) {
            UpdateProgress();
        }

        private void UpdateProgress() {
            int progress = 0;
            string message = "";

            bool hasTitle = !string.IsNullOrWhiteSpace(txtTitle.Text);
            bool hasLocation = !string.IsNullOrWhiteSpace(txtLocation.Text);
            bool hasCategory = cmbCategory.SelectedIndex >= 0;
            bool hasDescription = !string.IsNullOrWhiteSpace(txtDescription.Text);
            bool hasAttachment = attachmentPaths.Count > 0;

            if (hasTitle) progress += 20;
            if (hasLocation) progress += 20;
            if (hasCategory) progress += 20;
            if (hasDescription) progress += 20;
            if (hasAttachment) progress += 20;

            if (progress == 0) {
                message = "Start by entering a title for the issue";
            }
            else if (progress == 20) {
                if (hasTitle) {
                    message = "Great start! Now enter the location";
                }
                else if (hasLocation) {
                    message = "Good! Now add a title";
                }
                else if (hasCategory) {
                    message = "Good! Now add a title";
                }
                else if (hasDescription) {
                    message = "Good! Now add a title";
                }
                else if (hasAttachment) {
                    message = "Good! Now add a title";
                }
            }
            else if (progress == 40) {
                if (!hasCategory) {
                    message = "Nice! Now select the category";
                }
                else if (!hasLocation) {
                    message = "Nice! Now enter the location";
                }
                else if (!hasDescription) {
                    message = "Nice! Now describe the issue";
                }
            }
            else if (progress == 60) {
                if (!hasDescription) {
                    message = "Almost there! Please describe the issue";
                }
                else if (!hasCategory) {
                    message = "Looking good! Please select a category";
                }
                else if (!hasLocation) {
                    message = "Looking good! Please enter the location";
                }
            }
            else if (progress == 80) {
                message = "Excellent! You can add photos or submit now";
            }
            else if (progress == 100) {
                message = "Perfect! All fields complete - ready to submit";
            }

            progressBar.Value = progress;
            lblProgress.Text = string.Format("Progress: {0}%", progress);
            if (lblProgressMessage != null) {
                lblProgressMessage.Text = message;
            }
        }

        private void TxtTitle_TextChanged(object sender, EventArgs e) {
            Input_Changed(sender, e);
            UpdateSuggestion();
        }

        private void TxtTitle_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Space && !string.IsNullOrEmpty(currentSuggestion)) {
                e.SuppressKeyPress = true;
                ApplySuggestion();
            }
            else if (e.KeyCode != Keys.Space && !string.IsNullOrEmpty(currentSuggestion)) {
                lblSuggestion.Visible = false;
                currentSuggestion = string.Empty;
            }
        }

        private void UpdateSuggestion() {
            if (txtTitle == null || lblSuggestion == null) {
                return;
            }

            string text = txtTitle.Text;
            
            if (text.Length < 2) {
                lblSuggestion.Visible = false;
                currentSuggestion = string.Empty;
                return;
            }

            string suggestion = predictionTrie.GetTopSuggestion(text);
            
            if (!string.IsNullOrEmpty(suggestion) && suggestion.ToLower().StartsWith(text.ToLower())) {
                currentSuggestion = suggestion;
                string remainingSuggestion = suggestion.Substring(text.Length);
                lblSuggestion.Text = text + remainingSuggestion;
                lblSuggestion.Visible = true;
            }
            else {
                lblSuggestion.Visible = false;
                currentSuggestion = string.Empty;
            }
        }

        private void ApplySuggestion() {
            if (string.IsNullOrEmpty(currentSuggestion) || txtTitle == null) {
                return;
            }

            txtTitle.Text = currentSuggestion + " ";
            txtTitle.SelectionStart = txtTitle.Text.Length;
            
            lblSuggestion.Visible = false;
            currentSuggestion = string.Empty;
        }

        private void BtnAttachment_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Document Files|*.pdf;*.doc;*.docx|All Files|*.*",
                Title = "Select Files to Attach",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                foreach (string fileName in openFileDialog.FileNames) {
                    if (!attachmentPaths.Contains(fileName)) {
                        attachmentPaths.Add(fileName);
                        AddAttachmentPreview(fileName);
                    }
                }
                UpdateProgress();
            }
        }

        private void AddAttachmentPreview(string filePath) {
            Panel previewPanel = new Panel {
                Size = new Size(120, 70),
                BackColor = AppPalette.Surface,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5),
                Tag = filePath
            };

            string fileName = Path.GetFileName(filePath);
            string extension = Path.GetExtension(filePath).ToLowerInvariant();

            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || 
                extension == ".gif" || extension == ".bmp") {
                try {
                    PictureBox picBox = new PictureBox {
                        Size = new Size(110, 40),
                        Location = new Point(5, 5),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Image = Image.FromFile(filePath)
                    };
                    previewPanel.Controls.Add(picBox);
                }
                catch {
                    Label lblFileIcon = new Label {
                        Text = "IMAGE",
                        Location = new Point(5, 15),
                        Size = new Size(110, 20),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Segoe UI", 8, FontStyle.Bold),
                        ForeColor = AppPalette.TextMuted
                    };
                    previewPanel.Controls.Add(lblFileIcon);
                }
            }
            else {
                Label lblFileIcon = new Label {
                    Text = extension.ToUpper().Replace(".", ""),
                    Location = new Point(5, 15),
                    Size = new Size(110, 20),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = AppPalette.TextMuted
                };
                previewPanel.Controls.Add(lblFileIcon);
            }

            Label lblFileName = new Label {
                Text = fileName.Length > 15 ? fileName.Substring(0, 12) + "..." : fileName,
                Location = new Point(5, 47),
                Size = new Size(80, 18),
                Font = new Font("Segoe UI", 7),
                ForeColor = AppPalette.TextSecondary,
                TextAlign = ContentAlignment.MiddleLeft
            };
            previewPanel.Controls.Add(lblFileName);

            Button btnRemove = new Button {
                Text = "X",
                Size = new Size(20, 20),
                Location = new Point(95, 47),
                BackColor = Color.FromArgb(220, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 7, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Tag = filePath
            };
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.Click += (s, e) => {
                string pathToRemove = (s as Button).Tag.ToString();
                attachmentPaths.Remove(pathToRemove);
                flowAttachments.Controls.Remove(previewPanel);
                previewPanel.Dispose();
                UpdateProgress();
            };
            btnRemove.MouseEnter += (s, e) => btnRemove.BackColor = Color.FromArgb(180, 40, 40);
            btnRemove.MouseLeave += (s, e) => btnRemove.BackColor = Color.FromArgb(220, 50, 50);
            previewPanel.Controls.Add(btnRemove);

            flowAttachments.Controls.Add(previewPanel);
        }

        private void BtnSubmit_Click(object sender, EventArgs e) {
            if (ValidateInput()) {
                string combinedAttachments = string.Join(";", attachmentPaths);
                
                Issue issue = new Issue(
                    txtTitle.Text.Trim(),
                    txtLocation.Text.Trim(),
                    cmbCategory.SelectedItem.ToString(),
                    txtDescription.Text.Trim(),
                    combinedAttachments
                );
                issue.Priority = DeterminePriority(issue.Category);

                try {
                    int issueId = dbHelper.SaveIssue(issue);
                    issues.Add(issue);

                    predictionTrie.LearnFromInput(txtTitle.Text.Trim());

                    string successMessage = string.Format(
                        "Issue reported successfully!\n\n" +
                        "Reference ID: {0:D6}\n" +
                        "Title: {1}\n" +
                        "Category: {2}\n" +
                        "Location: {3}\n\n" +
                        "Thank you for helping improve our community!",
                        issueId,
                        issue.Title,
                        issue.Category,
                        issue.Location
                    );

                    MessageBox.Show(successMessage, "Report Submitted", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearForm();
                }
                catch (Exception ex) {
                    MessageBox.Show("Error saving issue: " + ex.Message,
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateInput() {
            if (string.IsNullOrWhiteSpace(txtTitle.Text)) {
                MessageBox.Show("Please enter a title for the issue.", 
                    "Title Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLocation.Text)) {
                MessageBox.Show("Please enter the location of the issue.", 
                    "Location Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLocation.Focus();
                return false;
            }

            if (cmbCategory.SelectedIndex < 0) {
                MessageBox.Show("Please select a category for the issue.", 
                    "Category Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text)) {
                MessageBox.Show("Please provide a description of the issue.", 
                    "Description Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescription.Focus();
                return false;
            }

            if (txtDescription.Text.Trim().Length < 10) {
                MessageBox.Show("Please provide a more detailed description (at least 10 characters).", 
                    "Description Too Short", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescription.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm() {
            txtTitle.Clear();
            txtLocation.Clear();
            cmbCategory.SelectedIndex = -1;
            txtDescription.Clear();
            attachmentPaths.Clear();
            flowAttachments.Controls.Clear();
            currentSuggestion = string.Empty;
            if (lblSuggestion != null) {
                lblSuggestion.Visible = false;
            }
            UpdateProgress();
        }

        private void BtnAdmin_Click(object sender, EventArgs e) {
            AdminForm adminForm = new AdminForm();
            adminForm.ShowDialog();
        }

        private void BtnShare_Click(object sender, EventArgs e) {
            Button clickedButton = sender as Button;
            string platform = clickedButton?.Text ?? "Social Media";
            
            MessageBox.Show(
                string.Format("Share to {0} feature coming soon!\n\n" +
                "This will allow you to share issue reports with your community.", platform),
                "Feature In Development", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
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