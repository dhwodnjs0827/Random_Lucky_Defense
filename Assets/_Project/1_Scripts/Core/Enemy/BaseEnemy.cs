using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Generated;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// 모든 Enemy의 부모 클래스
/// </summary>
[RequireComponent(typeof(SplineAnimate))]
public abstract class BaseEnemy : MonoBehaviour, IPoolable, IDetectable, IDamageable
{
    private static readonly int EnemyMoveAnimParam = Animator.StringToHash("1_Move");

    [SerializeField] private GameObject enemyObject;
    [SerializeField] private GameObject rootEnemyObject;
    [SerializeField] private SplineAnimate splineAnimate;
    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshPro healthText;

    private SpriteRenderer[] spriteRenderers;
    private Color[] originalColors;
    private CancellationTokenSource flashCts;

    private Vector3 previousPosition;
    private const float FLIP_THRESHOLD = 0.01f;

    protected EnemyDataSO enemyData;
    protected float maxHealth;
    protected float currentHealth;
    protected float defense;
    protected float moveSpeed;

    public Transform Transform => transform;
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

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
    public virtual void Initialize(EnemyDataSO data, SplineContainer splineContainer)
    {
        InitializeSpline(splineContainer);
        InitializeEnemyData(data);
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
        splineAnimate.MaxSpeed = moveSpeed; // 이동속도 설정
        splineAnimate.Loop = SplineAnimate.LoopMode.Loop; // 경로 이동 Loop 설정
        splineAnimate.PlayOnAwake = false; // 생성 시, 바로 이동 안하게 설정
    }

    /// <summary>
    /// 자식의 SpriteRender 및 Color 저장
    /// </summary>
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
    /// 적이 이동할 Spline 경로 SplineAnimate에 할당
    /// </summary>
    private void InitializeSpline(SplineContainer splineContainer)
    {
        if (splineAnimate != null && splineAnimate.Container == null)
        {
            splineAnimate.Container = splineContainer;
        }
    }

    /// <summary>
    /// 적 데이터 초기화
    /// </summary>
    private void InitializeEnemyData(EnemyDataSO data)
    {
        enemyData = data;
        maxHealth = data.Health;
        currentHealth = maxHealth;
        healthText.text = $"{currentHealth:N0}";
        moveSpeed = data.MoveSpeed;
        splineAnimate.MaxSpeed = moveSpeed;
        defense = data.Defense;
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
            enemyObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f); // 오른쪽 방향
        }
        else if (directionX < -FLIP_THRESHOLD)
        {
            enemyObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f); // 왼쪽 방향
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

    public void TakeDamage(float damage)
    {
        HitEffect();
        CDebug.Log("[BaseEnemy] 피격 받음!");

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0f);
        healthText.text = $"{currentHealth:N0}";

        if (currentHealth <= 0)
        {
            Die();
        }
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

    public virtual void Die()
    {
        EventManager.Dispatch(GameEventType.EnemyDie);
        ObjectPoolManager.Instance.Release(gameObject);
        CDebug.Log("[BaseEnemy] 적 사망");
    }
}