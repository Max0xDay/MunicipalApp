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
            this.Text = "Municipal Services Hub";
            this.Size = new Size(800, 600);
            this.MinimumSize = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            
            SetupButtonHoverEffects();
        }

        private void SetupButtonHoverEffects()
        {
            btnReportIssues.MouseEnter += (s, e) => btnReportIssues.BackColor = Color.FromArgb(67, 160, 71);
            btnReportIssues.MouseLeave += (s, e) => btnReportIssues.BackColor = Color.FromArgb(76, 175, 80);
            
            btnLocalEvents.MouseEnter += (s, e) => btnLocalEvents.BackColor = Color.FromArgb(30, 136, 229);
            btnLocalEvents.MouseLeave += (s, e) => btnLocalEvents.BackColor = Color.FromArgb(33, 150, 243);
            
            btnServiceStatus.MouseEnter += (s, e) => btnServiceStatus.BackColor = Color.FromArgb(117, 117, 117);
            btnServiceStatus.MouseLeave += (s, e) => btnServiceStatus.BackColor = Color.FromArgb(158, 158, 158);
        }

        private void btnReportIssues_Click(object sender, EventArgs e)
        {
            var reportForm = new ReportIssuesForm();
            reportForm.ShowDialog();
        }

        private void btnLocalEvents_Click(object sender, EventArgs e)
        {
            var eventsForm = new LocalEventsForm();
            eventsForm.ShowDialog();
        }

        private void btnServiceStatus_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature will be implemented in Part 3.", "Coming Soon", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}