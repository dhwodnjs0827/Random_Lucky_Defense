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

    public int LevelUpRequiredStack => CalculateLevelUpStack();

    public int LevelUpRequiredGold => CalculateLevelUpGold();

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
    
    private int CalculateLevelUpStack()
    {
        return Rank switch
        {
            HeroRankType.B =>
                GameConstants.INITAIL_RANK_B_LEVEL_UP_REQUIRMENT_STACK +
                GameConstants.INCREASE_RANK_B_LEVEL_UP_REQUIRMENT_STACK * (Level - 1),

            HeroRankType.A =>
                GameConstants.INITAIL_RANK_A_LEVEL_UP_REQUIRMENT_STACK +
                GameConstants.INCREASE_RANK_A_LEVEL_UP_REQUIRMENT_STACK * (Level - 1),

            HeroRankType.S =>
                GameConstants.INITAIL_RANK_S_LEVEL_UP_REQUIRMENT_STACK +
                GameConstants.INCREASE_RANK_S_LEVEL_UP_REQUIRMENT_STACK * (Level - 1),

            _ => 0
        };
    }
    
    private int CalculateLevelUpGold()
    {
        return Rank switch
        {
            HeroRankType.B =>
                GameConstants.INITIAL_LEVEL_UP_REQUIRMENT_GOLD +
                GameConstants.INCREASE_RANK_B_LEVEL_UP_REQUIRMENT_GOLD * (Level - 1),

            HeroRankType.A =>
                GameConstants.INITIAL_LEVEL_UP_REQUIRMENT_GOLD +
                GameConstants.INCREASE_RANK_A_LEVEL_UP_REQUIRMENT_GOLD * (Level - 1),

            HeroRankType.S =>
                GameConstants.INITIAL_LEVEL_UP_REQUIRMENT_GOLD +
                GameConstants.INCREASE_RANK_S_LEVEL_UP_REQUIRMENT_GOLD * (Level - 1),

            _ => 0
        };
    }
}
