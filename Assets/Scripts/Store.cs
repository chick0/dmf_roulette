using UnityEngine;

public enum StoreStatus
{
    ACTIVATE = 1,
    DEACTIVATE = 0
}

public class Store : MonoBehaviour
{
    private static readonly string Version = "v1";

    private static string Key(string name, string tune)
    {
        return $"store-{Version}-{name}-{tune}";
    }

    public static bool IsActivate(string name, string tune)
    {
        return PlayerPrefs.GetInt(Key(name, tune), (int)StoreStatus.ACTIVATE) == (int)StoreStatus.ACTIVATE;
    }

    public static void Activate(string name, string tune)
    {
        PlayerPrefs.SetInt(Key(name, tune), (int)StoreStatus.ACTIVATE);
    }

    public static void Deactivate(string name, string tune)
    {
        PlayerPrefs.SetInt(Key(name, tune), (int)StoreStatus.DEACTIVATE);
    }
}
