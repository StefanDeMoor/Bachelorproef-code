using CommunityToolkit.Mvvm.ComponentModel;

namespace ATS.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        private string? title;
        public string? Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
    }
}
