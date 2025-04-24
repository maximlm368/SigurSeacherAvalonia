using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using SigurSeacherAvalonia.Configurations;

namespace SigurSeacherAvalonia.Views.AuthView;

public sealed partial class AuthWindow : Window
{
    public bool IsAccepted { get; private set; }

    public AuthWindow()
    {
        InitializeComponent();

        Loaded += ( s, a ) => { PasswordTextBox.Focus (NavigationMethod.Tab); };
    }


    private void AuthClicked ( object sender, RoutedEventArgs e )
    {
        if ( PasswordTextBox.Text == Configuration.Instance.Password )
        {
            IsAccepted = true;
            Close ();
        }
        else
        {
            IsAccepted = false;
            ErrorLabel.IsVisible = true;
            PasswordTextBox.Clear ();
            PasswordTextBox.Focus ();
        }
    }
}