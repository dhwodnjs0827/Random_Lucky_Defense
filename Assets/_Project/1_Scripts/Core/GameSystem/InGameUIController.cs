using Cysharp.Threading.Tasks;

public class InGameUIController : IEventListener
{
    private UIInGame uiInGame;
    
    public UIInGame UIInGame => uiInGame;
    
    public void SubscribeEvents()
    {
        uiInGame.SubscribeEvents();
    }

    public void UnsubscribeEvents()
    {
        uiInGame.UnsubscribeEvents();
    }
    
    public async UniTask InitializeAsync()
    {
        uiInGame = await UIManager.Instance.OpenAsync<UIInGame>();
    }
}