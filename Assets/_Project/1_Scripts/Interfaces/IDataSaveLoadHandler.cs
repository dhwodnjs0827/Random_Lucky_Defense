using Cysharp.Threading.Tasks;

public interface IDataSaveLoadHandler
{
    public UniTask SaveAsync(SaveData data);
    public UniTask<SaveData> LoadAsync();
    public UniTask DeleteAsync();
}
