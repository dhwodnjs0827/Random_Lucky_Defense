using Cysharp.Threading.Tasks;

/// <summary>
/// VFX 관리 담당 클래스
/// </summary>
public class EffectManager : MonoSingleton<EffectManager>
{
    private const string EFFECT_RESOURCE_PATH = "VFX/";
    private VFXInstance vfxInstance;

    protected override bool isInitialized { get; set; }

    /// <summary>
    /// EffectManager 초기화
    /// </summary>
    public override async UniTask InitializeAsync()
    {
        if (isInitialized)
        {
            return;
        }
        
        vfxInstance = await AddressableManager.Instance.LoadAsync<VFXInstance>($"{EFFECT_RESOURCE_PATH}VFXInstance");
        ObjectPoolManager.Instance.Preload(vfxInstance);
        
        isInitialized = true;
        await UniTask.CompletedTask;
    }

    public async UniTask PlayEffectAsync()
    {
        await UniTask.CompletedTask;
    }

    public void ClearAll()
    {
        
    }
}
