using System;
using Cysharp.Threading.Tasks;
using Generated;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightningController : MonoBehaviour
{
    [SerializeField] private Lightning lightning;

    private HeroClassType classType;
    private HeroStat baseStat;
    private float attackTimer;

    private int enemyLayerMask;
    
    private bool isInitialized;

    private const string LIGHTNING_DATA_SO_PATH = "Data/SO/SummonData/Lightning";

    private HeroStat LevelUpStat => InGameManager.Instance.HeroBuffController.LevelUpStats[classType];
    private HeroStat AbilityEffectStat => InGameManager.Instance.HeroBuffController.AbilityEffectStats[classType];

    private float AcquiredHeroBonusDamage =>
        InGameManager.Instance.HeroBuffController.AcquiredHeroBonusDamages[classType];

    public async UniTask InitializeAsync()
    {
        try
        {
            var lightningData = await AddressableManager.Instance.LoadAsync<SummonDataSO>(LIGHTNING_DATA_SO_PATH);
            classType = lightningData.ClassType;
            baseStat = new HeroStat(lightningData);
            enemyLayerMask = LayerMask.GetMask("Enemy");

            isInitialized = true;
        }
        catch (Exception e)
        {
            CDebug.LogError($"[LightningController] 초기화 실패: {e}");
        }
    }

    private void Update()
    {
        if (!isInitialized)
        {
            return;
        }
        
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            var attackSpeed = DamageCalculator.CalculateMultipliers(
                baseStat.AttackSpeed,
                LevelUpStat.AttackSpeedMultiplier,
                AbilityEffectStat.AttackSpeedMultiplier
            );
            
            attackTimer = attackSpeed;
            TryAttack();
        }
    }

    public void IncreaseStat(AbilityContainer abilityContainer)
    {
        baseStat.IncreaseAttackSpeed(abilityContainer.AbilityLevelData.value);
        baseStat.IncreaseAttackPower(abilityContainer.AbilityLevelData.value1);
    }

    private void TryAttack()
    {
        var target = FindRandomTarget();
        if (target == null) return;

        var targetPosition = target.transform.position;

        lightning.Strike(targetPosition);
        HitTarget(target, targetPosition);
    }

    /// <summary>
    /// 공격 범위 내 랜덤 타겟 찾기
    /// </summary>
    private BaseEnemy FindRandomTarget()
    {
        float attackRange = DamageCalculator.CalculateMultipliers(
            baseStat.AttackRange,
            LevelUpStat.AttackRangeMultiplier,
            AbilityEffectStat.AttackRangeMultiplier
        );
        var hits = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayerMask);

        if (hits.Length == 0) return null;

        var randomHit = hits[Random.Range(0, hits.Length)];
        return randomHit.TryGetComponent<BaseEnemy>(out var enemy) ? enemy : null;
    }

    private void HitTarget(IDamageable target, Vector3 targetPosition)
    {
        float attackPower = DamageCalculator.CalculateMultipliers(
            baseStat.AttackPower,
            LevelUpStat.AttackPowerMultiplier,
            AbilityEffectStat.AttackPowerMultiplier,
            AcquiredHeroBonusDamage
        );
        float criticalRate = DamageCalculator.CalculateAdditives(
            baseStat.CriticalRate,
            LevelUpStat.CriticalRate,
            AbilityEffectStat.CriticalRate
        );
        float criticalDamage = DamageCalculator.CalculateAdditives(
            baseStat.CriticalDamage,
            LevelUpStat.CriticalDamage,
            AbilityEffectStat.CriticalDamage
        );
        float penetration = DamageCalculator.CalculateAdditives(
            baseStat.Penetration,
            LevelUpStat.Penetration,
            AbilityEffectStat.Penetration
        );

        var damageContext = new DamageContext(
            attackPower,
            criticalRate,
            criticalDamage,
            penetration,
            classType
        );
        target.TakeDamage(damageContext);

        // 스플래시 데미지 (타겟 위치 기준)
        float splashRange = DamageCalculator.CalculateMultipliers(
            baseStat.SplashRange,
            LevelUpStat.SplashRangeMultiplier,
            AbilityEffectStat.SplashRangeMultiplier
        );
        if (splashRange > 0)
        {
            ApplySplashDamage(targetPosition, splashRange, target, damageContext);
        }
    }

    private void ApplySplashDamage(Vector3 center, float range, IDamageable mainTarget, DamageContext damageContext)
    {
        var hits = Physics2D.OverlapCircleAll(center, range, enemyLayerMask);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out var damageable) && damageable != mainTarget)
            {
                damageable.TakeDamage(damageContext);
            }
        }
    }
}
