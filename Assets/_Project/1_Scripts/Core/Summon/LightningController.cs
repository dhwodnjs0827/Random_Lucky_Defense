using System;
using Cysharp.Threading.Tasks;
using Generated;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightningController : MonoBehaviour
{
    [SerializeField] private Lightning lightning;

    private HeroClassType classType;
    private float attackTimer;

    private int enemyLayerMask;

    private HeroStat BaseStat => InGameManager.Instance.SummonController.SummonBaseStat[classType];
    private HeroStat LevelUpStat => InGameManager.Instance.HeroBuffController.LevelUpStats[classType];
    private HeroStat AbilityEffectStat => InGameManager.Instance.HeroBuffController.AbilityEffectStats[classType];

    private float AcquiredHeroBonusDamage =>
        InGameManager.Instance.HeroBuffController.AcquiredHeroBonusDamages[classType];

    private void Awake()
    {
        enemyLayerMask = LayerMask.GetMask("Enemy");
        classType = HeroClassType.Knight;
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            var attackSpeed = DamageCalculator.CalculateMultipliers(
                BaseStat.AttackSpeed,
                LevelUpStat.AttackSpeedMultiplier,
                AbilityEffectStat.AttackSpeedMultiplier
            );
            
            attackTimer = attackSpeed;
            TryAttack();
        }
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
            BaseStat.AttackRange,
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
            BaseStat.AttackPower,
            LevelUpStat.AttackPowerMultiplier,
            AbilityEffectStat.AttackPowerMultiplier,
            AcquiredHeroBonusDamage
        );
        float criticalRate = DamageCalculator.CalculateAdditives(
            BaseStat.CriticalRate,
            LevelUpStat.CriticalRate,
            AbilityEffectStat.CriticalRate
        );
        float criticalDamage = DamageCalculator.CalculateAdditives(
            BaseStat.CriticalDamage,
            LevelUpStat.CriticalDamage,
            AbilityEffectStat.CriticalDamage
        );
        float penetration = DamageCalculator.CalculateAdditives(
            BaseStat.Penetration,
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
            BaseStat.SplashRange,
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
