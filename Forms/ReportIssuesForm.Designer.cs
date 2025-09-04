namespace MunicipalApp.Forms
{
    partial class ReportIssuesForm
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
            this.lblLocation = new Label();
            this.txtLocation = new TextBox();
            this.lblCategory = new Label();
            this.cmbCategory = new ComboBox();
            this.lblDescription = new Label();
            this.rtbDescription = new RichTextBox();
            this.btnAttachFiles = new Button();
            this.lstAttachedFiles = new ListBox();
            this.btnRemoveFile = new Button();
            this.progressBar = new ProgressBar();
            this.lblProgress = new Label();
            this.lblEncouragement = new Label();
            this.btnSubmit = new Button();
            this.btnBack = new Button();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblTitle.ForeColor = Color.FromArgb(25, 118, 210);
            this.lblTitle.Location = new Point(250, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(200, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Report an Issue";
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            
            // Progress tracking section
            this.progressBar.Location = new Point(50, 70);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new Size(600, 15);
            this.progressBar.TabIndex = 1;
            this.progressBar.Style = ProgressBarStyle.Continuous;
            
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblProgress.Location = new Point(50, 95);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new Size(120, 15);
            this.lblProgress.TabIndex = 2;
            this.lblProgress.Text = "0 of 4 fields completed";
            
            this.lblEncouragement.AutoSize = true;
            this.lblEncouragement.Font = new Font("Segoe UI", 10F, FontStyle.Italic, GraphicsUnit.Point);
            this.lblEncouragement.ForeColor = Color.FromArgb(76, 175, 80);
            this.lblEncouragement.Location = new Point(50, 115);
            this.lblEncouragement.Name = "lblEncouragement";
            this.lblEncouragement.Size = new Size(250, 19);
            this.lblEncouragement.TabIndex = 3;
            this.lblEncouragement.Text = "Let's get started!";
            
            // Location field
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblLocation.Location = new Point(50, 160);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new Size(62, 19);
            this.lblLocation.TabIndex = 4;
            this.lblLocation.Text = "Location:";
            
            this.txtLocation.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtLocation.Location = new Point(150, 157);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new Size(500, 25);
            this.txtLocation.TabIndex = 5;
            this.txtLocation.TextChanged += new EventHandler(this.txtLocation_TextChanged);
            
            // Category field
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblCategory.Location = new Point(50, 200);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new Size(69, 19);
            this.lblCategory.TabIndex = 6;
            this.lblCategory.Text = "Category:";
            
            this.cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCategory.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.cmbCategory.Location = new Point(150, 197);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new Size(200, 25);
            this.cmbCategory.TabIndex = 7;
            this.cmbCategory.SelectedIndexChanged += new EventHandler(this.cmbCategory_SelectedIndexChanged);
            
            // Description field
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblDescription.Location = new Point(50, 240);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new Size(81, 19);
            this.lblDescription.TabIndex = 8;
            this.lblDescription.Text = "Description:";
            
            this.rtbDescription.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.rtbDescription.Location = new Point(150, 240);
            this.rtbDescription.Name = "rtbDescription";
            this.rtbDescription.Size = new Size(500, 120);
            this.rtbDescription.TabIndex = 9;
            this.rtbDescription.TextChanged += new EventHandler(this.rtbDescription_TextChanged);
            
            // File attachment section
            this.btnAttachFiles.BackColor = Color.FromArgb(33, 150, 243);
            this.btnAttachFiles.FlatAppearance.BorderSize = 0;
            this.btnAttachFiles.FlatStyle = FlatStyle.Flat;
            this.btnAttachFiles.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnAttachFiles.ForeColor = Color.White;
            this.btnAttachFiles.Location = new Point(50, 380);
            this.btnAttachFiles.Name = "btnAttachFiles";
            this.btnAttachFiles.Size = new Size(120, 35);
            this.btnAttachFiles.TabIndex = 10;
            this.btnAttachFiles.Text = "Attach Files";
            this.btnAttachFiles.UseVisualStyleBackColor = false;
            this.btnAttachFiles.Click += new EventHandler(this.btnAttachFiles_Click);
            
            this.lstAttachedFiles.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.lstAttachedFiles.Location = new Point(190, 380);
            this.lstAttachedFiles.Name = "lstAttachedFiles";
            this.lstAttachedFiles.Size = new Size(350, 50);
            this.lstAttachedFiles.TabIndex = 11;
            
            this.btnRemoveFile.BackColor = Color.FromArgb(244, 67, 54);
            this.btnRemoveFile.FlatAppearance.BorderSize = 0;
            this.btnRemoveFile.FlatStyle = FlatStyle.Flat;
            this.btnRemoveFile.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnRemoveFile.ForeColor = Color.White;
            this.btnRemoveFile.Location = new Point(560, 380);
            this.btnRemoveFile.Name = "btnRemoveFile";
            this.btnRemoveFile.Size = new Size(90, 35);
            this.btnRemoveFile.TabIndex = 12;
            this.btnRemoveFile.Text = "Remove";
            this.btnRemoveFile.UseVisualStyleBackColor = false;
            this.btnRemoveFile.Click += new EventHandler(this.btnRemoveFile_Click);
            
            // Action buttons
            this.btnSubmit.BackColor = Color.FromArgb(158, 158, 158);
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.FlatStyle = FlatStyle.Flat;
            this.btnSubmit.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnSubmit.ForeColor = Color.White;
            this.btnSubmit.Location = new Point(450, 480);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new Size(120, 40);
            this.btnSubmit.TabIndex = 13;
            this.btnSubmit.Text = "Submit Issue";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
            
            this.btnBack.BackColor = Color.FromArgb(96, 125, 139);
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = FlatStyle.Flat;
            this.btnBack.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.btnBack.ForeColor = Color.White;
            this.btnBack.Location = new Point(50, 480);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new Size(100, 40);
            this.btnBack.TabIndex = 14;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new EventHandler(this.btnBack_Click);
            
            // Form
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(700, 550);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.btnRemoveFile);
            this.Controls.Add(this.lstAttachedFiles);
            this.Controls.Add(this.btnAttachFiles);
            this.Controls.Add(this.rtbDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.lblEncouragement);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblTitle);
            this.Name = "ReportIssuesForm";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblLocation;
        private TextBox txtLocation;
        private Label lblCategory;
        private ComboBox cmbCategory;
        private Label lblDescription;
        private RichTextBox rtbDescription;
        private Button btnAttachFiles;
        private ListBox lstAttachedFiles;
        private Button btnRemoveFile;
        private ProgressBar progressBar;
        private Label lblProgress;
        private Label lblEncouragement;
        private Button btnSubmit;
        private Button btnBack;
    }
}