using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// 모든 Enemy의 부모 클래스
/// </summary>
[RequireComponent(typeof(SplineAnimate))]
public abstract class BaseEnemy : MonoBehaviour, IPoolable
{
    private static readonly int EnemyMoveAnimParam = Animator.StringToHash("1_Move");
    
    [SerializeField] private SplineAnimate splineAnimate;
    [SerializeField] private Animator animator;

    private Vector3 previousPosition;
    private const float FlipThreshold = 0.01f;

    private void Awake()
    {
        SetSplineAnimateComponent();
    }
    
    private void Update()
    {
        UpdateFlip();
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

    private void UpdateFlip()
    {
        var currentPosition = transform.position;
        var directionX = currentPosition.x - previousPosition.x;

        if (directionX > FlipThreshold)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (directionX < -FlipThreshold)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        previousPosition = currentPosition;
    }

    public void OnGet()
    {
        splineAnimate.Restart(false);
        previousPosition = transform.position;
    }

    public void OnRelease()
    {
        splineAnimate.Pause();
    }
}
