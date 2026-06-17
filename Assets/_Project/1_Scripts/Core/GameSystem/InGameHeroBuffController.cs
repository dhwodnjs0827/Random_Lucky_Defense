using System;
using System.Collections.Generic;

/// <summary>
/// 인게임 영웅 버프 효과(카드, 레벨) 관리 담당 클래스
/// </summary>
public class InGameHeroBuffController : IEventListener, IAbilityEffect
{
    private Dictionary<HeroClassType, HeroStat> levelUpStats = new();
    private Dictionary<HeroClassType, HeroStat> abilityEffectStats = new();
    private Dictionary<HeroClassType, float> acquiredHeroBonusDamages = new();
    
    public Dictionary<HeroClassType, HeroStat> LevelUpStats => levelUpStats;
    public Dictionary<HeroClassType, HeroStat> AbilityEffectStats => abilityEffectStats;
    public Dictionary<HeroClassType, float> AcquiredHeroBonusDamages => acquiredHeroBonusDamages;
    
    private Action<InGameLevelUpEventData> onLevelUp;

    public InGameHeroBuffController()
    {
        levelUpStats.Add(HeroClassType.Magician, new HeroStat());
        levelUpStats.Add(HeroClassType.Archer, new HeroStat());
        levelUpStats.Add(HeroClassType.Knight, new HeroStat());
        
        abilityEffectStats.Add(HeroClassType.Magician, new HeroStat());
        abilityEffectStats.Add(HeroClassType.Archer, new HeroStat());
        abilityEffectStats.Add(HeroClassType.Knight, new HeroStat());
        
        acquiredHeroBonusDamages.Add(HeroClassType.Magician, 1f + PlayerDataManager.Instance.CalculateHeroAcquiredBonusDamage(HeroClassType.Magician));
        acquiredHeroBonusDamages.Add(HeroClassType.Archer, 1f + PlayerDataManager.Instance.CalculateHeroAcquiredBonusDamage(HeroClassType.Archer));
        acquiredHeroBonusDamages.Add(HeroClassType.Knight, 1f + PlayerDataManager.Instance.CalculateHeroAcquiredBonusDamage(HeroClassType.Knight));
    }

    public void SubscribeEvents()
    {
        onLevelUp += LevelUp;
        EventManager.Subscribe(GameEventType.InGameHeroLevelUpCompleted, onLevelUp);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.InGameHeroLevelUpCompleted, onLevelUp);
        onLevelUp -= LevelUp;
    }

    private void LevelUp(InGameLevelUpEventData eventData)
    {
        levelUpStats[eventData.TargetClass].IncreaseAttackPowerMultiplier(eventData.DamageMultiplier);
    }

    public void RegisterAbilityEffect(AbilityEffectFactory abilityEffectFactory)
    {
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.IncreaseCriticalRate, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.IncreaseCriticalDamage, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.MagicianIncreaseMoveSpeed, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.ArcherIncreaseMoveSpeed, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.ArcherSummonAncientStatue, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.KnightIncreaseMoveSpeed, this);
    }

    public void UnregisterAbilityEffect(AbilityEffectFactory abilityEffectFactory)
    {
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.IncreaseCriticalRate, this);
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.IncreaseCriticalDamage, this);
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.MagicianIncreaseMoveSpeed, this);
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.ArcherIncreaseMoveSpeed, this);
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.ArcherSummonAncientStatue, this);
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.KnightIncreaseMoveSpeed, this);
    }

    public void ApplyAbilityEffect(AbilityContainer abilityContainer)
    {
        switch (abilityContainer.AbilityData.AbilityEffectType)
        {
            case AbilityEffectType.IncreaseCriticalRate:
                abilityEffectStats[HeroClassType.Magician].IncreaseCriticalRate(abilityContainer.AbilityLevelData.value / 100f);
                abilityEffectStats[HeroClassType.Archer].IncreaseCriticalRate(abilityContainer.AbilityLevelData.value / 100f);
                abilityEffectStats[HeroClassType.Knight].IncreaseCriticalRate(abilityContainer.AbilityLevelData.value / 100f);
                break;
            case AbilityEffectType.IncreaseCriticalDamage:
                abilityEffectStats[HeroClassType.Magician].IncreaseCriticalDamage(abilityContainer.AbilityLevelData.value / 100f);
                abilityEffectStats[HeroClassType.Archer].IncreaseCriticalDamage(abilityContainer.AbilityLevelData.value / 100f);
                abilityEffectStats[HeroClassType.Knight].IncreaseCriticalDamage(abilityContainer.AbilityLevelData.value / 100f);
                break;
            case AbilityEffectType.MagicianIncreaseMoveSpeed:
                abilityEffectStats[HeroClassType.Magician].IncreaseMoveSpeedMultiplier(abilityContainer.AbilityLevelData.value / 100f);
                break;
            case AbilityEffectType.ArcherIncreaseMoveSpeed:
                abilityEffectStats[HeroClassType.Archer].IncreaseMoveSpeedMultiplier(abilityContainer.AbilityLevelData.value / 100f);
                break;
            case AbilityEffectType.KnightIncreaseMoveSpeed:
                abilityEffectStats[HeroClassType.Knight].IncreaseMoveSpeedMultiplier(abilityContainer.AbilityLevelData.value / 100f);
                break;
            case AbilityEffectType.ArcherSummonAncientStatue:
                CDebug.Log("[ArcherCardEffectHandler] 고대 석상 소환]");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
