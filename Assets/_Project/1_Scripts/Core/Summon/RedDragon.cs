using Generated;
using UnityEngine;

public class RedDragon : MonoBehaviour
{
    private HeroClassType classType;

    private HeroStat baseStat;
    private float attackCooldown;
    
    [SerializeField] private Animator animator;
    private static readonly int AttackAnimParam = Animator.StringToHash("Attack");

    private BaseProjectile projectilePrefab;

    private int enemyLayerMask;
    private BaseEnemy targetEnemy;
    
    private const string RED_DRAGON_DATA_SO_PATH = "Data/SO/SummonData/Red_Dragon";

    private HeroStat LevelUpStat => InGameManager.Instance.HeroBuffController.LevelUpStats[classType];
    private HeroStat AbilityEffectStat => InGameManager.Instance.HeroBuffController.AbilityEffectStats[classType];

    private float AcquiredHeroBonusDamage =>
        InGameManager.Instance.HeroBuffController.AcquiredHeroBonusDamages[classType];

    private void Awake()
    {
        var redDragonData = ResourceManager.Instance.Load<SummonDataSO>(RED_DRAGON_DATA_SO_PATH);
        classType = redDragonData.ClassType;
        baseStat = new HeroStat(redDragonData);
        enemyLayerMask = LayerMask.GetMask("Enemy");
        projectilePrefab = ResourceManager.Instance.Load<BaseProjectile>("Prefabs/Projectile/BaseProjectile");
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
            animator.SetTrigger(AttackAnimParam);
            CreateProjectile();
            attackCooldown = 0f;
        }
    }

    private void CreateProjectile()
    {
        var projectile = ObjectPoolManager.Instance.Get(projectilePrefab);
        projectile.transform.position = transform.position;
        var projectileData = new ProjectileData
        (
            targetEnemy,
            baseStat,
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
        var attackRange = DamageCalculator.CalculateMultipliers(baseStat.AttackRange, LevelUpStat.AttackRangeMultiplier,
            AbilityEffectStat.AttackRangeMultiplier);
        if (distance > attackRange)
        {
            return false;
        }

        return true;
    }
}