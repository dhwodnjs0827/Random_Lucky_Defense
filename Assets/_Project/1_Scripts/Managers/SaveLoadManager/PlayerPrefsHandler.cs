using UnityEngine;

public class PlayerPrefsHandler : IDataSaveLoadHandler
{
    public void Save(SaveData data)
    {
        var jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SaveData", jsonData);
    }

    public SaveData Load()
    {
        var jsonData = PlayerPrefs.GetString("SaveData");
        var loadData = JsonUtility.FromJson<SaveData>(jsonData);
        return loadData;
    }

    public void Delete()
    {
        PlayerPrefs.DeleteKey("SaveData");
    }
}
