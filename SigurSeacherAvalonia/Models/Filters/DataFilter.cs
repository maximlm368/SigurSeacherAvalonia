namespace SigurSeacherAvalonia.Models.Filters;

internal sealed class DataFilter
{
    public string Name { get; set; } = string.Empty;
    public bool HasExpired { get; set; } = false;
}