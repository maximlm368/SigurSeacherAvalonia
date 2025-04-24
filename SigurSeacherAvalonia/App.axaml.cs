using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using SigurSeacherAvalonia.Views.MainView;
using System.Linq;
using SigurSeacherAvalonia.Views.AuthView;

namespace SigurSeacherAvalonia
{
    public sealed partial class SigurSeacherApp : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                DisableAvaloniaDataAnnotationValidation ();

                AuthWindow auth = new ();

                auth.Closed += ( s, e ) =>
                {
                    if ( auth.IsAccepted )
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

                //desktop.MainWindow = new MainWindow (new MainWindowViewModel ());
            }
            
            base.OnFrameworkInitializationCompleted();
        }

        private static void DisableAvaloniaDataAnnotationValidation()
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