using UnityEngine;

public class AncientStatue : MonoBehaviour
{
    private HeroClassType classType;

    [SerializeField] private Transform laserPoint;
    [SerializeField] private AncientStatueLaser laser;

    private int enemyLayerMask;
    private BaseEnemy targetEnemy;
    private bool isLaserActive;

    private HeroStat BaseStat => InGameManager.Instance.SummonController.SummonBaseStat[classType];
    private HeroStat LevelUpStat => InGameManager.Instance.HeroBuffController.LevelUpStats[classType];
    private HeroStat AbilityEffectStat => InGameManager.Instance.HeroBuffController.AbilityEffectStats[classType];

    private float AcquiredHeroBonusDamage =>
        InGameManager.Instance.HeroBuffController.AcquiredHeroBonusDamages[classType];

    private void Awake()
    {
        enemyLayerMask = LayerMask.GetMask("Enemy");
        classType = HeroClassType.Archer;
    }
    
    private void Update()
    {
        if (targetEnemy == null)
        {
            isLaserActive = false;
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
            isLaserActive = false;
            return;
        }

        if (!isLaserActive)
        {
            InitializeLaser();
            isLaserActive = true;
        }
    }

    private void InitializeLaser()
    {
        var laserData = new LaserData(
            targetEnemy,
            BaseStat,
            LevelUpStat,
            AbilityEffectStat,
            AcquiredHeroBonusDamage,
            classType
        );

        laser.Initialize(laserData, laserPoint);
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
