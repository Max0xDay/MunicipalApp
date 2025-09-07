using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using MunicipalApp.Models;
using MunicipalApp.Services;

namespace MunicipalApp.ViewModels;

public class LocalEventsViewModel : ViewModelBase
{
    private readonly EventService _eventService;
    private string _searchKeyword = string.Empty;
    private string _selectedCategory = string.Empty;
    private DateTime _fromDate = DateTime.Today.AddDays(-30);
    private DateTime _toDate = DateTime.Today.AddDays(30);
    private Event? _selectedEvent;
    private string _eventDetails = string.Empty;
    private int _eventCount;

    public LocalEventsViewModel()
    {
        _eventService = new EventService();
        Events = new ObservableCollection<Event>();
        Recommendations = new ObservableCollection<Event>();
        Categories = new ObservableCollection<string>
        {
            "All Categories",
            "Municipal Meeting",
            "Community Event", 
            "Public Notice",
            "Festival",
            "Workshop",
            "Sports",
            "Cultural"
        };
        RecentSearches = new ObservableCollection<string>();

        SearchCommand = ReactiveCommand.Create(PerformSearch);
        ClearSearchCommand = ReactiveCommand.Create(ClearSearch);
        ShowPriorityEventsCommand = ReactiveCommand.Create(ShowPriorityEvents);
        ShowUpcomingEventsCommand = ReactiveCommand.Create(ShowUpcomingEvents);

        this.WhenAnyValue(x => x.SelectedEvent)
            .Subscribe(LoadEventDetails);

        LoadEvents();
        LoadRecommendations();
    }

    public string Title => "Local Events and Announcements";
    
    public string SearchKeyword
    {
        get => _searchKeyword;
        set => this.RaiseAndSetIfChanged(ref _searchKeyword, value);
    }

    public string SelectedCategory
    {
        get => _selectedCategory;
        set => this.RaiseAndSetIfChanged(ref _selectedCategory, value);
    }

    public DateTime FromDate
    {
        get => _fromDate;
        set => this.RaiseAndSetIfChanged(ref _fromDate, value);
    }

    public DateTime ToDate
    {
        get => _toDate;
        set => this.RaiseAndSetIfChanged(ref _toDate, value);
    }

    public Event? SelectedEvent
    {
        get => _selectedEvent;
        set => this.RaiseAndSetIfChanged(ref _selectedEvent, value);
    }

    public string EventDetails
    {
        get => _eventDetails;
        set => this.RaiseAndSetIfChanged(ref _eventDetails, value);
    }

    public int EventCount
    {
        get => _eventCount;
        set => this.RaiseAndSetIfChanged(ref _eventCount, value);
    }

    public ObservableCollection<Event> Events { get; }
    public ObservableCollection<Event> Recommendations { get; }
    public ObservableCollection<string> Categories { get; }
    public ObservableCollection<string> RecentSearches { get; }

    public ReactiveCommand<Unit, Unit> SearchCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearSearchCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowPriorityEventsCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowUpcomingEventsCommand { get; }

    private void LoadEvents()
    {
        var events = _eventService.GetAllEvents();
        Events.Clear();
        foreach (var evt in events)
        {
            Events.Add(evt);
        }
        EventCount = Events.Count;
    }

    private void LoadRecommendations()
    {
        var recommendations = _eventService.GetRecommendedEvents();
        Recommendations.Clear();
        foreach (var evt in recommendations.Take(5))
        {
            Recommendations.Add(evt);
        }
    }

    private void PerformSearch()
    {
        if (string.IsNullOrWhiteSpace(SearchKeyword) && SelectedCategory == "All Categories")
        {
            LoadEvents();
            return;
        }

        var results = new List<Event>();

        if (!string.IsNullOrWhiteSpace(SearchKeyword))
        {
            results = _eventService.SearchEvents(SearchKeyword);
            if (!RecentSearches.Contains(SearchKeyword))
            {
                RecentSearches.Insert(0, SearchKeyword);
                if (RecentSearches.Count > 5)
                {
                    RecentSearches.RemoveAt(RecentSearches.Count - 1);
                }
            }
        }

        if (!string.IsNullOrWhiteSpace(SelectedCategory) && SelectedCategory != "All Categories")
        {
            var categoryResults = _eventService.SearchEventsByCategory(SelectedCategory);
            results = results.Any() ? 
                results.Intersect(categoryResults, new EventComparer()).ToList() : 
                categoryResults;
        }

        Events.Clear();
        foreach (var evt in results)
        {
            Events.Add(evt);
        }
        EventCount = Events.Count;
    }

    private void ClearSearch()
    {
        SearchKeyword = string.Empty;
        SelectedCategory = "All Categories";
        LoadEvents();
    }

    private void ShowPriorityEvents()
    {
        var priorityEvents = _eventService.GetPriorityEvents();
        Events.Clear();
        foreach (var evt in priorityEvents)
        {
            Events.Add(evt);
        }
        EventCount = Events.Count;
    }

    private void ShowUpcomingEvents()
    {
        var upcomingEvents = _eventService.GetUpcomingEvents();
        Events.Clear();
        foreach (var evt in upcomingEvents)
        {
            Events.Add(evt);
        }
        EventCount = Events.Count;
    }

    private void LoadEventDetails(Event? selectedEvent)
    {
        if (selectedEvent == null)
        {
            EventDetails = "Select an event to view details.";
            return;
        }

        EventDetails = $"Title: {selectedEvent.Title}\n\n" +
                      $"Date: {selectedEvent.Date:dddd, MMMM dd, yyyy}\n\n" +
                      $"Location: {selectedEvent.Location}\n\n" +
                      $"Category: {selectedEvent.Category}\n\n" +
                      $"Organizer: {selectedEvent.Organizer}\n\n" +
                      $"Priority: {selectedEvent.Priority}\n\n" +
                      $"Description:\n{selectedEvent.Description}";
    }

    private class EventComparer : IEqualityComparer<Event>
    {
        public bool Equals(Event? x, Event? y) => x?.Id == y?.Id;
        public int GetHashCode(Event obj) => obj.Id.GetHashCode();
    }
}