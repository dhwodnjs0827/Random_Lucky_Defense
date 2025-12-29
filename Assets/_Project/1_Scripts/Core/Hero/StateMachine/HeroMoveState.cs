using UnityEngine;

public class HeroMoveState : BaseHeroState
{
    private static readonly int MoveAnimParam = Animator.StringToHash("1_Move");
    
    private Vector3 targetPosition;
    
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
}
