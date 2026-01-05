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
    protected BaseHeroCardEffectHandler effectHandler;

    protected HeroDataSO heroData; // 영웅 데이터
    protected IHeroSkill skill;

    protected float attackPower; // 공격력
    protected float attackSpeed; // 공격속도
    protected float attackRange; // 공격범위
    protected float splashRange; // 스플래쉬 범위

    public Animator Animator => animator;
    
    public abstract HeroClassType ClassType { get; }
    public HeroGradeType GradeType => heroData.GradeType;
    public float AttackPower => attackPower;
    public float AttackSpeed => attackSpeed;
    public float AttackRange => attackRange;
    public float SplashRange => splashRange;

    protected virtual void Awake()
    {
        // 상태머신 초기화
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
    public void Initialize(HeroDataSO data)
    {
        heroData = data;

        attackPower = heroData.AttackPower;
        attackSpeed = heroData.AttackSpeed;
        attackRange = heroData.AttackRange / 50f;
        splashRange = heroData.SplashRange / 50f;
    }

    /// <summary>
    /// 영웅 이동
    /// </summary>
    public void Move(Vector3 position)
    {
        stateMachine?.MoveState?.SetTargetPosition(position);
        stateMachine?.ChangeState(stateMachine?.MoveState);
    }

    public virtual void OnGet()
    {
        effectHandler.RegisterCardEffect();
    }

    public virtual void OnRelease()
    {
        effectHandler.UnregisterCardEffect();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
#endif
}