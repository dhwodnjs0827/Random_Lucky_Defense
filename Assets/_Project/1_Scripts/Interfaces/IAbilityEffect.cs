/// <summary>
/// 재능 효과 구현을 위한 인터페이스
/// </summary>
public interface IAbilityEffect
{
    /// <summary>
    /// 재능 효과를 AbilityEffectFactory에 등록
    /// </summary>
    public void RegisterAbilityEffect(AbilityEffectFactory abilityEffectFactory);
    
    /// <summary>
    /// 재능 효과를 AbilityEffectFactory에서 해제
    /// </summary>
    public void UnregisterAbilityEffect(AbilityEffectFactory abilityEffectFactory);
    
    /// <summary>
    /// 선택한 재능 정보를 기반으로 효과 적용
    /// </summary>
    public void ApplyAbilityEffect(AbilityContainer abilityContainer);
}
