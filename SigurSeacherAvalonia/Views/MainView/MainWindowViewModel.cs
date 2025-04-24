using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SigurSeacherAvalonia.Models;
using SigurSeacherAvalonia.Models.Filters;
using SigurSeacherAvalonia.Services;
using System;
using System.Collections.Generic;

namespace SigurSeacherAvalonia.Views.MainView;

public sealed partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private IEnumerable<CarPass> _cars = [];
    [ObservableProperty]
    private string _carNumber = string.Empty;
    [ObservableProperty]
    private bool _isExpired = false;

    internal event Action<string> SearchFailed;
    internal event Action BeforeSearch;
    internal event Action AfterSearch;


    public MainWindowViewModel() {}


    [RelayCommand]
    internal void Search ()
    {
        BeforeSearch?.Invoke ();

        DataFilter filter = new (CarNumber, IsExpired);

        if ( !filter.IsValid )
        {
            Clear ();

            return;
        }

        bool isSuccess = DataService.TryGetCars (filter, out string error, out List<CarPass> carPasses);
        Cars = carPasses;
        Clear ();

        if ( !isSuccess )
        {
            SearchFailed?.Invoke (error);
        }
    }


    [RelayCommand]
    internal void Clear ()
    {
        CarNumber = string.Empty;
        AfterSearch?.Invoke ();
    }
}


