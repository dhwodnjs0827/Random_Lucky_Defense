using Cysharp.Threading.Tasks;

public class GameScene : BaseScene
{
    public override SceneType SceneType => SceneType.GameScene;

    public override async UniTask InitializeAsync()
    {
        await InGameManager.Instance.InitializeAsync();
        await EffectManager.Instance.InitializeAsync();
    }

    public override UniTask CleanupAsync()
    {
        ObjectPoolManager.Instance.ClearAll();
        EffectManager.Instance.ClearAll();
        return base.CleanupAsync();
    }
}