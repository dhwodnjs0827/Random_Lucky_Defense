using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// VFX 관리 담당 클래스
/// </summary>
public class EffectManager : MonoSingleton<EffectManager>
{
    private ResourceManager resourceManager;
    private ObjectPoolManager objectPoolManager;
    
    private bool isInitialized = false;

    private const string EFFECT_RESOURCE_PATH = "VFX/";
    private VFXInstance vfxInstance;

    protected override void Awake()
    {
        base.Awake();
        resourceManager = ResourceManager.Instance;
        objectPoolManager = ObjectPoolManager.Instance;
    }

    /// <summary>
    /// EffectManager 초기화
    /// </summary>
    public async UniTask InitializeAsync()
    {
        if (isInitialized)
        {
            return;
        }
        
        vfxInstance = await resourceManager.LoadAsync<VFXInstance>($"{EFFECT_RESOURCE_PATH}VFXInstance");
        objectPoolManager.Preload(vfxInstance);
        
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
