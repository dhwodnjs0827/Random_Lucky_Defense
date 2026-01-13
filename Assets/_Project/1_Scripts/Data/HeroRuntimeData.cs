using Generated;

/// <summary>
/// 런타임 용 영웅 데이터
/// </summary>
public class HeroRuntimeData
{
    public HeroDataSO HeroData;
    
    public bool IsAcquiredHero; // 영웅 획득 여부 
    public int Level; // 영웅 레벨
    public int AcquiredStack; // 영웅 획득 스택
    public bool IsSelected; // 사용 선택 여부
    
    public int ID => HeroData.ID;
    public string Name => HeroData.Name;
    public HeroClassType Class => HeroData.ClassType;
    public HeroGradeType Grade => HeroData.GradeType;
    public HeroRankType Rank => HeroData.RankType;

    /// <summary>
    /// 데이터 저장용으로 변환
    /// </summary>
    public PlayerHeroSaveData Convert()
    {
        PlayerHeroSaveData data = new()
        {
            ID = ID,
            IsAcquiredHero = IsAcquiredHero,
            Level = Level,
            AcquiredStack = AcquiredStack,
            IsSelected = IsSelected
        };
        return data;
    }
}
