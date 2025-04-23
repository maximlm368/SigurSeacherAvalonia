using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using SigurSeacherAvalonia.Configurations;

namespace SigurSeacherAvalonia.Views.Auth;

public sealed partial class AuthWindow : Window
{
    public bool UserIsAccepted { get; private set; }

    public AuthWindow()
    {
        InitializeComponent();

        Loaded += ( s, a ) => { PasswordTextBox.Focus (NavigationMethod.Tab); };
    }


    private void AuthClicked ( object sender, RoutedEventArgs e )
    {
        if ( PasswordTextBox.Text == Configuration.Instance.Password )
        {
            UserIsAccepted = true;
            Close ();
        }
        else
        {
            UserIsAccepted = false;
            PasswordErrorLabel.IsVisible = true;
            PasswordTextBox.Clear ();
            PasswordTextBox.Focus ();
        }
    }
}