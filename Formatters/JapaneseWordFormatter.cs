using System.Text.RegularExpressions;
using BlueBoxes.WordSearchBuilder.Helpers;
using WordSearchGenJapanese.Helpers;

namespace BlueBoxes.WordSearchBuilder.Formatters;

public class JapaneseWordFormatter : IKanaWordFormatter
{
    /// <summary>
    /// Formats the input word to fit into the grid removing spaces, hyphens etc.
    /// </summary>
    /// <param name="inputWord"></param>
    /// <returns></returns>
    public List<string> FormatWord(string inputWord)
    {
        var kanaConversions = new KanaConversions().GetKanaFrequencyList();
        return SortingExtensions.TokenizeKana(inputWord, kanaConversions);

        ;
    }
}
