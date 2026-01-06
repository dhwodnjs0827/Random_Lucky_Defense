using UnityEngine;

/// <summary>
/// 영웅의 이동 상태
/// </summary>
public class HeroMoveState : BaseHeroState
{
    private static readonly int MoveAnimParam = Animator.StringToHash("1_Move");
    
    private Vector3 targetPosition;
    private Vector3 previousPosition;
    private const float FLIP_THRESHOLD = 0.01f;
    
    public HeroMoveState(BaseHero hero, HeroStateMachine heroStateMachine) : base(hero,  heroStateMachine)
    {
    }

    public override void Enter()
    {
        hero.Animator.SetBool(MoveAnimParam, true);
    }

    public override void Execute()
    {
        Move();
        Flip();
    }

    public override void Exit()
    {
        hero.Animator.SetBool(MoveAnimParam, false);
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    /// <summary>
    /// 영웅 이동
    /// </summary>
    private void Move()
    {
        var moveSpeed = DamageCalculator.CalculateMultipliers(hero.BaseStat.MoveSpeed , hero.LevelUpStat.MoveSpeedMultiplier, hero.CardEffectStat.MoveSpeedMultiplier);
        var movePos = Vector3.MoveTowards(hero.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        hero.transform.position = movePos;
        if (hero.transform.position == targetPosition)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    /// <summary>
    /// 이동 방향으로 Flip
    /// </summary>
    private void Flip()
    {
        var currentPosition = hero.transform.position;
        var directionX = currentPosition.x - previousPosition.x;

        if (directionX > FLIP_THRESHOLD)
        {
            hero.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (directionX < -FLIP_THRESHOLD)
        {
            hero.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        previousPosition = currentPosition;
    }
}
