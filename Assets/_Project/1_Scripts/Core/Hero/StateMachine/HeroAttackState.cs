using UnityEngine;

/// <summary>
/// 영웅의 공격 상태
/// </summary>
public class HeroAttackState : BaseHeroState
{
    private static readonly int AttackAnimParam = Animator.StringToHash("2_Attack");

    private BaseEnemy targetEnemy;
    private float attackCooldown;
    
    private BaseProjectile projectilePrefab;

    public HeroAttackState(BaseHero hero, HeroStateMachine heroStateMachine) : base(hero, heroStateMachine)
    {
        projectilePrefab = ResourceManager.Instance.Load<BaseProjectile>("Prefabs/Projectile/BaseProjectile");
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
        if (attackCooldown >= hero.Stat.AttackSpeed)
        {
            //TODO: 투사체 생성
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
            hero.Stat.AttackPower,
            hero.Stat.SplashRange,
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
        if (distance > hero.Stat.AttackRange)
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