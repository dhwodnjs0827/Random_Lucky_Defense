using System;
using UnityEngine;

public class SummonController : MonoBehaviour, IAbilityEffect
{
    [SerializeField] private Transform redDragonSpawnPoint;
    [SerializeField] private Transform ancientStatueSpawnPoint;

    private RedDragon redDragon;
    private AncientStatue ancientStatue;
    private Lightning lightning;
    
    private const string RED_DRAGON_PREFAB_PATH = "Prefabs/Summon/RedDragon";
    private const string ANCIENT_STATUE_PREFAB_PATH = "Prefabs/Summon/AncientStatue";
    private const string LIGHTNING_PREFAB_PATH = "Prefabs/Summon/Lightning";

    private void OnEnable()
    {
        RegisterAbilityEffect(InGameManager.Instance.AbilityEffectFactory);
    }

    private void OnDisable()
    {
        UnregisterAbilityEffect(InGameManager.Instance?.AbilityEffectFactory);
    }

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
        if (redDragon == null)
        {
            SpawnRedDragon();
        }

        redDragon.IncreaseStat(abilityContainer);
    }

    private void ApplyAncientStatueAbility(AbilityContainer abilityContainer)
    {
        if (ancientStatue == null)
        {
            SpawnAncientStatue();
        }
        ancientStatue.IncreaseStat(abilityContainer);
    }
    
    private void ApplyLightningAbility(AbilityContainer abilityContainer)
    {
        if (lightning == null)
        {
            CreateLightning();
        }
        lightning.IncreaseStat(abilityContainer);
    }

    private void SpawnRedDragon()
    {
        var prefab = ResourceManager.Instance.Load<RedDragon>(RED_DRAGON_PREFAB_PATH);
        redDragon = Instantiate(prefab, redDragonSpawnPoint.position, redDragonSpawnPoint.rotation);
    }

    private void SpawnAncientStatue()
    {
        var prefab = ResourceManager.Instance.Load<AncientStatue>(ANCIENT_STATUE_PREFAB_PATH);
        ancientStatue = Instantiate(prefab, ancientStatueSpawnPoint.position, ancientStatueSpawnPoint.rotation);
        
    }
    
    private void CreateLightning()
    {
        var prefab = ResourceManager.Instance.Load<Lightning>(LIGHTNING_PREFAB_PATH);
        lightning = Instantiate(prefab);
        lightning.gameObject.SetActive(false);
    }
}
