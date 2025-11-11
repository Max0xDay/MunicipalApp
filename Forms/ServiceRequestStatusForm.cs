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
        private ComboBox cmbTreeView;
        private Label lblStatistics;
        private Panel pnlBottom;

        private List<ServiceRequest> allServiceRequests;
        private ServiceRequestBST requestTree;
        private ServiceRequestAVL requestBalancedTree;
        private ServiceRequestHeap requestHeap;
        private ServiceRequestGraph requestGraph;
        private ServiceRequestRedBlackTree requestRedBlackTree;
        private ServiceRequestTree requestHierarchyTree;
        private ServiceRequestBinaryTree requestBinaryTree;
        private DatabaseHelper dbHelper;
        
        private TabControl tabControl;
        private TabPage tabOverview;
        private TabPage tabTreeAnalysis;
        private Label lblDataStructureHints;

        public ServiceRequestStatusForm() {
            InitializeComponent();
            SetupForm();
            dbHelper = new DatabaseHelper();
            LoadServiceRequests();
        }

        private void SetupForm() {
            Text = "Service Request Status";
            Size = new Size(1250, 970);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = AppPalette.Background;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(1250, 970);

            lblTitle = new Label();
            lblTitle.Text = "Service Request Status Tracking";
            lblTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitle.ForeColor = AppPalette.TextHeading;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(50, 30);
            Controls.Add(lblTitle);

            Panel pnlStats = new Panel();
            pnlStats.Location = new Point(50, 70);
            pnlStats.Size = new Size(1118, 40);
            pnlStats.BackColor = AppPalette.CodeBlock;
            pnlStats.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(pnlStats);

            lblTotalRequests = new Label();
            lblTotalRequests.Text = "Total: 0";
            lblTotalRequests.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblTotalRequests.ForeColor = AppPalette.TextHeading;
            lblTotalRequests.AutoSize = true;
            lblTotalRequests.Location = new Point(20, 10);
            pnlStats.Controls.Add(lblTotalRequests);

            lblStatistics = new Label();
            lblStatistics.Text = string.Empty;
            lblStatistics.Font = new Font("Segoe UI", 10);
            lblStatistics.ForeColor = AppPalette.TextSecondary;
            lblStatistics.AutoSize = true;
            lblStatistics.Location = new Point(120, 11);
            pnlStats.Controls.Add(lblStatistics);
            
            Panel pnlAlgorithmInfo = new Panel();
            pnlAlgorithmInfo.Location = new Point(810, 70);
            pnlAlgorithmInfo.Size = new Size(358, 40);
            pnlAlgorithmInfo.BackColor = AppPalette.Surface;
            pnlAlgorithmInfo.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(pnlAlgorithmInfo);
            
            Label lblAlgoTitle = new Label();
            lblAlgoTitle.Text = "BST, AVL, Red-Black, Heap, Graph, BFS/DFS, MST";
            lblAlgoTitle.Font = new Font("Segoe UI", 8);
            lblAlgoTitle.ForeColor = AppPalette.TextMuted;
            lblAlgoTitle.AutoSize = false;
            lblAlgoTitle.Size = new Size(340, 30);
            lblAlgoTitle.Location = new Point(10, 5);
            pnlAlgorithmInfo.Controls.Add(lblAlgoTitle);

            pnlControls = new Panel();
            pnlControls.Location = new Point(50, 125);
            pnlControls.Size = new Size(1118, 90);
            pnlControls.BackColor = AppPalette.Surface;
            pnlControls.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(pnlControls);

            Label lblSearchLabel = new Label();
            lblSearchLabel.Text = "Search ID:";
            lblSearchLabel.Font = new Font("Segoe UI", 9);
            lblSearchLabel.ForeColor = AppPalette.TextPrimary;
            lblSearchLabel.AutoSize = true;
            lblSearchLabel.Location = new Point(20, 18);
            pnlControls.Controls.Add(lblSearchLabel);

            txtSearchID = new TextBox();
            txtSearchID.Location = new Point(20, 38);
            txtSearchID.Size = new Size(180, 25);
            txtSearchID.Font = new Font("Segoe UI", 10);
            txtSearchID.BackColor = AppPalette.CodeBlock;
            txtSearchID.ForeColor = AppPalette.TextPrimary;
            txtSearchID.BorderStyle = BorderStyle.FixedSingle;
            pnlControls.Controls.Add(txtSearchID);

            btnSearch = new Button();
            btnSearch.Text = "Search";
            btnSearch.Size = new Size(90, 32);
            btnSearch.Location = new Point(210, 35);
            btnSearch.BackColor = AppPalette.AccentPrimary;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.ForeColor = AppPalette.TextOnAccent;
            btnSearch.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += BtnSearch_Click;
            btnSearch.MouseEnter += (s, e) => btnSearch.BackColor = AppPalette.AccentHover;
            btnSearch.MouseLeave += (s, e) => btnSearch.BackColor = AppPalette.AccentPrimary;
            pnlControls.Controls.Add(btnSearch);

            lblStatusFilter = new Label();
            lblStatusFilter.Text = "Status:";
            lblStatusFilter.Font = new Font("Segoe UI", 9);
            lblStatusFilter.ForeColor = AppPalette.TextPrimary;
            lblStatusFilter.AutoSize = true;
            lblStatusFilter.Location = new Point(330, 18);
            pnlControls.Controls.Add(lblStatusFilter);

            cmbStatusFilter = new ComboBox();
            cmbStatusFilter.Location = new Point(330, 38);
            cmbStatusFilter.Size = new Size(150, 25);
            cmbStatusFilter.Font = new Font("Segoe UI", 9);
            cmbStatusFilter.BackColor = AppPalette.CodeBlock;
            cmbStatusFilter.ForeColor = AppPalette.TextPrimary;
            cmbStatusFilter.FlatStyle = FlatStyle.Flat;
            cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatusFilter.Items.AddRange(new object[] { "All", "Pending", "InProgress", "Completed", "Rejected" });
            cmbStatusFilter.SelectedIndex = 0;
            cmbStatusFilter.SelectedIndexChanged += FilterChanged;
            pnlControls.Controls.Add(cmbStatusFilter);

            lblCategoryFilter = new Label();
            lblCategoryFilter.Text = "Category:";
            lblCategoryFilter.Font = new Font("Segoe UI", 9);
            lblCategoryFilter.ForeColor = AppPalette.TextPrimary;
            lblCategoryFilter.AutoSize = true;
            lblCategoryFilter.Location = new Point(500, 18);
            pnlControls.Controls.Add(lblCategoryFilter);

            cmbCategoryFilter = new ComboBox();
            cmbCategoryFilter.Location = new Point(500, 38);
            cmbCategoryFilter.Size = new Size(150, 25);
            cmbCategoryFilter.Font = new Font("Segoe UI", 9);
            cmbCategoryFilter.BackColor = AppPalette.CodeBlock;
            cmbCategoryFilter.ForeColor = AppPalette.TextPrimary;
            cmbCategoryFilter.FlatStyle = FlatStyle.Flat;
            cmbCategoryFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoryFilter.Items.AddRange(new object[] { "All", "Sanitation", "Roads", "Utilities", "Water", "Electricity" });
            cmbCategoryFilter.SelectedIndex = 0;
            cmbCategoryFilter.SelectedIndexChanged += FilterChanged;
            pnlControls.Controls.Add(cmbCategoryFilter);

            lblSortBy = new Label();
            lblSortBy.Text = "Sort (AVL):";
            lblSortBy.Font = new Font("Segoe UI", 9);
            lblSortBy.ForeColor = AppPalette.TextPrimary;
            lblSortBy.AutoSize = true;
            lblSortBy.Location = new Point(670, 18);
            pnlControls.Controls.Add(lblSortBy);

            cmbSortBy = new ComboBox();
            cmbSortBy.Location = new Point(670, 38);
            cmbSortBy.Size = new Size(170, 25);
            cmbSortBy.Font = new Font("Segoe UI", 9);
            cmbSortBy.BackColor = AppPalette.CodeBlock;
            cmbSortBy.ForeColor = AppPalette.TextPrimary;
            cmbSortBy.FlatStyle = FlatStyle.Flat;
            cmbSortBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSortBy.Items.AddRange(new object[] { "Newest first", "Oldest first", "Priority high-low", "Priority low-high", "Category A-Z" });
            cmbSortBy.SelectedIndex = 0;
            cmbSortBy.SelectedIndexChanged += SortChanged;
            pnlControls.Controls.Add(cmbSortBy);
            
            Label lblTreeView = new Label();
            lblTreeView.Text = "Tree View:";
            lblTreeView.Font = new Font("Segoe UI", 9);
            lblTreeView.ForeColor = AppPalette.TextPrimary;
            lblTreeView.AutoSize = true;
            lblTreeView.Location = new Point(860, 18);
            pnlControls.Controls.Add(lblTreeView);
            
            cmbTreeView = new ComboBox();
            cmbTreeView.Location = new Point(860, 38);
            cmbTreeView.Size = new Size(180, 25);
            cmbTreeView.Font = new Font("Segoe UI", 9);
            cmbTreeView.BackColor = AppPalette.CodeBlock;
            cmbTreeView.ForeColor = AppPalette.TextPrimary;
            cmbTreeView.FlatStyle = FlatStyle.Flat;
            cmbTreeView.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTreeView.Items.AddRange(new object[] { 
                "Red-Black by Category", 
                "Hierarchy Tree", 
                "Binary Tree Order" 
            });
            cmbTreeView.SelectedIndex = 0;
            cmbTreeView.SelectedIndexChanged += TreeViewChanged;
            pnlControls.Controls.Add(cmbTreeView);

            btnRefresh = new Button();
            btnRefresh.Text = "Refresh";
            btnRefresh.Size = new Size(100, 60);
            btnRefresh.Location = new Point(890, 15);
            btnRefresh.BackColor = AppPalette.AccentPrimary;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.ForeColor = AppPalette.TextOnAccent;
            btnRefresh.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += BtnRefresh_Click;
            btnRefresh.MouseEnter += (s, e) => btnRefresh.BackColor = AppPalette.AccentHover;
            btnRefresh.MouseLeave += (s, e) => btnRefresh.BackColor = AppPalette.AccentPrimary;
            pnlControls.Controls.Add(btnRefresh);

            btnBack = new Button();
            btnBack.Text = "Back";
            btnBack.Size = new Size(100, 60);
            btnBack.Location = new Point(1000, 15);
            btnBack.BackColor = AppPalette.Surface;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderColor = AppPalette.Border;
            btnBack.FlatAppearance.BorderSize = 2;
            btnBack.ForeColor = AppPalette.TextPrimary;
            btnBack.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += BtnBack_Click;
            pnlControls.Controls.Add(btnBack);

            dgvRequests = new DataGridView();
            dgvRequests.Location = new Point(50, 230);
            dgvRequests.Size = new Size(1118, 285);
            dgvRequests.BackgroundColor = AppPalette.CodeBlock;
            dgvRequests.GridColor = AppPalette.Border;
            dgvRequests.BorderStyle = BorderStyle.FixedSingle;
            dgvRequests.AllowUserToAddRows = false;
            dgvRequests.AllowUserToDeleteRows = false;
            dgvRequests.AllowUserToResizeColumns = false;
            dgvRequests.AllowUserToResizeRows = false;
            dgvRequests.ReadOnly = true;
            dgvRequests.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRequests.MultiSelect = false;
            dgvRequests.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRequests.RowHeadersVisible = false;
            dgvRequests.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvRequests.Font = new Font("Segoe UI", 9);
            dgvRequests.DefaultCellStyle.BackColor = AppPalette.CodeBlock;
            dgvRequests.DefaultCellStyle.ForeColor = AppPalette.TextPrimary;
            dgvRequests.AlternatingRowsDefaultCellStyle.BackColor = AppPalette.Surface;
            dgvRequests.AlternatingRowsDefaultCellStyle.ForeColor = AppPalette.TextPrimary;
            dgvRequests.ColumnHeadersDefaultCellStyle.BackColor = AppPalette.AccentPrimary;
            dgvRequests.ColumnHeadersDefaultCellStyle.ForeColor = AppPalette.TextOnAccent;
            dgvRequests.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvRequests.EnableHeadersVisualStyles = false;
            dgvRequests.DefaultCellStyle.SelectionBackColor = Color.FromArgb(80, 0, 102, 204);
            dgvRequests.DefaultCellStyle.SelectionForeColor = AppPalette.TextPrimary;
            dgvRequests.SelectionChanged += DgvRequests_SelectionChanged;
            Controls.Add(dgvRequests);

            pnlBottom = new Panel();
            pnlBottom.Location = new Point(50, 530);
            pnlBottom.Size = new Size(1118, 330);
            pnlBottom.BackColor = AppPalette.Surface;
            pnlBottom.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(pnlBottom);

            lblUpdateStatus = new Label();
            lblUpdateStatus.Text = "Update Status:";
            lblUpdateStatus.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblUpdateStatus.ForeColor = AppPalette.TextHeading;
            lblUpdateStatus.AutoSize = true;
            lblUpdateStatus.Location = new Point(25, 20);
            pnlBottom.Controls.Add(lblUpdateStatus);

            cmbUpdateStatus = new ComboBox();
            cmbUpdateStatus.Location = new Point(140, 18);
            cmbUpdateStatus.Size = new Size(140, 25);
            cmbUpdateStatus.Font = new Font("Segoe UI", 9);
            cmbUpdateStatus.BackColor = AppPalette.CodeBlock;
            cmbUpdateStatus.ForeColor = AppPalette.TextPrimary;
            cmbUpdateStatus.FlatStyle = FlatStyle.Flat;
            cmbUpdateStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUpdateStatus.Items.AddRange(new object[] { "Pending", "InProgress", "Completed", "Rejected" });
            cmbUpdateStatus.SelectedIndex = 0;
            pnlBottom.Controls.Add(cmbUpdateStatus);

            btnUpdateStatus = new Button();
            btnUpdateStatus.Text = "Apply";
            btnUpdateStatus.Size = new Size(90, 28);
            btnUpdateStatus.Location = new Point(290, 17);
            btnUpdateStatus.BackColor = AppPalette.AccentPrimary;
            btnUpdateStatus.FlatStyle = FlatStyle.Flat;
            btnUpdateStatus.FlatAppearance.BorderSize = 0;
            btnUpdateStatus.ForeColor = AppPalette.TextOnAccent;
            btnUpdateStatus.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnUpdateStatus.UseVisualStyleBackColor = false;
            btnUpdateStatus.Click += BtnUpdateStatus_Click;
            btnUpdateStatus.Enabled = false;
            btnUpdateStatus.MouseEnter += (s, e) => { if (btnUpdateStatus.Enabled) btnUpdateStatus.BackColor = AppPalette.AccentHover; };
            btnUpdateStatus.MouseLeave += (s, e) => { if (btnUpdateStatus.Enabled) btnUpdateStatus.BackColor = AppPalette.AccentPrimary; };
            pnlBottom.Controls.Add(btnUpdateStatus);

            lblPriorityQueue = new Label();
            lblPriorityQueue.Text = "Priority Queue (Heap)";
            lblPriorityQueue.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblPriorityQueue.ForeColor = AppPalette.TextHeading;
            lblPriorityQueue.AutoSize = true;
            lblPriorityQueue.Location = new Point(25, 60);
            pnlBottom.Controls.Add(lblPriorityQueue);

            lvPriorityQueue = new ListView();
            lvPriorityQueue.Location = new Point(25, 90);
            lvPriorityQueue.Size = new Size(525, 83);
            lvPriorityQueue.View = View.Details;
            lvPriorityQueue.FullRowSelect = true;
            lvPriorityQueue.GridLines = true;
            lvPriorityQueue.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvPriorityQueue.Font = new Font("Segoe UI", 9);
            lvPriorityQueue.BackColor = AppPalette.CodeBlock;
            lvPriorityQueue.ForeColor = AppPalette.TextPrimary;
            lvPriorityQueue.BorderStyle = BorderStyle.FixedSingle;
            lvPriorityQueue.AllowColumnReorder = false;
            lvPriorityQueue.Columns.Add("Priority", 60);
            lvPriorityQueue.Columns.Add("Category", 98);
            lvPriorityQueue.Columns.Add("Location", 150);
            lvPriorityQueue.Columns.Add("Status", 90);
            lvPriorityQueue.Columns.Add("Submitted", 105);
            foreach (ColumnHeader column in lvPriorityQueue.Columns) {
                column.Width = column.Width;
            }
            pnlBottom.Controls.Add(lvPriorityQueue);

            lblRelatedRequests = new Label();
            lblRelatedRequests.Text = "Related Requests (Graph)";
            lblRelatedRequests.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblRelatedRequests.ForeColor = AppPalette.TextHeading;
            lblRelatedRequests.AutoSize = true;
            lblRelatedRequests.Location = new Point(570, 60);
            pnlBottom.Controls.Add(lblRelatedRequests);

            lvRelatedRequests = new ListView();
            lvRelatedRequests.Location = new Point(570, 90);
            lvRelatedRequests.Size = new Size(540, 83);
            lvRelatedRequests.View = View.Details;
            lvRelatedRequests.FullRowSelect = true;
            lvRelatedRequests.GridLines = true;
            lvRelatedRequests.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvRelatedRequests.Font = new Font("Segoe UI", 9);
            lvRelatedRequests.BackColor = AppPalette.CodeBlock;
            lvRelatedRequests.ForeColor = AppPalette.TextPrimary;
            lvRelatedRequests.BorderStyle = BorderStyle.FixedSingle;
            lvRelatedRequests.AllowColumnReorder = false;
            lvRelatedRequests.Columns.Add("Request ID", 120);
            lvRelatedRequests.Columns.Add("Category", 98);
            lvRelatedRequests.Columns.Add("Status", 83);
            lvRelatedRequests.Columns.Add("Priority", 90);
            lvRelatedRequests.Columns.Add("Location", 120);
            foreach (ColumnHeader column in lvRelatedRequests.Columns) {
                column.Width = column.Width;
            }
            pnlBottom.Controls.Add(lvRelatedRequests);

            lblGraphTraversal = new Label();
            lblGraphTraversal.Text = "Traversal:";
            lblGraphTraversal.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblGraphTraversal.ForeColor = AppPalette.TextHeading;
            lblGraphTraversal.AutoSize = true;
            lblGraphTraversal.Location = new Point(25, 190);
            pnlBottom.Controls.Add(lblGraphTraversal);

            cmbGraphTraversal = new ComboBox();
            cmbGraphTraversal.Location = new Point(110, 188);
            cmbGraphTraversal.Size = new Size(180, 25);
            cmbGraphTraversal.Font = new Font("Segoe UI", 9);
            cmbGraphTraversal.BackColor = AppPalette.CodeBlock;
            cmbGraphTraversal.ForeColor = AppPalette.TextPrimary;
            cmbGraphTraversal.FlatStyle = FlatStyle.Flat;
            cmbGraphTraversal.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGraphTraversal.Items.AddRange(new object[] { "BFS (Breadth-First)", "DFS (Depth-First)", "MST (Min Span Tree)" });
            cmbGraphTraversal.SelectedIndexChanged += GraphTraversalChanged;
            pnlBottom.Controls.Add(cmbGraphTraversal);

            lvGraphTraversal = new ListView();
            lvGraphTraversal.Location = new Point(25, 220);
            lvGraphTraversal.Size = new Size(1080, 90);
            lvGraphTraversal.View = View.Details;
            lvGraphTraversal.FullRowSelect = true;
            lvGraphTraversal.GridLines = true;
            lvGraphTraversal.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvGraphTraversal.Font = new Font("Segoe UI", 9);
            lvGraphTraversal.BackColor = AppPalette.CodeBlock;
            lvGraphTraversal.ForeColor = AppPalette.TextPrimary;
            lvGraphTraversal.BorderStyle = BorderStyle.FixedSingle;
            lvGraphTraversal.AllowColumnReorder = false;
            lvGraphTraversal.Columns.Add("Step", 45);
            lvGraphTraversal.Columns.Add("Details", 675);
            lvGraphTraversal.Columns.Add("Connection Info", 330);
            foreach (ColumnHeader column in lvGraphTraversal.Columns) {
                column.Width = column.Width;
            }
            pnlBottom.Controls.Add(lvGraphTraversal);

            cmbGraphTraversal.SelectedIndex = 0;

            allServiceRequests = new List<ServiceRequest>();
            requestTree = new ServiceRequestBST();
            requestBalancedTree = new ServiceRequestAVL();
            requestHeap = new ServiceRequestHeap();
            requestGraph = new ServiceRequestGraph();
            requestRedBlackTree = new ServiceRequestRedBlackTree();
            requestHierarchyTree = new ServiceRequestTree();
            requestBinaryTree = new ServiceRequestBinaryTree();
            
            SetupDataStructureHints();
        }
        
        private void SetupDataStructureHints() {
            lblDataStructureHints = new Label();
            lblDataStructureHints.Location = new Point(50, 875);
            lblDataStructureHints.Size = new Size(1118, 20);
            lblDataStructureHints.Font = new Font("Segoe UI", 8, FontStyle.Italic);
            lblDataStructureHints.ForeColor = AppPalette.TextMuted;
            lblDataStructureHints.Text = "Algorithms: BST (search) | AVL (balanced sort) | Red-Black (category) | Trees (hierarchy/binary/basic) | Heap (priority) | Graph (relationships) | BFS/DFS/MST (traversal)";
            Controls.Add(lblDataStructureHints);
        }
        
        private void UpdateDataStructureHints() {
            if (lblDataStructureHints == null) {
                return;
            }
            
            int bstCount = requestTree.Count;
            int avlCount = requestBalancedTree.Count;
            int heapCount = requestHeap.Count;
            int graphCount = requestGraph.Count;
            int rbCount = requestRedBlackTree.Count;
            int hierarchyCount = requestHierarchyTree.Count;
            int binaryCount = requestBinaryTree.Count;
            
            lblDataStructureHints.Text = string.Format(
                "Algorithms Active - BST: {0} nodes | AVL Tree: {1} nodes | Red-Black Tree: {2} nodes | " +
                "Basic Tree: {3} nodes | Binary Tree: {4} nodes | Heap: {5} priority items | Graph: {6} vertices with BFS/DFS/MST",
                bstCount, avlCount, rbCount, hierarchyCount, binaryCount, heapCount, graphCount);
        }

        private void LoadServiceRequests() {
            try {
                allServiceRequests.Clear();
                requestTree.Clear();
                requestBalancedTree.Clear();
                requestHeap.Clear();
                requestGraph.Clear();
                requestRedBlackTree.Clear();
                requestHierarchyTree.Clear();
                requestBinaryTree.Clear();

                List<Issue> issues = dbHelper.GetAllIssues();

                foreach (Issue issue in issues) {
                    ServiceRequest request = new ServiceRequest(issue);
                    AssignPriorityBasedOnCategory(request);
                    allServiceRequests.Add(request);
                    requestTree.Insert(request);
                    requestBalancedTree.Insert(request);
                    requestHeap.Insert(request);
                    requestGraph.AddOrUpdate(request);
                    requestRedBlackTree.Insert(request);
                    requestHierarchyTree.Insert(request);
                    requestBinaryTree.Insert(request);
                }

                requestGraph.BuildRelationships(allServiceRequests);
                ApplyFilters();
                PopulatePriorityQueueDisplay();
                UpdateGraphDisplays();
                UpdateDataStructureHints();
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

            foreach (DataGridViewRow row in dgvRequests.Rows) {
                string status = row.Cells["Status"].Value?.ToString();
                switch (status) {
                    case "Pending":
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 224);
                        row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(80, 0, 102, 204);
                        row.DefaultCellStyle.SelectionForeColor = AppPalette.TextPrimary;
                        break;
                    case "InProgress":
                        row.DefaultCellStyle.BackColor = Color.FromArgb(173, 216, 230);
                        row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(80, 0, 102, 204);
                        row.DefaultCellStyle.SelectionForeColor = AppPalette.TextPrimary;
                        break;
                    case "Completed":
                        row.DefaultCellStyle.BackColor = Color.FromArgb(144, 238, 144);
                        row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(80, 0, 102, 204);
                        row.DefaultCellStyle.SelectionForeColor = AppPalette.TextPrimary;
                        break;
                    case "Rejected":
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 182, 193);
                        row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(80, 0, 102, 204);
                        row.DefaultCellStyle.SelectionForeColor = AppPalette.TextPrimary;
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

            if (mode.Contains("MST")) {
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
                if (mode.Contains("DFS")) {
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
            if (lblStatistics == null || lblTotalRequests == null) {
                return;
            }

            List<ServiceRequest> list = requests == null ? new List<ServiceRequest>() : (requests as List<ServiceRequest> ?? requests.ToList());

            if (list.Count == 0) {
                lblTotalRequests.Text = "Total: 0";
                lblStatistics.Text = "No requests found";
                return;
            }

            int pending = list.Count(r => r.Status == ServiceRequestStatus.Pending);
            int inProgress = list.Count(r => r.Status == ServiceRequestStatus.InProgress);
            int completed = list.Count(r => r.Status == ServiceRequestStatus.Completed);
            int rejected = list.Count(r => r.Status == ServiceRequestStatus.Rejected);
            int highPriority = list.Count(r => r.Priority >= ServiceRequestPriority.High);

            lblTotalRequests.Text = "Total: " + list.Count;
            lblStatistics.Text = "Pending: " + pending + " | In Progress: " + inProgress + 
                " | Completed: " + completed + " | Rejected: " + rejected + " | High Priority: " + highPriority;
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
        
        private void TreeViewChanged(object sender, EventArgs e) {
            if (cmbTreeView == null || cmbTreeView.SelectedItem == null) {
                return;
            }
            
            string selectedView = cmbTreeView.SelectedItem.ToString();
            List<ServiceRequest> displayRequests = new List<ServiceRequest>();
            
            if (selectedView.Contains("Red-Black")) {
                displayRequests = requestRedBlackTree.InOrder().ToList();
            }
            else if (selectedView.Contains("Hierarchy")) {
                displayRequests = requestHierarchyTree.GetAll().ToList();
            }
            else if (selectedView.Contains("Binary Tree")) {
                displayRequests = requestBinaryTree.LevelOrder().ToList();
            }
            
            DisplayServiceRequests(displayRequests);
            UpdateStatistics(displayRequests);
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
