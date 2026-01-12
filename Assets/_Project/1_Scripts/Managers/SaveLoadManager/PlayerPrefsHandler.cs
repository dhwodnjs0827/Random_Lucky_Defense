using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerPrefsHandler : IDataSaveLoadHandler
{
    public UniTask SaveAsync(SaveData data)
    {
        var jsonData = JsonConvert.SerializeObject(data);
        PlayerPrefs.SetString("SaveData", jsonData);
        PlayerPrefs.Save();
        return UniTask.CompletedTask;
    }

    public UniTask<SaveData> LoadAsync()
    {
        var jsonData = PlayerPrefs.GetString("SaveData");
        var loadData = JsonConvert.DeserializeObject<SaveData>(jsonData);
        return UniTask.FromResult(loadData);
    }

    public UniTask DeleteAsync()
    {
        PlayerPrefs.DeleteKey("SaveData");
        PlayerPrefs.Save();
        return UniTask.CompletedTask;
    }
}
