using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Sidequest_municiple_app
{
    public partial class AdminForm : Form
    {
        private DataGridView dgvIssues;
        private Label lblTitle;
        private Button btnRefresh;
        private DatabaseHelper dbHelper;

        public AdminForm()
        {
            InitializeComponent();
            SetupForm();
            dbHelper = new DatabaseHelper();
            LoadIssues();
        }

        private void SetupForm()
        {
            this.Text = "Admin - View All Reports";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = AppPalette.Background;

            lblTitle = new Label();
            lblTitle.Text = "Municipal Issues Reports";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.ForeColor = AppPalette.TextPrimary;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(30, 20);
            this.Controls.Add(lblTitle);

            btnRefresh = new Button();
            btnRefresh.Text = "Refresh";
            btnRefresh.Location = new Point(800, 20);
            btnRefresh.Size = new Size(80, 30);
            btnRefresh.BackColor = AppPalette.AccentPrimary;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderColor = AppPalette.Border;
            btnRefresh.FlatAppearance.BorderSize = 1;
            btnRefresh.ForeColor = AppPalette.TextOnAccent;
            btnRefresh.Font = new Font("Segoe UI", 9);
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += BtnRefresh_Click;
            this.Controls.Add(btnRefresh);

            dgvIssues = new DataGridView();
            dgvIssues.Location = new Point(30, 60);
            dgvIssues.Size = new Size(840, 480);
            dgvIssues.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvIssues.BackgroundColor = AppPalette.Surface;
            dgvIssues.BorderStyle = BorderStyle.Fixed3D;
            dgvIssues.ReadOnly = true;
            dgvIssues.AllowUserToAddRows = false;
            dgvIssues.AllowUserToDeleteRows = false;
            dgvIssues.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIssues.EnableHeadersVisualStyles = false;
            
            dgvIssues.ColumnHeadersDefaultCellStyle.BackColor = AppPalette.AccentPrimary;
            dgvIssues.ColumnHeadersDefaultCellStyle.ForeColor = AppPalette.TextOnAccent;
            dgvIssues.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvIssues.ColumnHeadersHeight = 30;
            
            dgvIssues.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvIssues.DefaultCellStyle.BackColor = AppPalette.Surface;
            dgvIssues.DefaultCellStyle.ForeColor = AppPalette.TextPrimary;
            dgvIssues.DefaultCellStyle.SelectionBackColor = AppPalette.AccentPrimary;
            dgvIssues.DefaultCellStyle.SelectionForeColor = AppPalette.TextOnAccent;
            dgvIssues.AlternatingRowsDefaultCellStyle.BackColor = AppPalette.SurfaceAlt;
            
            this.Controls.Add(dgvIssues);

            SetupDataGridColumns();
        }

        private void SetupDataGridColumns()
        {
            dgvIssues.Columns.Add("Id", "ID");
            dgvIssues.Columns.Add("Location", "Location");
            dgvIssues.Columns.Add("Category", "Category");
            dgvIssues.Columns.Add("Status", "Status");
            dgvIssues.Columns.Add("Priority", "Priority");
            dgvIssues.Columns.Add("Description", "Description");
            dgvIssues.Columns.Add("AttachmentPath", "Attachment");
            dgvIssues.Columns.Add("ReportDate", "Report Date");

            dgvIssues.Columns["Id"].Width = 60;
            dgvIssues.Columns["Location"].Width = 150;
            dgvIssues.Columns["Category"].Width = 100;
            dgvIssues.Columns["Status"].Width = 100;
            dgvIssues.Columns["Priority"].Width = 100;
            dgvIssues.Columns["Description"].Width = 300;
            dgvIssues.Columns["AttachmentPath"].Width = 120;
            dgvIssues.Columns["ReportDate"].Width = 120;

            dgvIssues.Columns["Description"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvIssues.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
        }

        private void LoadIssues()
        {
            try
            {
                dgvIssues.Rows.Clear();
                List<Issue> issues = dbHelper.GetAllIssues();

                foreach (Issue issue in issues)
                {
                    string attachmentDisplay = string.IsNullOrEmpty(issue.AttachmentPath) ? 
                                             "No attachment" : 
                                             System.IO.Path.GetFileName(issue.AttachmentPath);

                    dgvIssues.Rows.Add(
                        issue.Id.ToString("D6"),
                        issue.Location,
                        issue.Category,
                        issue.Status.ToString(),
                        issue.Priority.ToString(),
                        issue.Description,
                        attachmentDisplay,
                        issue.ReportDate.ToString("yyyy-MM-dd HH:mm")
                    );
                }

                if (issues.Count == 0)
                {
                    dgvIssues.Rows.Add("", "No reports found", "", "", "", "", "", "");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading issues: " + ex.Message, 
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadIssues();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(284, 261);
            this.Name = "AdminForm";
            this.Text = "AdminForm";
            this.ResumeLayout(false);
        }
    }
}