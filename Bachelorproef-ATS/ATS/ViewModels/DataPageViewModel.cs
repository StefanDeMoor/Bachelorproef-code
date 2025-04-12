using ATS.Services.Users;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ATS.ViewModels
{
    public partial class DataPageViewModel : ObservableObject
    {
        private readonly UserService _userService;

        private string? userName;
        public string? UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        private string? userRole;
        public string? UserRole
        {
            get => userRole;
            set => SetProperty(ref userRole, value);
        }

        public DataPageViewModel(UserService userService)
        {
            _userService = userService;
            Task.Run(async () => await LoadUserData());
        }

        private async Task LoadUserData()
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user != null)
            {
                UserName = user.UserName?.ToLower();
                UserRole = user.Role?.ToLower();
            }
            else
            {
                UserName = "Guest";
                UserRole = "No Role Assigned";
            }
        }
    }
}
