using System;
using System.Collections.Generic;

/// <summary>
/// 인게임 영웅 버프 효과(재능, 레벨) 관리 담당 클래스
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

    #region IEventListener implementation

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

    #endregion

    #region IAbilityEffect implementation

    public void RegisterAbilityEffect(AbilityEffectFactory abilityEffectFactory)
    {
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.IncreaseCriticalRate, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.IncreaseCriticalDamage, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.IncreaseDamage, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.IncreaseAttackRange, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.IncreaseSplashRange, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.IncreasePenetratingPower, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.MagicianIncreaseAttackPower, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.MagicianIncreaseMoveSpeed, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.ArcherIncreaseAttackPower, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.ArcherIncreaseMoveSpeed, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.KnightIncreaseAttackPower, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.KnightIncreaseMoveSpeed, this);
    }

    public void UnregisterAbilityEffect(AbilityEffectFactory abilityEffectFactory)
    {
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.IncreaseCriticalRate, this);
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.IncreaseCriticalDamage, this);
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.IncreaseDamage, this);
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.IncreaseAttackRange, this);
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.IncreaseSplashRange, this);
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.IncreasePenetratingPower, this);
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.MagicianIncreaseAttackPower, this);
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.MagicianIncreaseMoveSpeed, this);
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.ArcherIncreaseAttackPower, this);
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.ArcherIncreaseMoveSpeed, this);
        abilityEffectFactory.UnregisterAbilityEffectHandler(AbilityEffectType.KnightIncreaseAttackPower, this);
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
                CDebug.Log($"[InGameHeroBuffController] 크리티컬 확률 {abilityContainer.AbilityLevelData.value}% 증가");
                break;
            case AbilityEffectType.IncreaseCriticalDamage:
                abilityEffectStats[HeroClassType.Magician].IncreaseCriticalDamage(abilityContainer.AbilityLevelData.value / 100f);
                abilityEffectStats[HeroClassType.Archer].IncreaseCriticalDamage(abilityContainer.AbilityLevelData.value / 100f);
                abilityEffectStats[HeroClassType.Knight].IncreaseCriticalDamage(abilityContainer.AbilityLevelData.value / 100f);
                CDebug.Log($"[InGameHeroBuffController] 크리티컬 데미지 {abilityContainer.AbilityLevelData.value}% 증가");
                break;
            case AbilityEffectType.IncreaseDamage:
                abilityEffectStats[HeroClassType.Magician].IncreaseAttackPowerMultiplier(abilityContainer.AbilityLevelData.value / 100f);
                abilityEffectStats[HeroClassType.Archer].IncreaseAttackPowerMultiplier(abilityContainer.AbilityLevelData.value / 100f);
                abilityEffectStats[HeroClassType.Knight].IncreaseAttackPowerMultiplier(abilityContainer.AbilityLevelData.value / 100f);
                CDebug.Log($"[InGameHeroBuffController] 피해량 {abilityContainer.AbilityLevelData.value}% 증가");
                break;
            case AbilityEffectType.IncreaseAttackRange:
                abilityEffectStats[HeroClassType.Magician].IncreaseAttackRangeMultiplier(abilityContainer.AbilityLevelData.value / 100f);
                abilityEffectStats[HeroClassType.Archer].IncreaseAttackRangeMultiplier(abilityContainer.AbilityLevelData.value / 100f);
                abilityEffectStats[HeroClassType.Knight].IncreaseAttackRangeMultiplier(abilityContainer.AbilityLevelData.value / 100f);
                CDebug.Log($"[InGameHeroBuffController] 모든 클래스 공격 범위 {abilityContainer.AbilityLevelData.value} 증가");
                break;
            case AbilityEffectType.IncreaseSplashRange:
                abilityEffectStats[HeroClassType.Magician].IncreaseSplashRangeMultiplier(abilityContainer.AbilityLevelData.value / 100f);
                abilityEffectStats[HeroClassType.Archer].IncreaseSplashRangeMultiplier(abilityContainer.AbilityLevelData.value / 100f);
                abilityEffectStats[HeroClassType.Knight].IncreaseSplashRangeMultiplier(abilityContainer.AbilityLevelData.value / 100f);
                CDebug.Log($"[InGameHeroBuffController] 스플래시 데미지 범위{abilityContainer.AbilityLevelData.value} 증가");
                break;
            case AbilityEffectType.IncreasePenetratingPower:
                abilityEffectStats[HeroClassType.Magician].IncreasePenetration(abilityContainer.AbilityLevelData.value / 100f);
                abilityEffectStats[HeroClassType.Archer].IncreasePenetration(abilityContainer.AbilityLevelData.value / 100f);
                abilityEffectStats[HeroClassType.Knight].IncreasePenetration(abilityContainer.AbilityLevelData.value / 100f);
                CDebug.Log($"[InGameHeroBuffController] 적 방어력 {abilityContainer.AbilityLevelData.value}% 무시함");
                break;
            case AbilityEffectType.MagicianIncreaseAttackPower:
                abilityEffectStats[HeroClassType.Magician].IncreaseAttackPowerMultiplier(abilityContainer.AbilityLevelData.value / 100f);
                CDebug.Log($"[InGameHeroBuffController] 마법사 기본 공격력의 {abilityContainer.AbilityLevelData.value}% 증가");
                break;
            case AbilityEffectType.MagicianIncreaseMoveSpeed:
                abilityEffectStats[HeroClassType.Magician].IncreaseMoveSpeedMultiplier(abilityContainer.AbilityLevelData.value / 100f);
                CDebug.Log($"[InGameHeroBuffController] 마법사 이동속도 {abilityContainer.AbilityLevelData.value}% 증가");
                break;
            case AbilityEffectType.ArcherIncreaseAttackPower:
                abilityEffectStats[HeroClassType.Archer].IncreaseAttackPowerMultiplier(abilityContainer.AbilityLevelData.value / 100f);
                CDebug.Log($"[InGameHeroBuffController] 궁수 기본 공격력의 {abilityContainer.AbilityLevelData.value}% 증가");
                break;
            case AbilityEffectType.ArcherIncreaseMoveSpeed:
                abilityEffectStats[HeroClassType.Archer].IncreaseMoveSpeedMultiplier(abilityContainer.AbilityLevelData.value / 100f);
                CDebug.Log($"[InGameHeroBuffController] 궁수 이동속도 {abilityContainer.AbilityLevelData.value}% 증가");
                break;
            case AbilityEffectType.KnightIncreaseAttackPower:
                abilityEffectStats[HeroClassType.Knight].IncreaseAttackPowerMultiplier(abilityContainer.AbilityLevelData.value / 100f);
                CDebug.Log($"[InGameHeroBuffController] 전사 기본 공격력의 {abilityContainer.AbilityLevelData.value}% 증가");
                break;
            case AbilityEffectType.KnightIncreaseMoveSpeed:
                abilityEffectStats[HeroClassType.Knight].IncreaseMoveSpeedMultiplier(abilityContainer.AbilityLevelData.value / 100f);
                CDebug.Log($"[InGameHeroBuffController] 전사 이동속도 {abilityContainer.AbilityLevelData.value}% 증가");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion

    private void LevelUp(InGameLevelUpEventData eventData)
    {
        levelUpStats[eventData.TargetClass].IncreaseAttackPowerMultiplier(eventData.DamageMultiplier);
    }
}
