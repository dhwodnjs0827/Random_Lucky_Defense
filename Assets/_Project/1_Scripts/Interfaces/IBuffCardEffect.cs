/// <summary>
/// 버프 카드 효과 구현을 위한 인터페이스
/// </summary>
public interface IBuffCardEffect
{
    /// <summary>
    /// 카드 효과를 CardEffectFactory에 등록
    /// </summary>
    public void RegisterCardEffect(CardEffectFactory cardEffectFactory);
    
    /// <summary>
    /// 카드 효과를 CardEffectFactory에서 해제
    /// </summary>
    public void UnregisterCardEffect(CardEffectFactory cardEffectFactory);
    
    /// <summary>
    /// 선택한 카드 정보를 기반으로 효과 적용
    /// </summary>
    public void ApplyCardEffect(BuffCardContainer cardContainer);
}
