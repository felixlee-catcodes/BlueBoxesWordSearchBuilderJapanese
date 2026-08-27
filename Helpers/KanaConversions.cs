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

    // public async Task TestConversion()
    // {
    //     Console.WriteLine($"TestConversion called...");
    //     var kanaConversions = new KanaConversions();
    //     List<KanaObj> hiraganaList = await kanaConversions.GetKanaObjList(ListType.Hiragana);
    //     List<KanaObj> katakanaList = await kanaConversions.GetKanaObjList(ListType.Katakana);
    //     List<KanaUnit> kanaLookupList = await kanaConversions.MergeKanaAndFrequencyLists();
    // }

    // public async Task<List<KanaUnit>> GetKanaFrequencyList()
    // {
    //     return await MergeKanaAndFrequencyLists();
    // }

    public static async Task<List<KanaUnit>> GetKanaFrequencyList()
    {
        var conversions = new KanaConversions();
        return await conversions.MergeKanaAndFrequencyLists();
    }

    private async Task<List<KanaUnit>> MergeKanaAndFrequencyLists()
    {
        List<FreqUnit> frequencyList = await GetFrequencyList();

        List<KanaUnit> kanaList = await MergeHiraAndKata();

        foreach (KanaUnit unit in kanaList)
        {
            int index = frequencyList.FindIndex(x => x.Kana == unit.Hiragana);

            if (index != -1)
            {
                unit.Frequency = frequencyList[index].Frequency;
            }
            else
            {
                unit.Frequency = 0;
            }

        }
        return kanaList;
    }

    private async Task<List<KanaUnit>> MergeHiraAndKata()
    {
        //get lists by kana/list type
        List<KanaObj> hiraganaList = await GetKanaObjList(ListType.Hiragana);
        List<KanaObj> katakanaList = await GetKanaObjList(ListType.Katakana);

        List<KanaUnit> kanaList = new List<KanaUnit>();

        for (int i = 0; i < hiraganaList.Count; i++)
        {
            int kataIndex = katakanaList.FindIndex(k => k.Romanization == hiraganaList[i].Romanization);
            if (kataIndex != -1)
            {
                var kanaUnit = new KanaUnit
                {
                    Hiragana = hiraganaList[i].Character,
                    Katakana = katakanaList[kataIndex].Character,
                    Romaji = hiraganaList[i].Romanization
                };
                kanaList.Add(kanaUnit);
            }
        }
        return kanaList;
    }

    private async Task<List<KanaObj>> GetKanaObjList(ListType listType)
    {
        List<KanaObj> deserializedList = new();
        switch (listType)
        {
            case ListType.Hiragana:
                deserializedList = await DeserializeKanaJson(HIRAGANA);
                break;
            case ListType.Katakana:
                deserializedList = await DeserializeKanaJson(KATAKANA);
                break;
        }
        return deserializedList;

    }

    private async Task<List<FreqUnit>> GetFrequencyList()
    {
        return await DeserializeFreqJson(KANAFREQ);
    }

    private async Task<List<KanaObj>> DeserializeKanaJson(string extension)
    {
        string path = Path.Combine(PATH, extension);

        string json = await File.ReadAllTextAsync(path);

        var result = JsonConvert.DeserializeObject<List<KanaObj>>(json);

        return result ?? new List<KanaObj>();
    }

    private async Task<List<FreqUnit>> DeserializeFreqJson(string extension)
    {
        string path = Path.Combine(PATH, extension);

        string json = await File.ReadAllTextAsync(Path.Combine(path));

        var result = JsonConvert.DeserializeObject<List<FreqUnit>>(json);

        return result ?? new List<FreqUnit>();
    }
}
#region models
public class FreqUnit
{
    [JsonProperty("Hiragana")]
    public required string Kana { get; set; }
    [JsonProperty("Occurrence")]
    public int Frequency { get; set; }
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
    public required string Hiragana { get; set; }
    public required string Katakana { get; set; }
    public required string Romaji { get; set; }
    public int? Frequency { get; set; }
}
#endregion