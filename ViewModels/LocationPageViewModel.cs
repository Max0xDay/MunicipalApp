using MunicipalApp.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MunicipalApp.ViewModels
{
    public class LocationPageViewModel : ViewModelBase
    {
    private readonly MapboxService _mapboxService;
    private string _location = string.Empty;
        private List<MapboxSuggestion> _suggestions;
        private bool _isLoading;
        private bool _showSuggestions;
        private MapboxSuggestion? _selectedSuggestion;
    private ReactiveCommand<MapboxSuggestion, Unit> _selectSuggestionCommand = null!; // initialized in InitializeCommands
    private System.Threading.CancellationTokenSource? _suggestCts;

        public LocationPageViewModel()
        {
            _mapboxService = new MapboxService();
            _suggestions = new List<MapboxSuggestion>();
            
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            _selectSuggestionCommand = ReactiveCommand.Create<MapboxSuggestion, Unit>(SelectSuggestion);
        }

        public string Location
        {
            get => _location;
            set
            {
                this.RaiseAndSetIfChanged(ref _location, value);
                this.RaisePropertyChanged(nameof(LocationCharCount));
                this.RaisePropertyChanged(nameof(HasLocation));
                this.RaisePropertyChanged(nameof(SelectedLocationDisplay));
            }
        }

        public string LocationCharCount => $"{Location?.Length ?? 0}/200";
        public bool HasLocation => !string.IsNullOrWhiteSpace(Location);
    public string SelectedLocationDisplay => _selectedSuggestion?.PlaceName ?? Location ?? "No location selected";

        public MapboxSuggestion? SelectedSuggestion
        {
            get => _selectedSuggestion;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedSuggestion, value);
                if (value != null)
                {
                    SelectSuggestion(value);
                }
            }
        }

        public List<MapboxSuggestion> Suggestions
        {
            get => _suggestions;
            set => this.RaiseAndSetIfChanged(ref _suggestions, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

        public bool ShowSuggestions
        {
            get => _showSuggestions;
            set => this.RaiseAndSetIfChanged(ref _showSuggestions, value);
        }

        public async Task SearchAsync()
        {
            var text = Location;
            _suggestCts?.Cancel();
            _suggestCts = null;

            if (string.IsNullOrWhiteSpace(text) || text.Length < 2)
            {
                Suggestions = new List<MapboxSuggestion>();
                ShowSuggestions = false;
                IsLoading = false;
                return;
            }

            var cts = new System.Threading.CancellationTokenSource();
            _suggestCts = cts;
            IsLoading = true;
            ShowSuggestions = true;

            try
            {
                var newSuggestions = await _mapboxService.GetLocationSuggestions(text);
                if (cts.IsCancellationRequested) return;
                Suggestions = newSuggestions;
            }
            catch (OperationCanceledException) { }
            catch (Exception)
            {
                if (!cts.IsCancellationRequested)
                {
                    Suggestions = new List<MapboxSuggestion>();
                }
            }
            finally
            {
                if (!cts.IsCancellationRequested)
                {
                    IsLoading = false;
                }
            }
        }

        private Unit SelectSuggestion(MapboxSuggestion suggestion)
        {
            _selectedSuggestion = suggestion;
            Location = suggestion.PlaceName;
            ShowSuggestions = false;
            Suggestions.Clear();
            return Unit.Default;
        }

        public void Clear()
        {
            Location = string.Empty;
            _selectedSuggestion = null;
            Suggestions.Clear();
            ShowSuggestions = false;
            IsLoading = false;
        }

        public ReactiveCommand<MapboxSuggestion, Unit> SelectSuggestionCommand => _selectSuggestionCommand;
    }
}