using System;

namespace SigurSeacherAvalonia.Models;

public sealed record CarPass
{
    public string CarNumber { get; private set; }
    public string Model { get; private set; }
    public DateOnly? ExpirationDate { get; private set; }
    public string Owner { get; private set; }
    public string Comments { get; private set; }
    public bool IsExpired { get; private set; } 


    public CarPass ( string carNumber, string model, DateOnly? expirationDate, string owner, string comments )
    {
        CarNumber = carNumber;
        Model = model;
        ExpirationDate = expirationDate;
        Owner = owner;
        Comments = comments;
        IsExpired = ( ( ExpirationDate != null ) && ( ExpirationDate < DateOnly.FromDateTime (DateTime.Now) ) );
    }
}