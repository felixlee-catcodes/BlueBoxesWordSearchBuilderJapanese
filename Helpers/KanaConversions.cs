using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace WordSearchGenJapanese.Helpers;

public class KanaConversions
{
    //summary: reads data from json into models
    //returns lists of kana and kana frequency

    private readonly string PATH = "/Users/felix/Desktop/WordSearchGenJapanese/LanguageData";

    private const string HIRAGANA = "hiragana.json";
    private const string KATAKANA = "katakana.json";
    private const string KANAFREQ = "KanaFrequency.json";

    private List<KanaObj> hiraganaList = new List<KanaObj>();
    private List<KanaObj> katakanaList = new List<KanaObj>();
    public static async void TestConversion()
    {
        var kanaConversions = new KanaConversions();
        var hiraganaList = kanaConversions.GetKanaObjList(ListType.Hiragana);
        var katakanaList = kanaConversions.GetKanaObjList(ListType.Katakana);
        Console.WriteLine($"test result: {hiraganaList.Result}");
        Console.WriteLine($"test result: {katakanaList.Result}");
    }

    // public static List<KanaUnit> GetKanaList()
    // {
    //     List<KanaUnit> KanaUnitList = new();
    //     return KanaUnitList;
    // }

    private List<KanaUnit> MergeLists()
    {
        //TODO: merge lists into a single
        throw new NotImplementedException();
    }

    private async Task<List<KanaObj>> GetKanaObjList(ListType listType)
    {
        List<KanaObj> deserializedList = new();
        switch (listType)
        {
            case ListType.Hiragana:
                deserializedList = await DeserializeJson(HIRAGANA);
                break;
            case ListType.Katakana:
                deserializedList = await DeserializeJson(KATAKANA);
                break;
        }
        return deserializedList;

    }

    private async Task<List<KanaObj>> DeserializeJson(string extension)
    {
        string json = await File.ReadAllTextAsync(Path.Combine(PATH, extension));

        return JsonConvert.DeserializeObject<List<KanaObj>>(json) ?? new List<KanaObj>();
    }
}


public class KanaObjList
{
    public List<KanaObj> KanaObjs { get; set; } = new();
}

public class KanaObj
{
    [JsonProperty("char_id")]
    public required string CharId { get; set; }
    public required string Character { get; set; }
    public required string Romanization { get; set; }
}

public class KanaUnit
{
    public required string Kana { get; set; }
    public required string Katakana { get; set; }
    public required string Romaji { get; set; }
    public int? Frequency { get; set; }
}
