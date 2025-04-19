using ATS.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ATS.Services.Users;
using Plugin.Firebase.CloudMessaging;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;


namespace ATS.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private string? _userRole;
        private string? _fcmToken;

        public HomePageViewModel(UserService userService)
        {
            _userService = userService;
            Task.Run(async () => await InitializeUserRole());
        }

        public bool IsButtonVisible => !string.IsNullOrEmpty(_userRole) && _userRole != "Guest";

        [RelayCommand]
        private async Task GoToData()
        {
            await Shell.Current.GoToAsync($"//{nameof(DataPage)}");
        }

        [RelayCommand]
        private async Task GetFcmToken()
        {
            try
            {
                await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
                var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
                _fcmToken = token;
                Console.WriteLine($"FCM token: {token}");
                await Shell.Current.DisplayAlert("FCM Token", token ?? "Token not available", "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting FCM token: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task SendPushNotification()
        {
            var app = FirebaseApp.Create(new AppOptions
            {
                Credential = await GetCredential()
            });

            FirebaseMessaging messaging = FirebaseMessaging.GetMessaging(app);
            var message = new Message()
            {
                Token = _fcmToken,
                Notification = new Notification { Title = "Hello world!", Body = "It's a message for Android with MAUI" },
                Data = new Dictionary<string, string> { { "greating", "hello" } },
                Android = new AndroidConfig { Priority = Priority.Normal },
                Apns = new ApnsConfig { Headers = new Dictionary<string, string> { { "apns-priority", "5" } } }
            };
            var response = await messaging.SendAsync(message);
            await Shell.Current.DisplayAlert("Response", response, "OK");
        }

        private async Task<GoogleCredential> GetCredential()
        {
            var path = await FileSystem.OpenAppPackageFileAsync("firebase-adminsdk.json");
            return GoogleCredential.FromStream(path);
        }


        private async Task InitializeUserRole()
        {
            _userRole = await _userService.GetUserRoleAsync();
            OnPropertyChanged(nameof(IsButtonVisible));
        }
    }
}
