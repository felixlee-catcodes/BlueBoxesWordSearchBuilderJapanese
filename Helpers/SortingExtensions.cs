using System.Reflection.PortableExecutable;
using WordSearchGenJapanese.Helpers;

namespace BlueBoxes.WordSearchBuilder.Helpers;

static class SortingExtensions
{
    private readonly static Random rnd = new Random();

    public static IList<T> Shuffle<T>(this IList<T> items)
    {
        return items.OrderBy(x => rnd.Next(0, 10)).ToList();
    }

    /// <summary>
    /// Sort words that are most likely to allow other words to be placed
    /// Longer words score more as harder to place and so do words with more common letters
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IList<string> SortByComplexity(this IList<string> items)
    {
        return items.OrderByDescending(word => word.Sum(ScoreLetterJP)).ToList();
    }

    //SORT BY COMPLEXITY JP VERSION
    // public static async Task<IList<string>> SortByComplexityJP(this IList<string> words)
    // {
    //     var scoredWords = new List<(string Word, int Score)>();

    //     foreach (string word in words)
    //     {
    //         int score = 0;

    //         foreach (char kana in word)
    //         {
    //             score += ScoreLetterJP(kana);
    //         }
    //         scoredWords.Add((word, score));
    //     }

    //     return scoredWords
    //     .OrderByDescending(item => item.Score)
    //     .Select(item => item.Word)
    //     .ToList();
    // }

    //!!!NEEDS ADAPTATION FOR HIRAGANA & KATAKANA 
    //Pull in LIST OF CHARS/SYMBOLS FROM JSON FILES??
    private static int ScoreLetter(char letter)
    {
        //This method captures the longest words but also the words with most vowels
        var letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        var weights = new int[] { 8, 2, 3, 4, 13, 2, 2, 6, 7, 1, 1, 4, 2, 7, 8, 2, 1, 6, 6, 9, 3, 1, 2, 1, 2, 1 };

        letter = char.ToUpper(letter);
        int index = Array.IndexOf(letters, letter);

        return index >= 0 && index < weights.Length ? weights[index] : 0;

    }

    private static int ScoreLetterJP(char letter)
    {
        List<KanaUnit> kanaLookupList = KanaConversions.GetKanaFrequencyList();
        var mojiChars = CharactersToString(kanaLookupList).ToCharArray();
        var mojiWeights = CharWeights(kanaLookupList);

        int index = Array.IndexOf(mojiChars, letter);

        return index >= 0 && index < mojiWeights.Length ? mojiWeights[index] : 0;
    }

    //just doing hiragana hard-coded for now, will add katakana/dynamic input later
    private static string CharactersToString(List<KanaUnit> units)
    {
        string charString = "";
        foreach (KanaUnit unit in units)
        {
            charString += unit.Hiragana;
        }
        return charString;
    }

    private static int[] CharWeights(List<KanaUnit> units)
    {
        var weightList = new int[] { };
        foreach (KanaUnit unit in units)
        {
            weightList.Append<int>(unit.Frequency);
        }
        return weightList;
    }
}
