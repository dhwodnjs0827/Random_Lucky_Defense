public abstract class BaseHeroState
{
    protected BaseHero hero;
    protected HeroStateMachine stateMachine;

    protected BaseHeroState(BaseHero baseHero, HeroStateMachine heroStateMachine)
    {
        hero = baseHero;
        stateMachine =  heroStateMachine;
    }
    

    public abstract void Enter();

    public virtual void Execute()
    {
        
    }

    public abstract void Exit();
}