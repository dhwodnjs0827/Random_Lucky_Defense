using System.Collections.Generic;
using Generated;

/// <summary>
/// 플레이어 데이터 관리 매니저 클래스
/// </summary>
public class PlayerDataManager : Singleton<PlayerDataManager>
{
    private readonly Dictionary<HeroClassType, Dictionary<HeroGradeType, HeroDataSO>> selectedHeroes = new();

    public IDictionary<HeroClassType, Dictionary<HeroGradeType, HeroDataSO>> SelectedHeroes => selectedHeroes;

    public PlayerDataManager()
    {
        InitializeSelectedHeroes();
    }

    public void SaveSelectedHeroData(SelectedHeroData selectedHeroData)
    {
        if (selectedHeroes.TryGetValue(selectedHeroData.HeroClassType,
                out Dictionary<HeroGradeType, HeroDataSO> heroDatas))
        {
            heroDatas[selectedHeroData.HeroGradeType] = selectedHeroData.HeroDataSO;
        }
    }

    private void InitializeSelectedHeroes()
    {
        selectedHeroes.Clear();
        selectedHeroes.Add(HeroClassType.Magician, new Dictionary<HeroGradeType, HeroDataSO>());
        selectedHeroes.Add(HeroClassType.Archer, new Dictionary<HeroGradeType, HeroDataSO>());
        selectedHeroes.Add(HeroClassType.Warrior, new Dictionary<HeroGradeType, HeroDataSO>());
    }
}

public struct SelectedHeroData
{
    public HeroClassType HeroClassType;
    public HeroGradeType HeroGradeType;
    public HeroDataSO HeroDataSO;
}