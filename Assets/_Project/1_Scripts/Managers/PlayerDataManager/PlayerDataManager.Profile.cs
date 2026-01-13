using Cysharp.Threading.Tasks;

public partial class PlayerDataManager
{
    public ProfileSaveData ProfileSaveData {get; private set;}
    
    private void InitializeProfileData(ProfileSaveData data)
    {
        ProfileSaveData = data;
    }
    
    /// <summary>
    /// 프로필 데이터 저장
    /// </summary>
    private void SaveProfileData()
    {
        var saveData = SaveLoadManager.Instance.SaveData;
        saveData.ProfileData = ProfileSaveData;
        SaveLoadManager.Instance.SaveAsync(saveData).Forget();
    }
}