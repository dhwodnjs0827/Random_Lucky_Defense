public abstract class BaseHeroState
{
    protected readonly BaseHero hero;
    protected readonly HeroStateMachine stateMachine;

    protected BaseHeroState(BaseHero baseHero, HeroStateMachine heroStateMachine)
    {
        hero = baseHero;
        stateMachine =  heroStateMachine;
    }
    

    public abstract void Enter();

    public abstract void Execute();

    public abstract void Exit();
}