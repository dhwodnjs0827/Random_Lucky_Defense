/// <summary>
/// 데미지 처리 인터페이스
/// </summary>
public interface IDamageable
{
    public float MaxHealth { get; }
    public float CurrentHealth { get; }
    
    /// <summary>
    /// 데미지 처리
    /// </summary>
    public void TakeDamage(DamageContext damageContext);

    /// <summary>
    /// 피격 이펙트
    /// </summary>
    public void HitEffect();

    public void Die();
}
