using System;
using Cysharp.Threading.Tasks;
using Generated;
using UnityEngine;

public class AncientStatue : MonoBehaviour
{
    private HeroClassType classType;

    private HeroStat baseStat;

    [SerializeField] private Transform laserPoint;
    [SerializeField] private AncientStatueLaser laser;

    private int enemyLayerMask;
    private BaseEnemy targetEnemy;
    private bool isLaserActive;
    
    private bool isInitialized;
    
    private const string ANCIENT_STATUE_DATA_SO_PATH = "Data/SO/SummonData/Ancient_Statue";

    private HeroStat LevelUpStat => InGameManager.Instance.HeroBuffController.LevelUpStats[classType];
    private HeroStat AbilityEffectStat => InGameManager.Instance.HeroBuffController.AbilityEffectStats[classType];

    private float AcquiredHeroBonusDamage =>
        InGameManager.Instance.HeroBuffController.AcquiredHeroBonusDamages[classType];

    public async UniTask InitializeAsync()
    {
        try
        {
            var ancientStatueData = await AddressableManager.Instance.LoadAsync<SummonDataSO>(ANCIENT_STATUE_DATA_SO_PATH);
            classType = ancientStatueData.ClassType;
            baseStat = new HeroStat(ancientStatueData);
            enemyLayerMask = LayerMask.GetMask("Enemy");

            isInitialized = true;
        }
        catch (Exception e)
        {
            CDebug.LogError($"[AncientStatue] 초기화 실패: {e}]");
        }
    }
    
    private void Update()
    {
        if (!isInitialized)
        {
            return;
        }
        
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
            baseStat,
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
        var attackRange = DamageCalculator.CalculateMultipliers(baseStat.AttackRange, LevelUpStat.AttackRangeMultiplier,
            AbilityEffectStat.AttackRangeMultiplier);
        if (distance > attackRange)
        {
            return false;
        }

        return true;
    }
}
