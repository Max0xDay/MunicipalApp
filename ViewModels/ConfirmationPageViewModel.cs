using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MunicipalApp.ViewModels
{
    public class ConfirmationPageViewModel : ViewModelBase
    {
    private string _reportNumber = string.Empty;
    private string _location = string.Empty;
    private string _category = string.Empty;
    private string _description = string.Empty;
        private int _photoCount;
    private ReactiveCommand<Unit, Unit> _shareWhatsAppCommand = null!;
    private ReactiveCommand<Unit, Unit> _shareEmailCommand = null!;
    private ReactiveCommand<Unit, Unit> _shareSMSCommand = null!;
    private ReactiveCommand<Unit, Unit> _shareTwitterCommand = null!;
    private ReactiveCommand<Unit, Unit> _shareFacebookCommand = null!;
    private ReactiveCommand<Unit, Unit> _shareInstagramCommand = null!;

        public ConfirmationPageViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            _shareWhatsAppCommand = ReactiveCommand.Create(ShareWhatsApp);
            _shareEmailCommand = ReactiveCommand.Create(ShareEmail);
            _shareSMSCommand = ReactiveCommand.Create(ShareSMS);
            _shareTwitterCommand = ReactiveCommand.Create(ShareTwitter);
            _shareFacebookCommand = ReactiveCommand.Create(ShareFacebook);
            _shareInstagramCommand = ReactiveCommand.Create(ShareInstagram);
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

        private void ShareTwitter()
        {
            // Construct a Tweet intent URL (X / Twitter)
            var text = System.Web.HttpUtility.UrlEncode($"Reported municipal issue {ReportNumber} at {Location} (#MunicipalServices)");
            var url = $"https://twitter.com/intent/tweet?text={text}";
            OpenExternal(url);
        }

        private void ShareFacebook()
        {
            // Facebook share (share a generic page or placeholder until a public URL exists)
            var shareText = System.Web.HttpUtility.UrlEncode($"Reported municipal issue {ReportNumber} at {Location}");
            var shareUrl = "https://example.com/municipal-report"; // Placeholder; replace with public issue URL if available
            var url = $"https://www.facebook.com/sharer/sharer.php?u={System.Web.HttpUtility.UrlEncode(shareUrl)}&quote={shareText}";
            OpenExternal(url);
        }

        private void ShareInstagram()
        {
            // Instagram doesn’t support direct text share via URL on desktop; placeholder action
            // Could copy text to clipboard or open a guidance dialog in a full implementation
        }

        private void OpenExternal(string? url)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(url))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
            }
            catch { /* swallow */ }
        }

        public ReactiveCommand<Unit, Unit> ShareWhatsAppCommand => _shareWhatsAppCommand;
        public ReactiveCommand<Unit, Unit> ShareEmailCommand => _shareEmailCommand;
        public ReactiveCommand<Unit, Unit> ShareSMSCommand => _shareSMSCommand;
    public ReactiveCommand<Unit, Unit> ShareTwitterCommand => _shareTwitterCommand;
    public ReactiveCommand<Unit, Unit> ShareFacebookCommand => _shareFacebookCommand;
    public ReactiveCommand<Unit, Unit> ShareInstagramCommand => _shareInstagramCommand;
    }
}