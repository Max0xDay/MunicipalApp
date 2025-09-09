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
        private string _location;
        private List<MapboxSuggestion> _suggestions;
        private bool _isLoading;
        private bool _showSuggestions;
        private MapboxSuggestion? _selectedSuggestion;
        private ReactiveCommand<MapboxSuggestion, Unit> _selectSuggestionCommand;

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
        public string SelectedLocationDisplay => _selectedSuggestion?.Place_Name ?? Location ?? "No location selected";

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

        public async void LocationTextChanged(string text)
        {
            Location = text;

            if (string.IsNullOrWhiteSpace(text) || text.Length < 2)
            {
                Suggestions.Clear();
                ShowSuggestions = false;
                return;
            }

            IsLoading = true;
            ShowSuggestions = true;

            try
            {
                var newSuggestions = await _mapboxService.GetLocationSuggestions(text);
                Suggestions = newSuggestions;
            }
            catch (Exception)
            {
                Suggestions.Clear();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private Unit SelectSuggestion(MapboxSuggestion suggestion)
        {
            _selectedSuggestion = suggestion;
            Location = suggestion.Place_Name;
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