using UnityEngine;

public class BaseProjectile : MonoBehaviour, IPoolable
{
    [SerializeField] protected ParticleSystem projectileParticles;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float arrivalThreshold = 0.1f;

    protected ProjectileData projectileData;
    private float speed = 10f;
    private bool isFired;
    private Vector3 lastTargetPosition;

    private void FixedUpdate()
    {
        if (!isFired) return;

        FlyToTarget();
        CheckArrival();
    }

    public virtual void Initialize(ProjectileData data)
    {
        projectileData = data;
        lastTargetPosition = data.Target?.Transform.position ?? transform.position;
    }

    public void Fire()
    {
        isFired = true;
    }

    private void FlyToTarget()
    {
        // 타겟이 살아있으면 위치 갱신
        if (projectileData.Target?.Transform != null)
        {
            lastTargetPosition = projectileData.Target.Transform.position;
        }

        var direction = (lastTargetPosition - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    private void CheckArrival()
    {
        float distance = Vector3.Distance(transform.position, lastTargetPosition);
        if (distance <= arrivalThreshold)
        {
            OnArrived();
        }
    }

    private void OnArrived()
    {
        // 메인 타겟 데미지
        if (projectileData.Target?.Transform != null &&
            projectileData.Target.Transform.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(projectileData.Damage);
        }

        // 스플래시 데미지
        if (projectileData.SplashRange > 0)
        {
            ApplySplashDamage();
        }

        Release();
    }

    private void ApplySplashDamage()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, projectileData.SplashRange);

        foreach (var hit in hits)
        {
            // 메인 타겟 제외
            if (hit.transform == projectileData.Target?.Transform)
                continue;

            if (hit.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(projectileData.Damage);
            }
        }
    }

    private void Release()
    {
        ObjectPoolManager.Instance.Release(gameObject);
    }

    public virtual void OnGet()
    {
        isFired = false;
    }

    public virtual void OnRelease()
    {
        projectileData = default;
        rb.linearVelocity = Vector2.zero;
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, projectileData.SplashRange);
    }
#endif
}

public struct ProjectileData
{
    public readonly IDetectable Target;
    public readonly float Damage;
    public readonly float SplashRange;
    public readonly HeroClassType HeroClass;

    public ProjectileData(IDetectable target, float damage, float splashRange, HeroClassType heroClassType)
    {
        Target = target;
        Damage = damage;
        SplashRange = splashRange;
        HeroClass = heroClassType;
    }
}