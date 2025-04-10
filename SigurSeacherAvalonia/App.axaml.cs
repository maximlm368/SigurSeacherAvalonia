using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using SigurSeacherAvalonia.Views.Auth;
using SigurSeacherAvalonia.Views.Main;
using System;
using System.Linq;

namespace SigurSeacherAvalonia
{
    public partial class SigurSeacherApp : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                MainWindow.OsName = OperatingSystem.IsWindows () ? "Windows" : "Linux";

                DisableAvaloniaDataAnnotationValidation ();

                AuthWindow auth = new ();

                auth.Closed += ( s, e ) =>
                {
                    if ( auth.UserIsAccepted )
                    {
                        desktop.MainWindow = new MainWindow (new MainWindowViewModel ());
                        desktop.MainWindow.Show ();
                    }
                    else
                    {
                        desktop.Shutdown ();
                    }
                };

                desktop.MainWindow = auth;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}