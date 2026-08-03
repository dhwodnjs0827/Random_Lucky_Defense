using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SummonController : MonoBehaviour, IAbilityEffect
{
    [SerializeField] private Transform redDragonSpawnPoint;
    [SerializeField] private Transform ancientStatueSpawnPoint;
    [SerializeField] private Transform lightningSpawnPoint;

    private RedDragon redDragon;
    private AncientStatue ancientStatue;
    private LightningController lightning;

    private UniTask<RedDragon> redDragonSpawning;
    private UniTask<AncientStatue> ancientStatueSpawning;
    private UniTask<LightningController> lightningSpawning;
    
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
        ApplyRedDragonAbilityAsync(abilityContainer).Forget();
    }

    private async UniTaskVoid ApplyRedDragonAbilityAsync(AbilityContainer abilityContainer)
    {
        if (redDragon == null)
        {
            redDragonSpawning = redDragonSpawning.Status == UniTaskStatus.Pending
                ? redDragonSpawning
                : SpawnRedDragonAsync().Preserve();
            
            redDragon = await redDragonSpawning;
        }
        
        redDragon.IncreaseStat(abilityContainer);
    }

    private void ApplyAncientStatueAbility(AbilityContainer abilityContainer)
    {
        ApplyAncientStatueAsync(abilityContainer).Forget();
    }
    
    private async UniTaskVoid ApplyAncientStatueAsync(AbilityContainer abilityContainer)
    {
        if (ancientStatue == null)
        {
            ancientStatueSpawning = ancientStatueSpawning.Status == UniTaskStatus.Pending
                ? ancientStatueSpawning
                : SpawnAncientStatueAsync().Preserve();
            
            ancientStatue = await ancientStatueSpawning;
        }
        
        ancientStatue.IncreaseStat(abilityContainer);
    }
    
    private void ApplyLightningAbility(AbilityContainer abilityContainer)
    {
        ApplyLightningAsync(abilityContainer).Forget();
    }
    
    private async UniTaskVoid ApplyLightningAsync(AbilityContainer abilityContainer)
    {
        if (lightning == null)
        {
            lightningSpawning = lightningSpawning.Status == UniTaskStatus.Pending
                ? lightningSpawning
                : CreateLightningAsync().Preserve();
            
            lightning = await lightningSpawning;
        }
        
        lightning.IncreaseStat(abilityContainer);
    }

    private async UniTask<RedDragon> SpawnRedDragonAsync()
    {
        var prefab = await AddressableManager.Instance.LoadAsync<RedDragon>(RED_DRAGON_PREFAB_PATH);
        redDragon = Instantiate(prefab, redDragonSpawnPoint.position, redDragonSpawnPoint.rotation);
        redDragon.transform.SetParent(redDragonSpawnPoint);
        await redDragon.InitializeAsync();
        return redDragon;
    }

    private async UniTask<AncientStatue> SpawnAncientStatueAsync()
    {
        var prefab = await AddressableManager.Instance.LoadAsync<AncientStatue>(ANCIENT_STATUE_PREFAB_PATH);
        ancientStatue = Instantiate(prefab, ancientStatueSpawnPoint.position, ancientStatueSpawnPoint.rotation);
        ancientStatue.transform.SetParent(ancientStatueSpawnPoint);
        await ancientStatue.InitializeAsync();
        return ancientStatue;
    }

    private async UniTask<LightningController> CreateLightningAsync()
    {
        var prefab = await AddressableManager.Instance.LoadAsync<LightningController>(LIGHTNING_PREFAB_PATH);
        lightning = Instantiate(prefab, lightningSpawnPoint.position, lightningSpawnPoint.rotation);
        lightning.transform.SetParent(lightningSpawnPoint);
        await lightning.InitializeAsync();
        return lightning;
    }
}
