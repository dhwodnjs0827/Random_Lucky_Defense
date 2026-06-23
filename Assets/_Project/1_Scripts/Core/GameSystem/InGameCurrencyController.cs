using System;
using UniRx;
using UnityEngine;

/// <summary>
/// 인게임 재화 담당 클래스
/// </summary>
public class InGameCurrencyController : IEventListener, IAbilityEffect
{
    private ReactiveProperty<int> currentSpawnPoint = new(); // 현재 영웅 소환 재화

    private bool isActiveSPGainRateEffect = false;
    private float spGainInterval;
    private int spGainAmount;
    private float spGainTimer;

    private Action<InGameLevelUpEventData> onLevelUp;
    
    public IReadOnlyReactiveProperty<int> CurrentSpawnPoint => currentSpawnPoint;

    public InGameCurrencyController()
    {
        currentSpawnPoint.Value = GameConstants.INITIAL_HERO_SPAWN_POINT;
    }

    #region IEvenetListener implementation

    public void SubscribeEvents()
    {
        onLevelUp += OnLevelUp;
        EventManager.Subscribe(GameEventType.InGameHeroLevelUpCompleted, onLevelUp);
        EventManager.Subscribe(GameEventType.SpawnHero, OnSpawnHero);
        EventManager.Subscribe(GameEventType.NormalEnemyDie, OnNormalEnemyDie);
        EventManager.Subscribe(GameEventType.BossEnemyDie, OnBossEnemyDie);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.BossEnemyDie, OnBossEnemyDie);
        EventManager.Unsubscribe(GameEventType.NormalEnemyDie, OnNormalEnemyDie);
        EventManager.Unsubscribe(GameEventType.SpawnHero, OnSpawnHero);
        EventManager.Unsubscribe(GameEventType.InGameHeroLevelUpCompleted, onLevelUp);
        onLevelUp -= OnLevelUp;
    }

    #endregion
    
    #region IAbilityEffect implementation

    public void RegisterAbilityEffect(AbilityEffectFactory abilityEffectFactory)
    {
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.IncreaseSpawnPointGainRate, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.AcquireLuckyStone, this);
    }

    public void UnregisterAbilityEffect(AbilityEffectFactory abilityEffectFactory)
    {
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.IncreaseSpawnPointGainRate, this);
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.AcquireLuckyStone, this);
    }

    public void ApplyAbilityEffect(AbilityContainer abilityContainer)
    {
        switch (abilityContainer.AbilityData.AbilityEffectType)
        {
            case AbilityEffectType.IncreaseSpawnPointGainRate:
                CDebug.Log($"[InGameCurrencyController] spGainInterval: {spGainInterval} -> {abilityContainer.AbilityLevelData.value} spGainAmount: {spGainAmount} -> {(int)abilityContainer.AbilityLevelData.value1}");
                isActiveSPGainRateEffect =  true;
                spGainInterval = abilityContainer.AbilityLevelData.value;
                spGainAmount = (int)abilityContainer.AbilityLevelData.value1;
                CDebug.Log($"[InGameCurrencyController] {abilityContainer.AbilityLevelData.value}초 마다 sp {abilityContainer.AbilityLevelData.value1}획득");
                break;
            case AbilityEffectType.AcquireLuckyStone:
                CDebug.Log($"[InGameCurrencyController] 즉시 행운석을 {abilityContainer.AbilityLevelData.value}개 획득");
                break;
        }
    }

    #endregion
    
    public void GainSpawnPointAbilityEffect()
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
            CDebug.Log($"[InGameCurrencyController] 현재 재능 효과 간격: {spGainInterval}, 획득량: {spGainAmount}");
        }
    }
    
    private void OnSpawnHero()
    {
        currentSpawnPoint.Value -= GameConstants.HERO_SPAWN_POINT_COST;
    }
    
    private void OnNormalEnemyDie()
    {
        currentSpawnPoint.Value += 1;
    }

    private void OnBossEnemyDie()
    {
        currentSpawnPoint.Value += 10;
    }

    private void OnLevelUp(InGameLevelUpEventData data)
    {
        currentSpawnPoint.Value -= data.LevelUpCost;
        CDebug.Log($"[InGameCurrencyController] LevelUp 이벤트, 비용: {data.LevelUpCost}");
    }
}