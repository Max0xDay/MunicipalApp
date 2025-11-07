using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sidequest_municiple_app
{
    public class LocalEventsForm : Form
    {
        private ListView lvEvents;
        private ComboBox cmbCategory;
        private DateTimePicker dtpDate;
        private CheckBox chkFilterByDate;
        private Button btnSearch;
        private Button btnClear;
        private Label lblTitle;
        private Label lblFilters;
        private Label lblCategory;
        private Label lblDate;
        private TextBox txtSearch;
        private Label lblSearch;

        private Dictionary<string, List<LocalEvent>> eventsByCategory;
        private SortedDictionary<DateTime, List<LocalEvent>> eventsByDate;
        private HashSet<string> eventCategories;
        private Queue<LocalEvent> upcomingEventsQueue;
        private Dictionary<string, int> searchHistory;
        private SortedDictionary<string, int> recommendationScores;
        private List<LocalEvent> allEvents;

        public LocalEventsForm()
        {
            InitializeComponent();
            SetupForm();
            InitializeData();
            PopulateFilters();
            DisplayEvents(allEvents.OrderBy(evt => evt.EventDate).ThenBy(evt => evt.Priority).ToList());
        }
        private void SetupForm()
        {
            Text = "Local Events and Announcements";
            Size = new Size(900, 620);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = AppPalette.Background;

            lblTitle = new Label
            {
                Text = "Local Events and Announcements",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = AppPalette.TextPrimary,
                AutoSize = true,
                Location = new Point(20, 20)
            };
            Controls.Add(lblTitle);

            lblFilters = new Label
            {
                Text = "Find events by category and date",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = AppPalette.TextPrimary,
                AutoSize = true,
                Location = new Point(20, 60)
            };
            Controls.Add(lblFilters);

            lblCategory = new Label
            {
                Text = "Category:",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = AppPalette.TextSecondary,
                AutoSize = true,
                Location = new Point(20, 100)
            };
            Controls.Add(lblCategory);

            cmbCategory = new ComboBox
            {
                Location = new Point(20, 122),
                Size = new Size(240, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9),
                BackColor = AppPalette.Input,
                ForeColor = AppPalette.TextPrimary,
                FlatStyle = FlatStyle.Flat
            };
            Controls.Add(cmbCategory);

            lblDate = new Label
            {
                Text = "Date:",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = AppPalette.TextSecondary,
                AutoSize = true,
                Location = new Point(320, 100)
            };
            Controls.Add(lblDate);

            dtpDate = new DateTimePicker
            {
                Location = new Point(320, 122),
                Size = new Size(200, 25),
                Format = DateTimePickerFormat.Short,
                Enabled = false,
                CalendarForeColor = AppPalette.TextPrimary,
                CalendarMonthBackground = AppPalette.Input
            };
            Controls.Add(dtpDate);

            chkFilterByDate = new CheckBox
            {
                Text = "Filter by selected date",
                AutoSize = true,
                Location = new Point(320, 160),
                Font = new Font("Segoe UI", 9),
                ForeColor = AppPalette.TextSecondary
            };
            chkFilterByDate.CheckedChanged += ChkFilterByDate_CheckedChanged;
            Controls.Add(chkFilterByDate);

            lblSearch = new Label
            {
                Text = "Search:",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = AppPalette.TextSecondary,
                AutoSize = true,
                Location = new Point(560, 100)
            };
            Controls.Add(lblSearch);

            txtSearch = new TextBox
            {
                Location = new Point(560, 122),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 9),
                BackColor = AppPalette.Input,
                ForeColor = AppPalette.TextPrimary,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            Controls.Add(txtSearch);

            btnSearch = new Button
            {
                Text = "Search",
                Location = new Point(20, 200),
                Size = new Size(110, 32),
                BackColor = AppPalette.AccentPrimary,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = AppPalette.TextOnAccent
            };
            btnSearch.FlatAppearance.BorderColor = AppPalette.Border;
            btnSearch.FlatAppearance.BorderSize = 1;
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += BtnSearch_Click;
            Controls.Add(btnSearch);

            btnClear = new Button
            {
                Text = "Clear",
                Location = new Point(140, 200),
                Size = new Size(110, 32),
                BackColor = AppPalette.SurfaceAlt,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9),
                ForeColor = AppPalette.TextPrimary
            };
            btnClear.FlatAppearance.BorderColor = AppPalette.Border;
            btnClear.FlatAppearance.BorderSize = 1;
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += BtnClear_Click;
            Controls.Add(btnClear);

            lvEvents = new ListView
            {
                Location = new Point(20, 250),
                Size = new Size(840, 320),
                View = View.Details,
                FullRowSelect = true,
                GridLines = false,
                HideSelection = false,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = AppPalette.Surface,
                ForeColor = AppPalette.TextPrimary
            };
            lvEvents.Columns.Add("Date", 110);
            lvEvents.Columns.Add("Title", 220);
            lvEvents.Columns.Add("Category", 140);
            lvEvents.Columns.Add("Description", 350);
            Controls.Add(lvEvents);
        }

        private void InitializeData()
        {
            eventsByCategory = new Dictionary<string, List<LocalEvent>>(StringComparer.OrdinalIgnoreCase);
            eventsByDate = new SortedDictionary<DateTime, List<LocalEvent>>();
            eventCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            upcomingEventsQueue = new Queue<LocalEvent>();
            searchHistory = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            recommendationScores = new SortedDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            allEvents = SeedEvents();

            foreach (LocalEvent localEvent in allEvents)
            {
                DateTime eventKey = localEvent.EventDate.Date;
                
                if (!eventsByCategory.TryGetValue(localEvent.Category, out List<LocalEvent> categoryList))
                {
                    categoryList = new List<LocalEvent>();
                    eventsByCategory[localEvent.Category] = categoryList;
                }
                categoryList.Add(localEvent);

                if (!eventsByDate.TryGetValue(eventKey, out List<LocalEvent> dateList))
                {
                    dateList = new List<LocalEvent>();
                    eventsByDate[eventKey] = dateList;
                }
                dateList.Add(localEvent);

                eventCategories.Add(localEvent.Category);
            }

            foreach (LocalEvent evt in allEvents.Where(e => e.EventDate >= DateTime.Today).OrderBy(e => e.EventDate).ThenBy(e => e.Priority))
            {
                upcomingEventsQueue.Enqueue(evt);
            }
        }

        private void PopulateFilters()
        {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("All");
            foreach (string category in eventCategories.OrderBy(c => c))
            {
                cmbCategory.Items.Add(category);
            }
            if (cmbCategory.Items.Count > 0)
            {
                cmbCategory.SelectedIndex = 0;
            }
            dtpDate.Value = DateTime.Today;
            chkFilterByDate.Checked = false;
        }

        private List<LocalEvent> SeedEvents()
        {
            DateTime today = DateTime.Today;
            return new List<LocalEvent>
            {
                new LocalEvent("Waste Collection Outreach", "Sanitation", today.AddDays(2), "Community clean-up campaign in Zone 5.", 1),
                new LocalEvent("Water Maintenance Briefing", "Utilities", today.AddDays(4), "Update on scheduled water pipe upgrades.", 2),
                new LocalEvent("Youth Sports Festival", "Community", today.AddDays(6), "Full-day sports and recreation for local schools.", 3),
                new LocalEvent("Road Safety Workshop", "Roads", today.AddDays(3), "Interactive session on pedestrian safety initiatives.", 2),
                new LocalEvent("Electricity Network Upgrade", "Electricity", today.AddDays(8), "Briefing on power grid enhancements in Ward 11.", 1),
                new LocalEvent("Farmers Market", "Community", today.AddDays(1), "Fresh produce market at the civic park.", 2),
                new LocalEvent("Water Conservation Training", "Water", today.AddDays(10), "Household tips to reduce water consumption.", 2),
                new LocalEvent("Sanitation Feedback Forum", "Sanitation", today.AddDays(12), "Resident engagement session on sanitation services.", 3),
                new LocalEvent("Energy Saving Expo", "Electricity", today.AddDays(5), "Vendors showcase affordable energy-efficient products.", 2),
                new LocalEvent("Public Transport Dialogue", "Roads", today.AddDays(9), "Discuss bus route improvements with officials.", 3)
            };
        }

        private void DisplayEvents(List<LocalEvent> events)
        {
            lvEvents.Items.Clear();
            if (events.Count == 0)
            {
                ListViewItem emptyItem = new ListViewItem("No events match your filters.");
                emptyItem.SubItems.Add(string.Empty);
                emptyItem.SubItems.Add(string.Empty);
                emptyItem.SubItems.Add(string.Empty);
                lvEvents.Items.Add(emptyItem);
                return;
            }

            foreach (LocalEvent localEvent in events.OrderBy(evt => evt.EventDate).ThenBy(evt => evt.Priority))
            {
                ListViewItem item = new ListViewItem(localEvent.EventDate.ToString("dd MMM yyyy"));
                item.SubItems.Add(localEvent.Title);
                item.SubItems.Add(localEvent.Category);
                item.SubItems.Add(localEvent.Description);
                item.Tag = localEvent;
                lvEvents.Items.Add(item);
            }
        }

        private void ChkFilterByDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpDate.Enabled = chkFilterByDate.Checked;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            cmbCategory.SelectedIndex = 0;
            chkFilterByDate.Checked = false;
            dtpDate.Value = DateTime.Today;
            txtSearch.Clear();
            lblFilters.Text = "Find events by category and date";
            DisplayEvents(allEvents.OrderBy(evt => evt.EventDate).ThenBy(evt => evt.Priority).ToList());
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            string selectedCategory = cmbCategory.SelectedItem != null ? cmbCategory.SelectedItem.ToString() : "All";
            bool filterByCategory = selectedCategory != "All";
            bool filterByDate = chkFilterByDate.Checked;
            DateTime selectedDate = dtpDate.Value.Date;
            string searchText = txtSearch.Text.Trim().ToLower();

            if (!string.IsNullOrEmpty(searchText))
            {
                TrackSearchPattern(searchText);
            }

            IEnumerable<LocalEvent> filtered = allEvents;

            if (filterByCategory && eventsByCategory.TryGetValue(selectedCategory, out List<LocalEvent> categoryEvents))
            {
                filtered = categoryEvents;
            }

            if (filterByDate)
            {
                if (eventsByDate.TryGetValue(selectedDate, out List<LocalEvent> dateEvents))
                {
                    filtered = filterByCategory ? filtered.Intersect(dateEvents) : dateEvents;
                }
                else
                {
                    filtered = new List<LocalEvent>();
                }
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(evt => 
                    evt.Title.ToLower().Contains(searchText) || 
                    evt.Description.ToLower().Contains(searchText) ||
                    evt.Category.ToLower().Contains(searchText));
            }

            List<LocalEvent> result = filtered
                .OrderBy(evt => evt.EventDate)
                .ThenBy(evt => evt.Priority)
                .ToList();

            DisplayEvents(result);
            
            if (!string.IsNullOrEmpty(searchText) && result.Count > 0)
            {
                ShowRecommendations(searchText);
            }
        }

        private void TrackSearchPattern(string searchTerm)
        {
            if (searchHistory.ContainsKey(searchTerm))
            {
                searchHistory[searchTerm]++;
            }
            else
            {
                searchHistory[searchTerm] = 1;
            }

            UpdateRecommendationScores(searchTerm);
        }

        private void UpdateRecommendationScores(string searchTerm)
        {
            foreach (LocalEvent evt in allEvents)
            {
                string eventKey = evt.Category.ToLower();
                if (evt.Title.ToLower().Contains(searchTerm) || 
                    evt.Description.ToLower().Contains(searchTerm) ||
                    evt.Category.ToLower().Contains(searchTerm))
                {
                    if (recommendationScores.ContainsKey(eventKey))
                    {
                        recommendationScores[eventKey]++;
                    }
                    else
                    {
                        recommendationScores[eventKey] = 1;
                    }
                }
            }
        }

        private void ShowRecommendations(string currentSearch)
        {
            HashSet<string> relatedCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            
            foreach (KeyValuePair<string, int> score in recommendationScores.OrderByDescending(s => s.Value).Take(3))
            {
                relatedCategories.Add(score.Key);
            }

            if (relatedCategories.Count > 0)
            {
                string recommendationMessage = "Based on your searches, you might also be interested in events related to: " + 
                    string.Join(", ", relatedCategories);
                
                lblFilters.Text = recommendationMessage;
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 261);
            Name = "LocalEventsForm";
            Text = "LocalEventsForm";
            ResumeLayout(false);
        }
    }
}
