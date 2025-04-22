using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using SigurSeacherAvalonia.Models;
using SigurSeacherAvalonia.Models.Filters;
using SigurSeacherAvalonia.Services;
using System.Collections.Generic;

namespace SigurSeacherAvalonia.Views.Main;

public sealed partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private IEnumerable<CarPass> _cars = [];


    public MainWindowViewModel() {}


    internal string SearchCars (DataFilter filter)
    {
        bool isSuccess = DataService.TryGetCars (filter, out string error, out List<CarPass> carPasses);

        if ( !isSuccess )
        {
            return error;
        }

        Dispatcher.UIThread.Invoke
            (
                () => Cars = ( carPasses.Count > 0 ) ? carPasses : []
            );

        return string.Empty;
    }
}


