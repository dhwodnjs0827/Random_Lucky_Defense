using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// 모든 Enemy의 부모 클래스
/// </summary>
[RequireComponent(typeof(SplineAnimate))]
public abstract class BaseEnemy : MonoBehaviour, IPoolable, IDetectable, IDamageable
{
    private static readonly int EnemyMoveAnimParam = Animator.StringToHash("1_Move");

    [SerializeField] private GameObject rootEnemyObject;
    [SerializeField] private SplineAnimate splineAnimate;
    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] private Animator animator;

    private SpriteRenderer[] spriteRenderers;
    private Color[] originalColors;
    private CancellationTokenSource flashCts;

    private Vector3 previousPosition;
    private const float FLIP_THRESHOLD = 0.01f;

    public Transform Transform => transform;

    private void Awake()
    {
        InitializeSpriteRenderer();
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

    private void InitializeSpriteRenderer()
    {
        if (rootEnemyObject != null)
        {
            spriteRenderers = rootEnemyObject.GetComponentsInChildren<SpriteRenderer>();
            originalColors = new Color[spriteRenderers.Length];
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                originalColors[i] = spriteRenderers[i].color;
            }
        }
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

    public void TakeDamage()
    {
        HitEffect();
        CDebug.Log("[BaseEnemy] 피격 받음!");
    }

    public void HitEffect()
    {
        HitFlash().Forget();
    }

    private async UniTask HitFlash()
    {
        // 기존 플래시 취소
        flashCts?.Cancel();
        flashCts?.Dispose();
        flashCts = new CancellationTokenSource();
        var token = flashCts.Token;

        // 빨간색으로 변경
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = Color.red;
        }

        try
        {
            await UniTask.Delay(100, cancellationToken: token);
        }
        catch (OperationCanceledException)
        {
            return; // 취소되면 복구하지 않음 (새 플래시가 처리)
        }

        // 원래 색상으로 복구
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = originalColors[i];
        }
    }
}