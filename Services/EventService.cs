using MunicipalApp.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MunicipalApp.Services
{
    public class EventService
    {
        private readonly string _dataFilePath;
        
        private readonly Dictionary<string, List<Event>> _eventsByCategory;
        private readonly SortedDictionary<DateTime, List<Event>> _eventsByDate;
        private readonly HashSet<string> _uniqueCategories;
        private readonly Queue<string> _recentSearches;
        private readonly Stack<Event> _recentlyViewedEvents;
        private readonly PriorityQueue<Event, int> _priorityEvents;
        private readonly Dictionary<string, int> _searchPatterns;

        private const int MaxRecentSearches = 10;
        private const int MaxRecentlyViewed = 5;

        public EventService()
        {
            _dataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "events.json");
            
            _eventsByCategory = new Dictionary<string, List<Event>>();
            _eventsByDate = new SortedDictionary<DateTime, List<Event>>();
            _uniqueCategories = new HashSet<string>();
            _recentSearches = new Queue<string>();
            _recentlyViewedEvents = new Stack<Event>();
            _priorityEvents = new PriorityQueue<Event, int>();
            _searchPatterns = new Dictionary<string, int>();
            
            EnsureDataDirectoryExists();
            LoadEventsIntoDataStructures();
        }

        private void EnsureDataDirectoryExists()
        {
            var directory = Path.GetDirectoryName(_dataFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }
        }

        public List<Event> LoadEvents()
        {
            try
            {
                if (!File.Exists(_dataFilePath))
                {
                    return GenerateSampleEvents();
                }

                var json = File.ReadAllText(_dataFilePath);
                var events = JsonConvert.DeserializeObject<List<Event>>(json) ?? new List<Event>();
                
                if (events.Count == 0)
                {
                    events = GenerateSampleEvents();
                    SaveEvents(events);
                }
                
                return events;
            }
            catch
            {
                return GenerateSampleEvents();
            }
        }

        public void SaveEvents(List<Event> events)
        {
            try
            {
                var json = JsonConvert.SerializeObject(events, Formatting.Indented);
                File.WriteAllText(_dataFilePath, json);
                LoadEventsIntoDataStructures();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error saving events: {ex.Message}", ex);
            }
        }

        private void LoadEventsIntoDataStructures()
        {
            var events = LoadEvents();
            
            _eventsByCategory.Clear();
            _eventsByDate.Clear();
            _uniqueCategories.Clear();
            
            while (_priorityEvents.Count > 0)
            {
                _priorityEvents.Dequeue();
            }

            foreach (var eventItem in events)
            {
                if (!_eventsByCategory.ContainsKey(eventItem.Category))
                {
                    _eventsByCategory[eventItem.Category] = new List<Event>();
                }
                _eventsByCategory[eventItem.Category].Add(eventItem);

                var dateKey = eventItem.Date.Date;
                if (!_eventsByDate.ContainsKey(dateKey))
                {
                    _eventsByDate[dateKey] = new List<Event>();
                }
                _eventsByDate[dateKey].Add(eventItem);

                _uniqueCategories.Add(eventItem.Category);
                
                _priorityEvents.Enqueue(eventItem, -eventItem.Priority);
            }
        }

        public List<Event> SearchEventsByCategory(string category)
        {
            TrackSearch($"category:{category}");
            
            if (_eventsByCategory.ContainsKey(category))
            {
                return new List<Event>(_eventsByCategory[category]);
            }
            
            return new List<Event>();
        }

        public List<Event> SearchEventsByDate(DateTime date)
        {
            TrackSearch($"date:{date:yyyy-MM-dd}");
            
            var dateKey = date.Date;
            if (_eventsByDate.ContainsKey(dateKey))
            {
                return new List<Event>(_eventsByDate[dateKey]);
            }
            
            return new List<Event>();
        }

        public List<Event> SearchEventsByDateRange(DateTime startDate, DateTime endDate)
        {
            TrackSearch($"daterange:{startDate:yyyy-MM-dd}_{endDate:yyyy-MM-dd}");
            
            var results = new List<Event>();
            
            foreach (var kvp in _eventsByDate)
            {
                if (kvp.Key >= startDate.Date && kvp.Key <= endDate.Date)
                {
                    results.AddRange(kvp.Value);
                }
            }
            
            return results;
        }

        public HashSet<string> GetUniqueCategories()
        {
            return new HashSet<string>(_uniqueCategories);
        }

        public Queue<string> GetRecentSearches()
        {
            return new Queue<string>(_recentSearches.ToArray());
        }

        public Stack<Event> GetRecentlyViewedEvents()
        {
            return new Stack<Event>(_recentlyViewedEvents.ToArray().Reverse());
        }

        public List<Event> GetPriorityEvents(int count = 5)
        {
            var priorityEventsCopy = new PriorityQueue<Event, int>();
            var results = new List<Event>();
            
            var tempEvents = new List<(Event, int)>();
            while (_priorityEvents.Count > 0 && results.Count < count)
            {
                var eventItem = _priorityEvents.Dequeue();
                results.Add(eventItem);
                tempEvents.Add((eventItem, -eventItem.Priority));
            }
            
            foreach (var (eventItem, priority) in tempEvents)
            {
                _priorityEvents.Enqueue(eventItem, priority);
            }
            
            return results;
        }

        public void AddRecentlyViewedEvent(Event eventItem)
        {
            if (_recentlyViewedEvents.Count >= MaxRecentlyViewed)
            {
                var tempStack = new Stack<Event>();
                for (int i = 0; i < MaxRecentlyViewed - 1; i++)
                {
                    if (_recentlyViewedEvents.Count > 0)
                        tempStack.Push(_recentlyViewedEvents.Pop());
                }
                
                _recentlyViewedEvents.Clear();
                while (tempStack.Count > 0)
                {
                    _recentlyViewedEvents.Push(tempStack.Pop());
                }
            }
            
            _recentlyViewedEvents.Push(eventItem);
        }

        private void TrackSearch(string searchTerm)
        {
            if (_recentSearches.Count >= MaxRecentSearches)
            {
                _recentSearches.Dequeue();
            }
            _recentSearches.Enqueue(searchTerm);
            
            if (_searchPatterns.ContainsKey(searchTerm))
            {
                _searchPatterns[searchTerm]++;
            }
            else
            {
                _searchPatterns[searchTerm] = 1;
            }
        }

        public List<Event> GetAllEvents()
        {
            return LoadEvents();
        }

        public List<Event> SearchEvents(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<Event>();

            TrackSearch(keyword);
            var allEvents = LoadEvents();
            
            return allEvents.Where(e => 
                e.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                e.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                e.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                e.Location.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                e.Organizer.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                e.Tags.Any(tag => tag.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        public List<Event> GetUpcomingEvents(int count = 10)
        {
            var allEvents = LoadEvents();
            return allEvents
                .Where(e => e.Date >= DateTime.Now)
                .OrderBy(e => e.Date)
                .Take(count)
                .ToList();
        }

        public List<Event> GetRecommendedEvents()
        {
            var recommendations = new List<Event>();
            var allEvents = LoadEvents();
            
            var topSearchPatterns = _searchPatterns
                .OrderByDescending(kvp => kvp.Value)
                .Take(3)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var pattern in topSearchPatterns)
            {
                if (pattern.StartsWith("category:"))
                {
                    var category = pattern.Substring(9);
                    var categoryEvents = SearchEventsByCategory(category);
                    recommendations.AddRange(categoryEvents.Take(2));
                }
                else if (pattern.StartsWith("date:"))
                {
                    if (DateTime.TryParse(pattern.Substring(5), out var searchDate))
                    {
                        var futureEvents = allEvents
                            .Where(e => e.Date >= DateTime.Now && e.Date.Date == searchDate.Date)
                            .Take(2);
                        recommendations.AddRange(futureEvents);
                    }
                }
            }
            
            if (recommendations.Count < 5)
            {
                var upcomingEvents = allEvents
                    .Where(e => e.Date >= DateTime.Now)
                    .OrderBy(e => e.Date)
                    .Take(5 - recommendations.Count);
                recommendations.AddRange(upcomingEvents);
            }
            
            return recommendations.Distinct().Take(5).ToList();
        }

        private List<Event> GenerateSampleEvents()
        {
            var categories = new[] { "Community", "Government", "Recreation", "Education", "Health", "Safety" };
            var events = new List<Event>();
            var random = new Random();

            for (int i = 0; i < 20; i++)
            {
                var category = categories[random.Next(categories.Length)];
                var eventDate = DateTime.Now.AddDays(random.Next(-5, 30));
                
                var sampleEvent = new Event
                {
                    Title = GenerateEventTitle(category, i + 1),
                    Description = GenerateEventDescription(category),
                    Category = category,
                    Date = eventDate,
                    Location = GenerateEventLocation(),
                    Organizer = GenerateEventOrganizer(category),
                    Priority = random.Next(1, 6),
                    Tags = GenerateEventTags(category)
                };

                events.Add(sampleEvent);
            }

            return events;
        }

        private string GenerateEventTitle(string category, int number)
        {
            var titles = new Dictionary<string, string[]>
            {
                {"Community", new[] {"Community Clean-up Day", "Neighborhood Watch Meeting", "Local Market Day", "Community Garden Workshop"}},
                {"Government", new[] {"Council Meeting", "Public Budget Review", "Municipal Planning Session", "Citizen Engagement Forum"}},
                {"Recreation", new[] {"Park Festival", "Sports Day", "Family Fun Run", "Outdoor Movie Night"}},
                {"Education", new[] {"Digital Literacy Workshop", "Youth Development Program", "Adult Education Classes", "School Board Meeting"}},
                {"Health", new[] {"Health Screening Day", "Vaccination Drive", "Mental Health Awareness", "Fitness Challenge"}},
                {"Safety", new[] {"Fire Safety Training", "Disaster Preparedness Workshop", "Road Safety Campaign", "Emergency Response Drill"}}
            };

            var categoryTitles = titles[category];
            return $"{categoryTitles[number % categoryTitles.Length]} #{number}";
        }

        private string GenerateEventDescription(string category)
        {
            var descriptions = new Dictionary<string, string>
            {
                {"Community", "Join your neighbors for this important community initiative that brings everyone together."},
                {"Government", "Official municipal event open to all residents. Public participation encouraged."},
                {"Recreation", "Fun-filled event for the whole family. Come enjoy activities and entertainment."},
                {"Education", "Educational opportunity for community members to learn and develop new skills."},
                {"Health", "Health and wellness event promoting community wellbeing and healthy lifestyle."},
                {"Safety", "Important safety initiative to keep our community prepared and secure."}
            };

            return descriptions[category];
        }

        private string GenerateEventLocation()
        {
            var locations = new[] {
                "Municipal Hall", "Community Center", "Central Park", "City Library",
                "Recreation Complex", "Town Square", "Civic Center", "Public Gardens"
            };

            return locations[new Random().Next(locations.Length)];
        }

        private string GenerateEventOrganizer(string category)
        {
            var organizers = new Dictionary<string, string[]>
            {
                {"Community", new[] {"Community Association", "Residents Committee", "Volunteer Group"}},
                {"Government", new[] {"Municipal Council", "City Administration", "Public Works Department"}},
                {"Recreation", new[] {"Parks & Recreation", "Sports Committee", "Cultural Affairs"}},
                {"Education", new[] {"Education Department", "Library Services", "Skills Development"}},
                {"Health", new[] {"Health Department", "Community Health Center", "Wellness Committee"}},
                {"Safety", new[] {"Emergency Services", "Safety Committee", "Fire Department"}}
            };

            var categoryOrganizers = organizers[category];
            return categoryOrganizers[new Random().Next(categoryOrganizers.Length)];
        }

        private List<string> GenerateEventTags(string category)
        {
            var tags = new Dictionary<string, string[]>
            {
                {"Community", new[] {"community", "volunteer", "neighborhood", "local"}},
                {"Government", new[] {"official", "public", "municipal", "civic"}},
                {"Recreation", new[] {"fun", "family", "entertainment", "outdoor"}},
                {"Education", new[] {"learning", "workshop", "skills", "development"}},
                {"Health", new[] {"wellness", "health", "fitness", "medical"}},
                {"Safety", new[] {"safety", "emergency", "preparedness", "security"}}
            };

            var categoryTags = tags[category];
            var eventTags = new List<string>();
            var random = new Random();
            
            for (int i = 0; i < random.Next(2, 4); i++)
            {
                var tag = categoryTags[random.Next(categoryTags.Length)];
                if (!eventTags.Contains(tag))
                    eventTags.Add(tag);
            }

            return eventTags;
        }
    }
}