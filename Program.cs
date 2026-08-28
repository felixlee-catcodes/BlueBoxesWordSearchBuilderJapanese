using System.Data;
using BlueBoxes.WordSearchBuilder;
using BlueBoxes.WordSearchBuilder.SpaceFillers;
using BlueBoxes.WordSearchBuilder.WordPlacers;
using WordSearchGenJapanese.Helpers;

const int GRID_SIZE = 6;
var builder = new WordSearchBuilder(GRID_SIZE, GRID_SIZE);

builder
.WithSpaceFiller(new JapaneseSpaceFiller())
.WithDifficulty(Difficulty.Hard)
.WithWords("きっぷ", "やった", "みます", "きっと", "はしる", "たまご", "れいぞうこ", "まど", "さかな");

Console.WriteLine("new puzzle created");
var newPuzzle = builder.Build();
foreach (var row in newPuzzle.Puzzle)
{
    foreach (var col in row)
    {
        Console.Write(col.ToString());
    }
    Console.WriteLine();
}
Console.WriteLine($"num words in puz: {newPuzzle.Solution.Count}");

Console.WriteLine("program ended");


