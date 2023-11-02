using Microsoft.Extensions.Logging;
using PasswordManagerApp.Components.Dialog;
using PasswordManagerApp.Data;

namespace PasswordManagerApp
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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddBlazorBootstrap();

            return builder.Build();
        }
    }
}