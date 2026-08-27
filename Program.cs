using BlueBoxes.WordSearchBuilder;
using BlueBoxes.WordSearchBuilder.WordPlacers;
using WordSearchGenJapanese.Helpers;

const int GRID_SIZE = 6;
// var builder = new WordSearchBuilder(GRID_SIZE, GRID_SIZE);

// builder
// .WithDifficulty(Difficulty.Easy)
// .WithWords("pea", "plum", "mint", "fig");

// var newPuzzle = builder.Build();
// Console.WriteLine(newPuzzle.Puzzle);

// Console.WriteLine("new puzzle created");

// Console.WriteLine("start running project...");
// var conversions = new KanaConversions();
// await conversions.TestConversion();
// Console.WriteLine("...program ended");

var conversions = new KanaConversions();
List<KanaUnit> kanaList2 = conversions.GetKanaFrequencyList();



var builder = new WordSearchBuilder(GRID_SIZE, GRID_SIZE);
builder.WithWords("にほん", "ねこ", "たまご");


Console.WriteLine("program ended");


