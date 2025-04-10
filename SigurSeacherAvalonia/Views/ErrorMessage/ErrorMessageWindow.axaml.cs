using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace SigurSeacherAvalonia.Views.ErrorMessage;

public sealed partial class MessageWindow : Window
{
    public MessageWindow ()
    {
        InitializeComponent ();
    }


    public MessageWindow ( string message ) : this ()
    {
        messageText.Text = message;
        ok.FocusAdorner = null;
        Activated += ( s, a ) => ok.Focus (NavigationMethod.Tab, KeyModifiers.None);
    }


    private void OkClicked ( object sender, RoutedEventArgs args ) 
    {
        Close ();
    }
}