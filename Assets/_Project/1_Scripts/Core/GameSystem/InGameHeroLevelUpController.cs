using System;
using System.Collections.Generic;
using Generated;
using UniRx;
using UnityEngine;

/// <summary>
/// 인게임 영웅 레벨 업 담당 클래스
/// </summary>
public class InGameHeroLevelUpController : IEventListener, IBuffCardEffect
{
    private ReactiveProperty<int> currentSpawnPoint = new(); // 현재 영웅 소환 재화

    private readonly Dictionary<HeroClassType, Dictionary<int, ClassLevelUpData>> levelUpDataDict = new(); // 클래스 별 레벨 업 데이터
    private readonly Dictionary<HeroClassType, ReactiveProperty<int>> currentLevelDict = new(); // 클래스 별 현재 레벨

    private bool isActiveSPGainRateEffect = false;
    private float spGainInterval;
    private int spGainAmount;
    private float spGainTimer;
    
    private Action<HeroClassType> onLevelUp;
    
    public IReadOnlyReactiveProperty<int> CurrentSpawnPoint => currentSpawnPoint;
    public IDictionary<HeroClassType, Dictionary<int, ClassLevelUpData>> LevelUpDataDict => levelUpDataDict;
    public IDictionary<HeroClassType, ReactiveProperty<int>> CurrentLevelDict => currentLevelDict;
    
    public InGameHeroLevelUpController()
    {
        currentSpawnPoint.Value = GameConstants.INITIAL_HERO_SPAWN_POINT;
        InitializeLevelUpData();
    }
    
    public void SubscribeEvents()
    {
        onLevelUp += LevelUp;
        EventManager.Subscribe(GameEventType.InGameHeroLevelUpRequest, onLevelUp);
        EventManager.Subscribe(GameEventType.SpawnHero, OnSpawnHero);
        EventManager.Subscribe(GameEventType.NormalEnemyDie, OnNormalEnemyDie);
        EventManager.Subscribe(GameEventType.BossEnemyDie, OnBossEnemyDie);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.InGameHeroLevelUpRequest, onLevelUp);
        onLevelUp -= LevelUp;
        EventManager.Unsubscribe(GameEventType.SpawnHero, OnSpawnHero);
        EventManager.Unsubscribe(GameEventType.NormalEnemyDie, OnNormalEnemyDie);
        EventManager.Unsubscribe(GameEventType.BossEnemyDie, OnBossEnemyDie);
    }
    
    private void OnSpawnHero()
    {
        currentSpawnPoint.Value -= GameConstants.HERO_SPAWN_POINT_COST;
    }

    private void LevelUp(HeroClassType classType)
    {
        currentSpawnPoint.Value -= levelUpDataDict[classType][currentLevelDict[classType].Value].LevelUpCost;
        currentLevelDict[classType].Value++;
        EventManager.Dispatch(GameEventType.InGameHeroLevelUpCompleted, new GameInGameLevelUpEventData(classType, levelUpDataDict[classType][currentLevelDict[classType].Value].AttackPowerMultiplier));
        CDebug.Log($"[InGameHeroLevelUpController] {classType} 레벨 업, 현재 레벨: {currentLevelDict[classType].Value}");
    }
    
    private void InitializeLevelUpData()
    {
        currentLevelDict.Add(HeroClassType.Magician, new ReactiveProperty<int>(1));
        currentLevelDict.Add(HeroClassType.Archer, new ReactiveProperty<int>(1));
        currentLevelDict.Add(HeroClassType.Warrior, new ReactiveProperty<int>(1));
        
        var datas = ResourceManager.Instance.LoadAll<InGameLevelUpDataSO>("Data/SO/InGameLevelUpData");
        levelUpDataDict.Add(HeroClassType.Magician, new Dictionary<int, ClassLevelUpData>());
        levelUpDataDict.Add(HeroClassType.Archer, new Dictionary<int, ClassLevelUpData>());
        levelUpDataDict.Add(HeroClassType.Warrior, new Dictionary<int, ClassLevelUpData>());
        foreach (var data in datas)
        {
            var dict = levelUpDataDict[data.HeroClassType];
            var levelUpData = new ClassLevelUpData(data.Cost, data.AttackPowerMultiplier);
            dict.Add(data.Level, levelUpData);
        }
    }
    
    private void OnNormalEnemyDie()
    {
        currentSpawnPoint.Value += 1;
    }

    private void OnBossEnemyDie()
    {
        currentSpawnPoint.Value += 10;
    }

    public void GainSpawnPointCardEffect()
    {
        if (!isActiveSPGainRateEffect)
        {
            return;
        }
        spGainTimer += Time.deltaTime;
        if (spGainTimer >= spGainInterval)
        {
            currentSpawnPoint.Value += spGainAmount;
            spGainTimer = 0;
            CDebug.Log($"[InGameHeroLevelUpController] 현재 카드 효과 간격: {spGainInterval}, 획득량: {spGainAmount}");
        }
    }

    public void RegisterCardEffect(CardEffectFactory cardEffectFactory)
    {
        cardEffectFactory.RegisterCardEffectHandler(BuffEffectType.IncreaseSpawnPointGainRate, this);
    }

    public void UnregisterCardEffect(CardEffectFactory cardEffectFactory)
    {
        cardEffectFactory.UnregisterCardEffectHandler(BuffEffectType.IncreaseSpawnPointGainRate, this);
    }

    public void ApplyCardEffect(BuffCardContainer cardContainer)
    {
        if (cardContainer.CardData.BuffEffectType == BuffEffectType.IncreaseSpawnPointGainRate)
        {
            isActiveSPGainRateEffect =  true;
            spGainInterval = cardContainer.CardLevelData.value;
            spGainAmount = (int)cardContainer.CardLevelData.value1;
        }
    }
}

public struct ClassLevelUpData
{
    public int LevelUpCost;
    public float AttackPowerMultiplier;

    public ClassLevelUpData(int levelUpCost, float attackPowerMultiplier)
    {
        LevelUpCost = levelUpCost;
        AttackPowerMultiplier = attackPowerMultiplier;
    }
}