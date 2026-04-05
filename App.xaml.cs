using System.Windows;
using Hangman.Dialog_Service;
using Hangman.Game;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hangman
{
    public partial class App : Application
    {
        private IHost _host;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<UserService>();
                    services.AddSingleton<IDialogService, DialogService>();
                    services.AddTransient<SignInViewModel>();
                    services.AddTransient<SignInWindow>();
                    services.AddTransient<GameWindow>();
                    services.AddTransient<GameViewModel>();
                })
                .Build();

            var signInWindow = _host.Services.GetRequiredService<SignInWindow>();
            signInWindow.Show();
        }
    }
}
