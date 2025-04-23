using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using SigurSeacherAvalonia.Models.Filters;
using SigurSeacherAvalonia.Views.ErrorMessage;
using SigurSeacherAvalonia.Views.Loading;
using System.Threading.Tasks;

namespace SigurSeacherAvalonia.Views.Main;

public sealed partial class MainWindow : Window
{
    private MainWindowViewModel _viewModel;


    

    public MainWindow ()
    {
        InitializeComponent ();
    }


    public MainWindow ( MainWindowViewModel viewModel ) : this () 
    {
       _viewModel = viewModel;
        DataContext = _viewModel;
        DataFilterTextBox.Text = string.Empty;

        Loaded += ( s, a ) => 
        {
            foreach ( Visual descendant in ExpiredCheckBox.GetVisualDescendants () ) 
            {
                if ( descendant.Name == "CheckGlyph" ) 
                {
                    Path checkGlyph = descendant as Path;
                    checkGlyph.Fill = new SolidColorBrush (new Color (255, 0, 120, 25));
                }  
            }

            DataFilterTextBox.Focus (NavigationMethod.Tab); 
        };


    }


    private void SearchButtonClicked ( object sender, RoutedEventArgs args )
    {
        Search ();
    }


    private void SearchViaEnterButton ( object sender, KeyEventArgs args )
    {
        if ( args.Key != Key.Enter ) return;

        Search ();
    }


    private void Search ( )
    {
        if ( FilterValidate () )
        {
            DataFilterTextBox.Clear ();
            DataFilterTextBox.Focus (NavigationMethod.Tab, KeyModifiers.None);

            return;
        }

        string error = string.Empty;

        LoadingWindow loadingModalWindow = new ();
        loadingModalWindow.ShowDialog (this);

        string carNum = DataFilterTextBox.Text.Trim ();
        bool hasExpired = ExpiredCheckBox.IsChecked.Value;

        DataFilter filter = new DataFilter (carNum, hasExpired);

        Task task = new Task (() =>
        {
            error = _viewModel.SearchCars (filter);
        });

        task.Start ();

        task.ContinueWith (tsk =>
        {
            Dispatcher.UIThread.Invoke
            (() =>
            {
                loadingModalWindow.Close ();
                DataFilterTextBox.Clear ();
                DataFilterTextBox.Focus (NavigationMethod.Tab, KeyModifiers.None);

                if ( !string.IsNullOrWhiteSpace (error) )
                {
                    MessageWindow dialog = new (error);
                    dialog.ShowDialog (this);
                }
            });
        });
    }


    private bool FilterValidate ()
    {
        string input = DataFilterTextBox?.Text.Trim ();

        return string.IsNullOrWhiteSpace (input);
    }


    private void ClearButtonClicked ( object sender, RoutedEventArgs e )
    {
        DataFilterTextBox.Clear ();
        DataFilterTextBox.Focus (NavigationMethod.Tab);
    }


    private void InputLostFocus ( object sender, RoutedEventArgs args )
    {
        SearchButton.BorderBrush = new SolidColorBrush (new Color (255, 150, 150, 150));
        SearchButton.BorderThickness = new Thickness (1);
    }


    private void InputGotFocus ( object sender, GotFocusEventArgs e )
    {
        SearchButton.BorderBrush = new SolidColorBrush (new Color (255, 0, 120, 215));
        SearchButton.BorderThickness = new Thickness (2);
    }
}