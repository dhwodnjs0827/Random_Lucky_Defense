using UnityEngine;

public class HeroIdleState : BaseHeroState
{
    private readonly int enemyLayerMask;

    public HeroIdleState(BaseHero hero, HeroStateMachine heroStateMachine) : base(hero, heroStateMachine)
    {
        enemyLayerMask = LayerMask.GetMask("Enemy");
    }

    public override void Enter()
    {
    }

    public override void Execute()
    {
        FindTarget();
    }

    public override void Exit()
    {
    }

    private void FindTarget()
    {
        var hits = Physics2D.OverlapCircleAll(hero.transform.position, hero.AttackRange, enemyLayerMask);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<IDetectable>(out var target))
            {
                stateMachine.ChangeState(stateMachine.AttackState);
                stateMachine.AttackState.SetTarget(target);
            }
        }
    }
}