using UnityEngine;

public class HeroAttackState : BaseHeroState
{
    private static readonly int AttackAnimParam = Animator.StringToHash("2_Attack");

    private BaseEnemy targetEnemy;

    public HeroAttackState(BaseHero hero, HeroStateMachine heroStateMachine) : base(hero, heroStateMachine)
    {
    }

    public override void Enter()
    {
        hero.Animator.SetTrigger(AttackAnimParam);
    }

    public override void Execute()
    {
        Attack();
    }

    public override void Exit()
    {
        targetEnemy = null;
    }

    public void SetTarget(BaseEnemy target)
    {
        targetEnemy =  target;
    }

    private void Attack()
    {
        //TODO: 투사체 생성
        CDebug.Log("[AttackState] 투사체 공격");
    }
}
