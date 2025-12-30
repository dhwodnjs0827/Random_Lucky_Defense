using System.Collections.Generic;
using System.Linq;
using Generated;
using UnityEngine;

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

    /// <summary>
    /// 선택 영웅 데이터 저장
    /// </summary>
    public void SaveSelectedHeroData(SelectedHeroData selectedHeroData)
    {
        if (selectedHeroes.TryGetValue(selectedHeroData.HeroClassType,
                out Dictionary<HeroGradeType, HeroDataSO> heroDatas))
        {
            heroDatas[selectedHeroData.HeroGradeType] = selectedHeroData.HeroDataSO;
        }
    }

    /// <summary>
    /// 초기 선택 영웅 데이터 초기화
    /// </summary>
    private void InitializeSelectedHeroes()
    {
        var initialGameConfig = Resources.Load<InitialGameConfig>("Data/SO/InitialGameConfig");
        
        selectedHeroes.Clear();
        selectedHeroes.Add(HeroClassType.Magician, new Dictionary<HeroGradeType, HeroDataSO>());
        selectedHeroes.Add(HeroClassType.Archer, new Dictionary<HeroGradeType, HeroDataSO>());
        selectedHeroes.Add(HeroClassType.Warrior, new Dictionary<HeroGradeType, HeroDataSO>());
        
        selectedHeroes[HeroClassType.Magician] = initialGameConfig.StartHeroes.magicians.ToDictionary(k => k.GradeType, v => v);
        selectedHeroes[HeroClassType.Archer] = initialGameConfig.StartHeroes.archers.ToDictionary(k => k.GradeType, v => v);
        selectedHeroes[HeroClassType.Warrior] = initialGameConfig.StartHeroes.warriors.ToDictionary(k => k.GradeType, v => v);
    }
}

public struct SelectedHeroData
{
    public HeroClassType HeroClassType;
    public HeroGradeType HeroGradeType;
    public HeroDataSO HeroDataSO;
}