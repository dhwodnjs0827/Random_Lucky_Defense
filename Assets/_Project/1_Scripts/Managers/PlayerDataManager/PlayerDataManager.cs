using Cysharp.Threading.Tasks;

/// <summary>
/// 플레이어 데이터 관리 매니저 클래스
/// </summary>
public partial class PlayerDataManager : MonoSingleton<PlayerDataManager>
{
    protected override bool isInitialized { get; set; }
    
    public override async UniTask InitializeAsync()
    {
        var saveData = SaveLoadManager.Instance.SaveData;
        InitializeCurrencyData(saveData.CurrencyData);
        InitializeProfileData(saveData.ProfileData);
        InitializeHeroData(saveData.HeroData);

        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 데이터 저장
    /// </summary>
    public void SaveData(SaveDataType dataType)
    {
        switch (dataType)
        {
            case SaveDataType.Currency:
                SaveCurrencyData();
                break;
            case SaveDataType.Profile:
                SaveProfileData();
                break;
            case SaveDataType.Hero:
                SaveHeroData();
                break;
        }
    }
}