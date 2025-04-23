using ATS.ViewModels;
using Plugin.Firebase.CloudMessaging;

namespace ATS.Views
{
    public partial class DataPage : ContentPage, IQueryAttributable
    {
        public DataPage(DataPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Notification", out var notifObj) && notifObj is FCMNotification notification)
            {
                var title = notification.Data.TryGetValue("Title", out var titleVal) ? titleVal : "Notification";
                var body = notification.Data.TryGetValue("Body", out var bodyVal) ? bodyVal : "No body";

                Shell.Current.DisplayAlert(title, body, "OK");
            }
        }
    }
}
