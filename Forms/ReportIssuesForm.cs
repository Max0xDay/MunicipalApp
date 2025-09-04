using MunicipalApp.Models;
using MunicipalApp.Services;

namespace MunicipalApp.Forms
{
    public partial class ReportIssuesForm : Form
    {
        private readonly DataService _dataService;
        private readonly List<string> _attachedFiles;

        public ReportIssuesForm()
        {
            InitializeComponent();
            _dataService = new DataService();
            _attachedFiles = new List<string>();
            SetupForm();
            SetupCategories();
            UpdateProgress();
        }

        private void SetupForm()
        {
            this.Text = "Report Municipal Issue";
            this.Size = new Size(700, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 248, 255);
        }

        private void SetupCategories()
        {
            cmbCategory.Items.AddRange(new string[]
            {
                "Sanitation",
                "Roads", 
                "Utilities",
                "Water",
                "Electricity"
            });
        }

        private void UpdateProgress()
        {
            int completedFields = 0;
            int totalFields = 4;

            if (!string.IsNullOrWhiteSpace(txtLocation.Text)) completedFields++;
            if (cmbCategory.SelectedIndex != -1) completedFields++;
            if (!string.IsNullOrWhiteSpace(rtbDescription.Text)) completedFields++;
            if (_attachedFiles.Count > 0) completedFields++;

            int percentage = (int)((double)completedFields / totalFields * 100);
            progressBar.Value = percentage;
            
            lblProgress.Text = $"{completedFields} of {totalFields} fields completed";
            
            if (percentage == 0)
                lblEncouragement.Text = "Let's get started! Please fill in your issue details.";
            else if (percentage < 50)
                lblEncouragement.Text = "Great start! Keep going.";
            else if (percentage < 75)
                lblEncouragement.Text = "You're making excellent progress!";
            else if (percentage < 100)
                lblEncouragement.Text = "Almost done! Just a few more details.";
            else
                lblEncouragement.Text = "Perfect! All fields completed. Ready to submit!";

            btnSubmit.BackColor = percentage == 100 ? Color.FromArgb(76, 175, 80) : Color.FromArgb(158, 158, 158);
        }

        private void txtLocation_TextChanged(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void rtbDescription_TextChanged(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void btnAttachFiles_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "All Files (*.*)|*.*|Images (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Documents (*.pdf;*.doc;*.docx;*.txt)|*.pdf;*.doc;*.docx;*.txt";
                openFileDialog.Multiselect = true;
                openFileDialog.Title = "Attach Files";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in openFileDialog.FileNames)
                    {
                        if (!_attachedFiles.Contains(file))
                        {
                            _attachedFiles.Add(file);
                        }
                    }
                    
                    UpdateAttachedFilesList();
                    UpdateProgress();
                }
            }
        }

        private void UpdateAttachedFilesList()
        {
            lstAttachedFiles.Items.Clear();
            foreach (var file in _attachedFiles)
            {
                lstAttachedFiles.Items.Add(Path.GetFileName(file));
            }
            
            btnAttachFiles.Text = _attachedFiles.Count > 0 ? $"Files Attached ({_attachedFiles.Count})" : "Attach Files";
        }

        private void btnRemoveFile_Click(object sender, EventArgs e)
        {
            if (lstAttachedFiles.SelectedIndex >= 0)
            {
                int index = lstAttachedFiles.SelectedIndex;
                _attachedFiles.RemoveAt(index);
                UpdateAttachedFilesList();
                UpdateProgress();
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                var issue = new Issue
                {
                    Location = txtLocation.Text.Trim(),
                    Category = cmbCategory.SelectedItem?.ToString() ?? string.Empty,
                    Description = rtbDescription.Text.Trim(),
                    AttachedFiles = new List<string>(_attachedFiles)
                };

                _dataService.AddIssue(issue);

                MessageBox.Show(
                    $"Thank you! Your issue has been successfully submitted.\n\n" +
                    $"Reference ID: {issue.Id[..8]}\n" +
                    $"Category: {issue.Category}\n" +
                    $"Status: {issue.Status}",
                    "Issue Submitted Successfully",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting issue: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show("Please enter the location of the issue.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLocation.Focus();
                return false;
            }

            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category for the issue.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(rtbDescription.Text))
            {
                MessageBox.Show("Please provide a description of the issue.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtbDescription.Focus();
                return false;
            }

            return true;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}