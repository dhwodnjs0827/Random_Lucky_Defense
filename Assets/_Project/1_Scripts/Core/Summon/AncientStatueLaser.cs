using UnityEngine;

public class AncientStatueLaser : MonoBehaviour
{
    [SerializeField] private LineRenderer laserRenderer;

    private LaserData laserData;
    private Transform laserStartPoint;
    private float attackTimer;

    private IDamageable target;

    private void Update()
    {
        if (!IsTargetValid())
        {
            Deactivate();
            return;
        }

        RenderLaser();
        Attack();
    }

    public void Initialize(LaserData data, Transform startPoint)
    {
        laserData = data;
        laserStartPoint = startPoint;
        attackTimer = 0f;

        if (data.Target != null && data.Target.Transform != null && data.Target.Transform.gameObject.activeSelf)
        {
            target = data.Target.Transform.GetComponent<IDamageable>();
            laserRenderer.positionCount = 2;
            gameObject.SetActive(true);
        }
        else
        {
            Deactivate();
        }
    }

    private bool IsTargetValid()
    {
        return target != null
               && laserData.Target != null
               && laserData.Target.Transform != null
               && laserData.Target.Transform.gameObject.activeSelf;
    }

    private void RenderLaser()
    {
        laserRenderer.SetPosition(0, laserStartPoint.position);
        laserRenderer.SetPosition(1, laserData.Target.Transform.position);
    }

    private void Attack()
    {
        attackTimer -= Time.deltaTime;
        
        if (attackTimer <= 0)
        {
            DealDamage();
            attackTimer = laserData.AttackSpeed;
        }
    }

    private void DealDamage()
    {
        var damageContext = new DamageContext(
            laserData.AttackPower,
            laserData.CriticalRate,
            laserData.CriticalDamage,
            laserData.Penetration,
            laserData.HeroClass
        );
        target.TakeDamage(damageContext);
    }

    private void Deactivate()
    {
        target = null;
        laserRenderer.positionCount = 0;
        gameObject.SetActive(false);
    }
}

public readonly struct LaserData
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
    
    public float AttackSpeed => DamageCalculator.CalculateMultipliers(HeroStat.AttackSpeed, LevelUpStat.AttackSpeedMultiplier,
        AbilityEffectStat.AttackSpeedMultiplier);

    public LaserData(IDetectable target, HeroStat heroStat, HeroStat levelUpStat, HeroStat cardEffectStat,
        float acquiredHeroBonusDamage,
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