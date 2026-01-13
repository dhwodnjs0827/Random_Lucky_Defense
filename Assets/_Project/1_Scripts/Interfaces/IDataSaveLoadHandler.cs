using Cysharp.Threading.Tasks;

/// <summary>
/// SaveData의 저장, 불러오기, 삭제를 담당하는 인터페이스
/// </summary>
public interface IDataSaveLoadHandler
{
    /// <summary>
    /// SaveData를 비동기로 저장
    /// </summary>
    public UniTask SaveAsync(SaveData data);
    
    /// <summary>
    /// SaveData를 비동기로 불러오기
    /// </summary>
    public UniTask<SaveData> LoadAsync();
    
    /// <summary>
    /// 저장된 SaveData 비동기로 삭제
    /// </summary>
    public UniTask DeleteAsync();
}
