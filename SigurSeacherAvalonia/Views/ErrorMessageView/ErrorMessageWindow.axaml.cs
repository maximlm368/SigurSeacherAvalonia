using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace SigurSeacherAvalonia.Views.ErrorMessageView;

public sealed partial class MessageWindow : Window
{
    public MessageWindow ()
    {
        InitializeComponent ();
    }


    public MessageWindow ( string message ) : this ()
    {
        MessageText.Text = message;
        Ok.FocusAdorner = null;
        Activated += ( s, a ) => Ok.Focus (NavigationMethod.Tab, KeyModifiers.None);
    }


    private void OkClicked ( object sender, RoutedEventArgs args ) 
    {
        Close ();
    }
}