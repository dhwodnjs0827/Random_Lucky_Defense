using Generated;

/// <summary>
/// 플레이어 데이터 관리 매니저 클래스
/// </summary>
public partial class PlayerDataManager : Singleton<PlayerDataManager>
{
    public PlayerDataManager()
    {
        InitializeProfileData(SaveLoadManager.Instance.SaveData.ProfileData);
        InitializeCurrencyData(SaveLoadManager.Instance.SaveData.CurrencyData);
        InitializeHeroData(SaveLoadManager.Instance.SaveData.HeroData);
    }

    public ProfileSaveData ProfileSaveData {get; private set;}
    
    private void InitializeProfileData(ProfileSaveData data)
    {
        ProfileSaveData = data;
    }
    
    public CurrencySaveData CurrencySaveData {get; private set;}

    private void InitializeCurrencyData(CurrencySaveData data)
    {
        CurrencySaveData = data;
    }
}