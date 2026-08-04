using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Generated;
using UniRx;

/// <summary>
/// 인게임 영웅 레벨 업 담당 클래스
/// </summary>
public class InGameHeroLevelUpController : IEventListener
{
    private readonly Dictionary<HeroClassType, Dictionary<int, ClassLevelUpData>>
        levelUpDataDict = new(); // 클래스 별 레벨 업 데이터

    private readonly Dictionary<HeroClassType, ReactiveProperty<int>> currentLevelDict = new(); // 클래스 별 현재 레벨

    private Action<HeroClassType> onLevelUp;

#if ADDRESSABLE
    private const string IN_GAME_HERO_LEVEL_UP_DATA_SO_PATH = "InGameHeroLevelUpData";
#else
    private const string IN_GAME_HERO_LEVEL_UP_DATA_SO_PATH = "Data/SO/InGameHeroLevelUpData";
#endif

    public IDictionary<HeroClassType, Dictionary<int, ClassLevelUpData>> LevelUpDataDict => levelUpDataDict;
    public IDictionary<HeroClassType, ReactiveProperty<int>> CurrentLevelDict => currentLevelDict;

    public InGameHeroLevelUpController()
    {
        InitializeLevelUpData();
    }

    #region IEventListener implementation

    public void SubscribeEvents()
    {
        onLevelUp += LevelUp;
        EventManager.Subscribe(GameEventType.InGameHeroLevelUpRequest, onLevelUp);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.InGameHeroLevelUpRequest, onLevelUp);
        onLevelUp -= LevelUp;
    }

    #endregion

    private void LevelUp(HeroClassType classType)
    {
        var currentLevel = currentLevelDict[classType].Value;
        var levelUpCost = levelUpDataDict[classType][currentLevel].LevelUpCost;

        currentLevelDict[classType].Value++;

        var newLevel = currentLevelDict[classType].Value;
        var newLevelData = levelUpDataDict[classType][newLevel];
        EventManager.Dispatch(GameEventType.InGameHeroLevelUpCompleted,
            new InGameLevelUpEventData(classType, newLevelData.AttackPowerMultiplier, levelUpCost));
        CDebug.Log($"[InGameHeroLevelUpController] {classType} 레벨 업, 현재 레벨: {newLevel}");
    }

    /// <summary>
    /// 인게임 영웅 레벨업 데이터 초기화
    /// </summary>
    private void InitializeLevelUpData()
    {
        currentLevelDict.Add(HeroClassType.Magician, new ReactiveProperty<int>(1));
        currentLevelDict.Add(HeroClassType.Archer, new ReactiveProperty<int>(1));
        currentLevelDict.Add(HeroClassType.Knight, new ReactiveProperty<int>(1));

        var datas = DataManager.Instance.InGameHeroLevelUpDataList;
        levelUpDataDict.Add(HeroClassType.Magician, new Dictionary<int, ClassLevelUpData>());
        levelUpDataDict.Add(HeroClassType.Archer, new Dictionary<int, ClassLevelUpData>());
        levelUpDataDict.Add(HeroClassType.Knight, new Dictionary<int, ClassLevelUpData>());
        foreach (var data in datas)
        {
            var dict = levelUpDataDict[data.HeroClassType];
            var levelUpData = new ClassLevelUpData(data.Cost, data.AttackPowerMultiplier);
            dict.Add(data.Level, levelUpData);
        }
    }
}

public struct ClassLevelUpData
{
    public readonly int LevelUpCost;
    public readonly float AttackPowerMultiplier;

    public ClassLevelUpData(int levelUpCost, float attackPowerMultiplier)
    {
        LevelUpCost = levelUpCost;
        AttackPowerMultiplier = attackPowerMultiplier;
    }
}