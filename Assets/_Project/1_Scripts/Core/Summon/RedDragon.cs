using UnityEngine;

public class RedDragon : MonoBehaviour
{
    private HeroClassType classType;
    
    private float attackCooldown;
    
    [SerializeField] private Animator animator;
    private static readonly int AttackAnimParam = Animator.StringToHash("Attack");

    [SerializeField] private Transform projectilePoint;
    [SerializeField] private RedDragonProjectile projectilePrefab;

    private int enemyLayerMask;
    private BaseEnemy targetEnemy;
    
    private HeroStat BaseStat => InGameManager.Instance.SummonController.SummonBaseStat[classType];
    private HeroStat LevelUpStat => InGameManager.Instance.HeroBuffController.LevelUpStats[classType];
    private HeroStat AbilityEffectStat => InGameManager.Instance.HeroBuffController.AbilityEffectStats[classType];

    private float AcquiredHeroBonusDamage =>
        InGameManager.Instance.HeroBuffController.AcquiredHeroBonusDamages[classType];

    private void Awake()
    {
        enemyLayerMask = LayerMask.GetMask("Enemy");
        classType = HeroClassType.Magician;
    }

    private void Update()
    {
        attackCooldown += Time.deltaTime;

        if (targetEnemy == null)
        {
            FindTarget();
        }
        else
        {
            Attack();
        }
    }

    /// <summary>
    /// 공격 범위 내 타겟 찾기
    /// </summary>
    private void FindTarget()
    {
        var attackRange = DamageCalculator.CalculateMultipliers(BaseStat.AttackRange,
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

        var attackSpeed = DamageCalculator.CalculateMultipliers(BaseStat.AttackSpeed, LevelUpStat.AttackSpeedMultiplier,
            AbilityEffectStat.AttackSpeedMultiplier);
        if (attackCooldown >= attackSpeed)
        {
            animator.SetTrigger(AttackAnimParam);
            CreateProjectile();
            attackCooldown = 0f;
        }
    }

    private void CreateProjectile()
    {
        var projectile = ObjectPoolManager.Instance.Get(projectilePrefab);
        projectile.transform.position = projectilePoint.position;
        var projectileData = new ProjectileData
        (
            targetEnemy,
            BaseStat,
            LevelUpStat,
            AbilityEffectStat,
            AcquiredHeroBonusDamage,
            classType
        );
        projectile.Initialize(projectileData);
        projectile.Fire();
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
        var attackRange = DamageCalculator.CalculateMultipliers(BaseStat.AttackRange, LevelUpStat.AttackRangeMultiplier,
            AbilityEffectStat.AttackRangeMultiplier);
        if (distance > attackRange)
        {
            return false;
        }

        return true;
    }
}