using UnityEngine;

/// <summary>
/// 영웅의 기본 상태
/// </summary>
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

    /// <summary>
    /// 공격 범위 내 타겟 찾기
    /// </summary>
    private void FindTarget()
    {
        var attackRange = DamageCalculator.CalculateMultipliers(hero.BaseStat.AttackRange,
            hero.LevelUpStat.AttackRangeMultiplier, hero.AbilityEffectStat.AttackRangeMultiplier);
        var hits = Physics2D.OverlapCircleAll(hero.transform.position, attackRange, enemyLayerMask);

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