using UnityEngine;

public class BaseProjectile : MonoBehaviour, IPoolable
{
    [SerializeField] protected ParticleSystem projectileParticles;
    [SerializeField] private Rigidbody2D rb;
    
    private ProjectileData projectileData;
    private readonly float speed = 5f;
    private bool isFireTrigger;
    private Vector3 lastTargetPosition;

    private void FixedUpdate()
    {
        if (!isFireTrigger)
        {
            return;
        }

        FlyToTarget();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        if (other.gameObject.TryGetComponent<IDamageable>(out var enemy))
        {
            enemy.TakeDamage(projectileData.Damage);
        }
        
        ObjectPoolManager.Instance.Release(gameObject);
    }

    public virtual void Initialize(ProjectileData data)
    {
        projectileData = data;
    }

    public void FireProjectile()
    {
        isFireTrigger = true;
    }

    private void FlyToTarget()
    {
        if (projectileData.Target != null)
        {
            lastTargetPosition = projectileData.Target.Transform.position;
        }
        var dir = (lastTargetPosition - transform.position).normalized;
        rb.linearVelocity = dir * speed;

        if (transform.position == lastTargetPosition)
        {
            AttackTarget(projectileData.Target);
        }
    }

    private void AttackTarget(IDetectable target)
    {
        if (target != null && target.Transform.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(projectileData.Damage);
        }
        
        ObjectPoolManager.Instance.Release(gameObject);
    }

    public virtual void OnGet()
    {
        
    }

    public virtual void OnRelease()
    {
        projectileData = default;
        isFireTrigger = false;
    }
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