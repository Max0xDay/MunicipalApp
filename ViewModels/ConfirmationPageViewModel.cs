using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MunicipalApp.ViewModels
{
    public class ConfirmationPageViewModel : ViewModelBase
    {
        private string _reportNumber;
        private string _location;
        private string _category;
        private string _description;
        private int _photoCount;
        private ReactiveCommand<Unit, Unit> _shareWhatsAppCommand;
        private ReactiveCommand<Unit, Unit> _shareEmailCommand;
        private ReactiveCommand<Unit, Unit> _shareSMSCommand;

        public ConfirmationPageViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            _shareWhatsAppCommand = ReactiveCommand.Create(ShareWhatsApp);
            _shareEmailCommand = ReactiveCommand.Create(ShareEmail);
            _shareSMSCommand = ReactiveCommand.Create(ShareSMS);
        }

        public string ReportNumber
        {
            get => _reportNumber;
            set => this.RaiseAndSetIfChanged(ref _reportNumber, value);
        }

        public string Location
        {
            get => _location;
            set => this.RaiseAndSetIfChanged(ref _location, value);
        }

        public string Category
        {
            get => _category;
            set => this.RaiseAndSetIfChanged(ref _category, value);
        }

        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        public int PhotoCount
        {
            get => _photoCount;
            set
            {
                this.RaiseAndSetIfChanged(ref _photoCount, value);
                this.RaisePropertyChanged(nameof(PhotoCountText));
            }
        }

        public string PhotoCountText => $"{PhotoCount} photo{(PhotoCount != 1 ? "s" : "")} attached";

        private void ShareWhatsApp()
        {
            // Placeholder for WhatsApp sharing functionality
            // In a real implementation, this would open WhatsApp with a pre-filled message
            var message = $"I just reported an issue to the municipal services. Report Number: {ReportNumber}";
            // WhatsApp API call would go here
        }

        private void ShareEmail()
        {
            // Placeholder for email sharing functionality
            // In a real implementation, this would open the default email client
            var subject = $"Municipal Issue Report - {ReportNumber}";
            var body = $"I just reported the following issue:\n\nLocation: {Location}\nCategory: {Category}\nDescription: {Description}\n\nReport Number: {ReportNumber}";
            // Email client API call would go here
        }

        private void ShareSMS()
        {
            // Placeholder for SMS sharing functionality
            // In a real implementation, this would open the SMS app
            var message = $"I just reported an issue to municipal services. Report Number: {ReportNumber}. Location: {Location}";
            // SMS API call would go here
        }

        public ReactiveCommand<Unit, Unit> ShareWhatsAppCommand => _shareWhatsAppCommand;
        public ReactiveCommand<Unit, Unit> ShareEmailCommand => _shareEmailCommand;
        public ReactiveCommand<Unit, Unit> ShareSMSCommand => _shareSMSCommand;
    }
}