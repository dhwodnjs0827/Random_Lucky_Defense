/// <summary>
/// GameEvent 구독용 인터페이스
/// </summary>
public interface IEventListener
{
    public void SubscribeEvents();
    
    public void UnsubscribeEvents();
}
