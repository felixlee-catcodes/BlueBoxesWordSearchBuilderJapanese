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
    #region TokenizeKana
    /// <summary>
    /// takes in hiragana word [とうきょう] and outputs -> ["と","う","きょ","う"]
    /// </summary>
    /// <param name="word"></param>
    /// <param name="units"></param>
    /// <returns></returns>
    public static List<string> TokenizeKana(string word, List<KanaUnit> units)
    {
        var result = new List<string>();

        var kanaLookup = units
        .Select(unit => unit.Hiragana)
        .ToHashSet();

        int i = 0;

        while (i < word.Length)
        {
            string? match = null;

            for (int length = 3; length >= 1; length--)
            {
                if (i + length > word.Length)
                    continue;

                string candidate = word.Substring(i, length);

                if (kanaLookup.Contains(candidate))
                {
                    match = candidate;
                    break;
                }
            }

            if (match == null)
            {
                Console.WriteLine($"No result found for: {word[i].ToString()}");
                result.Add(word[i].ToString());
                i++;
            }
            else
            {
                result.Add(match);
                i += match.Length;
            }
        }
        return result;
    }
    #endregion
    #region SortByComplexityEN
    /// <summary>
    /// Sort words that are most likely to allow other words to be placed
    /// Longer words score more as harder to place and so do words with more common letters
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IList<string> SortByComplexity(this IList<string> items)
    {
        return items.OrderByDescending(word => word.Sum(ScoreLetter)).ToList();
    }
    #endregion
    #region SortByComplexityJP
    //SORT BY COMPLEXITY JP VERSION
    public static List<string> SortByComplexityJP(this IList<string> words)
    {
        var scoredWords = new List<(string Word, int Score)>();

        foreach (string word in words)
        {
            //TOKENIZE WORD then feed into ScoreLetter
            var conversions = new KanaConversions();

            List<KanaUnit> kanaLookupList = conversions.GetKanaFrequencyList();

            var tokenizedWord = TokenizeKana(word, kanaLookupList);
            int wordScore = 0;
            var chars = word.Split("");
            foreach (string kana in tokenizedWord)
            {
                Console.WriteLine($"current char: {kana}");
                int kanaScore = ScoreLetterJP(kana);
                wordScore += kanaScore;
            }
            Console.WriteLine($"Word: {word}\tScore: {wordScore}\n");
            scoredWords.Add((word, wordScore));
        }

        return scoredWords
        .OrderByDescending(item => item.Score)
        .Select(item => item.Word)
        .ToList();
    }
    #endregion
    #region ScoreLetterEN
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
    #endregion
    #region ScoreLetterJP
    private static int ScoreLetterJP(string letter)
    {
        var conversions = new KanaConversions();
        Console.WriteLine($"current char: {letter.ToString()}");
        List<KanaUnit> kanaLookupList = conversions.GetKanaFrequencyList();
        var mojiChars = CharactersToString(kanaLookupList).ToArray();
        var mojiWeights = CharWeights(kanaLookupList);

        int index = Array.IndexOf(mojiChars, letter.ToString());
        Console.WriteLine($"char weight: {mojiWeights[index]}");
        return index >= 0 && index < mojiWeights.Length ? mojiWeights[index] : 0;
    }
    #endregion
    #region Helpers
    //just doing hiragana hard-coded for now, will add katakana/dynamic input later
    private static string[] CharactersToString(List<KanaUnit> units)
    {
        return units
        .Select(unit => unit.Hiragana)
        .ToArray();
    }

    private static int[] CharWeights(List<KanaUnit> units)
    {
        return units
        .Select(unit => unit.Frequency)
        .ToArray();

    }
    #endregion
}
