using ATS.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ATS.Services.Users;
using Plugin.Firebase.CloudMessaging;


namespace ATS.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private string _userRole;

        public HomePageViewModel(UserService userService)
        {
            _userService = userService;
            InitializeUserRole();
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
                Console.WriteLine($"FCM token: {token}");
                await Shell.Current.DisplayAlert("FCM Token", token ?? "Token not available", "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting FCM token: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }


        private async Task InitializeUserRole()
        {
            _userRole = await _userService.GetUserRoleAsync();
            OnPropertyChanged(nameof(IsButtonVisible));
        }
    }
}
