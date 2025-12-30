/// <summary>
/// 영웅의 상태 클래스 (Idle, Move, Attack 등)
/// </summary>
public abstract class BaseHeroState
{
    protected readonly BaseHero hero;
    protected readonly HeroStateMachine stateMachine;

    protected BaseHeroState(BaseHero baseHero, HeroStateMachine heroStateMachine)
    {
        hero = baseHero;
        stateMachine =  heroStateMachine;
    }
    

    /// <summary>
    /// 상태 진입 시, 호출
    /// </summary>
    public abstract void Enter();
    
    /// <summary>
    /// Update 메서드에서 호출
    /// </summary>
    public abstract void Execute();

    /// <summary>
    /// 상태 종료 시, 호출
    /// </summary>
    public abstract void Exit();
}