using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
internal class RawMusicList
{
    public RawMusic[] musicList;
}

[System.Serializable]
public class RawMusic
{
    public string name;
    public string[] tune;
}

public class Music
{
    public string Name { get; }
    public string[] Tune { get; }

    public Music(RawMusic rawMusic)
    {
        Name = rawMusic.name;
        Tune = rawMusic.tune;
    }
}

public static class MusicData
{
    public static List<Music> GetMusicList()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "MusicData.json");
        string rawJson = File.ReadAllText(path);

        RawMusicList obj = JsonUtility.FromJson<RawMusicList>(rawJson);

        List<Music> result = new();

        foreach (RawMusic ctx in obj.musicList)
        {
            result.Add(new Music(ctx));
        }

        return result;
    }
}
