using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ATS.Models;
using ATS.Views;
using ATS.Services;
using ATS.Services.Users;

namespace ATS.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {
        public required RegisterModel registerModel;
        public RegisterModel RegisterModel
        {
            get => registerModel;
            set => SetProperty(ref registerModel, value);
        }

        public required LoginModel loginModel;
        public LoginModel LoginModel
        {
            get => loginModel;
            set => SetProperty(ref loginModel, value);
        }

        private string? userName;
        public string? UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        private string? errorMessage;
        public string? ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

        private bool isErrorVisible;
        public bool IsErrorVisible
        {
            get => isErrorVisible;
            set => SetProperty(ref isErrorVisible, value);
        }

        private bool isAuthenticated;
        public bool IsAuthenticated
        {
            get => isAuthenticated;
            set => SetProperty(ref isAuthenticated, value);
        }

        private readonly ClientService clientService;
        private readonly UserService userService;

        public LoginPageViewModel(ClientService clientService, UserService userService)
        {
            this.clientService = clientService;
            this.userService = userService;
            RegisterModel = new RegisterModel();
            LoginModel = new LoginModel();
            IsAuthenticated = false;
        }

        [RelayCommand]
        private async Task Register()
        {
            await clientService.Register(RegisterModel);
        }

        [RelayCommand]
        private async Task Login()
        {
            bool loginSuccess = await clientService.Login(LoginModel);

            if (loginSuccess)
            {
                var user = await userService.GetCurrentUserAsync();
                if (user != null && !string.IsNullOrEmpty(user.UserName))
                {
                    await userService.SetCurrentUserAsync(user);
                    IsAuthenticated = true;
                    UserName = user.UserName;
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                }
                else
                {
                    ShowError("Invalid login credentials.");
                }
            }
            else
            {
                ShowError("Login failed. Please try again.");
            }
        }

        [RelayCommand]
        private async Task Logout()
        {
            userService.Logout();
            IsAuthenticated = false;
            UserName = string.Empty;
            await Shell.Current.GoToAsync("..");
        }

        private void ShowError(string message)
        {
            IsErrorVisible = true;
            ErrorMessage = message;
        }
    }
}
