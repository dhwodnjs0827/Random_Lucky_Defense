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
        isFired = false;

        // 메인 타겟 데미지
        if (projectileData.Target?.Transform != null &&
            projectileData.Target.Transform.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(projectileData.AttackPower);
        }

        //TODO: HitEffect 재생

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
                damageable.TakeDamage(projectileData.AttackPower);
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
}

public readonly struct ProjectileData
{
    public readonly IDetectable Target;
    private readonly HeroStat HeroStat;
    private readonly HeroStat LevelUpStat;
    private readonly HeroStat CardEffectStat;
    public readonly HeroClassType HeroClass;

    public float AttackPower => DamageCalculator.CalculateMultipliers(HeroStat.AttackPower,
        LevelUpStat.AttackPowerMultiplier, CardEffectStat.AttackPowerMultiplier);

    public float CriticalRate =>
        DamageCalculator.CalculateAdditives(HeroStat.CriticalRate, LevelUpStat.CriticalRate,
            CardEffectStat.CriticalRate);

    public float CriticalDamage => DamageCalculator.CalculateAdditives(HeroStat.CriticalDamage,
        LevelUpStat.CriticalDamage, CardEffectStat.CriticalDamage);

    public float SplashRange => DamageCalculator.CalculateMultipliers(HeroStat.SplashRange,
        LevelUpStat.SplashRangeMultiplier, CardEffectStat.SplashRangeMultiplier);

    public ProjectileData(IDetectable target, HeroStat heroStat, HeroStat levelUpStat, HeroStat cardEffectStat,
        HeroClassType heroClassType)
    {
        Target = target;
        HeroStat = heroStat;
        LevelUpStat = levelUpStat;
        CardEffectStat = cardEffectStat;
        HeroClass = heroClassType;
    }
}