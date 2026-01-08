public interface IDataSaveLoadHandler
{
    public void Save(SaveData data);
    public SaveData Load();
    public void Delete();
}
