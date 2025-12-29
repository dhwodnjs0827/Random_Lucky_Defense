using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// 모든 Enemy의 부모 클래스
/// </summary>
[RequireComponent(typeof(SplineAnimate))]
public abstract class BaseEnemy : MonoBehaviour
{
    private static readonly int EnemyMoveAnimParam = Animator.StringToHash("1_Move");
    [SerializeField] private SplineAnimate splineAnimate;
    [SerializeField] private Animator animator;

    protected virtual void Awake()
    {
        SetSplineAnimateComponent();
    }

    /// <summary>
    /// 적 초기화
    /// </summary>
    public abstract void Initialize();

    public void InitializeSpline(SplineContainer splineContainer)
    {
        if (splineAnimate != null && splineAnimate.Container == null)
        {
            splineAnimate.Container = splineContainer;
        }
    }

    public void StartMove()
    {
        splineAnimate.Restart(true);
        animator.SetBool(EnemyMoveAnimParam, true);
    }

    /// <summary>
    /// SplineAnimate 컴포넌트 추가 및 초기화
    /// </summary>
    private void SetSplineAnimateComponent()
    {
        if (splineAnimate == null)
        {
            splineAnimate = gameObject.AddComponent<SplineAnimate>();
        }

        splineAnimate.Alignment = SplineAnimate.AlignmentMode.None;
        splineAnimate.AnimationMethod = SplineAnimate.Method.Speed;
        splineAnimate.MaxSpeed = 5f;
        splineAnimate.Loop = SplineAnimate.LoopMode.Loop;
        splineAnimate.PlayOnAwake = false;
    }
}
