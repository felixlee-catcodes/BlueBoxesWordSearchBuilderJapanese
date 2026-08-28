using BlueBoxes.WordSearchBuilder.Helpers;
using BlueBoxes.WordSearchBuilder.WordPlacers;
using WordSearchGenJapanese.Helpers;

namespace BlueBoxes.WordSearchBuilder.SpaceFillers;

/// <summary>
/// Fills the grid with letters where it finds empty cells
/// </summary>
public class JapaneseSpaceFiller : ISpaceFiller
{
    public virtual char[][] FillSpacesInGrid(char[][] grid)
    {
        for (int col = 0; col < grid.Width(); col++)
        {
            for (int row = 0; row < grid.Height(); row++)
            {
                if (grid[col][row] == WordPlacer.NullChar)
                {
                    grid[col][row] = GetWeightedRandomLetter().ToCharArray()[0];
                }
            }
        }

        return grid;
    }

    /// <summary>
    /// Gets random letter from the alphabet weighted by the frequency of the letter in the English language
    /// https://en.wikipedia.org/wiki/Letter_frequency
    /// </summary>
    /// <returns>Random Letter</returns>
    protected string GetWeightedRandomLetter()
    {
        var conversions = new KanaConversions();
        List<KanaUnit> kanaLookupList = conversions.GetKanaFrequencyList();

        var rnd = new Random();

        var mojiChars = kanaLookupList
        .Select(unit => unit.Hiragana)
        .ToArray();
        var weights = kanaLookupList
        .Select(unit => unit.Frequency)
        .ToArray();

        var total = weights.Sum();

        var r = rnd.Next(0, total);
        var sum = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            sum += weights[i];
            if (r < sum)
            {
                return mojiChars[i];
            }
        }
        return mojiChars[0];
    }

}

