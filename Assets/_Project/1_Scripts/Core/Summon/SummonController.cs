using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Generated;

public class SummonController : IAbilityEffect
{
    private readonly Dictionary<HeroClassType, HeroStat> summonBaseStat = new();
    
    private const string RED_DRAGON_DATA_SO_PATH = "Data/SO/SummonData/Red_Dragon";
    private const string ANCIENT_STATUE_DATA_SO_PATH = "Data/SO/SummonData/Ancient_Statue";
    private const string LIGHTNING_DATA_SO_PATH = "Data/SO/SummonData/Lightning";
    
    public Dictionary<HeroClassType, HeroStat> SummonBaseStat => summonBaseStat;

    #region IAbilityEffect implementation
    
    public void RegisterAbilityEffect(AbilityEffectFactory abilityEffectFactory)
    {
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.MagicianSummonRedDragon, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.ArcherSummonAncientStatue, this);
        abilityEffectFactory.RegisterAbilityEffectHandler(AbilityEffectType.KnightLightning, this);
    }

    public void UnregisterAbilityEffect(AbilityEffectFactory abilityEffectFactory)
    {
        abilityEffectFactory?.UnregisterAbilityEffectHandler(AbilityEffectType.MagicianSummonRedDragon, this);
        abilityEffectFactory?.UnregisterAbilityEffectHandler(AbilityEffectType.ArcherSummonAncientStatue, this);
        abilityEffectFactory?.UnregisterAbilityEffectHandler(AbilityEffectType.KnightLightning, this);
    }

    public async UniTask InitializeAsync()
    {
        var redDragonData = await AddressableManager.Instance.LoadAsync<SummonDataSO>(RED_DRAGON_DATA_SO_PATH);
        var redDragonStat = new HeroStat(redDragonData);
        summonBaseStat[HeroClassType.Magician] = redDragonStat;
        
        var ancientStatueData = await AddressableManager.Instance.LoadAsync<SummonDataSO>(ANCIENT_STATUE_DATA_SO_PATH);
        var ancientStatueStat = new HeroStat(ancientStatueData);
        summonBaseStat[HeroClassType.Archer] = ancientStatueStat;
        
        var lightningData = await AddressableManager.Instance.LoadAsync<SummonDataSO>(LIGHTNING_DATA_SO_PATH);
        var lightningStat = new HeroStat(lightningData);
        summonBaseStat[HeroClassType.Knight] = lightningStat;
    }

    public void ApplyAbilityEffect(AbilityContainer abilityContainer)
    {
        switch (abilityContainer.AbilityData.AbilityEffectType)
        {
            case AbilityEffectType.MagicianSummonRedDragon:
                ApplyRedDragonAbility(abilityContainer);
                CDebug.Log($"[SummonController] 레드 드래곤이 출현하여 {abilityContainer.AbilityLevelData.value}초마다 {abilityContainer.AbilityLevelData.value1} 스플래시 데미지를 가함 (마법사 업그레이드 적용)");
                break;
            case AbilityEffectType.ArcherSummonAncientStatue:
                ApplyAncientStatueAbility(abilityContainer);
                CDebug.Log($"[SummonController] 고대석상이 출현하여 적에게 {abilityContainer.AbilityLevelData.value}초마다 레이저로 {abilityContainer.AbilityLevelData.value1} 데미지를 가함 (방어력 무시) (궁수 업그레이드 적용)");
                break;
            case AbilityEffectType.KnightLightning:
                ApplyLightningAbility(abilityContainer);
                CDebug.Log($"[SummonController] {abilityContainer.AbilityLevelData.value}초마다 번개를 무작위 적에게 내리쳐 {abilityContainer.AbilityLevelData.value1} 스플래시 데미지를 가함 (전사 업그레이드 적용)");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    #endregion
    
    private void ApplyRedDragonAbility(AbilityContainer abilityContainer)
    {
        EventManager.Dispatch(GameEventType.SpawnRedDragon);
        summonBaseStat[HeroClassType.Magician].IncreaseAttackSpeed(abilityContainer.AbilityLevelData.value);
        summonBaseStat[HeroClassType.Magician].IncreaseAttackPower(abilityContainer.AbilityLevelData.value1);
    }

    private void ApplyAncientStatueAbility(AbilityContainer abilityContainer)
    {
        EventManager.Dispatch(GameEventType.SpawnAncientStatue);
        summonBaseStat[HeroClassType.Archer].IncreaseAttackSpeed(abilityContainer.AbilityLevelData.value);
        summonBaseStat[HeroClassType.Archer].IncreaseAttackPower(abilityContainer.AbilityLevelData.value1);
    }
    
    private void ApplyLightningAbility(AbilityContainer abilityContainer)
    {
        EventManager.Dispatch(GameEventType.SpawnLightning);
        summonBaseStat[HeroClassType.Knight].IncreaseAttackSpeed(abilityContainer.AbilityLevelData.value);
        summonBaseStat[HeroClassType.Knight].IncreaseAttackPower(abilityContainer.AbilityLevelData.value1);
    }
}
