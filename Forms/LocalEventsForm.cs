using MunicipalApp.Models;
using MunicipalApp.Services;

namespace MunicipalApp.Forms
{
    public partial class LocalEventsForm : Form
    {
        private readonly EventService _eventService;
        private List<Event> _currentEvents;
        private List<Event> _allEvents;

        public LocalEventsForm()
        {
            InitializeComponent();
            _eventService = new EventService();
            _currentEvents = new List<Event>();
            _allEvents = new List<Event>();
            SetupForm();
            LoadEvents();
            LoadRecommendations();
            LoadRecentActivity();
        }

        private void SetupForm()
        {
            this.Text = "Local Events and Announcements";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 248, 255);
        }

        private void LoadEvents()
        {
            _allEvents = _eventService.LoadEvents();
            _currentEvents = new List<Event>(_allEvents);
            PopulateEventsList(_currentEvents);
            LoadCategories();
        }

        private void LoadCategories()
        {
            var categories = _eventService.GetUniqueCategories();
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("All Categories");
            
            foreach (var category in categories.OrderBy(c => c))
            {
                cmbCategory.Items.Add(category);
            }
            
            cmbCategory.SelectedIndex = 0;
        }

        private void PopulateEventsList(List<Event> events)
        {
            lstEvents.Items.Clear();
            
            foreach (var eventItem in events.OrderBy(e => e.Date))
            {
                var listItem = new ListViewItem(eventItem.Title);
                listItem.SubItems.Add(eventItem.Category);
                listItem.SubItems.Add(eventItem.Date.ToString("yyyy-MM-dd HH:mm"));
                listItem.SubItems.Add(eventItem.Location);
                listItem.SubItems.Add(eventItem.Organizer);
                listItem.Tag = eventItem;
                
                if (eventItem.Date < DateTime.Now)
                {
                    listItem.ForeColor = Color.Gray;
                }
                else if (eventItem.Date <= DateTime.Now.AddDays(3))
                {
                    listItem.BackColor = Color.LightYellow;
                }
                
                lstEvents.Items.Add(listItem);
            }
            
            lblEventCount.Text = $"Showing {events.Count} events";
        }

        private void LoadRecommendations()
        {
            var recommendations = _eventService.GetRecommendedEvents();
            lstRecommendations.Items.Clear();
            
            foreach (var eventItem in recommendations)
            {
                var listItem = new ListViewItem(eventItem.Title);
                listItem.SubItems.Add(eventItem.Date.ToString("MMM dd"));
                listItem.SubItems.Add(eventItem.Category);
                listItem.Tag = eventItem;
                lstRecommendations.Items.Add(listItem);
            }
        }

        private void LoadRecentActivity()
        {
            var recentSearches = _eventService.GetRecentSearches();
            var recentViewed = _eventService.GetRecentlyViewedEvents();
            
            lstRecentSearches.Items.Clear();
            foreach (var search in recentSearches.ToArray().Reverse().Take(5))
            {
                lstRecentSearches.Items.Add(FormatSearchTerm(search));
            }
            
            lstRecentViewed.Items.Clear();
            foreach (var eventItem in recentViewed.ToArray().Take(5))
            {
                lstRecentViewed.Items.Add($"{eventItem.Title} - {eventItem.Date:MMM dd}");
            }
        }

        private string FormatSearchTerm(string searchTerm)
        {
            if (searchTerm.StartsWith("category:"))
                return $"Category: {searchTerm.Substring(9)}";
            if (searchTerm.StartsWith("date:"))
                return $"Date: {searchTerm.Substring(5)}";
            if (searchTerm.StartsWith("daterange:"))
                return $"Date Range: {searchTerm.Substring(10).Replace("_", " to ")}";
            
            return searchTerm;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var searchResults = new List<Event>();
            var hasFilter = false;

            if (cmbCategory.SelectedIndex > 0)
            {
                var selectedCategory = cmbCategory.SelectedItem.ToString();
                searchResults = _eventService.SearchEventsByCategory(selectedCategory);
                hasFilter = true;
            }

            if (dtpFromDate.Checked && dtpToDate.Checked)
            {
                var dateResults = _eventService.SearchEventsByDateRange(dtpFromDate.Value, dtpToDate.Value);
                
                if (hasFilter)
                {
                    searchResults = searchResults.Intersect(dateResults, new EventComparer()).ToList();
                }
                else
                {
                    searchResults = dateResults;
                    hasFilter = true;
                }
            }
            else if (dtpFromDate.Checked)
            {
                var dateResults = _eventService.SearchEventsByDate(dtpFromDate.Value);
                
                if (hasFilter)
                {
                    searchResults = searchResults.Intersect(dateResults, new EventComparer()).ToList();
                }
                else
                {
                    searchResults = dateResults;
                    hasFilter = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtSearchKeyword.Text))
            {
                var keyword = txtSearchKeyword.Text.ToLower();
                var keywordResults = _allEvents.Where(e => 
                    e.Title.ToLower().Contains(keyword) ||
                    e.Description.ToLower().Contains(keyword) ||
                    e.Tags.Any(t => t.ToLower().Contains(keyword))
                ).ToList();
                
                if (hasFilter)
                {
                    searchResults = searchResults.Intersect(keywordResults, new EventComparer()).ToList();
                }
                else
                {
                    searchResults = keywordResults;
                }
            }
            else if (!hasFilter)
            {
                searchResults = new List<Event>(_allEvents);
            }

            _currentEvents = searchResults;
            PopulateEventsList(_currentEvents);
            LoadRecentActivity();
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            cmbCategory.SelectedIndex = 0;
            dtpFromDate.Checked = false;
            dtpToDate.Checked = false;
            txtSearchKeyword.Clear();
            
            _currentEvents = new List<Event>(_allEvents);
            PopulateEventsList(_currentEvents);
        }

        private void lstEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstEvents.SelectedItems.Count > 0)
            {
                var selectedEvent = (Event)lstEvents.SelectedItems[0].Tag;
                ShowEventDetails(selectedEvent);
                _eventService.AddRecentlyViewedEvent(selectedEvent);
                LoadRecentActivity();
            }
        }

        private void lstRecommendations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstRecommendations.SelectedItems.Count > 0)
            {
                var selectedEvent = (Event)lstRecommendations.SelectedItems[0].Tag;
                ShowEventDetails(selectedEvent);
                _eventService.AddRecentlyViewedEvent(selectedEvent);
                LoadRecentActivity();
            }
        }

        private void ShowEventDetails(Event selectedEvent)
        {
            var details = $"Title: {selectedEvent.Title}\n\n" +
                         $"Category: {selectedEvent.Category}\n" +
                         $"Date: {selectedEvent.Date:dddd, MMMM dd, yyyy 'at' HH:mm}\n" +
                         $"Location: {selectedEvent.Location}\n" +
                         $"Organizer: {selectedEvent.Organizer}\n" +
                         $"Priority: {selectedEvent.Priority}/5\n\n" +
                         $"Description:\n{selectedEvent.Description}\n\n" +
                         $"Tags: {string.Join(", ", selectedEvent.Tags)}";

            rtbEventDetails.Text = details;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPriorityEvents_Click(object sender, EventArgs e)
        {
            var priorityEvents = _eventService.GetPriorityEvents(10);
            _currentEvents = priorityEvents;
            PopulateEventsList(_currentEvents);
        }

        private void btnUpcomingEvents_Click(object sender, EventArgs e)
        {
            var upcomingEvents = _allEvents
                .Where(e => e.Date >= DateTime.Now)
                .OrderBy(e => e.Date)
                .Take(10)
                .ToList();
                
            _currentEvents = upcomingEvents;
            PopulateEventsList(_currentEvents);
        }
    }

    public class EventComparer : IEqualityComparer<Event>
    {
        public bool Equals(Event x, Event y)
        {
            return x?.Id == y?.Id;
        }

        public int GetHashCode(Event obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}