using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SigurSeacherAvalonia.Models.Filters;

internal sealed class DataFilter
{
    private readonly Dictionary<char, char> _latinToCyrillic = new () {{'e','е'}, {'t','т'}, {'y','у'}, {'o','о'}, {'p','р'}, {'a','а'}
                                                                     , {'h','н'}, {'k','к'}, {'x','х'}, {'c','с'}, {'b','в'}, {'m','м'}};
    private readonly Dictionary<char, char> _cyrillicToLatin = new () {{'е','e'}, {'т','t'}, {'у','y'}, {'о','o'}, {'р','p'}, {'а','a'}
                                                                     , {'н','h'}, {'к','k'}, {'х','x'}, {'с','c'}, {'в','b'}, {'м','m'}};

    private readonly char [] _screenables = { '.', '*', '+', '?', '[', ']', '(', ')', '{', '}', '|', '\\' };

    public string CarNumber { get; private set; } = string.Empty;
    public bool HasExpired { get; private set; } = false;


    internal DataFilter ( string carNumber, bool hasExpired ) 
    {
        CarNumber = GetRegexp ( carNumber );
        HasExpired = hasExpired;
    }


    private string GetRegexp ( string carNumber )
    {
        List<char> glyphs = [];
        StringBuilder sb = new StringBuilder ();

        foreach ( char glyph in carNumber )
        {
            if ( _latinToCyrillic.ContainsKey (glyph) )
            {
                sb.Append (new char [] { '[', glyph, _latinToCyrillic [glyph], ']' });
            }
            else if ( _cyrillicToLatin.ContainsKey (glyph) )
            {
                sb.Append (new char [] { '[', glyph, _cyrillicToLatin [glyph], ']' });
            }
            else if ( _screenables.Contains (glyph) )
            {
                sb.Append (new char [] { '\\', '\\', glyph });
            }
            else
            {
                sb.Append (glyph);
            }
        }

        return sb.ToString ();
    }
}