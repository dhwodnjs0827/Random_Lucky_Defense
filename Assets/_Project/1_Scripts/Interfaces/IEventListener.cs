/// <summary>
/// GameEvent 구독용 인터페이스
/// </summary>
public interface IEventListener
{
    /// <summary>
    /// EventManager에 구독 등록
    /// </summary>
    public void SubscribeEvents();
    
    /// <summary>
    /// EventManager에서 구독 해제
    /// </summary>
    public void UnsubscribeEvents();
}
