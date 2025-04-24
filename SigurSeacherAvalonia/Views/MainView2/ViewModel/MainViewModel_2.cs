using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using SigurSeacherAvalonia.Models.Filters;
using SigurSeacherAvalonia.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace SigurSeacherAvalonia.Views.Main.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private bool _sortIsIncreasing;
    private Dictionary<CarComparationType, Action> _carSorters;

    public ObservableCollection<Car_2> Cars { get; private set; }
    public event PropertyChangedEventHandler? PropertyChanged;


    public MainViewModel ()
    {
        Cars = new ();

        _carSorters = new Dictionary<CarComparationType, Action>
        {
            { CarComparationType.ByNumber, SortByNumber },
            { CarComparationType.ByModel, SortByModel },
            { CarComparationType.ByExpirationDate, SortByExpirationDate },
            { CarComparationType.ByOwner, SortByOwner },
            { CarComparationType.ByComment, SortByComment },
        };
    }


    protected void OnPropertyChanged ( string propName )
    {
        PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propName));
    }


    internal void SearchCarsBy ( DataFilter filter )
    {
        Cars.Clear ();
        //DataTable table = _service.GetTable (filter);

        //foreach ( var row in table.Rows )
        //{
        //    DataRow carRow = ( DataRow ) row;

        //    DateTime dateTime = ( carRow [2] is DateTime ) ? ( DateTime ) carRow [2] : DateTime.MinValue;
        //    DateOnly date = new DateOnly (dateTime.Year, dateTime.Month, dateTime.Day);
        //    DateOnly nowDate = new DateOnly (DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        //    SolidColorBrush dateBackground = new SolidColorBrush (new Color (255, 255, 255, 206));

        //    if ( (date < nowDate) && (date != DateOnly.MinValue) ) 
        //        dateBackground = new SolidColorBrush (new Color (255, 255, 206, 206));

        //    Car car = new Car
        //                     (
        //                       carRow [0].ToString ()
        //                     , carRow [1].ToString ()
        //                     , date
        //                     , carRow [3].ToString ()
        //                     , carRow [4].ToString ()
        //                     , dateBackground
        //                     );

        //    Cars.Add (car);
        //}

        OnPropertyChanged (nameof (Cars));
    }


    internal void SortCars ( CarComparationType comparationType, bool sortIsIncreasing )
    {
        _sortIsIncreasing = sortIsIncreasing;
        _carSorters [comparationType] ();
    }


    private void SortByNumber ()
    {
        Car_2.ComparationType = CarComparationType.ByNumber;
        SortCars ();
    }


    private void SortByModel ()
    {
        Car_2.ComparationType = CarComparationType.ByModel;
        SortCars ();
    }


    private void SortByExpirationDate ()
    {
        Car_2.ComparationType = CarComparationType.ByExpirationDate;
        SortCars ();
    }


    private void SortByOwner ()
    {
        Car_2.ComparationType = CarComparationType.ByOwner;
        SortCars ();
    }


    private void SortByComment ()
    {
        Car_2.ComparationType = CarComparationType.ByComment;
        SortCars ();
    }


    private void SortCars ()
    {
        List<Car_2> carList = Cars.ToList ();
        carList.Sort ();

        if ( ! _sortIsIncreasing ) carList.Reverse ();

        Cars.Clear ();

        foreach ( Car_2 car in carList ) 
        {
            Cars.Add ( car );
        }

        OnPropertyChanged (nameof (Cars));
    }
}



public class Car_2 : IComparable
{
    public static CarComparationType ComparationType;

    private DateOnly _expirationDate;

    public string CarNumber { get; private set; }
    public string Model { get; private set; }
    public string ExpirationDate { get; private set; }
    public string Owner { get; private set; }
    public string Comments { get; private set; }
    public SolidColorBrush Background { get; private set; }


    public Car_2 ( string carNumber, string model, DateOnly expirationDate, string owner, string comments, SolidColorBrush background )
    {
        _expirationDate = expirationDate;

        CarNumber = carNumber;
        Model = model;
        ExpirationDate = (expirationDate == DateOnly.MinValue) 
                         ? ExpirationDate = string.Empty 
                         : ExpirationDate = expirationDate.ToString ();
        Owner = owner;
        Comments = comments;
        Background = background;
    }


    public int CompareTo ( object? obj )
    {
        if (ComparationType == CarComparationType.ByNumber) 
        {
            return CarNumber.CompareTo (( ( Car_2 ) obj ).CarNumber);
        }

        if ( ComparationType == CarComparationType.ByModel )
        {
            return Model.CompareTo (( ( Car_2 ) obj ).Model);
        }

        if ( ComparationType == CarComparationType.ByExpirationDate )
        {
            return _expirationDate.CompareTo (( ( Car_2 ) obj )._expirationDate);
        }

        if ( ComparationType == CarComparationType.ByOwner )
        {
            return Owner.CompareTo (( ( Car_2 ) obj ).Owner);
        }

        if ( ComparationType == CarComparationType.ByComment )
        {
            return Comments.CompareTo (( ( Car_2 ) obj ).Comments);
        }

        return 0;
    }
}



public enum CarComparationType 
{
    ByNumber = 0,
    ByModel = 1,
    ByExpirationDate = 2, 
    ByOwner = 3,
    ByComment = 4,
}