using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : MonoSingleton<AddressableManager>
{
    protected override bool isInitialized { get; set; }

    private readonly Dictionary<string, AsyncOperationHandle> handleMap = new();

    protected override void OnDestroy()
    {
        ReleaseAll();
        base.OnDestroy();
    }

    public override async UniTask InitializeAsync()
    {
        if (isInitialized)
        {
            return;
        }

        await Addressables.InitializeAsync();

        isInitialized = true;
    }

    /// <summary>
    /// 단일 리소스 로드
    /// </summary>
    /// <param name="address">주소</param>
    public async UniTask<T> LoadAsync<T>(string address) where T : Object
    {
        // 캐시 확인
        if (handleMap.TryGetValue(address, out var cacheHandle))
        {
            if (typeof(T).IsSubclassOf(typeof(Component)))
            {
                var go = cacheHandle.Result as GameObject;
                return go != null ? go.GetComponent<T>() : null;
            }

            return cacheHandle.Result as T;
        }

        if (typeof(T).IsSubclassOf(typeof(Component)))
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(address);
            var go = await handle;
            if (go == null) return null;

            handleMap[address] = handle;
            return go.GetComponent<T>();
        }

        var assetHandle = Addressables.LoadAssetAsync<T>(address);
        var resource = await assetHandle;
        if (resource == null) return null;

        handleMap[address] = assetHandle;
        return resource;
    }

    /// <summary>
    /// 단일 리소스 로드
    /// </summary>
    /// <param name="reference">AssetReference</param>
    public async UniTask<T> LoadAsync<T>(AssetReference reference) where T : Object
    {
        if (reference == null || !reference.RuntimeKeyIsValid())
        {
            CDebug.LogError("[AddressableManager] AssetReference가 유효하지 않음");
            return null;
        }

        return await LoadAsync<T>(reference.RuntimeKey.ToString());
    }

    /// <summary>
    /// 다중 리소스 로드
    /// </summary>
    /// <param name="label">라벨</param>
    public async UniTask<T[]> LoadAllAsync<T>(string label) where T : Object
    {
        // 캐시 확인
        if (handleMap.TryGetValue(label, out var cachedHandle))
        {
            if (typeof(T).IsSubclassOf(typeof(Component)))
            {
                var gameObjects = cachedHandle.Result as IList<GameObject>;
                if (gameObjects == null) return null;

                var components = new List<T>();
                foreach (var go in gameObjects)
                {
                    var comp = go.GetComponent<T>();
                    if (comp != null) components.Add(comp);
                }

                return components.ToArray();
            }

            return ((IList<T>)cachedHandle.Result).ToArray();
        }

        // Component 타입 처리
        if (typeof(T).IsSubclassOf(typeof(Component)))
        {
            var handle = Addressables.LoadAssetsAsync<GameObject>(label, null);
            var gameObjects = await handle;
            if (gameObjects == null || gameObjects.Count == 0) return null;

            handleMap[label] = handle;

            var components = new List<T>();
            foreach (var go in gameObjects)
            {
                var comp = go.GetComponent<T>();
                if (comp != null) components.Add(comp);
            }

            return components.ToArray();
        }

        // 일반 에셋
        var assetHandle = Addressables.LoadAssetsAsync<T>(label, null);
        var resources = await assetHandle;
        if (resources == null || resources.Count == 0) return null;

        handleMap[label] = assetHandle;
        return resources.ToArray();
    }
    
    /// <summary>
    /// 다중 리소스 로드
    /// </summary>
    /// <param name="references">List&lt;AssetReference&gt;</param>
    public async UniTask<T[]> LoadAllAsync<T>(List<AssetReference> references) where T : Object
    {
        if (references == null || references.Count == 0) return null;

        var tasks = new List<UniTask<T>>();
        foreach (var reference in references)
        {
            if (reference != null && reference.RuntimeKeyIsValid())
            {
                tasks.Add(LoadAsync<T>(reference));
            }
        }

        return await UniTask.WhenAll(tasks);
    }

    /// <summary>
    /// 리소스 메모리 정리 및 캐시 정리
    /// </summary>
    /// <param name="key">단일: address, 다중: label</param>
    public void Release(string key)
    {
        if (!handleMap.Remove(key, out var handle)) return;
        handle.Release();
    }

    /// <summary>
    /// 모든 리소스 메모리 해제 및 캐시 정리
    /// </summary>
    private void ReleaseAll()
    {
        foreach (var handle in handleMap.Values)
        {
            if (handle.IsValid())
            {
                handle.Release();
            }
        }

        handleMap.Clear();
    }
}