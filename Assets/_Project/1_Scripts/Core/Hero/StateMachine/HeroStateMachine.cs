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
        idleState = new HeroIdleState(hero, this);
        moveState = new HeroMoveState(hero, this);
        attackState = new HeroAttackState(hero, this);
    }
}
