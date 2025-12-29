using Cysharp.Threading.Tasks;

public class GameScene : BaseScene
{
    public override SceneType SceneType => SceneType.GameScene;

    public override async UniTask InitializeAsync()
    {
        await UIManager.Instance.OpenAsync<HUDUI>();
        await UIManager.Instance.OpenAsync<WaveInfoUI>();
    }
}