using ReactiveUI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MunicipalApp.ViewModels
{
    public class CategoryPageViewModel : ViewModelBase
    {
        private string _selectedCategory;
        private string _description;

        public CategoryPageViewModel()
        {
            Categories = new List<string>
            {
                "Sanitation",
                "Roads",
                "Utilities",
                "Water",
                "Electricity"
            };
        }

        public List<string> Categories { get; }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedCategory, value);
                this.RaisePropertyChanged(nameof(HasSelectedCategory));
                this.RaisePropertyChanged(nameof(CategoryDescription));
                this.RaisePropertyChanged(nameof(CategoryLongDescription));
                this.RaisePropertyChanged(nameof(SelectedCategoryIcon));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                this.RaiseAndSetIfChanged(ref _description, value);
                this.RaisePropertyChanged(nameof(DescriptionCharCount));
                this.RaisePropertyChanged(nameof(ShowDescriptionGuidelines));
            }
        }

        public string DescriptionCharCount => $"{Description?.Length ?? 0}/1000";
        public bool HasSelectedCategory => !string.IsNullOrWhiteSpace(SelectedCategory);
        public bool ShowDescriptionGuidelines => string.IsNullOrWhiteSpace(Description);

        public string CategoryDescription => SelectedCategory switch
        {
            "Sanitation" => "Waste collection, street cleaning, public toilets",
            "Roads" => "Potholes, traffic lights, road signs, pavement issues",
            "Utilities" => "General utility services not covered elsewhere",
            "Water" => "Water leaks, burst pipes, water quality issues",
            "Electricity" => "Power outages, faulty wiring, street lights",
            _ => ""
        };

        public string CategoryLongDescription => SelectedCategory switch
        {
            "Sanitation" => "Report issues related to waste management, street cleaning, or public sanitation facilities.",
            "Roads" => "Report road surface problems, traffic control issues, or pedestrian pathway concerns.",
            "Utilities" => "Report general utility service issues that don't fit in other categories.",
            "Water" => "Report water supply issues, leaks, or water quality problems.",
            "Electricity" => "Report power supply problems, electrical hazards, or street light outages.",
            _ => ""
        };

        public string SelectedCategoryIcon => SelectedCategory switch
        {
            "Sanitation" => "Sanitation",
            "Roads" => "Roads",
            "Utilities" => "Utilities",
            "Water" => "Water",
            "Electricity" => "Electricity",
            _ => "Category"
        };

        public void Clear()
        {
            SelectedCategory = string.Empty;
            Description = string.Empty;
        }
    }
}