using Cysharp.Threading.Tasks;

public class LobbyScene : BaseScene
{
    public override SceneType SceneType => SceneType.LobbyScene;

    public override async UniTask InitializeAsync()
    {
        await UIManager.Instance.OpenAsync<UIMainLobby>();
        
        await base.InitializeAsync();
    }

    public override async UniTask CleanupAsync()
    {
        UIManager.Instance.Cleanup();
        ToastManager.Instance.Clear();
        await base.CleanupAsync();
    }
}