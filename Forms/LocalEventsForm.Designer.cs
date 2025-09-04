namespace MunicipalApp.Forms
{
    partial class LocalEventsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.grpSearch = new GroupBox();
            this.lblCategory = new Label();
            this.cmbCategory = new ComboBox();
            this.lblFromDate = new Label();
            this.dtpFromDate = new DateTimePicker();
            this.lblToDate = new Label();
            this.dtpToDate = new DateTimePicker();
            this.lblKeyword = new Label();
            this.txtSearchKeyword = new TextBox();
            this.btnSearch = new Button();
            this.btnClearSearch = new Button();
            this.grpEvents = new GroupBox();
            this.lstEvents = new ListView();
            this.colTitle = new ColumnHeader();
            this.colCategory = new ColumnHeader();
            this.colDate = new ColumnHeader();
            this.colLocation = new ColumnHeader();
            this.colOrganizer = new ColumnHeader();
            this.lblEventCount = new Label();
            this.btnPriorityEvents = new Button();
            this.btnUpcomingEvents = new Button();
            this.grpRecommendations = new GroupBox();
            this.lstRecommendations = new ListView();
            this.colRecTitle = new ColumnHeader();
            this.colRecDate = new ColumnHeader();
            this.colRecCategory = new ColumnHeader();
            this.grpDetails = new GroupBox();
            this.rtbEventDetails = new RichTextBox();
            this.grpActivity = new GroupBox();
            this.lblRecentSearches = new Label();
            this.lstRecentSearches = new ListBox();
            this.lblRecentViewed = new Label();
            this.lstRecentViewed = new ListBox();
            this.btnBack = new Button();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblTitle.ForeColor = Color.FromArgb(25, 118, 210);
            this.lblTitle.Location = new Point(350, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(300, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Local Events & Announcements";
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            
            // Search Group
            this.grpSearch.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            this.grpSearch.ForeColor = Color.FromArgb(25, 118, 210);
            this.grpSearch.Location = new Point(20, 60);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Size = new Size(960, 80);
            this.grpSearch.TabIndex = 1;
            this.grpSearch.TabStop = false;
            this.grpSearch.Text = "Search & Filter";
            
            // Category
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblCategory.ForeColor = Color.Black;
            this.lblCategory.Location = new Point(15, 25);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new Size(58, 15);
            this.lblCategory.TabIndex = 0;
            this.lblCategory.Text = "Category:";
            
            this.cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCategory.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.cmbCategory.Location = new Point(15, 45);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new Size(120, 23);
            this.cmbCategory.TabIndex = 1;
            
            // From Date
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblFromDate.ForeColor = Color.Black;
            this.lblFromDate.Location = new Point(150, 25);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new Size(66, 15);
            this.lblFromDate.TabIndex = 2;
            this.lblFromDate.Text = "From Date:";
            
            this.dtpFromDate.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.dtpFromDate.Format = DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new Point(150, 45);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.ShowCheckBox = true;
            this.dtpFromDate.Size = new Size(120, 23);
            this.dtpFromDate.TabIndex = 3;
            
            // To Date
            this.lblToDate.AutoSize = true;
            this.lblToDate.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblToDate.ForeColor = Color.Black;
            this.lblToDate.Location = new Point(285, 25);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new Size(51, 15);
            this.lblToDate.TabIndex = 4;
            this.lblToDate.Text = "To Date:";
            
            this.dtpToDate.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.dtpToDate.Format = DateTimePickerFormat.Short;
            this.dtpToDate.Location = new Point(285, 45);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.ShowCheckBox = true;
            this.dtpToDate.Size = new Size(120, 23);
            this.dtpToDate.TabIndex = 5;
            
            // Keyword
            this.lblKeyword.AutoSize = true;
            this.lblKeyword.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblKeyword.ForeColor = Color.Black;
            this.lblKeyword.Location = new Point(420, 25);
            this.lblKeyword.Name = "lblKeyword";
            this.lblKeyword.Size = new Size(54, 15);
            this.lblKeyword.TabIndex = 6;
            this.lblKeyword.Text = "Keyword:";
            
            this.txtSearchKeyword.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtSearchKeyword.Location = new Point(420, 45);
            this.txtSearchKeyword.Name = "txtSearchKeyword";
            this.txtSearchKeyword.Size = new Size(200, 23);
            this.txtSearchKeyword.TabIndex = 7;
            
            // Search buttons
            this.btnSearch.BackColor = Color.FromArgb(76, 175, 80);
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = FlatStyle.Flat;
            this.btnSearch.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnSearch.ForeColor = Color.White;
            this.btnSearch.Location = new Point(640, 45);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new Size(80, 23);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            
            this.btnClearSearch.BackColor = Color.FromArgb(96, 125, 139);
            this.btnClearSearch.FlatAppearance.BorderSize = 0;
            this.btnClearSearch.FlatStyle = FlatStyle.Flat;
            this.btnClearSearch.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnClearSearch.ForeColor = Color.White;
            this.btnClearSearch.Location = new Point(730, 45);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new Size(80, 23);
            this.btnClearSearch.TabIndex = 9;
            this.btnClearSearch.Text = "Clear";
            this.btnClearSearch.UseVisualStyleBackColor = false;
            this.btnClearSearch.Click += new EventHandler(this.btnClearSearch_Click);
            
            // Quick filter buttons
            this.btnPriorityEvents.BackColor = Color.FromArgb(255, 152, 0);
            this.btnPriorityEvents.FlatAppearance.BorderSize = 0;
            this.btnPriorityEvents.FlatStyle = FlatStyle.Flat;
            this.btnPriorityEvents.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnPriorityEvents.ForeColor = Color.White;
            this.btnPriorityEvents.Location = new Point(820, 25);
            this.btnPriorityEvents.Name = "btnPriorityEvents";
            this.btnPriorityEvents.Size = new Size(80, 23);
            this.btnPriorityEvents.TabIndex = 10;
            this.btnPriorityEvents.Text = "Priority";
            this.btnPriorityEvents.UseVisualStyleBackColor = false;
            this.btnPriorityEvents.Click += new EventHandler(this.btnPriorityEvents_Click);
            
            this.btnUpcomingEvents.BackColor = Color.FromArgb(33, 150, 243);
            this.btnUpcomingEvents.FlatAppearance.BorderSize = 0;
            this.btnUpcomingEvents.FlatStyle = FlatStyle.Flat;
            this.btnUpcomingEvents.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnUpcomingEvents.ForeColor = Color.White;
            this.btnUpcomingEvents.Location = new Point(820, 52);
            this.btnUpcomingEvents.Name = "btnUpcomingEvents";
            this.btnUpcomingEvents.Size = new Size(80, 23);
            this.btnUpcomingEvents.TabIndex = 11;
            this.btnUpcomingEvents.Text = "Upcoming";
            this.btnUpcomingEvents.UseVisualStyleBackColor = false;
            this.btnUpcomingEvents.Click += new EventHandler(this.btnUpcomingEvents_Click);
            
            // Add controls to search group
            this.grpSearch.Controls.Add(this.lblCategory);
            this.grpSearch.Controls.Add(this.cmbCategory);
            this.grpSearch.Controls.Add(this.lblFromDate);
            this.grpSearch.Controls.Add(this.dtpFromDate);
            this.grpSearch.Controls.Add(this.lblToDate);
            this.grpSearch.Controls.Add(this.dtpToDate);
            this.grpSearch.Controls.Add(this.lblKeyword);
            this.grpSearch.Controls.Add(this.txtSearchKeyword);
            this.grpSearch.Controls.Add(this.btnSearch);
            this.grpSearch.Controls.Add(this.btnClearSearch);
            this.grpSearch.Controls.Add(this.btnPriorityEvents);
            this.grpSearch.Controls.Add(this.btnUpcomingEvents);
            
            // Events Group
            this.grpEvents.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            this.grpEvents.ForeColor = Color.FromArgb(25, 118, 210);
            this.grpEvents.Location = new Point(20, 150);
            this.grpEvents.Name = "grpEvents";
            this.grpEvents.Size = new Size(630, 320);
            this.grpEvents.TabIndex = 2;
            this.grpEvents.TabStop = false;
            this.grpEvents.Text = "Events List";
            
            // Events ListView
            this.lstEvents.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.lstEvents.FullRowSelect = true;
            this.lstEvents.GridLines = true;
            this.lstEvents.Location = new Point(10, 25);
            this.lstEvents.MultiSelect = false;
            this.lstEvents.Name = "lstEvents";
            this.lstEvents.Size = new Size(610, 260);
            this.lstEvents.TabIndex = 0;
            this.lstEvents.UseCompatibleStateImageBehavior = false;
            this.lstEvents.View = View.Details;
            this.lstEvents.SelectedIndexChanged += new EventHandler(this.lstEvents_SelectedIndexChanged);
            
            // ListView columns
            this.colTitle.Text = "Title";
            this.colTitle.Width = 200;
            this.colCategory.Text = "Category";
            this.colCategory.Width = 100;
            this.colDate.Text = "Date";
            this.colDate.Width = 120;
            this.colLocation.Text = "Location";
            this.colLocation.Width = 100;
            this.colOrganizer.Text = "Organizer";
            this.colOrganizer.Width = 85;
            
            this.lstEvents.Columns.Add(this.colTitle);
            this.lstEvents.Columns.Add(this.colCategory);
            this.lstEvents.Columns.Add(this.colDate);
            this.lstEvents.Columns.Add(this.colLocation);
            this.lstEvents.Columns.Add(this.colOrganizer);
            
            this.lblEventCount.AutoSize = true;
            this.lblEventCount.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblEventCount.ForeColor = Color.Black;
            this.lblEventCount.Location = new Point(10, 295);
            this.lblEventCount.Name = "lblEventCount";
            this.lblEventCount.Size = new Size(100, 15);
            this.lblEventCount.TabIndex = 1;
            this.lblEventCount.Text = "Showing 0 events";
            
            this.grpEvents.Controls.Add(this.lstEvents);
            this.grpEvents.Controls.Add(this.lblEventCount);
            
            // Recommendations Group
            this.grpRecommendations.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            this.grpRecommendations.ForeColor = Color.FromArgb(25, 118, 210);
            this.grpRecommendations.Location = new Point(670, 150);
            this.grpRecommendations.Name = "grpRecommendations";
            this.grpRecommendations.Size = new Size(310, 160);
            this.grpRecommendations.TabIndex = 3;
            this.grpRecommendations.TabStop = false;
            this.grpRecommendations.Text = "Recommended for You";
            
            this.lstRecommendations.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.lstRecommendations.FullRowSelect = true;
            this.lstRecommendations.GridLines = true;
            this.lstRecommendations.Location = new Point(10, 25);
            this.lstRecommendations.MultiSelect = false;
            this.lstRecommendations.Name = "lstRecommendations";
            this.lstRecommendations.Size = new Size(290, 125);
            this.lstRecommendations.TabIndex = 0;
            this.lstRecommendations.UseCompatibleStateImageBehavior = false;
            this.lstRecommendations.View = View.Details;
            this.lstRecommendations.SelectedIndexChanged += new EventHandler(this.lstRecommendations_SelectedIndexChanged);
            
            this.colRecTitle.Text = "Title";
            this.colRecTitle.Width = 150;
            this.colRecDate.Text = "Date";
            this.colRecDate.Width = 70;
            this.colRecCategory.Text = "Category";
            this.colRecCategory.Width = 70;
            
            this.lstRecommendations.Columns.Add(this.colRecTitle);
            this.lstRecommendations.Columns.Add(this.colRecDate);
            this.lstRecommendations.Columns.Add(this.colRecCategory);
            
            this.grpRecommendations.Controls.Add(this.lstRecommendations);
            
            // Recent Activity Group
            this.grpActivity.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            this.grpActivity.ForeColor = Color.FromArgb(25, 118, 210);
            this.grpActivity.Location = new Point(670, 320);
            this.grpActivity.Name = "grpActivity";
            this.grpActivity.Size = new Size(310, 150);
            this.grpActivity.TabIndex = 4;
            this.grpActivity.TabStop = false;
            this.grpActivity.Text = "Recent Activity";
            
            this.lblRecentSearches.AutoSize = true;
            this.lblRecentSearches.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblRecentSearches.ForeColor = Color.Black;
            this.lblRecentSearches.Location = new Point(10, 25);
            this.lblRecentSearches.Name = "lblRecentSearches";
            this.lblRecentSearches.Size = new Size(95, 15);
            this.lblRecentSearches.TabIndex = 0;
            this.lblRecentSearches.Text = "Recent Searches:";
            
            this.lstRecentSearches.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            this.lstRecentSearches.Location = new Point(10, 45);
            this.lstRecentSearches.Name = "lstRecentSearches";
            this.lstRecentSearches.Size = new Size(140, 45);
            this.lstRecentSearches.TabIndex = 1;
            
            this.lblRecentViewed.AutoSize = true;
            this.lblRecentViewed.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblRecentViewed.ForeColor = Color.Black;
            this.lblRecentViewed.Location = new Point(160, 25);
            this.lblRecentViewed.Name = "lblRecentViewed";
            this.lblRecentViewed.Size = new Size(91, 15);
            this.lblRecentViewed.TabIndex = 2;
            this.lblRecentViewed.Text = "Recently Viewed:";
            
            this.lstRecentViewed.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            this.lstRecentViewed.Location = new Point(160, 45);
            this.lstRecentViewed.Name = "lstRecentViewed";
            this.lstRecentViewed.Size = new Size(140, 45);
            this.lstRecentViewed.TabIndex = 3;
            
            this.grpActivity.Controls.Add(this.lblRecentSearches);
            this.grpActivity.Controls.Add(this.lstRecentSearches);
            this.grpActivity.Controls.Add(this.lblRecentViewed);
            this.grpActivity.Controls.Add(this.lstRecentViewed);
            
            // Details Group
            this.grpDetails.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            this.grpDetails.ForeColor = Color.FromArgb(25, 118, 210);
            this.grpDetails.Location = new Point(20, 480);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Size = new Size(630, 160);
            this.grpDetails.TabIndex = 5;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "Event Details";
            
            this.rtbEventDetails.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.rtbEventDetails.Location = new Point(10, 25);
            this.rtbEventDetails.Name = "rtbEventDetails";
            this.rtbEventDetails.ReadOnly = true;
            this.rtbEventDetails.Size = new Size(610, 125);
            this.rtbEventDetails.TabIndex = 0;
            this.rtbEventDetails.Text = "Select an event to view details";
            
            this.grpDetails.Controls.Add(this.rtbEventDetails);
            
            // Back Button
            this.btnBack.BackColor = Color.FromArgb(96, 125, 139);
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = FlatStyle.Flat;
            this.btnBack.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnBack.ForeColor = Color.White;
            this.btnBack.Location = new Point(670, 600);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new Size(100, 40);
            this.btnBack.TabIndex = 6;
            this.btnBack.Text = "Back to Menu";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new EventHandler(this.btnBack_Click);
            
            // Form
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1000, 660);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.grpDetails);
            this.Controls.Add(this.grpActivity);
            this.Controls.Add(this.grpRecommendations);
            this.Controls.Add(this.grpEvents);
            this.Controls.Add(this.grpSearch);
            this.Controls.Add(this.lblTitle);
            this.Name = "LocalEventsForm";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private GroupBox grpSearch;
        private Label lblCategory;
        private ComboBox cmbCategory;
        private Label lblFromDate;
        private DateTimePicker dtpFromDate;
        private Label lblToDate;
        private DateTimePicker dtpToDate;
        private Label lblKeyword;
        private TextBox txtSearchKeyword;
        private Button btnSearch;
        private Button btnClearSearch;
        private Button btnPriorityEvents;
        private Button btnUpcomingEvents;
        private GroupBox grpEvents;
        private ListView lstEvents;
        private ColumnHeader colTitle;
        private ColumnHeader colCategory;
        private ColumnHeader colDate;
        private ColumnHeader colLocation;
        private ColumnHeader colOrganizer;
        private Label lblEventCount;
        private GroupBox grpRecommendations;
        private ListView lstRecommendations;
        private ColumnHeader colRecTitle;
        private ColumnHeader colRecDate;
        private ColumnHeader colRecCategory;
        private GroupBox grpActivity;
        private Label lblRecentSearches;
        private ListBox lstRecentSearches;
        private Label lblRecentViewed;
        private ListBox lstRecentViewed;
        private GroupBox grpDetails;
        private RichTextBox rtbEventDetails;
        private Button btnBack;
    }
}