using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sidequest_municiple_app {
    public partial class ServiceRequestStatusForm : Form {
        private DataGridView dgvRequests;
        private TextBox txtSearchID;
        private Button btnSearch;
        private Button btnRefresh;
        private Button btnBack;
        private ComboBox cmbStatusFilter;
        private ComboBox cmbCategoryFilter;
        private Label lblTitle;
        private Label lblStatusFilter;
        private Label lblCategoryFilter;
        private Label lblTotalRequests;
        private Label lblPriorityQueue;
        private Label lblUpdateStatus;
        private Panel pnlControls;
        private ComboBox cmbUpdateStatus;
        private Button btnUpdateStatus;
        private ListView lvPriorityQueue;
        private Label lblRelatedRequests;
        private ListView lvRelatedRequests;
        private Label lblGraphTraversal;
        private ComboBox cmbGraphTraversal;
        private ListView lvGraphTraversal;
        private Label lblSortBy;
        private ComboBox cmbSortBy;
        private Label lblStatistics;
        private Panel pnlBottom;

        private List<ServiceRequest> allServiceRequests;
        private ServiceRequestBST requestTree;
        private ServiceRequestAVL requestBalancedTree;
        private ServiceRequestHeap requestHeap;
        private ServiceRequestGraph requestGraph;
        private DatabaseHelper dbHelper;

        public ServiceRequestStatusForm() {
            InitializeComponent();
            SetupForm();
            dbHelper = new DatabaseHelper();
            LoadServiceRequests();
        }

        private void SetupForm() {
            Text = "Service Request Status";
            Size = new Size(1000, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = AppPalette.Background;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            lblTitle = new Label();
            lblTitle.Text = "Service Request Status Tracking";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.ForeColor = AppPalette.TextPrimary;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(30, 20);
            Controls.Add(lblTitle);

            pnlControls = new Panel();
            pnlControls.Location = new Point(30, 60);
            pnlControls.Size = new Size(940, 100);
            pnlControls.BackColor = AppPalette.Surface;
            pnlControls.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(pnlControls);

            Label lblSearchLabel = new Label();
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
            cmbStatusFilter.Items.AddRange(new object[] { "All", "Pending", "InProgress", "Completed", "Rejected" });
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
            cmbCategoryFilter.Items.AddRange(new object[] { "All", "Sanitation", "Roads", "Utilities", "Water", "Electricity" });
            cmbCategoryFilter.SelectedIndex = 0;
            cmbCategoryFilter.SelectedIndexChanged += FilterChanged;
            pnlControls.Controls.Add(cmbCategoryFilter);

            lblSortBy = new Label();
            lblSortBy.Text = "Sort:";
            lblSortBy.Font = new Font("Segoe UI", 10);
            lblSortBy.ForeColor = AppPalette.TextPrimary;
            lblSortBy.AutoSize = true;
            lblSortBy.Location = new Point(540, 15);
            pnlControls.Controls.Add(lblSortBy);

            cmbSortBy = new ComboBox();
            cmbSortBy.Location = new Point(580, 12);
            cmbSortBy.Size = new Size(160, 25);
            cmbSortBy.Font = new Font("Segoe UI", 10);
            cmbSortBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSortBy.Items.AddRange(new object[] { "Newest first", "Oldest first", "Priority high-low", "Priority low-high", "Category A-Z" });
            cmbSortBy.SelectedIndex = 0;
            cmbSortBy.SelectedIndexChanged += SortChanged;
            pnlControls.Controls.Add(cmbSortBy);

            btnRefresh = new Button();
            btnRefresh.Text = "Refresh";
            btnRefresh.Size = new Size(100, 35);
            btnRefresh.Location = new Point(580, 50);
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
            Controls.Add(lblTotalRequests);

            lblStatistics = new Label();
            lblStatistics.Text = string.Empty;
            lblStatistics.Font = new Font("Segoe UI", 9);
            lblStatistics.ForeColor = AppPalette.TextPrimary;
            lblStatistics.AutoSize = true;
            lblStatistics.Location = new Point(200, 175);
            Controls.Add(lblStatistics);

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
            Controls.Add(dgvRequests);

            pnlBottom = new Panel();
            pnlBottom.Location = new Point(30, 520);
            pnlBottom.Size = new Size(940, 190);
            pnlBottom.BackColor = AppPalette.Surface;
            pnlBottom.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(pnlBottom);

            lblUpdateStatus = new Label();
            lblUpdateStatus.Text = "Update Status:";
            lblUpdateStatus.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblUpdateStatus.ForeColor = AppPalette.TextPrimary;
            lblUpdateStatus.AutoSize = true;
            lblUpdateStatus.Location = new Point(20, 15);
            pnlBottom.Controls.Add(lblUpdateStatus);

            cmbUpdateStatus = new ComboBox();
            cmbUpdateStatus.Location = new Point(150, 13);
            cmbUpdateStatus.Size = new Size(180, 25);
            cmbUpdateStatus.Font = new Font("Segoe UI", 10);
            cmbUpdateStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUpdateStatus.Items.AddRange(new object[] { "Pending", "InProgress", "Completed", "Rejected" });
            cmbUpdateStatus.SelectedIndex = 0;
            pnlBottom.Controls.Add(cmbUpdateStatus);

            btnUpdateStatus = new Button();
            btnUpdateStatus.Text = "Apply";
            btnUpdateStatus.Size = new Size(100, 28);
            btnUpdateStatus.Location = new Point(340, 12);
            btnUpdateStatus.BackColor = AppPalette.AccentPrimary;
            btnUpdateStatus.FlatStyle = FlatStyle.Flat;
            btnUpdateStatus.FlatAppearance.BorderColor = AppPalette.Border;
            btnUpdateStatus.ForeColor = AppPalette.TextOnAccent;
            btnUpdateStatus.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnUpdateStatus.UseVisualStyleBackColor = false;
            btnUpdateStatus.Click += BtnUpdateStatus_Click;
            btnUpdateStatus.Enabled = false;
            pnlBottom.Controls.Add(btnUpdateStatus);

            lblPriorityQueue = new Label();
            lblPriorityQueue.Text = "Priority Queue";
            lblPriorityQueue.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblPriorityQueue.ForeColor = AppPalette.TextPrimary;
            lblPriorityQueue.AutoSize = true;
            lblPriorityQueue.Location = new Point(20, 55);
            pnlBottom.Controls.Add(lblPriorityQueue);

            lvPriorityQueue = new ListView();
            lvPriorityQueue.Location = new Point(20, 75);
            lvPriorityQueue.Size = new Size(500, 100);
            lvPriorityQueue.View = View.Details;
            lvPriorityQueue.FullRowSelect = true;
            lvPriorityQueue.GridLines = false;
            lvPriorityQueue.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvPriorityQueue.Columns.Add("Priority", 70);
            lvPriorityQueue.Columns.Add("Category", 130);
            lvPriorityQueue.Columns.Add("Location", 120);
            lvPriorityQueue.Columns.Add("Status", 80);
            lvPriorityQueue.Columns.Add("Submitted", 100);
            pnlBottom.Controls.Add(lvPriorityQueue);

            lblRelatedRequests = new Label();
            lblRelatedRequests.Text = "Related Requests";
            lblRelatedRequests.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblRelatedRequests.ForeColor = AppPalette.TextPrimary;
            lblRelatedRequests.AutoSize = true;
            lblRelatedRequests.Location = new Point(540, 15);
            pnlBottom.Controls.Add(lblRelatedRequests);

            lvRelatedRequests = new ListView();
            lvRelatedRequests.Location = new Point(540, 40);
            lvRelatedRequests.Size = new Size(360, 55);
            lvRelatedRequests.View = View.Details;
            lvRelatedRequests.FullRowSelect = true;
            lvRelatedRequests.GridLines = false;
            lvRelatedRequests.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvRelatedRequests.Columns.Add("Request", 140);
            lvRelatedRequests.Columns.Add("Category", 120);
            lvRelatedRequests.Columns.Add("Status", 100);
            pnlBottom.Controls.Add(lvRelatedRequests);

            lblGraphTraversal = new Label();
            lblGraphTraversal.Text = "Graph Traversal";
            lblGraphTraversal.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblGraphTraversal.ForeColor = AppPalette.TextPrimary;
            lblGraphTraversal.AutoSize = true;
            lblGraphTraversal.Location = new Point(540, 105);
            pnlBottom.Controls.Add(lblGraphTraversal);

            cmbGraphTraversal = new ComboBox();
            cmbGraphTraversal.Location = new Point(660, 102);
            cmbGraphTraversal.Size = new Size(130, 25);
            cmbGraphTraversal.Font = new Font("Segoe UI", 10);
            cmbGraphTraversal.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGraphTraversal.Items.AddRange(new object[] { "BFS", "DFS", "MST" });
            cmbGraphTraversal.SelectedIndexChanged += GraphTraversalChanged;
            pnlBottom.Controls.Add(cmbGraphTraversal);

            lvGraphTraversal = new ListView();
            lvGraphTraversal.Location = new Point(540, 130);
            lvGraphTraversal.Size = new Size(360, 45);
            lvGraphTraversal.View = View.Details;
            lvGraphTraversal.FullRowSelect = true;
            lvGraphTraversal.GridLines = false;
            lvGraphTraversal.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvGraphTraversal.Columns.Add("Step", 70);
            lvGraphTraversal.Columns.Add("Details", 180);
            lvGraphTraversal.Columns.Add("Extra", 100);
            pnlBottom.Controls.Add(lvGraphTraversal);

            cmbGraphTraversal.SelectedIndex = 0;

            allServiceRequests = new List<ServiceRequest>();
            requestTree = new ServiceRequestBST();
            requestBalancedTree = new ServiceRequestAVL();
            requestHeap = new ServiceRequestHeap();
            requestGraph = new ServiceRequestGraph();
        }

        private void LoadServiceRequests() {
            try {
                allServiceRequests.Clear();
                requestTree.Clear();
                requestBalancedTree.Clear();
                requestHeap.Clear();
                requestGraph.Clear();

                List<Issue> issues = dbHelper.GetAllIssues();

                foreach (Issue issue in issues) {
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
                UpdateGraphDisplays();
            }
            catch (Exception ex) {
                MessageBox.Show("Error loading service requests: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AssignPriorityBasedOnCategory(ServiceRequest request) {
            if (request == null || request.Priority != ServiceRequestPriority.Medium) {
                return;
            }

            if (string.IsNullOrWhiteSpace(request.Category)) {
                request.Priority = ServiceRequestPriority.Medium;
                return;
            }

            switch (request.Category.ToLowerInvariant()) {
                case "water":
                case "electricity":
                    request.Priority = ServiceRequestPriority.Urgent;
                    break;
                case "sanitation":
                case "utilities":
                    request.Priority = ServiceRequestPriority.High;
                    break;
                case "roads":
                    request.Priority = ServiceRequestPriority.Medium;
                    break;
                default:
                    request.Priority = ServiceRequestPriority.Low;
                    break;
            }
        }

        private void DisplayServiceRequests(List<ServiceRequest> requests) {
            List<ServiceRequestRow> rows = new List<ServiceRequestRow>();

            foreach (ServiceRequest request in requests) {
                string reference = request.UniqueID.Length >= 8 ? request.UniqueID.Substring(0, 8) : request.UniqueID;
                string description = string.IsNullOrWhiteSpace(request.Description) ? string.Empty : request.Description;
                if (description.Length > 50) {
                    description = description.Substring(0, 50) + "...";
                }

                rows.Add(new ServiceRequestRow {
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

            if (dgvRequests.Columns["UniqueId"] != null) {
                dgvRequests.Columns["UniqueId"].Visible = false;
            }

            if (dgvRequests.Columns["Reference"] != null) {
                dgvRequests.Columns["Reference"].HeaderText = "Request";
            }

            if (dgvRequests.Columns["DateSubmitted"] != null) {
                dgvRequests.Columns["DateSubmitted"].HeaderText = "Submitted";
            }

            lblTotalRequests.Text = "Total Requests: " + requests.Count;

            foreach (DataGridViewRow row in dgvRequests.Rows) {
                string status = row.Cells["Status"].Value?.ToString();
                switch (status) {
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

        private void BtnSearch_Click(object sender, EventArgs e) {
            string searchId = txtSearchID.Text.Trim();

            if (string.IsNullOrEmpty(searchId)) {
                MessageBox.Show("Please enter a request ID to search.",
                    "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<ServiceRequest> foundRequests = requestBalancedTree
                .InOrder()
                .Where(r => r.UniqueID.StartsWith(searchId, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (foundRequests.Count == 0) {
                MessageBox.Show("No service request found with ID starting with: " + searchId,
                    "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                List<ServiceRequest> sortedResults = SortRequests(foundRequests).ToList();
                DisplayServiceRequests(sortedResults);
                UpdateStatistics(sortedResults);
            }
        }

        private void FilterChanged(object sender, EventArgs e) {
            ApplyFilters();
        }

        private void ApplyFilters() {
            IEnumerable<ServiceRequest> filteredRequests = requestBalancedTree.InOrder();

            if (!string.Equals(cmbStatusFilter.SelectedItem?.ToString(), "All", StringComparison.OrdinalIgnoreCase)) {
                string selectedStatus = cmbStatusFilter.SelectedItem.ToString();
                filteredRequests = filteredRequests.Where(r => r.GetStatusString() == selectedStatus);
            }

            if (!string.Equals(cmbCategoryFilter.SelectedItem?.ToString(), "All", StringComparison.OrdinalIgnoreCase)) {
                string selectedCategory = cmbCategoryFilter.SelectedItem.ToString();
                filteredRequests = filteredRequests.Where(r =>
                    r.Category.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase));
            }

            List<ServiceRequest> sortedRequests = SortRequests(filteredRequests).ToList();
            DisplayServiceRequests(sortedRequests);
            PopulatePriorityQueueDisplay();
            UpdateStatistics(sortedRequests);
        }

        private void BtnRefresh_Click(object sender, EventArgs e) {
            txtSearchID.Text = string.Empty;
            cmbStatusFilter.SelectedIndex = 0;
            cmbCategoryFilter.SelectedIndex = 0;
            LoadServiceRequests();
        }

        private void SortChanged(object sender, EventArgs e) {
            ApplyFilters();
        }

        private void DgvRequests_SelectionChanged(object sender, EventArgs e) {
            SyncStatusControls();
        }

        private void BtnUpdateStatus_Click(object sender, EventArgs e) {
            string uniqueId = GetSelectedUniqueId();
            if (string.IsNullOrWhiteSpace(uniqueId)) {
                MessageBox.Show("Select a service request first.", "Status Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cmbUpdateStatus.SelectedItem == null) {
                MessageBox.Show("Choose a status before applying.", "Status Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ServiceRequestStatus newStatus = (ServiceRequestStatus)Enum.Parse(typeof(ServiceRequestStatus), cmbUpdateStatus.SelectedItem.ToString());
            ServiceRequest request = allServiceRequests.FirstOrDefault(r => string.Equals(r.UniqueID, uniqueId, StringComparison.OrdinalIgnoreCase));
            if (request == null) {
                MessageBox.Show("The selected request could not be found.", "Status Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (request.Status == newStatus) {
                MessageBox.Show("The request already has that status.", "Status Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try {
                request.Status = newStatus;
                dbHelper.UpdateIssueStatus(uniqueId, newStatus, request.Priority);
                LoadServiceRequests();
                ReselectRow(uniqueId);
            }
            catch (Exception ex) {
                MessageBox.Show("Error updating request: " + ex.Message,
                    "Status Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetSelectedUniqueId() {
            if (dgvRequests.CurrentRow == null) {
                return null;
            }

            object value = dgvRequests.CurrentRow.Cells["UniqueId"]?.Value;
            return value?.ToString();
        }

        private void SyncStatusControls() {
            if (cmbUpdateStatus == null) {
                return;
            }

            string uniqueId = GetSelectedUniqueId();
            if (string.IsNullOrWhiteSpace(uniqueId)) {
                btnUpdateStatus.Enabled = false;
                UpdateGraphDisplays();
                return;
            }

            ServiceRequest request = allServiceRequests.FirstOrDefault(r => string.Equals(r.UniqueID, uniqueId, StringComparison.OrdinalIgnoreCase));
            if (request == null) {
                btnUpdateStatus.Enabled = false;
                UpdateGraphDisplays();
                return;
            }

            btnUpdateStatus.Enabled = true;
            string statusText = request.Status.ToString();
            int index = cmbUpdateStatus.Items.IndexOf(statusText);
            if (index >= 0) {
                cmbUpdateStatus.SelectedIndex = index;
            }

            UpdateGraphDisplays();
        }

        private void ReselectRow(string uniqueId) {
            if (string.IsNullOrWhiteSpace(uniqueId)) {
                return;
            }

            foreach (DataGridViewRow row in dgvRequests.Rows) {
                string value = row.Cells["UniqueId"].Value?.ToString();
                if (string.Equals(value, uniqueId, StringComparison.OrdinalIgnoreCase)) {
                    row.Selected = true;
                    dgvRequests.CurrentCell = row.Cells["Reference"];
                    break;
                }
            }

            SyncStatusControls();
        }

        private void PopulatePriorityQueueDisplay() {
            lvPriorityQueue.BeginUpdate();
            lvPriorityQueue.Items.Clear();

            foreach (ServiceRequest request in requestHeap.GetOrderedSnapshot().Take(10)) {
                ListViewItem item = new ListViewItem(request.GetPriorityString());
                item.SubItems.Add(request.Category);
                item.SubItems.Add(request.Location);
                item.SubItems.Add(request.GetStatusString());
                item.SubItems.Add(request.DateSubmitted.ToString("yyyy-MM-dd HH:mm"));
                lvPriorityQueue.Items.Add(item);
            }

            lvPriorityQueue.EndUpdate();
        }

        private void BtnBack_Click(object sender, EventArgs e) {
            Close();
        }

        private void GraphTraversalChanged(object sender, EventArgs e) {
            PopulateTraversalDisplay(GetSelectedUniqueId());
        }

        private void UpdateGraphDisplays() {
            PopulateRelatedRequests(GetSelectedUniqueId());
            PopulateTraversalDisplay(GetSelectedUniqueId());
        }

        private void PopulateRelatedRequests(string uniqueId) {
            if (lvRelatedRequests == null) {
                return;
            }

            lvRelatedRequests.BeginUpdate();
            lvRelatedRequests.Items.Clear();

            if (string.IsNullOrWhiteSpace(uniqueId) || requestGraph == null) {
                lvRelatedRequests.EndUpdate();
                return;
            }

            IReadOnlyList<ServiceRequest> related = requestGraph.GetRelatedRequests(uniqueId);
            foreach (ServiceRequest relatedRequest in related
                .OrderByDescending(r => r.Priority)
                .ThenBy(r => r.DateSubmitted)) {
                ListViewItem item = new ListViewItem(ShortenId(relatedRequest.UniqueID));
                item.SubItems.Add(relatedRequest.Category ?? string.Empty);
                item.SubItems.Add(relatedRequest.GetStatusString());
                lvRelatedRequests.Items.Add(item);
            }

            lvRelatedRequests.EndUpdate();
        }

        private void PopulateTraversalDisplay(string uniqueId) {
            if (lvGraphTraversal == null) {
                return;
            }

            lvGraphTraversal.BeginUpdate();
            lvGraphTraversal.Items.Clear();

            if (string.IsNullOrWhiteSpace(uniqueId) || requestGraph == null || cmbGraphTraversal == null || cmbGraphTraversal.SelectedItem == null) {
                lvGraphTraversal.EndUpdate();
                return;
            }

            string mode = cmbGraphTraversal.SelectedItem.ToString();

            if (string.Equals(mode, "MST", StringComparison.OrdinalIgnoreCase)) {
                IReadOnlyList<GraphEdge> edges = requestGraph.MinimumSpanningTree(uniqueId);
                int step = 1;
                foreach (GraphEdge edge in edges) {
                    string details = ShortenId(edge.Source.Value.UniqueID) + " -> " + ShortenId(edge.Target.Value.UniqueID);
                    ListViewItem item = new ListViewItem(step.ToString());
                    item.SubItems.Add(details);
                    item.SubItems.Add(edge.Weight.ToString("0.##"));
                    lvGraphTraversal.Items.Add(item);
                    step++;
                }
            }
            else {
                IReadOnlyList<ServiceRequest> traversal;
                if (string.Equals(mode, "DFS", StringComparison.OrdinalIgnoreCase)) {
                    traversal = requestGraph.DepthFirst(uniqueId);
                }
                else {
                    traversal = requestGraph.BreadthFirst(uniqueId);
                }

                for (int i = 0; i < traversal.Count; i++) {
                    ServiceRequest request = traversal[i];
                    ListViewItem item = new ListViewItem((i + 1).ToString());
                    item.SubItems.Add(ShortenId(request.UniqueID) + " | " + (request.Location ?? string.Empty));
                    item.SubItems.Add(request.Category ?? string.Empty);
                    lvGraphTraversal.Items.Add(item);
                }
            }

            lvGraphTraversal.EndUpdate();
        }

        private static string ShortenId(string uniqueId) {
            if (string.IsNullOrWhiteSpace(uniqueId)) {
                return string.Empty;
            }

            return uniqueId.Length <= 8 ? uniqueId : uniqueId.Substring(0, 8);
        }

        private IEnumerable<ServiceRequest> SortRequests(IEnumerable<ServiceRequest> requests) {
            if (requests == null) {
                return Enumerable.Empty<ServiceRequest>();
            }

            string mode = cmbSortBy?.SelectedItem?.ToString() ?? string.Empty;

            switch (mode) {
                case "Oldest first":
                    return requests.OrderBy(r => r.DateSubmitted);
                case "Priority high-low":
                    return requests.OrderByDescending(r => r.Priority).ThenByDescending(r => r.DateSubmitted);
                case "Priority low-high":
                    return requests.OrderBy(r => r.Priority).ThenByDescending(r => r.DateSubmitted);
                case "Category A-Z":
                    return requests.OrderBy(r => r.Category ?? string.Empty).ThenByDescending(r => r.DateSubmitted);
                default:
                    return requests.OrderByDescending(r => r.DateSubmitted);
            }
        }

        private void UpdateStatistics(IEnumerable<ServiceRequest> requests) {
            if (lblStatistics == null) {
                return;
            }

            List<ServiceRequest> list = requests == null ? new List<ServiceRequest>() : (requests as List<ServiceRequest> ?? requests.ToList());

            if (list.Count == 0) {
                lblStatistics.Text = "No requests";
                return;
            }

            int pending = list.Count(r => r.Status == ServiceRequestStatus.Pending);
            int inProgress = list.Count(r => r.Status == ServiceRequestStatus.InProgress);
            int completed = list.Count(r => r.Status == ServiceRequestStatus.Completed);
            int rejected = list.Count(r => r.Status == ServiceRequestStatus.Rejected);
            int highPriority = list.Count(r => r.Priority >= ServiceRequestPriority.High);

            lblStatistics.Text = "Total: " + list.Count +
                Environment.NewLine + "Pending: " + pending +
                Environment.NewLine + "In progress: " + inProgress +
                Environment.NewLine + "Completed: " + completed +
                Environment.NewLine + "Rejected: " + rejected +
                Environment.NewLine + "High or urgent: " + highPriority +
                Environment.NewLine + ValidateDataStructures(allServiceRequests);
        }

        private string ValidateDataStructures(IEnumerable<ServiceRequest> source) {
            if (source == null) {
                return "Validation: no data";
            }

            List<ServiceRequest> list = source as List<ServiceRequest> ?? source.ToList();
            if (list.Count == 0) {
                return "Validation: no data";
            }

            List<string> issues = new List<string>();

            IReadOnlyList<ServiceRequest> bstOrder = requestTree.InOrder();
            if (!IsRequestOrderAscending(bstOrder)) {
                issues.Add("BST order");
            }

            IReadOnlyList<ServiceRequest> avlOrder = requestBalancedTree.InOrder();
            if (!IsRequestOrderAscending(avlOrder)) {
                issues.Add("AVL order");
            }

            IReadOnlyList<ServiceRequest> heapOrder = requestHeap.GetOrderedSnapshot();
            if (!IsHeapOrderValid(heapOrder)) {
                issues.Add("Heap order");
            }

            if (!IsGraphConsistent(list)) {
                issues.Add("Graph links");
            }

            if (issues.Count == 0) {
                return "Validation: ok";
            }

            return "Validation: " + string.Join(", ", issues);
        }

        private bool IsRequestOrderAscending(IEnumerable<ServiceRequest> requests) {
            if (requests == null) {
                return true;
            }

            string previous = null;
            foreach (ServiceRequest request in requests) {
                if (request == null) {
                    continue;
                }

                string key = request.UniqueID ?? string.Empty;
                if (previous != null && string.Compare(previous, key, StringComparison.OrdinalIgnoreCase) > 0) {
                    return false;
                }

                previous = key;
            }

            return true;
        }

        private bool IsHeapOrderValid(IReadOnlyList<ServiceRequest> requests) {
            if (requests == null) {
                return true;
            }

            ServiceRequestPriority? lastPriority = null;
            DateTime? lastTime = null;

            foreach (ServiceRequest request in requests) {
                if (request == null) {
                    continue;
                }

                if (lastPriority != null) {
                    int comparison = lastPriority.Value.CompareTo(request.Priority);
                    if (comparison < 0) {
                        return false;
                    }

                    if (comparison == 0 && lastTime != null && lastTime.Value > request.DateSubmitted) {
                        return false;
                    }
                }

                lastPriority = request.Priority;
                lastTime = request.DateSubmitted;
            }

            return true;
        }

        private bool IsGraphConsistent(IEnumerable<ServiceRequest> requests) {
            if (requests == null) {
                return true;
            }

            Dictionary<string, ServiceRequest> lookup = requests
                .Where(r => !string.IsNullOrWhiteSpace(r.UniqueID))
                .ToDictionary(r => r.UniqueID, StringComparer.OrdinalIgnoreCase);

            foreach (ServiceRequest request in requests) {
                if (request == null || string.IsNullOrWhiteSpace(request.UniqueID)) {
                    continue;
                }

                IReadOnlyList<ServiceRequest> related = requestGraph.GetRelatedRequests(request.UniqueID);

                HashSet<string> relatedSet = new HashSet<string>(related.Select(r => r.UniqueID), StringComparer.OrdinalIgnoreCase);
                foreach (string relatedId in request.RelatedRequestIDs) {
                    if (!relatedSet.Contains(relatedId)) {
                        return false;
                    }

                    if (!lookup.TryGetValue(relatedId, out ServiceRequest relatedRequest)) {
                        return false;
                    }

                    if (!relatedRequest.RelatedRequestIDs.Any(id => string.Equals(id, request.UniqueID, StringComparison.OrdinalIgnoreCase))) {
                        return false;
                    }
                }
            }

            return true;
        }

        private void InitializeComponent() {
            SuspendLayout();
            ClientSize = new Size(1000, 700);
            Name = "ServiceRequestStatusForm";
            ResumeLayout(false);
        }

        private class ServiceRequestRow {
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
