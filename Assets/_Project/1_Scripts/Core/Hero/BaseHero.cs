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
    
    protected HeroStat baseStat;

    public Animator Animator => animator;
    
    public abstract HeroClassType ClassType { get; }
    public HeroGradeType GradeType => heroData.GradeType;
    public HeroStat BaseStat => baseStat;
    public HeroStat LevelUpStat => InGameManager.Instance.HeroBuffController.LevelUpStats[ClassType];
    public HeroStat CardEffectStat => InGameManager.Instance.HeroBuffController.CardEffectStats[ClassType];

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
        baseStat = new HeroStat(data);
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
    }

    public virtual void OnRelease()
    {
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, baseStat.AttackRange * LevelUpStat.AttackRangeMultiplier * CardEffectStat.AttackRangeMultiplier);
    }
#endif
}