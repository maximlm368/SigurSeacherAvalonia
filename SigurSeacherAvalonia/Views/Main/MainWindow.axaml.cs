using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.VisualTree;
using SigurSeacherAvalonia.Models.Filters;
using SigurSeacherAvalonia.Views.ErrorMessage;
using SigurSeacherAvalonia.Views.Loading;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SigurSeacherAvalonia.Views.Main;

public partial class MainWindow : Window
{
    public static string OsName = "Windows";

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
            SearchButton.FocusAdorner = null;
            ClearButton.FocusAdorner = null;

            //AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer (SearchButton);

            //foreach ( var children in adornerLayer.GetVisualChildren () )
            //{
            //    ContentPresenter contentPresenter = ( ContentPresenter ) children;
            //    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer (contentPresenter);
            //    contentPresenter.BorderBrush = new SolidColorBrush (new Color (255, 200, 0, 0));

            //    if ( adornerLayer != null )
            //    {
            //        adornerLayer.Background = new SolidColorBrush (new Color (255, 200, 0, 0));
            //        Border adornedElement = new Border ();
            //        adornerLayer.Children.Add (adornedElement);
            //        AdornerLayer.SetAdornedElement (adornedElement, SearchButton);
            //    }
            //}

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

        DataFilter filter = new DataFilter ()
        {
            Name = DataFilterTextBox.Text.Trim (),
            HasExpired = ExpiredCheckBox.IsChecked.Value
        };

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
        SearchButton.BorderThickness = new Avalonia.Thickness (1);
    }


    private void InputGotFocus ( object sender, GotFocusEventArgs e )
    {
        SearchButton.BorderBrush = new SolidColorBrush (new Color (255, 0, 120, 215));
        SearchButton.BorderThickness = new Avalonia.Thickness (2);

        if ( OsName == "Windows" ) 
        {
            SetKeyBoardToENG ();

            return;
        }

        if ( OsName == "Linux" )
        {
            return;
        }
    }


    private void SetKeyBoardToENG ( )
    {
        string lang = "00000409";
        int ret = LoadKeyboardLayout (lang, 1);
        PostMessage (GetForegroundWindow (), 0x50, 1, ret);
    }


    [DllImport ("user32.dll")]
    public static extern IntPtr GetForegroundWindow ();

    [DllImport ("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool PostMessage ( IntPtr hWnd, int Msg, int wParam, int lParam );

    [DllImport ("user32.dll")]
    static extern int LoadKeyboardLayout ( string pwszKLID, uint Flags );
}