/// <summary>
/// 영웅의 상태 머신 클래스
/// </summary>
public class HeroStateMachine
{
    private HeroIdleState idleState;
    private HeroMoveState moveState;
    private HeroAttackState attackState;
    
    private BaseHeroState currentState;
    
    public HeroIdleState IdleState => idleState;
    public HeroMoveState MoveState => moveState;
    public HeroAttackState AttackState => attackState;

    public HeroStateMachine(BaseHero hero)
    {
        InitializeState(hero);
        
        // IdleState로 시작
        currentState = idleState;
    }

    /// <summary>
    /// State 전환
    /// </summary>
    public void ChangeState(BaseHeroState nextState)
    {
        currentState?.Exit();
        currentState = nextState;
        currentState.Enter();
    }

    public void Execute()
    {
        currentState?.Execute();
    }
    
    /// <summary>
    /// HeroState 목록 초기화
    /// </summary>
    private void InitializeState(BaseHero hero)
    {
        idleState = new HeroIdleState(hero, this);
        moveState = new HeroMoveState(hero, this);
        attackState = new HeroAttackState(hero, this);
    }
}
