using Avalonia.Controls;
using Avalonia;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using SigurSeacherAvalonia.Models.Filters;
using SigurSeacherAvalonia.Views.ErrorMessage;
using SigurSeacherAvalonia.Views.Loading;
using SigurSeacherAvalonia.Views.Main.ViewModel;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace SigurSeacherAvalonia.Views.Main;

public partial class MainWindow_2 : Window
{
    private MainViewModel _viewModel;
    private bool _isSearchStarted;
    private bool _byNumberSortingIsIncreasing;
    private bool _byModelSortingIsIncreasing;
    private bool _byEndDateSortingIsIncreasing;
    private bool _byOwnerSortingIsIncreasing;
    private bool _byCommentSortingIsIncreasing;
    private string _increasingDirectionMark = "\uF077";
    private string _decreasingDirectionMark = "\uF078";


    public MainWindow_2()
    {
        InitializeComponent ();
    }


    public MainWindow_2 ( MainViewModel viewModel ) : this ()
    {
        _viewModel = viewModel;
        DataContext = _viewModel;
        DataFilterTextBox.Text = string.Empty;

        Loaded += ( s, a ) => { DataFilterTextBox.Focus (NavigationMethod.Tab); };
        SizeChanged += SizeChangedHandler;

        Cars.SizeChanged += (s,a)=>
        {
            int counter = 0;

            foreach ( var column in Headers.ColumnDefinitions )
            {
                foreach ( var carLine in Cars.GetLogicalChildren () )
                {
                    double width = column.ActualWidth;
                    Grid line = ( ( ContentPresenter ) carLine ).Child as Grid;
                    if ( ( width > 15 ) && ( counter == ( Headers.ColumnDefinitions.Count - 2 ) ) ) width -= 16;
                    line.ColumnDefinitions [counter].Width = new GridLength (width);
                }

                counter++;
            }

            double y = Scroller.ScrollBarMaximum.Y;
        };

        //LayoutUpdated += (s,a) => 
        //{
        //    if (_isSearchStarted) 
        //    {
        //        if ( Scroller.ScrollBarMaximum.Y == 0 )
        //        {
        //            Scroller.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
        //        }
        //        else
        //        {
        //            Scroller.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
        //        }

        //        _isSearchStarted = false;
        //    }
        //};
    }


    private void SizeChangedHandler ( object sender, SizeChangedEventArgs args ) 
    {
        int counter = 0;

        foreach ( var column in Headers.ColumnDefinitions )
        {
            foreach ( var carLine in Cars.GetLogicalChildren () )
            {
                double width = column.ActualWidth;
                Grid line = ( ( ContentPresenter ) carLine ).Child as Grid;
                if ( (width > 15) && (counter == ( Headers.ColumnDefinitions.Count - 2 )) ) width -= 16;
                line.ColumnDefinitions [counter].Width = new GridLength (width);
            }

            counter++;
        }

        //if ( Scroller.ScrollBarMaximum.Y == 0 )
        //{
        //    Scroller.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
        //}
        //else
        //{
        //    Scroller.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
        //}


        Scroller.Width += ( args.NewSize.Width - Scroller.Width );
    }


    private void SearchButtonClicked ( object sender, RoutedEventArgs args )
    {
        if ( FilterValidate () )
        {
            DataFilterTextBox.Focus ();
            return;
        }

        _isSearchStarted = true;
        LoadingWindow loadingModalWindow = new LoadingWindow ();
        loadingModalWindow.Show ();

        EmptySortDirectionMarks ();

        try
        {
            DataFilter filter = new DataFilter ()
            {
                Name = DataFilterTextBox.Text.Trim (),
                HasExpired = ExpiredCheckBox.IsChecked.Value
            };

            _viewModel.SearchCarsBy (filter);
        }
        catch ( Exception ex )
        {
            MessageWindow errorMessage = new (ex.Message);
            errorMessage.Show ();
            errorMessage.Focus (NavigationMethod.Tab, KeyModifiers.None);
        }

        DataFilterTextBox.Clear ();
        DataFilterTextBox.Focus (NavigationMethod.Tab, KeyModifiers.None);
        loadingModalWindow.Close ();
    }


    private void SearchButtonClickedrr ( object sender, RoutedEventArgs args )
    {
        if ( FilterValidate () )
        {
            DataFilterTextBox.Focus ();
            return;
        }

        _isSearchStarted = true;
        LoadingWindow loadingModalWindow = new LoadingWindow ();
        Task task = loadingModalWindow.ShowDialog (this);

        task.ContinueWith
            (
                t =>
                {
                    Dispatcher.UIThread.InvokeAsync
                    (
                        () =>
                        {

                        }
                    );
                }
            );

        EmptySortDirectionMarks ();

        try
        {
            DataFilter filter = new DataFilter ()
            {
                Name = DataFilterTextBox.Text.Trim (),
                HasExpired = ExpiredCheckBox.IsChecked.Value
            };

            _viewModel.SearchCarsBy (filter);
        }
        catch ( Exception ex )
        {
            //MessageWindow errorMessage = new (ex.Message);
            //errorMessage.ShowDialog (this);
            //errorMessage.Focus (NavigationMethod.Tab, KeyModifiers.None);
        }


        DataFilterTextBox.Clear ();
        DataFilterTextBox.Focus (NavigationMethod.Tab, KeyModifiers.None);
        loadingModalWindow.Close ();
    }


    private bool FilterValidate ()
    {
        return DataFilterTextBox.Text.Trim () == string.Empty;
    }


    private void ClearButtonClicked ( object sender, RoutedEventArgs args )
    {
        DataFilterTextBox.Clear ();
        DataFilterTextBox.Focus (NavigationMethod.Tab);
    }


    private void SortByCarNumber ( object sender, TappedEventArgs args )
    {
        LoadingWindow loadingModalWindow = new LoadingWindow ();
        loadingModalWindow.ShowDialog (this);
        EmptySortDirectionMarks ();
        _byNumberSortingIsIncreasing = !_byNumberSortingIsIncreasing;
        _viewModel.SortCars (CarComparationType.ByNumber, _byNumberSortingIsIncreasing);
        NumberSortMark.Content = _byNumberSortingIsIncreasing ? _increasingDirectionMark : _decreasingDirectionMark;
        loadingModalWindow.Close ();
    }


    private void SortByCarModel ( object sender, TappedEventArgs args )
    {
        LoadingWindow loadingModalWindow = new LoadingWindow ();
        loadingModalWindow.ShowDialog (this);
        EmptySortDirectionMarks ();
        _byModelSortingIsIncreasing = !_byModelSortingIsIncreasing;
        _viewModel.SortCars (CarComparationType.ByModel, _byModelSortingIsIncreasing);
        ModelSortMark.Content = _byModelSortingIsIncreasing ? _increasingDirectionMark : _decreasingDirectionMark;
        loadingModalWindow.Close ();
    }


    private void SortByExpirationDate ( object sender, TappedEventArgs args )
    {
        LoadingWindow loadingModalWindow = new LoadingWindow ();
        loadingModalWindow.ShowDialog (this);
        EmptySortDirectionMarks ();
        _byEndDateSortingIsIncreasing = !_byEndDateSortingIsIncreasing;
        _viewModel.SortCars (CarComparationType.ByExpirationDate, _byEndDateSortingIsIncreasing);
        DateSortMark.Content = _byEndDateSortingIsIncreasing ? _increasingDirectionMark : _decreasingDirectionMark;
        loadingModalWindow.Close ();
    }


    private void SortByOwner ( object sender, TappedEventArgs args )
    {
        LoadingWindow loadingModalWindow = new LoadingWindow ();
        loadingModalWindow.ShowDialog (this);
        EmptySortDirectionMarks ();
        _byOwnerSortingIsIncreasing = !_byOwnerSortingIsIncreasing;
        _viewModel.SortCars (CarComparationType.ByOwner, _byOwnerSortingIsIncreasing);
        OwnerSortMark.Content = _byOwnerSortingIsIncreasing ? _increasingDirectionMark : _decreasingDirectionMark;
        loadingModalWindow.Close ();
    }


    private void SortByCarComment ( object sender, TappedEventArgs args )
    {
        LoadingWindow loadingModalWindow = new LoadingWindow ();
        loadingModalWindow.ShowDialog (this);
        EmptySortDirectionMarks ();
        _byCommentSortingIsIncreasing = !_byCommentSortingIsIncreasing;
        _viewModel.SortCars (CarComparationType.ByComment, _byCommentSortingIsIncreasing);
        CommentSortMark.Content = _byCommentSortingIsIncreasing ? _increasingDirectionMark : _decreasingDirectionMark;
        loadingModalWindow.Close ();
    }


    private void EmptySortDirectionMarks () 
    {
        NumberSortMark.Content = " ";
        ModelSortMark.Content = " ";
        DateSortMark.Content= " ";
        OwnerSortMark.Content = " ";
        CommentSortMark.Content = " ";
    }
}


//if ( Scroller.ScrollBarMaximum.Y == 0 )
//{
//    Scroller.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
//}
//else
//{
//    Scroller.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
//}