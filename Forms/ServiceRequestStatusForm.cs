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
        private Panel pnlControls;
    private ListView lvPriorityQueue;
        
        private List<ServiceRequest> allServiceRequests;
    private ServiceRequestBST requestTree;
    private ServiceRequestAVL requestBalancedTree;
    private ServiceRequestHeap requestHeap;

        public ServiceRequestStatusForm()
        {
            InitializeComponent();
            SetupForm();
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
            dgvRequests.Size = new Size(940, 320);
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
            
            this.Controls.Add(dgvRequests);

            lblPriorityQueue = new Label();
            lblPriorityQueue.Text = "Priority Queue";
            lblPriorityQueue.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblPriorityQueue.ForeColor = AppPalette.TextPrimary;
            lblPriorityQueue.AutoSize = true;
            lblPriorityQueue.Location = new Point(30, 530);
            this.Controls.Add(lblPriorityQueue);

            lvPriorityQueue = new ListView();
            lvPriorityQueue.Location = new Point(30, 555);
            lvPriorityQueue.Size = new Size(940, 120);
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
        }

        private void LoadServiceRequests()
        {
            try
            {
                allServiceRequests.Clear();
                requestTree.Clear();
                requestBalancedTree.Clear();
                requestHeap.Clear();
                
                DatabaseHelper dbHelper = new DatabaseHelper();
                List<Issue> issues = dbHelper.GetAllIssues();
                
                foreach (Issue issue in issues)
                {
                    ServiceRequest request = new ServiceRequest(issue);
                    AssignPriorityBasedOnCategory(request);
                    allServiceRequests.Add(request);
                    requestTree.Insert(request);
                    requestBalancedTree.Insert(request);
                    requestHeap.Insert(request);
                }
                
                DisplayServiceRequests(requestBalancedTree.InOrder().ToList());
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
                default:
                    request.Priority = ServiceRequestPriority.Low;
                    break;
            }
        }

        private void DisplayServiceRequests(List<ServiceRequest> requests)
        {
            dgvRequests.DataSource = null;
            
            var displayData = requests.Select(r => new
            {
                UniqueID = r.UniqueID.Substring(0, 8),
                Location = r.Location,
                Category = r.Category,
                Status = r.GetStatusString(),
                Priority = r.GetPriorityString(),
                DateSubmitted = r.DateSubmitted.ToString("yyyy-MM-dd HH:mm"),
                Description = r.Description.Length > 50 ? r.Description.Substring(0, 50) + "..." : r.Description
            }).ToList();
            
            dgvRequests.DataSource = displayData;
            
            lblTotalRequests.Text = "Total Requests: " + requests.Count;
            
            foreach (DataGridViewRow row in dgvRequests.Rows)
            {
                string status = row.Cells["Status"].Value.ToString();
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
    }
}
