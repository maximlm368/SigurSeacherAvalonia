using Avalonia.Controls;
using Avalonia.Input;
using SigurSeacherAvalonia.Views.ErrorMessageView;
using SigurSeacherAvalonia.Views.LoadingView;

namespace SigurSeacherAvalonia.Views.MainView;

public sealed partial class MainWindow : Window
{
    private LoadingWindow _loadingModalWindow;

    public MainWindow ()
    {
        InitializeComponent ();
    }


    public MainWindow ( MainWindowViewModel viewModel ) : this () 
    {
        DataContext = viewModel;

        viewModel.BeforeSearch += () => 
        {
            _loadingModalWindow = new ();
            _loadingModalWindow.ShowDialog (this);
        };

        viewModel.AfterSearch += () => 
        {
            _loadingModalWindow?.Close ();
            DataFilterTextBox.Focus (NavigationMethod.Tab, KeyModifiers.None);
        };

        viewModel.SearchFailed += (error) => 
        {
            MessageWindow dialog = new (error);
            dialog.ShowDialog (this);
            dialog.Ok.Focus (NavigationMethod.Tab, KeyModifiers.None);
        };

        Loaded += ( s, a ) => 
        {
            DataFilterTextBox.Focus (NavigationMethod.Tab); 
        };
    }
}