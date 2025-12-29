using Generated;
using UnityEngine;

/// <summary>
/// 모든 영웅의 부모 클래스
/// </summary>
public abstract class BaseHero : MonoBehaviour, IPoolable
{
    [SerializeField] protected SPUM_Prefabs prefab;
    [SerializeField] protected Animator animator;
    protected HeroStateMachine stateMachine;
    
    protected HeroDataSO heroData; // 영웅 데이터
    protected IHeroSkill skill;

    public abstract HeroClassType ClassType { get; }
    public virtual HeroGradeType GradeType => HeroGradeType.Normal;
    public Animator Animator => animator;

    private void Awake()
    {
        stateMachine = new HeroStateMachine(this);
        stateMachine?.ChangeState(stateMachine?.IdleState);
    }

    private void Update()
    {
        stateMachine?.Execute();
    }

    /// <summary>
    /// 영웅 초기화
    /// </summary>
    public virtual void Initialize(HeroDataSO data)
    {
        CDebug.Log($"[BaseHero] {data.ID} 데이터 초기화");
    }

    /// <summary>
    /// 기본 공격
    /// </summary>
    public abstract void Attack(BaseEnemy target);

    /// <summary>
    /// 기본 이동
    /// </summary>
    public void Move(Vector3 position)
    {
        stateMachine?.MoveState?.SetTargetPosition(position);
        stateMachine?.ChangeState(stateMachine?.MoveState);
    }
    
    public abstract void OnGet();

    public abstract void OnRelease();
}
