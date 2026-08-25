using BlueBoxes.WordSearchBuilder;
using BlueBoxes.WordSearchBuilder.WordPlacers;

const int GRID_SIZE = 7;
var builder = new WordSearchBuilder(GRID_SIZE, GRID_SIZE);

builder
.WithWords("apple", "orange", "melon")
.WithDifficulty(Difficulty.Easy);

var newPuzzle = builder.Build();

Console.WriteLine("new puzzle created");