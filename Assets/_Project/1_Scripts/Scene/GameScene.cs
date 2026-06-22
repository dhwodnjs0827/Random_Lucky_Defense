using Cysharp.Threading.Tasks;

public class GameScene : BaseScene
{
    public override SceneType SceneType => SceneType.GameScene;

    public override async UniTask InitializeAsync()
    {
        await InGameManager.Instance.InitializeAsync();
        await EffectManager.Instance.InitializeAsync();
        await base.InitializeAsync();
    }

    public override async UniTask CleanupAsync()
    {
        UIManager.Instance.Cleanup();
        ObjectPoolManager.Instance.ClearAll();
        EffectManager.Instance.ClearAll();
        await base.CleanupAsync();
    }
}