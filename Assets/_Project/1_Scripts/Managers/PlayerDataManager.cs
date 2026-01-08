/// <summary>
/// 플레이어 데이터 관리 매니저 클래스
/// </summary>
public partial class PlayerDataManager : Singleton<PlayerDataManager>
{
    public PlayerDataManager()
    {
        InitializeHeroData(DataSaveLoadManager.Instance.SaveData.HeroData);
    }
}