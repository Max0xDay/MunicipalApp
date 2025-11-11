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
            Size = new Size(1600, 950);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = AppPalette.Background;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(1600, 950);

            Panel pnlSidebar = new Panel();
            pnlSidebar.Size = new Size(280, 950);
            pnlSidebar.Location = new Point(0, 0);
            pnlSidebar.BackColor = AppPalette.Sidebar;
            pnlSidebar.Dock = DockStyle.Left;
            Controls.Add(pnlSidebar);

            Label lblSidebarTitle = new Label();
            lblSidebarTitle.Text = "Service Requests";
            lblSidebarTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblSidebarTitle.ForeColor = AppPalette.TextHeading;
            lblSidebarTitle.AutoSize = false;
            lblSidebarTitle.Size = new Size(240, 50);
            lblSidebarTitle.Location = new Point(20, 40);
            pnlSidebar.Controls.Add(lblSidebarTitle);

            Label lblSidebarDesc = new Label();
            lblSidebarDesc.Text = "Track and manage all service requests";
            lblSidebarDesc.Font = new Font("Segoe UI", 9);
            lblSidebarDesc.ForeColor = AppPalette.TextMuted;
            lblSidebarDesc.AutoSize = false;
            lblSidebarDesc.Size = new Size(240, 40);
            lblSidebarDesc.Location = new Point(20, 90);
            pnlSidebar.Controls.Add(lblSidebarDesc);

            btnBack = new Button();
            btnBack.Text = "Back to Menu";
            btnBack.Size = new Size(240, 50);
            btnBack.Location = new Point(20, 850);
            btnBack.BackColor = AppPalette.Surface;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderColor = AppPalette.Border;
            btnBack.FlatAppearance.BorderSize = 2;
            btnBack.ForeColor = AppPalette.TextPrimary;
            btnBack.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += BtnBack_Click;
            pnlSidebar.Controls.Add(btnBack);

            lblTitle = new Label();
            lblTitle.Text = "Service Request Status Tracking";
            lblTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitle.ForeColor = AppPalette.TextHeading;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(330, 40);
            Controls.Add(lblTitle);
            
            Panel pnlAlgorithmInfo = new Panel();
            pnlAlgorithmInfo.Location = new Point(1120, 30);
            pnlAlgorithmInfo.Size = new Size(420, 70);
            pnlAlgorithmInfo.BackColor = AppPalette.CodeBlock;
            pnlAlgorithmInfo.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(pnlAlgorithmInfo);
            
            Label lblAlgoTitle = new Label();
            lblAlgoTitle.Text = "Active Data Structures:";
            lblAlgoTitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblAlgoTitle.ForeColor = AppPalette.TextSecondary;
            lblAlgoTitle.AutoSize = true;
            lblAlgoTitle.Location = new Point(15, 10);
            pnlAlgorithmInfo.Controls.Add(lblAlgoTitle);
            
            Label lblAlgoList = new Label();
            lblAlgoList.Text = "BST, AVL Tree, Red-Black Tree, Basic Tree, Binary Tree,\nHeap, Graph, BFS, DFS, MST";
            lblAlgoList.Font = new Font("Segoe UI", 9);
            lblAlgoList.ForeColor = AppPalette.TextMuted;
            lblAlgoList.AutoSize = false;
            lblAlgoList.Size = new Size(390, 45);
            lblAlgoList.Location = new Point(15, 30);
            pnlAlgorithmInfo.Controls.Add(lblAlgoList);

            pnlControls = new Panel();
            pnlControls.Location = new Point(330, 120);
            pnlControls.Size = new Size(1210, 140);
            pnlControls.BackColor = AppPalette.Surface;
            pnlControls.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(pnlControls);

            Label lblSearchLabel = new Label();
            lblSearchLabel.Text = "Search by ID (using BST):";
            lblSearchLabel.Font = new Font("Segoe UI", 10);
            lblSearchLabel.ForeColor = AppPalette.TextPrimary;
            lblSearchLabel.AutoSize = true;
            lblSearchLabel.Location = new Point(30, 25);
            pnlControls.Controls.Add(lblSearchLabel);

            txtSearchID = new TextBox();
            txtSearchID.Location = new Point(220, 22);
            txtSearchID.Size = new Size(240, 25);
            txtSearchID.Font = new Font("Segoe UI", 10);
            txtSearchID.BackColor = AppPalette.CodeBlock;
            txtSearchID.ForeColor = AppPalette.TextPrimary;
            txtSearchID.BorderStyle = BorderStyle.FixedSingle;
            pnlControls.Controls.Add(txtSearchID);

            btnSearch = new Button();
            btnSearch.Text = "Search";
            btnSearch.Size = new Size(100, 32);
            btnSearch.Location = new Point(470, 20);
            btnSearch.BackColor = AppPalette.AccentPrimary;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.ForeColor = AppPalette.TextOnAccent;
            btnSearch.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += BtnSearch_Click;
            btnSearch.MouseEnter += (s, e) => btnSearch.BackColor = AppPalette.AccentHover;
            btnSearch.MouseLeave += (s, e) => btnSearch.BackColor = AppPalette.AccentPrimary;
            pnlControls.Controls.Add(btnSearch);

            lblStatusFilter = new Label();
            lblStatusFilter.Text = "Status Filter:";
            lblStatusFilter.Font = new Font("Segoe UI", 10);
            lblStatusFilter.ForeColor = AppPalette.TextPrimary;
            lblStatusFilter.AutoSize = true;
            lblStatusFilter.Location = new Point(30, 75);
            pnlControls.Controls.Add(lblStatusFilter);

            cmbStatusFilter = new ComboBox();
            cmbStatusFilter.Location = new Point(140, 72);
            cmbStatusFilter.Size = new Size(170, 25);
            cmbStatusFilter.Font = new Font("Segoe UI", 10);
            cmbStatusFilter.BackColor = AppPalette.CodeBlock;
            cmbStatusFilter.ForeColor = AppPalette.TextPrimary;
            cmbStatusFilter.FlatStyle = FlatStyle.Flat;
            cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatusFilter.Items.AddRange(new object[] { "All", "Pending", "InProgress", "Completed", "Rejected" });
            cmbStatusFilter.SelectedIndex = 0;
            cmbStatusFilter.SelectedIndexChanged += FilterChanged;
            pnlControls.Controls.Add(cmbStatusFilter);

            lblCategoryFilter = new Label();
            lblCategoryFilter.Text = "Category Filter:";
            lblCategoryFilter.Font = new Font("Segoe UI", 10);
            lblCategoryFilter.ForeColor = AppPalette.TextPrimary;
            lblCategoryFilter.AutoSize = true;
            lblCategoryFilter.Location = new Point(340, 75);
            pnlControls.Controls.Add(lblCategoryFilter);

            cmbCategoryFilter = new ComboBox();
            cmbCategoryFilter.Location = new Point(470, 72);
            cmbCategoryFilter.Size = new Size(170, 25);
            cmbCategoryFilter.Font = new Font("Segoe UI", 10);
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
            lblSortBy.Font = new Font("Segoe UI", 10);
            lblSortBy.ForeColor = AppPalette.TextPrimary;
            lblSortBy.AutoSize = true;
            lblSortBy.Location = new Point(670, 25);
            pnlControls.Controls.Add(lblSortBy);

            cmbSortBy = new ComboBox();
            cmbSortBy.Location = new Point(760, 22);
            cmbSortBy.Size = new Size(190, 25);
            cmbSortBy.Font = new Font("Segoe UI", 10);
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
            lblTreeView.Font = new Font("Segoe UI", 10);
            lblTreeView.ForeColor = AppPalette.TextPrimary;
            lblTreeView.AutoSize = true;
            lblTreeView.Location = new Point(670, 75);
            pnlControls.Controls.Add(lblTreeView);
            
            cmbTreeView = new ComboBox();
            cmbTreeView.Location = new Point(760, 72);
            cmbTreeView.Size = new Size(190, 25);
            cmbTreeView.Font = new Font("Segoe UI", 10);
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
            btnRefresh.Text = "Refresh All";
            btnRefresh.Size = new Size(130, 90);
            btnRefresh.Location = new Point(1020, 20);
            btnRefresh.BackColor = AppPalette.AccentPrimary;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.ForeColor = AppPalette.TextOnAccent;
            btnRefresh.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += BtnRefresh_Click;
            btnRefresh.MouseEnter += (s, e) => btnRefresh.BackColor = AppPalette.AccentHover;
            btnRefresh.MouseLeave += (s, e) => btnRefresh.BackColor = AppPalette.AccentPrimary;
            pnlControls.Controls.Add(btnRefresh);

            lblTotalRequests = new Label();
            lblTotalRequests.Text = "Total Requests: 0";
            lblTotalRequests.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTotalRequests.ForeColor = AppPalette.TextHeading;
            lblTotalRequests.AutoSize = true;
            lblTotalRequests.Location = new Point(330, 280);
            Controls.Add(lblTotalRequests);

            lblStatistics = new Label();
            lblStatistics.Text = string.Empty;
            lblStatistics.Font = new Font("Segoe UI", 10);
            lblStatistics.ForeColor = AppPalette.TextSecondary;
            lblStatistics.AutoSize = true;
            lblStatistics.Location = new Point(500, 282);
            Controls.Add(lblStatistics);

            dgvRequests = new DataGridView();
            dgvRequests.Location = new Point(330, 320);
            dgvRequests.Size = new Size(1210, 360);
            dgvRequests.BackgroundColor = AppPalette.CodeBlock;
            dgvRequests.GridColor = AppPalette.Border;
            dgvRequests.BorderStyle = BorderStyle.FixedSingle;
            dgvRequests.AllowUserToAddRows = false;
            dgvRequests.AllowUserToDeleteRows = false;
            dgvRequests.ReadOnly = true;
            dgvRequests.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRequests.MultiSelect = false;
            dgvRequests.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRequests.RowHeadersVisible = false;
            dgvRequests.Font = new Font("Segoe UI", 9);
            dgvRequests.DefaultCellStyle.BackColor = AppPalette.CodeBlock;
            dgvRequests.DefaultCellStyle.ForeColor = AppPalette.TextPrimary;
            dgvRequests.AlternatingRowsDefaultCellStyle.BackColor = AppPalette.Surface;
            dgvRequests.ColumnHeadersDefaultCellStyle.BackColor = AppPalette.AccentPrimary;
            dgvRequests.ColumnHeadersDefaultCellStyle.ForeColor = AppPalette.TextOnAccent;
            dgvRequests.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvRequests.EnableHeadersVisualStyles = false;
            dgvRequests.DefaultCellStyle.SelectionBackColor = AppPalette.AccentHover;
            dgvRequests.DefaultCellStyle.SelectionForeColor = AppPalette.TextOnAccent;
            dgvRequests.SelectionChanged += DgvRequests_SelectionChanged;
            Controls.Add(dgvRequests);

            pnlBottom = new Panel();
            pnlBottom.Location = new Point(330, 695);
            pnlBottom.Size = new Size(1210, 220);
            pnlBottom.BackColor = AppPalette.Surface;
            pnlBottom.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(pnlBottom);

            lblUpdateStatus = new Label();
            lblUpdateStatus.Text = "Update Status:";
            lblUpdateStatus.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblUpdateStatus.ForeColor = AppPalette.TextHeading;
            lblUpdateStatus.AutoSize = true;
            lblUpdateStatus.Location = new Point(30, 25);
            pnlBottom.Controls.Add(lblUpdateStatus);

            cmbUpdateStatus = new ComboBox();
            cmbUpdateStatus.Location = new Point(160, 22);
            cmbUpdateStatus.Size = new Size(160, 25);
            cmbUpdateStatus.Font = new Font("Segoe UI", 10);
            cmbUpdateStatus.BackColor = AppPalette.CodeBlock;
            cmbUpdateStatus.ForeColor = AppPalette.TextPrimary;
            cmbUpdateStatus.FlatStyle = FlatStyle.Flat;
            cmbUpdateStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUpdateStatus.Items.AddRange(new object[] { "Pending", "InProgress", "Completed", "Rejected" });
            cmbUpdateStatus.SelectedIndex = 0;
            pnlBottom.Controls.Add(cmbUpdateStatus);

            btnUpdateStatus = new Button();
            btnUpdateStatus.Text = "Apply Status";
            btnUpdateStatus.Size = new Size(120, 32);
            btnUpdateStatus.Location = new Point(330, 20);
            btnUpdateStatus.BackColor = AppPalette.AccentPrimary;
            btnUpdateStatus.FlatStyle = FlatStyle.Flat;
            btnUpdateStatus.FlatAppearance.BorderSize = 0;
            btnUpdateStatus.ForeColor = AppPalette.TextOnAccent;
            btnUpdateStatus.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnUpdateStatus.UseVisualStyleBackColor = false;
            btnUpdateStatus.Click += BtnUpdateStatus_Click;
            btnUpdateStatus.Enabled = false;
            btnUpdateStatus.MouseEnter += (s, e) => { if (btnUpdateStatus.Enabled) btnUpdateStatus.BackColor = AppPalette.AccentHover; };
            btnUpdateStatus.MouseLeave += (s, e) => { if (btnUpdateStatus.Enabled) btnUpdateStatus.BackColor = AppPalette.AccentPrimary; };
            pnlBottom.Controls.Add(btnUpdateStatus);

            lblPriorityQueue = new Label();
            lblPriorityQueue.Text = "Priority Queue (Heap Algorithm)";
            lblPriorityQueue.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblPriorityQueue.ForeColor = AppPalette.TextHeading;
            lblPriorityQueue.AutoSize = true;
            lblPriorityQueue.Location = new Point(30, 70);
            pnlBottom.Controls.Add(lblPriorityQueue);

            lvPriorityQueue = new ListView();
            lvPriorityQueue.Location = new Point(30, 100);
            lvPriorityQueue.Size = new Size(560, 100);
            lvPriorityQueue.View = View.Details;
            lvPriorityQueue.FullRowSelect = true;
            lvPriorityQueue.GridLines = true;
            lvPriorityQueue.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvPriorityQueue.Font = new Font("Segoe UI", 9);
            lvPriorityQueue.BackColor = AppPalette.CodeBlock;
            lvPriorityQueue.ForeColor = AppPalette.TextPrimary;
            lvPriorityQueue.BorderStyle = BorderStyle.FixedSingle;
            lvPriorityQueue.Columns.Add("Priority", 70);
            lvPriorityQueue.Columns.Add("Category", 120);
            lvPriorityQueue.Columns.Add("Location", 140);
            lvPriorityQueue.Columns.Add("Status", 90);
            lvPriorityQueue.Columns.Add("Submitted", 110);
            pnlBottom.Controls.Add(lvPriorityQueue);

            lblRelatedRequests = new Label();
            lblRelatedRequests.Text = "Related Requests (Graph Network)";
            lblRelatedRequests.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblRelatedRequests.ForeColor = AppPalette.TextHeading;
            lblRelatedRequests.AutoSize = true;
            lblRelatedRequests.Location = new Point(620, 25);
            pnlBottom.Controls.Add(lblRelatedRequests);

            lvRelatedRequests = new ListView();
            lvRelatedRequests.Location = new Point(620, 55);
            lvRelatedRequests.Size = new Size(560, 60);
            lvRelatedRequests.View = View.Details;
            lvRelatedRequests.FullRowSelect = true;
            lvRelatedRequests.GridLines = true;
            lvRelatedRequests.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvRelatedRequests.Font = new Font("Segoe UI", 9);
            lvRelatedRequests.BackColor = AppPalette.CodeBlock;
            lvRelatedRequests.ForeColor = AppPalette.TextPrimary;
            lvRelatedRequests.BorderStyle = BorderStyle.FixedSingle;
            lvRelatedRequests.Columns.Add("Request ID", 150);
            lvRelatedRequests.Columns.Add("Category", 140);
            lvRelatedRequests.Columns.Add("Status", 100);
            lvRelatedRequests.Columns.Add("Priority", 130);
            pnlBottom.Controls.Add(lvRelatedRequests);

            lblGraphTraversal = new Label();
            lblGraphTraversal.Text = "Graph Traversal:";
            lblGraphTraversal.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblGraphTraversal.ForeColor = AppPalette.TextHeading;
            lblGraphTraversal.AutoSize = true;
            lblGraphTraversal.Location = new Point(620, 125);
            pnlBottom.Controls.Add(lblGraphTraversal);

            cmbGraphTraversal = new ComboBox();
            cmbGraphTraversal.Location = new Point(760, 122);
            cmbGraphTraversal.Size = new Size(200, 25);
            cmbGraphTraversal.Font = new Font("Segoe UI", 10);
            cmbGraphTraversal.BackColor = AppPalette.CodeBlock;
            cmbGraphTraversal.ForeColor = AppPalette.TextPrimary;
            cmbGraphTraversal.FlatStyle = FlatStyle.Flat;
            cmbGraphTraversal.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGraphTraversal.Items.AddRange(new object[] { "BFS (Breadth-First)", "DFS (Depth-First)", "MST (Min Span Tree)" });
            cmbGraphTraversal.SelectedIndexChanged += GraphTraversalChanged;
            pnlBottom.Controls.Add(cmbGraphTraversal);

            lvGraphTraversal = new ListView();
            lvGraphTraversal.Location = new Point(620, 155);
            lvGraphTraversal.Size = new Size(560, 50);
            lvGraphTraversal.View = View.Details;
            lvGraphTraversal.FullRowSelect = true;
            lvGraphTraversal.GridLines = true;
            lvGraphTraversal.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvGraphTraversal.Font = new Font("Segoe UI", 9);
            lvGraphTraversal.BackColor = AppPalette.CodeBlock;
            lvGraphTraversal.ForeColor = AppPalette.TextPrimary;
            lvGraphTraversal.BorderStyle = BorderStyle.FixedSingle;
            lvGraphTraversal.Columns.Add("Step", 70);
            lvGraphTraversal.Columns.Add("Details", 310);
            lvGraphTraversal.Columns.Add("Connection Info", 150);
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
            lblDataStructureHints.Location = new Point(330, 925);
            lblDataStructureHints.Size = new Size(1210, 20);
            lblDataStructureHints.Font = new Font("Segoe UI", 8, FontStyle.Italic);
            lblDataStructureHints.ForeColor = AppPalette.TextMuted;
            lblDataStructureHints.Text = "Active Algorithms: BST (ID search) | AVL Tree (balanced sort) | Red-Black Tree (category sort) | " +
                "Basic Tree (hierarchy) | Binary Tree (insertion order) | Heap (priority) | Graph (relationships) | BFS/DFS (traversal) | MST (network)";
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
