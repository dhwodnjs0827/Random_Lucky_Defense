using Generated;
using UnityEngine;

/// <summary>
/// 모든 영웅의 부모 클래스
/// </summary>
public abstract class BaseHero : MonoBehaviour, IPoolable
{
    [SerializeField] protected SPUM_Prefabs prefab;
    [SerializeField] protected Animator animator;
    
    protected HeroDataSO heroData; // 영웅 데이터
    protected IHeroSkill skill;

    /// <summary>
    /// 영웅 초기화
    /// </summary>
    public abstract void Initialize(HeroDataSO data);

    /// <summary>
    /// 기본 공격
    /// </summary>
    public abstract void Attack(BaseEnemy target);
    
    /// <summary>
    /// 기본 이동
    /// </summary>
    public abstract void Move();
    
    public abstract void OnGet();

    public abstract void OnRelease();
}
