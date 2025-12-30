using UnityEngine;

public class HeroAttackState : BaseHeroState
{
    private static readonly int AttackAnimParam = Animator.StringToHash("2_Attack");

    private BaseEnemy targetEnemy;
    private float attackCooldown;

    public HeroAttackState(BaseHero hero, HeroStateMachine heroStateMachine) : base(hero, heroStateMachine)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Execute()
    {
        CDebug.Log("[HeroAttackState] 공격 상태");
        attackCooldown += Time.deltaTime;
        if (IsTargetValidity())
        {
            Attack();
        }
    }

    public override void Exit()
    {
        targetEnemy = null;
    }

    public void SetTarget(IDetectable target)
    {
        if (target is BaseEnemy)
        {
            targetEnemy = target as BaseEnemy;
        }
    }

    private void Attack()
    {
        if (attackCooldown >= hero.AttackSpeed)
        {
            //TODO: 투사체 생성
            hero.Animator.SetTrigger(AttackAnimParam);
            CDebug.Log("[AttackState] 투사체 공격");
            attackCooldown = 0f;
        }
    }

    private bool IsTargetValidity()
    {
        if (targetEnemy == null || targetEnemy.gameObject.activeSelf == false)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return false;
        }

        var distance = Vector2.Distance(hero.transform.position, targetEnemy.transform.position);
        if (distance > hero.AttackRange)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return false;
        }

        return true;
    }
}