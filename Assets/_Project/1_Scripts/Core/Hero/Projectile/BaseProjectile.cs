using UnityEngine;

public class BaseProjectile : MonoBehaviour, IPoolable
{
    [SerializeField] protected ParticleSystem projectileParticles;

    protected ProjectileData projectileData;
    private bool isFired;
    private Vector3 lastTargetPosition;

    private void Update()
    {
        if (!isFired) return;

        FlyToTarget();
        CheckTargetValidity();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFired) return;
        if (projectileData.Target == null || projectileData.Target.Transform == null) return;
        if (collision.transform != projectileData.Target.Transform) return;
        
        if (collision.TryGetComponent<IDamageable>(out var damageable))
        {
            isFired = false;
            HitTarget(damageable);
        }
    }

    public virtual void Initialize(ProjectileData data)
    {
        projectileData = data;
        if (data.Target != null && data.Target.Transform != null && projectileData.Target.Transform.gameObject.activeSelf)
        {
            lastTargetPosition = data.Target.Transform.position;
        }
        else
        {
            Release();
        }
    }

    public void Fire()
    {
        isFired = true;
    }

    private void FlyToTarget()
    {
        // 타겟이 살아있으면 위치 갱신
        if (projectileData.Target != null && projectileData.Target.Transform != null && projectileData.Target.Transform.gameObject.activeSelf)
        {
            lastTargetPosition = projectileData.Target.Transform.position;
        }
        var movePos = Vector3.MoveTowards(transform.position, lastTargetPosition, GameConstants.PROJECTILE_SPEED * Time.deltaTime);
        transform.position = movePos;
    }

    private void CheckTargetValidity()
    {
        if (projectileData.Target == null || projectileData.Target.Transform == null || !projectileData.Target.Transform.gameObject.activeSelf)
        {
            if (transform.position == lastTargetPosition)
            {
                Release();
            }
        }
    }

    private void HitTarget(IDamageable target)
    {
        // 메인 타겟 데미지
        var damageContext = new DamageContext
        (
            projectileData.AttackPower,
            projectileData.CriticalRate,
            projectileData.CriticalDamage,
            projectileData.Penetration,
            projectileData.HeroClass
        );
        target.TakeDamage(damageContext);

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
                var damageContext = new DamageContext
                (
                    projectileData.AttackPower,
                    projectileData.CriticalRate,
                    projectileData.CriticalDamage,
                    projectileData.Penetration,
                    projectileData.HeroClass
                );
                damageable.TakeDamage(damageContext);
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
        lastTargetPosition = Vector2.zero;
        isFired = false;
    }
}

public readonly struct ProjectileData
{
    public readonly IDetectable Target;
    private readonly HeroStat HeroStat;
    private readonly HeroStat LevelUpStat;
    private readonly HeroStat AbilityEffectStat;
    private readonly float AcquiredHeroBonusDamage;
    public readonly HeroClassType HeroClass;

    public float AttackPower => DamageCalculator.CalculateMultipliers(HeroStat.AttackPower,
        LevelUpStat.AttackPowerMultiplier, AbilityEffectStat.AttackPowerMultiplier, AcquiredHeroBonusDamage);

    public float CriticalRate =>
        DamageCalculator.CalculateAdditives(HeroStat.CriticalRate, LevelUpStat.CriticalRate,
            AbilityEffectStat.CriticalRate);

    public float CriticalDamage => DamageCalculator.CalculateAdditives(HeroStat.CriticalDamage,
        LevelUpStat.CriticalDamage, AbilityEffectStat.CriticalDamage);

    public float SplashRange => DamageCalculator.CalculateMultipliers(HeroStat.SplashRange,
        LevelUpStat.SplashRangeMultiplier, AbilityEffectStat.SplashRangeMultiplier);

    public float Penetration => DamageCalculator.CalculateAdditives(HeroStat.Penetration,
        LevelUpStat.Penetration, AbilityEffectStat.Penetration);

    public ProjectileData(IDetectable target, HeroStat heroStat, HeroStat levelUpStat, HeroStat cardEffectStat, float acquiredHeroBonusDamage,
        HeroClassType heroClassType)
    {
        Target = target;
        HeroStat = heroStat;
        LevelUpStat = levelUpStat;
        AbilityEffectStat = cardEffectStat;
        AcquiredHeroBonusDamage = acquiredHeroBonusDamage;
        HeroClass = heroClassType;
    }
}