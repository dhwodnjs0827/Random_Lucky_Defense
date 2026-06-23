using Generated;
using UnityEngine;

//TODO: 공격 로직 임시 작성. 공격 로직 전체적인 수정 필요.
// 임시로 baseStat 사용
public class Lightning : MonoBehaviour
{
    private HeroClassType classType;

    private HeroStat baseStat;
    private float attackCooldown;

    private int enemyLayerMask;
    private BaseEnemy targetEnemy;
    
    private const string LIGHTNING_DATA_SO_PATH = "Data/SO/SummonData/30000";

    private HeroStat LevelUpStat => InGameManager.Instance.HeroBuffController.LevelUpStats[classType];
    private HeroStat AbilityEffectStat => InGameManager.Instance.HeroBuffController.AbilityEffectStats[classType];

    private float AcquiredHeroBonusDamage =>
        InGameManager.Instance.HeroBuffController.AcquiredHeroBonusDamages[classType];

    private void Awake()
    {
        var lightningData = ResourceManager.Instance.Load<SummonDataSO>(LIGHTNING_DATA_SO_PATH);
        classType = lightningData.ClassType;
        baseStat = new HeroStat(lightningData);
        enemyLayerMask = LayerMask.GetMask("Enemy");
    }

    public void IncreaseStat(AbilityContainer abilityContainer)
    {
        baseStat.IncreaseAttackSpeed(abilityContainer.AbilityLevelData.value);
        baseStat.IncreaseAttackPower(abilityContainer.AbilityLevelData.value1);
    }

    /// <summary>
    /// 공격 범위 내 타겟 찾기
    /// </summary>
    private void FindTarget()
    {
        var attackRange = DamageCalculator.CalculateMultipliers(baseStat.AttackRange,
            LevelUpStat.AttackRangeMultiplier, AbilityEffectStat.AttackRangeMultiplier);
        var hits = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayerMask);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<IDetectable>(out var target))
            {
                SetTarget(target);
            }
        }
    }

    /// <summary>
    /// 공격할 타겟 설정
    /// </summary>
    private void SetTarget(IDetectable target)
    {
        if (target is BaseEnemy enemy)
        {
            targetEnemy = enemy;
        }
    }

    private void Attack()
    {
        // 타겟 유효성 검사
        if (!IsTargetValidity())
        {
            targetEnemy = null;
            return;
        }

        var attackSpeed = DamageCalculator.CalculateMultipliers(baseStat.AttackSpeed, LevelUpStat.AttackSpeedMultiplier,
            AbilityEffectStat.AttackSpeedMultiplier);
        if (attackCooldown >= attackSpeed)
        {
            HitTarget(targetEnemy);
            attackCooldown = 0f;
        }
    }

    /// <summary>
    /// 타겟의 유효성 검사
    /// </summary>
    private bool IsTargetValidity()
    {
        if (targetEnemy == null || targetEnemy.gameObject.activeSelf == false)
        {
            return false;
        }

        var distance = Vector2.Distance(transform.position, targetEnemy.transform.position);
        var attackRange = DamageCalculator.CalculateMultipliers(baseStat.AttackRange, LevelUpStat.AttackRangeMultiplier,
            AbilityEffectStat.AttackRangeMultiplier);
        if (distance > attackRange)
        {
            return false;
        }

        return true;
    }
    
    private void HitTarget(IDamageable target)
    {
        // 메인 타겟 데미지
        var damageContext = new DamageContext
        (
            baseStat.AttackPower,
            baseStat.CriticalRate,
            baseStat.CriticalDamage,
            baseStat.Penetration,
            classType
        );
        target.TakeDamage(damageContext);

        //TODO: HitEffect 재생

        // 스플래시 데미지
        if (baseStat.SplashRange > 0)
        {
            ApplySplashDamage();
        }
    }
    
    private void ApplySplashDamage()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, baseStat.SplashRange);

        foreach (var hit in hits)
        {
            // 메인 타겟 제외
            if (hit.transform == targetEnemy?.Transform)
                continue;

            if (hit.TryGetComponent<IDamageable>(out var damageable))
            {
                var damageContext = new DamageContext
                (
                    baseStat.AttackPower,
                    baseStat.CriticalRate,
                    baseStat.CriticalDamage,
                    baseStat.Penetration,
                    classType
                );
                damageable.TakeDamage(damageContext);
            }
        }
    }
}
