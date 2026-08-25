using BlueBoxes.WordSearchBuilder;
using BlueBoxes.WordSearchBuilder.WordPlacers;

const int GRID_SIZE = 7;
var builder = new WordSearchBuilder(GRID_SIZE, GRID_SIZE);

builder
.WithDifficulty(Difficulty.Easy)
.WithWords("apple", "orange", "melon");

var newPuzzle = builder.Build();
Console.WriteLine(newPuzzle.Puzzle);

Console.WriteLine("new puzzle created");