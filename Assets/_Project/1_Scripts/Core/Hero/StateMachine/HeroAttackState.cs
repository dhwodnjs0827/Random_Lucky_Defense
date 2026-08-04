using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 영웅의 공격 상태
/// </summary>
public class HeroAttackState : BaseHeroState
{
    private static readonly int AttackAnimParam = Animator.StringToHash("2_Attack");

    private BaseEnemy targetEnemy;
    private float attackCooldown;

    private static BaseProjectile projectilePrefab;
    private static bool isLoaded;

    public HeroAttackState(BaseHero hero, HeroStateMachine heroStateMachine) : base(hero, heroStateMachine)
    {
    }

    public override void Enter()
    {
    }

    public override void Execute()
    {
        attackCooldown += Time.deltaTime;

        // 타겟 유효성 검사
        if (IsTargetValidity())
        {
            LookAtTarget();
            Attack();
        }
    }

    public override void Exit()
    {
        targetEnemy = null;
    }

    public static async UniTask PreLoadProjectileAsync()
    {
        if (isLoaded)
        {
            return;
        }

        projectilePrefab =
            await AddressableManager.Instance.LoadAsync<BaseProjectile>("Prefabs/Projectile/BaseProjectile");
        if (projectilePrefab != null)
        {
            isLoaded = true;
        }
        else
        {
            CDebug.LogError("[HeroAttackState] 영웅 투사체 로드 실패");
        }
    }

    public void SetProjectile(BaseProjectile projectile)
    {
        projectilePrefab = projectile;
    }

    /// <summary>
    /// 공격할 타겟 설정
    /// </summary>
    public void SetTarget(IDetectable target)
    {
        if (target is BaseEnemy)
        {
            targetEnemy = target as BaseEnemy;
        }
    }

    /// <summary>
    /// 쿨타임 기반 공격
    /// </summary>
    private void Attack()
    {
        var attackSpeed = DamageCalculator.CalculateMultipliers(hero.BaseStat.AttackSpeed,
            hero.LevelUpStat.AttackSpeedMultiplier, hero.AbilityEffectStat.AttackSpeedMultiplier);
        if (attackCooldown >= attackSpeed)
        {
            hero.Animator.SetTrigger(AttackAnimParam);
            CreateProjectile();
            attackCooldown = 0f;
        }
    }

    private void CreateProjectile()
    {
        var projectile = ObjectPoolManager.Instance.Get(projectilePrefab);
        projectile.transform.position = hero.transform.position;
        var projectileData = new ProjectileData
        (
            targetEnemy,
            hero.BaseStat,
            hero.LevelUpStat,
            hero.AbilityEffectStat,
            hero.AcquiredHeroBonusDamage,
            hero.ClassType
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
            stateMachine.ChangeState(stateMachine.IdleState);
            return false;
        }

        var distance = Vector2.Distance(hero.transform.position, targetEnemy.transform.position);
        var attackRange = DamageCalculator.CalculateMultipliers(hero.BaseStat.AttackRange,
            hero.LevelUpStat.AttackRangeMultiplier, hero.AbilityEffectStat.AttackRangeMultiplier);
        if (distance > attackRange)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return false;
        }

        return true;
    }

    /// <summary>
    /// 타겟 방향으로 Flip
    /// </summary>
    private void LookAtTarget()
    {
        var direction = (targetEnemy.transform.position - hero.transform.position).normalized;
        if (direction.x < 0)
        {
            hero.transform.rotation = Quaternion.Euler(0f, 0f, 0f); // 왼쪽
        }
        else if (direction.x > 0)
        {
            hero.transform.rotation = Quaternion.Euler(0f, 180f, 0f); // 오른쪽
        }
    }
}