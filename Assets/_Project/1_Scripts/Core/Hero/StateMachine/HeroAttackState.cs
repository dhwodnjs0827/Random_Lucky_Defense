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
        attackCooldown += Time.deltaTime;
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
            CDebug.Log($"[AttackState] {targetEnemy.GetInstanceID()} 타겟팅 공격!");
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

    private void LookAtTarget()
    {
        var direction = (targetEnemy.transform.position - hero.transform.position).normalized;
        if (direction.x < 0)
        {
            hero.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if(direction.x > 0)
        {
            hero.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
}