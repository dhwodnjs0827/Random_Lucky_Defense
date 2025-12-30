using UnityEngine;

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
        CDebug.Log("[HeroMoveState] 이동 상태");
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

    private void Move()
    {
        //TODO: 임시 이동속도 5
        var movePos = Vector3.MoveTowards(hero.transform.position, targetPosition, 5f * Time.deltaTime);
        hero.transform.position = movePos;
        if (hero.transform.position == targetPosition)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

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
