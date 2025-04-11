using ATS.Services;
using ATS.Services.Users;
using ATS.ViewModels;
using ATS.Views;
using Microsoft.Extensions.Logging;

namespace ATS
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //niet de beste manier, later oplossen
            builder.Services.AddHttpClient("custom-httpclient")
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        return new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
    })
    .ConfigureHttpClient(httpClient =>
    {
        var baseAddress = DeviceInfo.Platform == DevicePlatform.Android
            ? "https://10.0.2.2:7194"
            : "https://localhost:7194";

        httpClient.BaseAddress = new Uri(baseAddress);
    });


#if DEBUG
            builder.Logging.AddDebug();

            builder.Services.AddSingleton<ClientService>();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<DataPage>();

            builder.Services.AddSingleton<LoginPageViewModel>();
            builder.Services.AddSingleton<HomePageViewModel>();
            builder.Services.AddSingleton<DataPageViewModel>();
#endif

            return builder.Build();
        }
    }
}
