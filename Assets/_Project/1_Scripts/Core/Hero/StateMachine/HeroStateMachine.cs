public class HeroStateMachine
{
    private IdleState idleState;
    private MoveState moveState;
    private AttackState attackState;
    
    private BaseHeroState currentState;
    
    public IdleState IdleState => idleState;
    public MoveState MoveState => moveState;
    public AttackState AttackState => attackState;

    public HeroStateMachine(BaseHero hero)
    {
        InitializeState(hero);
        
        currentState = idleState;
    }

    public void ChangeState(BaseHeroState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Execute()
    {
        currentState?.Execute();
    }
    
    private void InitializeState(BaseHero hero)
    {
        idleState = new IdleState(hero, this);
        moveState = new MoveState(hero, this);
        attackState = new AttackState(hero, this);
    }
}
