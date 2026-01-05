using System;
using System.Collections.Generic;

public class InGameHeroBuffController : IEventListener, IBuffCardEffect
{
    private Dictionary<HeroClassType, HeroStat> levelUpStats = new();
    private Dictionary<HeroClassType, HeroStat> cardEffectStats = new();
    
    public Dictionary<HeroClassType, HeroStat> LevelUpStats => levelUpStats;
    public Dictionary<HeroClassType, HeroStat> CardEffectStats => cardEffectStats;
    
    private Action<GameInGameLevelUpEventData> onLevelUp;

    public InGameHeroBuffController()
    {
        levelUpStats.Add(HeroClassType.Magician, new HeroStat());
        levelUpStats.Add(HeroClassType.Archer, new HeroStat());
        levelUpStats.Add(HeroClassType.Warrior, new HeroStat());
        
        cardEffectStats.Add(HeroClassType.Magician, new HeroStat());
        cardEffectStats.Add(HeroClassType.Archer, new HeroStat());
        cardEffectStats.Add(HeroClassType.Warrior, new HeroStat());
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

    private void LevelUp(GameInGameLevelUpEventData eventData)
    {
        levelUpStats[eventData.TargetClass].IncreaseAttackPowerMultiplier(eventData.DamageMultiplier);
    }

    public void RegisterCardEffect(CardEffectFactory cardEffectFactory)
    {
        cardEffectFactory.RegisterCardEffectHandler(BuffEffectType.IncreaseCriticalRate, this);
        cardEffectFactory.RegisterCardEffectHandler(BuffEffectType.IncreaseCriticalDamage, this);
        cardEffectFactory.RegisterCardEffectHandler(BuffEffectType.MagicianIncreaseMoveSpeed, this);
        cardEffectFactory.RegisterCardEffectHandler(BuffEffectType.ArcherIncreaseMoveSpeed, this);
        cardEffectFactory.RegisterCardEffectHandler(BuffEffectType.ArcherSummonAncientStatue, this);
        cardEffectFactory.RegisterCardEffectHandler(BuffEffectType.WarriorIncreaseMoveSpeed, this);
    }

    public void UnregisterCardEffect(CardEffectFactory cardEffectFactory)
    {
        cardEffectFactory.UnregisterCardEffectHandler(BuffEffectType.IncreaseCriticalRate, this);
        cardEffectFactory.UnregisterCardEffectHandler(BuffEffectType.IncreaseCriticalDamage, this);
        cardEffectFactory.UnregisterCardEffectHandler(BuffEffectType.MagicianIncreaseMoveSpeed, this);
        cardEffectFactory.UnregisterCardEffectHandler(BuffEffectType.ArcherIncreaseMoveSpeed, this);
        cardEffectFactory.UnregisterCardEffectHandler(BuffEffectType.ArcherSummonAncientStatue, this);
        cardEffectFactory.UnregisterCardEffectHandler(BuffEffectType.WarriorIncreaseMoveSpeed, this);
    }

    public void ApplyCardEffect(BuffCardContainer cardContainer)
    {
        switch (cardContainer.CardData.BuffEffectType)
        {
            case BuffEffectType.IncreaseCriticalRate:
                cardEffectStats[HeroClassType.Magician].IncreaseCriticalRate(cardContainer.CardLevelData.value);
                cardEffectStats[HeroClassType.Archer].IncreaseCriticalRate(cardContainer.CardLevelData.value);
                cardEffectStats[HeroClassType.Warrior].IncreaseCriticalRate(cardContainer.CardLevelData.value);
                break;
            case BuffEffectType.IncreaseCriticalDamage:
                cardEffectStats[HeroClassType.Magician].IncreaseCriticalDamage(cardContainer.CardLevelData.value);
                cardEffectStats[HeroClassType.Archer].IncreaseCriticalDamage(cardContainer.CardLevelData.value);
                cardEffectStats[HeroClassType.Warrior].IncreaseCriticalDamage(cardContainer.CardLevelData.value);
                break;
            case BuffEffectType.MagicianIncreaseMoveSpeed:
                cardEffectStats[HeroClassType.Magician].IncreaseMoveSpeedMultiplier(cardContainer.CardLevelData.value);
                break;
            case BuffEffectType.ArcherIncreaseMoveSpeed:
                cardEffectStats[HeroClassType.Archer].IncreaseMoveSpeedMultiplier(cardContainer.CardLevelData.value);
                break;
            case BuffEffectType.WarriorIncreaseMoveSpeed:
                cardEffectStats[HeroClassType.Warrior].IncreaseMoveSpeedMultiplier(cardContainer.CardLevelData.value);
                break;
            case BuffEffectType.ArcherSummonAncientStatue:
                CDebug.Log("[ArcherCardEffectHandler] 고대 석상 소환]");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
