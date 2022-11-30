namespace Neo4jJsonToCSharpClasses;

using System.Text.RegularExpressions;

public static class StringExtensions
{
    private static readonly Regex UpperCaseSplitRegex = 
        new("((?<=\\p{Ll})\\p{Lu}|\\p{Lu}(?=\\p{Ll}))", 
            RegexOptions.Compiled | RegexOptions.CultureInvariant
            );

    private static string[] TwoLetterWordsToCaseCorrectly = { "in", "as", "on", "is" };

    public static string ToUpperCamelCase(this string value)
    {
        if(string.IsNullOrWhiteSpace(value)) return value;

        var str = UpperCaseSplitRegex.Replace(value, " $1");
        var split = str.Split("_ ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        split = split.Select(x => x.Trim()).ToArray();

        for (var i = 0; i < split.Length; i++)
        {
            if (split[i].Length <= 2)
            {
                if(!TwoLetterWordsToCaseCorrectly.Contains(split[i].ToLowerInvariant()))
                    continue;
            }
            split[i] = $"{char.ToUpperInvariant(split[i][0])}{split[i].ToLowerInvariant()[1..]}";
        }

        return string.Join(string.Empty, split);
    }
}