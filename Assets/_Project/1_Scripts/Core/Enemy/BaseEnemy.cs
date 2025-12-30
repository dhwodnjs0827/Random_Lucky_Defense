using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// 모든 Enemy의 부모 클래스
/// </summary>
[RequireComponent(typeof(SplineAnimate))]
public abstract class BaseEnemy : MonoBehaviour, IPoolable, IDetectable
{
    private static readonly int EnemyMoveAnimParam = Animator.StringToHash("1_Move");
    
    [SerializeField] private SplineAnimate splineAnimate;
    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] private Animator animator;

    private Vector3 previousPosition;
    private const float FLIP_THRESHOLD = 0.01f;
    
    public Transform Transform => transform;

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

    /// <summary>
    /// 적이 이동할 Spline 경로 SplineAnimate에 할당
    /// </summary>
    public void InitializeSpline(SplineContainer splineContainer)
    {
        if (splineAnimate != null && splineAnimate.Container == null)
        {
            splineAnimate.Container = splineContainer;
        }
    }

    /// <summary>
    /// 이동 시작 (SplineAnimate Restart 및 Move 애니메이션 시작)
    /// </summary>
    public void StartMove()
    {
        splineAnimate.Restart(true); // 경로 시작 지점으로 이동
        animator.SetBool(EnemyMoveAnimParam, true); // 이동 애니메이션 재생
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
        splineAnimate.AnimationMethod = SplineAnimate.Method.Speed; // Time과 Speed 중 Speed로 설정
        //TODO: EnemyData 기반으로 이동속도 설정으로 변경
        splineAnimate.MaxSpeed = 5f; // 이동속도 설정
        splineAnimate.Loop = SplineAnimate.LoopMode.Loop; // 경로 이동 Loop 설정
        splineAnimate.PlayOnAwake = false; // 생성 시, 바로 이동 안하게 설정
    }

    /// <summary>
    /// 적 이동 방향에 맞게 Flip 설정
    /// </summary>
    private void UpdateFlip()
    {
        var currentPosition = transform.position;
        var directionX = currentPosition.x - previousPosition.x;

        if (directionX > FLIP_THRESHOLD)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f); // 오른쪽 방향
        }
        else if (directionX < -FLIP_THRESHOLD)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f); // 왼쪽 방향
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
