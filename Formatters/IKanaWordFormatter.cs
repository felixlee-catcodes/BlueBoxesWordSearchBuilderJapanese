namespace BlueBoxes.WordSearchBuilder.Formatters
{
    /// <summary>
    /// Formats the input word to fit into the grid removing charactors that are no accepted
    /// </summary>
    public interface IKanaWordFormatter
    {
        List<string> FormatWord(string inputWord);
    }
}