using Cysharp.Threading.Tasks;

public class GameScene : BaseScene
{
    public override SceneType SceneType => SceneType.GameScene;

    public override async UniTask InitializeAsync()
    {
        await EffectManager.Instance.InitializeAsync();
        await UIManager.Instance.OpenAsync<HUDUI>();
    }

    public override UniTask CleanupAsync()
    {
        ObjectPoolManager.Instance.ClearAll();
        EffectManager.Instance.ClearAll();
        return base.CleanupAsync();
    }
}