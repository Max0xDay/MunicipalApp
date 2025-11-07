using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sidequest_municiple_app
{
    public partial class ServiceRequestStatusForm : Form
    {
        private DataGridView dgvRequests;
        private TextBox txtSearchID;
        private Button btnSearch;
        private Button btnRefresh;
        private Button btnBack;
        private ComboBox cmbStatusFilter;
        private ComboBox cmbCategoryFilter;
        private Label lblTitle;
        private Label lblSearchLabel;
        private Label lblStatusFilter;
        private Label lblCategoryFilter;
        private Label lblTotalRequests;
        private Label lblPriorityQueue;
        private Label lblUpdateStatus;
        private Panel pnlControls;
        private ComboBox cmbUpdateStatus;
        private Button btnUpdateStatus;
        private ListView lvPriorityQueue;
        
        private List<ServiceRequest> allServiceRequests;
        private ServiceRequestBST requestTree;
        private ServiceRequestAVL requestBalancedTree;
        private ServiceRequestHeap requestHeap;
        private ServiceRequestGraph requestGraph;
        private DatabaseHelper dbHelper;

        public ServiceRequestStatusForm()
        {
            InitializeComponent();
            SetupForm();
            dbHelper = new DatabaseHelper();
            LoadServiceRequests();
        }

        private void SetupForm()
        {
            this.Text = "Service Request Status";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = AppPalette.Background;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblTitle = new Label();
            lblTitle.Text = "Service Request Status Tracking";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.ForeColor = AppPalette.TextPrimary;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(30, 20);
            this.Controls.Add(lblTitle);

            pnlControls = new Panel();
            pnlControls.Location = new Point(30, 60);
            pnlControls.Size = new Size(940, 100);
            pnlControls.BackColor = AppPalette.Surface;
            pnlControls.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(pnlControls);

            lblSearchLabel = new Label();
            lblSearchLabel.Text = "Search by ID:";
            lblSearchLabel.Font = new Font("Segoe UI", 10);
            lblSearchLabel.ForeColor = AppPalette.TextPrimary;
            lblSearchLabel.AutoSize = true;
            lblSearchLabel.Location = new Point(10, 15);
            pnlControls.Controls.Add(lblSearchLabel);

            txtSearchID = new TextBox();
            txtSearchID.Location = new Point(110, 12);
            txtSearchID.Size = new Size(200, 25);
            txtSearchID.Font = new Font("Segoe UI", 10);
            pnlControls.Controls.Add(txtSearchID);

            btnSearch = new Button();
            btnSearch.Text = "Search";
            btnSearch.Size = new Size(80, 28);
            btnSearch.Location = new Point(320, 11);
            btnSearch.BackColor = AppPalette.AccentSecondary;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.FlatAppearance.BorderColor = AppPalette.Border;
            btnSearch.ForeColor = AppPalette.TextOnAccent;
            btnSearch.Font = new Font("Segoe UI", 9);
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += BtnSearch_Click;
            pnlControls.Controls.Add(btnSearch);

            lblStatusFilter = new Label();
            lblStatusFilter.Text = "Status:";
            lblStatusFilter.Font = new Font("Segoe UI", 10);
            lblStatusFilter.ForeColor = AppPalette.TextPrimary;
            lblStatusFilter.AutoSize = true;
            lblStatusFilter.Location = new Point(10, 55);
            pnlControls.Controls.Add(lblStatusFilter);

            cmbStatusFilter = new ComboBox();
            cmbStatusFilter.Location = new Point(110, 52);
            cmbStatusFilter.Size = new Size(150, 25);
            cmbStatusFilter.Font = new Font("Segoe UI", 10);
            cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatusFilter.Items.Add("All");
            cmbStatusFilter.Items.Add("Pending");
            cmbStatusFilter.Items.Add("InProgress");
            cmbStatusFilter.Items.Add("Completed");
            cmbStatusFilter.Items.Add("Rejected");
            cmbStatusFilter.SelectedIndex = 0;
            cmbStatusFilter.SelectedIndexChanged += FilterChanged;
            pnlControls.Controls.Add(cmbStatusFilter);

            lblCategoryFilter = new Label();
            lblCategoryFilter.Text = "Category:";
            lblCategoryFilter.Font = new Font("Segoe UI", 10);
            lblCategoryFilter.ForeColor = AppPalette.TextPrimary;
            lblCategoryFilter.AutoSize = true;
            lblCategoryFilter.Location = new Point(280, 55);
            pnlControls.Controls.Add(lblCategoryFilter);

            cmbCategoryFilter = new ComboBox();
            cmbCategoryFilter.Location = new Point(360, 52);
            cmbCategoryFilter.Size = new Size(150, 25);
            cmbCategoryFilter.Font = new Font("Segoe UI", 10);
            cmbCategoryFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoryFilter.Items.Add("All");
            cmbCategoryFilter.Items.Add("Sanitation");
            cmbCategoryFilter.Items.Add("Roads");
            cmbCategoryFilter.Items.Add("Utilities");
            cmbCategoryFilter.Items.Add("Water");
            cmbCategoryFilter.Items.Add("Electricity");
            cmbCategoryFilter.SelectedIndex = 0;
            cmbCategoryFilter.SelectedIndexChanged += FilterChanged;
            pnlControls.Controls.Add(cmbCategoryFilter);

            btnRefresh = new Button();
            btnRefresh.Text = "Refresh";
            btnRefresh.Size = new Size(100, 35);
            btnRefresh.Location = new Point(550, 50);
            btnRefresh.BackColor = AppPalette.AccentSecondary;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderColor = AppPalette.Border;
            btnRefresh.ForeColor = AppPalette.TextOnAccent;
            btnRefresh.Font = new Font("Segoe UI", 10);
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += BtnRefresh_Click;
            pnlControls.Controls.Add(btnRefresh);

            btnBack = new Button();
            btnBack.Text = "Back to Menu";
            btnBack.Size = new Size(120, 35);
            btnBack.Location = new Point(800, 50);
            btnBack.BackColor = AppPalette.SurfaceAlt;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderColor = AppPalette.Border;
            btnBack.ForeColor = AppPalette.TextPrimary;
            btnBack.Font = new Font("Segoe UI", 10);
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += BtnBack_Click;
            pnlControls.Controls.Add(btnBack);

            lblTotalRequests = new Label();
            lblTotalRequests.Text = "Total Requests: 0";
            lblTotalRequests.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTotalRequests.ForeColor = AppPalette.TextPrimary;
            lblTotalRequests.AutoSize = true;
            lblTotalRequests.Location = new Point(30, 175);
            this.Controls.Add(lblTotalRequests);

            dgvRequests = new DataGridView();
            dgvRequests.Location = new Point(30, 200);
            dgvRequests.Size = new Size(940, 300);
            dgvRequests.BackgroundColor = AppPalette.Surface;
            dgvRequests.BorderStyle = BorderStyle.FixedSingle;
            dgvRequests.AllowUserToAddRows = false;
            dgvRequests.AllowUserToDeleteRows = false;
            dgvRequests.ReadOnly = true;
            dgvRequests.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRequests.MultiSelect = false;
            dgvRequests.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRequests.RowHeadersVisible = false;
            dgvRequests.Font = new Font("Segoe UI", 9);
            dgvRequests.ColumnHeadersDefaultCellStyle.BackColor = AppPalette.AccentSecondary;
            dgvRequests.ColumnHeadersDefaultCellStyle.ForeColor = AppPalette.TextOnAccent;
            dgvRequests.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvRequests.EnableHeadersVisualStyles = false;
            dgvRequests.DefaultCellStyle.SelectionBackColor = AppPalette.AccentPrimary;
            dgvRequests.DefaultCellStyle.SelectionForeColor = AppPalette.TextOnAccent;
            dgvRequests.SelectionChanged += DgvRequests_SelectionChanged;
            this.Controls.Add(dgvRequests);

            lblUpdateStatus = new Label();
            lblUpdateStatus.Text = "Update Status:";
            lblUpdateStatus.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblUpdateStatus.ForeColor = AppPalette.TextPrimary;
            lblUpdateStatus.AutoSize = true;
            lblUpdateStatus.Location = new Point(30, 520);
            this.Controls.Add(lblUpdateStatus);

            cmbUpdateStatus = new ComboBox();
            cmbUpdateStatus.Location = new Point(150, 518);
            cmbUpdateStatus.Size = new Size(180, 25);
            cmbUpdateStatus.Font = new Font("Segoe UI", 10);
            cmbUpdateStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUpdateStatus.Items.AddRange(new object[] { "Pending", "InProgress", "Completed", "Rejected" });
            cmbUpdateStatus.SelectedIndex = 0;
            this.Controls.Add(cmbUpdateStatus);

            btnUpdateStatus = new Button();
            btnUpdateStatus.Text = "Apply";
            btnUpdateStatus.Size = new Size(100, 28);
            btnUpdateStatus.Location = new Point(340, 516);
            btnUpdateStatus.BackColor = AppPalette.AccentPrimary;
            btnUpdateStatus.FlatStyle = FlatStyle.Flat;
            btnUpdateStatus.FlatAppearance.BorderColor = AppPalette.Border;
            btnUpdateStatus.ForeColor = AppPalette.TextOnAccent;
            btnUpdateStatus.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnUpdateStatus.UseVisualStyleBackColor = false;
            btnUpdateStatus.Click += BtnUpdateStatus_Click;
            btnUpdateStatus.Enabled = false;
            this.Controls.Add(btnUpdateStatus);

            lblPriorityQueue = new Label();
            lblPriorityQueue.Text = "Priority Queue";
            lblPriorityQueue.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblPriorityQueue.ForeColor = AppPalette.TextPrimary;
            lblPriorityQueue.AutoSize = true;
            lblPriorityQueue.Location = new Point(30, 560);
            this.Controls.Add(lblPriorityQueue);

            lvPriorityQueue = new ListView();
            lvPriorityQueue.Location = new Point(30, 585);
            lvPriorityQueue.Size = new Size(940, 90);
            lvPriorityQueue.View = View.Details;
            lvPriorityQueue.FullRowSelect = true;
            lvPriorityQueue.GridLines = false;
            lvPriorityQueue.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvPriorityQueue.Columns.Add("Priority", 120);
            lvPriorityQueue.Columns.Add("Category", 180);
            lvPriorityQueue.Columns.Add("Location", 280);
            lvPriorityQueue.Columns.Add("Status", 120);
            lvPriorityQueue.Columns.Add("Submitted", 200);
            this.Controls.Add(lvPriorityQueue);

            allServiceRequests = new List<ServiceRequest>();
            requestTree = new ServiceRequestBST();
            requestBalancedTree = new ServiceRequestAVL();
            requestHeap = new ServiceRequestHeap();
            requestGraph = new ServiceRequestGraph();
        }

        private void LoadServiceRequests()
        {
            try
            {
                allServiceRequests.Clear();
                requestTree.Clear();
                requestBalancedTree.Clear();
                requestHeap.Clear();
                requestGraph.Clear();

                List<Issue> issues = dbHelper.GetAllIssues();

                foreach (Issue issue in issues)
                {
                    ServiceRequest request = new ServiceRequest(issue);
                    AssignPriorityBasedOnCategory(request);
                    allServiceRequests.Add(request);
                    requestTree.Insert(request);
                    requestBalancedTree.Insert(request);
                    requestHeap.Insert(request);
                    requestGraph.AddOrUpdate(request);
                }

                requestGraph.BuildRelationships(allServiceRequests);
                ApplyFilters();
                PopulatePriorityQueueDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading service requests: " + ex.Message, 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AssignPriorityBasedOnCategory(ServiceRequest request)
        {
            if (request == null)
            {
                return;
            }

            if (request.Priority != ServiceRequestPriority.Medium)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(request.Category))
            {
                request.Priority = ServiceRequestPriority.Medium;
                return;
            }

            switch (request.Category.ToLower())
            {
                case "water":
                case "electricity":
                    request.Priority = ServiceRequestPriority.Urgent;
                    break;
                case "sanitation":
                    request.Priority = ServiceRequestPriority.High;
                    break;
                case "roads":
                    request.Priority = ServiceRequestPriority.Medium;
                    break;
                case "utilities":
                    request.Priority = ServiceRequestPriority.High;
                    break;
                default:
                    request.Priority = ServiceRequestPriority.Low;
                    break;
            }
        }

        private void DisplayServiceRequests(List<ServiceRequest> requests)
        {
            List<ServiceRequestRow> rows = new List<ServiceRequestRow>();

            foreach (ServiceRequest request in requests)
            {
                string reference = request.UniqueID.Length >= 8 ? request.UniqueID.Substring(0, 8) : request.UniqueID;
                string description = string.IsNullOrWhiteSpace(request.Description) ? string.Empty : request.Description;
                if (description.Length > 50)
                {
                    description = description.Substring(0, 50) + "...";
                }

                rows.Add(new ServiceRequestRow
                {
                    UniqueId = request.UniqueID,
                    Reference = reference,
                    Location = request.Location,
                    Category = request.Category,
                    Status = request.GetStatusString(),
                    Priority = request.GetPriorityString(),
                    DateSubmitted = request.DateSubmitted.ToString("yyyy-MM-dd HH:mm"),
                    Description = description
                });
            }

            dgvRequests.DataSource = rows;

            if (dgvRequests.Columns["UniqueId"] != null)
            {
                dgvRequests.Columns["UniqueId"].Visible = false;
            }

            if (dgvRequests.Columns["Reference"] != null)
            {
                dgvRequests.Columns["Reference"].HeaderText = "Request";
            }

            if (dgvRequests.Columns["DateSubmitted"] != null)
            {
                dgvRequests.Columns["DateSubmitted"].HeaderText = "Submitted";
            }

            lblTotalRequests.Text = "Total Requests: " + requests.Count;

            foreach (DataGridViewRow row in dgvRequests.Rows)
            {
                string status = row.Cells["Status"].Value?.ToString();
                switch (status)
                {
                    case "Pending":
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 224);
                        break;
                    case "InProgress":
                        row.DefaultCellStyle.BackColor = Color.FromArgb(173, 216, 230);
                        break;
                    case "Completed":
                        row.DefaultCellStyle.BackColor = Color.FromArgb(144, 238, 144);
                        break;
                    case "Rejected":
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 182, 193);
                        break;
                }
            }

            SyncStatusControls();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchID = txtSearchID.Text.Trim();
            
            if (string.IsNullOrEmpty(searchID))
            {
                MessageBox.Show("Please enter a request ID to search.", 
                    "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            var foundRequests = requestBalancedTree
                .InOrder()
                .Where(r => r.UniqueID.StartsWith(searchID, StringComparison.OrdinalIgnoreCase))
                .ToList();
            
            if (foundRequests.Count == 0)
            {
                MessageBox.Show("No service request found with ID starting with: " + searchID, 
                    "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DisplayServiceRequests(foundRequests);
            }
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var filteredRequests = requestBalancedTree.InOrder().AsEnumerable();
            
            if (cmbStatusFilter.SelectedItem.ToString() != "All")
            {
                string selectedStatus = cmbStatusFilter.SelectedItem.ToString();
                filteredRequests = filteredRequests.Where(r => r.GetStatusString() == selectedStatus);
            }
            
            if (cmbCategoryFilter.SelectedItem.ToString() != "All")
            {
                string selectedCategory = cmbCategoryFilter.SelectedItem.ToString();
                filteredRequests = filteredRequests.Where(r => 
                    r.Category.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase));
            }
            
            DisplayServiceRequests(filteredRequests.ToList());
            PopulatePriorityQueueDisplay();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearchID.Text = "";
            cmbStatusFilter.SelectedIndex = 0;
            cmbCategoryFilter.SelectedIndex = 0;
            LoadServiceRequests();
        }

        private void DgvRequests_SelectionChanged(object sender, EventArgs e)
        {
            SyncStatusControls();
        }

        private void BtnUpdateStatus_Click(object sender, EventArgs e)
        {
            string uniqueId = GetSelectedUniqueId();
            if (string.IsNullOrWhiteSpace(uniqueId))
            {
                MessageBox.Show("Select a service request first.", "Status Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cmbUpdateStatus.SelectedItem == null)
            {
                MessageBox.Show("Choose a status before applying.", "Status Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ServiceRequestStatus newStatus = (ServiceRequestStatus)Enum.Parse(typeof(ServiceRequestStatus), cmbUpdateStatus.SelectedItem.ToString());
            ServiceRequest request = allServiceRequests.FirstOrDefault(r => string.Equals(r.UniqueID, uniqueId, StringComparison.OrdinalIgnoreCase));
            if (request == null)
            {
                MessageBox.Show("The selected request could not be found.", "Status Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (request.Status == newStatus)
            {
                MessageBox.Show("The request already has that status.", "Status Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                request.Status = newStatus;
                dbHelper.UpdateIssueStatus(uniqueId, newStatus, request.Priority);
                LoadServiceRequests();
                ReselectRow(uniqueId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating request: " + ex.Message, "Status Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetSelectedUniqueId()
        {
            if (dgvRequests.CurrentRow == null)
            {
                return null;
            }

            object value = dgvRequests.CurrentRow.Cells["UniqueId"]?.Value;
            return value?.ToString();
        }

        private void SyncStatusControls()
        {
            if (cmbUpdateStatus == null)
            {
                return;
            }

            string uniqueId = GetSelectedUniqueId();
            if (string.IsNullOrWhiteSpace(uniqueId))
            {
                btnUpdateStatus.Enabled = false;
                return;
            }

            ServiceRequest request = allServiceRequests.FirstOrDefault(r => string.Equals(r.UniqueID, uniqueId, StringComparison.OrdinalIgnoreCase));
            if (request == null)
            {
                btnUpdateStatus.Enabled = false;
                return;
            }

            btnUpdateStatus.Enabled = true;
            string statusText = request.Status.ToString();
            int index = cmbUpdateStatus.Items.IndexOf(statusText);
            if (index >= 0)
            {
                cmbUpdateStatus.SelectedIndex = index;
            }
        }

        private void ReselectRow(string uniqueId)
        {
            if (string.IsNullOrWhiteSpace(uniqueId))
            {
                return;
            }

            foreach (DataGridViewRow row in dgvRequests.Rows)
            {
                string value = row.Cells["UniqueId"].Value?.ToString();
                if (string.Equals(value, uniqueId, StringComparison.OrdinalIgnoreCase))
                {
                    row.Selected = true;
                    dgvRequests.CurrentCell = row.Cells["Reference"];
                    break;
                }
            }

            SyncStatusControls();
        }

        private void PopulatePriorityQueueDisplay()
        {
            lvPriorityQueue.BeginUpdate();
            lvPriorityQueue.Items.Clear();

            var queueItems = requestHeap.GetOrderedSnapshot().Take(10);
            foreach (ServiceRequest request in queueItems)
            {
                ListViewItem item = new ListViewItem(request.GetPriorityString());
                item.SubItems.Add(request.Category);
                item.SubItems.Add(request.Location);
                item.SubItems.Add(request.GetStatusString());
                item.SubItems.Add(request.DateSubmitted.ToString("yyyy-MM-dd HH:mm"));
                lvPriorityQueue.Items.Add(item);
            }

            lvPriorityQueue.EndUpdate();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(1000, 700);
            this.Name = "ServiceRequestStatusForm";
            this.ResumeLayout(false);
        }

        private class ServiceRequestRow
        {
            public string UniqueId { get; set; }
            public string Reference { get; set; }
            public string Location { get; set; }
            public string Category { get; set; }
            public string Status { get; set; }
            public string Priority { get; set; }
            public string DateSubmitted { get; set; }
            public string Description { get; set; }
        }
    }
}
