using System;
using System.Collections.Generic;
using System.Linq;

namespace SigurSeacherAvalonia.Models.Filters;

internal sealed class DataFilter
{
    private static readonly Dictionary<char, char> _glyphMap = new () 
    {
        {'e','е'}, {'t','т'}, {'y','у'}, {'o','о'}, {'p','р'}, {'a','а'}, {'h','н'}, {'k','к'}, {'x','х'}, {'c','с'}, {'b','в'}, {'m','м'},
        {'е','e'}, {'т','t'}, {'у','y'}, {'о','o'}, {'р','p'}, {'а','a'}, {'н','h'}, {'к','k'}, {'х','x'}, {'с','c'}, {'в','b'}, {'м','m'}
    };
    private static readonly HashSet<char> _screenables = new () { '.', '*', '+', '?', '[', ']', '(', ')', '{', '}', '|' };
    private static readonly char [] _ignorables = { '\'', '\\' };
    private static readonly char [] _glyphs = new char [24];

    public string CarNumber { get; init; } = string.Empty;
    public bool HasExpired { get; init; } = false;
    public bool IsValid => ! string.IsNullOrWhiteSpace(CarNumber);


    internal DataFilter ( string carNumber, bool hasExpired ) 
    {
        CarNumber = GetRegexp ( carNumber );
        HasExpired = hasExpired;
    }


    private static string GetRegexp ( string carNumber )
    {
        byte position = 0;

        foreach ( char glyph in carNumber )
        {
            char low = char.ToLowerInvariant (glyph);

            if ( _ignorables.Contains (glyph) ) continue;

            if ( _glyphMap.ContainsKey (low) )
            {
                _glyphs [position++] = '[';
                _glyphs [position++] = low;
                _glyphs [position++] = _glyphMap [low];
                _glyphs [position++] = ']';
            }
            else if ( _screenables.Contains (glyph) )
            {
                _glyphs [position++] = '\\';
                _glyphs [position++] = '\\';
                _glyphs [position++] = glyph;
            }
            else
            {
                _glyphs [position++] = glyph;
            }
        }

        return new (_glyphs.AsSpan (0, position));
    }
}