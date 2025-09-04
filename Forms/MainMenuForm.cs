namespace MunicipalApp.Forms
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = "Municipal Services Application";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 248, 255);
        }

        private void btnReportIssues_Click(object sender, EventArgs e)
        {
            var reportForm = new ReportIssuesForm();
            reportForm.ShowDialog();
        }

        private void btnLocalEvents_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature will be implemented in Part 2.", "Coming Soon", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnServiceStatus_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature will be implemented in Part 3.", "Coming Soon", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}