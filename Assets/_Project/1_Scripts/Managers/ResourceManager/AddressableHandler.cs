#if ADDRESSABLE
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

/// <summary>
/// Addressable API 사용 (Concrete Strategy)
/// <para>ResourceManager에서 사용</para>
/// </summary>
public class AddressableHandler : IResourceHandler
{
    // 로드된 객체와 Handle 매핑 (Release 시 사용)
    private readonly Dictionary<Object, AsyncOperationHandle> _handleMap = new();

    public async UniTask<T> LoadAsync<T>(string path) where T : Object
    {
        if (typeof(T).IsSubclassOf(typeof(Component)))
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(path);
            var go = await handle;
            if (go == null)
            {
                return null;
            }
            var component = go.GetComponent<T>();
            _handleMap[component] = handle;  // Component를 키로 저장
            return component;
        }

        var assetHandle = Addressables.LoadAssetAsync<T>(path);
        var resource = await assetHandle;
        if (resource == null)
        {
            return null;
        }
        _handleMap[resource] = assetHandle;
        return resource;
    }

    public T Load<T>(string path) where T : Object
    {
        if (typeof(T).IsSubclassOf(typeof(Component)))
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(path);
            var go = handle.WaitForCompletion();
            if (go == null)
            {
                return null;
            }
            var component = go.GetComponent<T>();
            _handleMap[component] = handle;
            return component;
        }

        var assetHandle = Addressables.LoadAssetAsync<T>(path);
        var resource = assetHandle.WaitForCompletion();
        if (resource == null)
        {
            return null;
        }
        _handleMap[resource] = assetHandle;
        return resource;
    }

    public async UniTask<T[]> LoadAllAsync<T>(string label) where T : Object
    {
        var handle = Addressables.LoadAssetsAsync<T>(label, null);
        var resources = await handle;
        if (resources == null || resources.Count == 0)
        {
            return null;
        }

        // LoadAll의 경우 첫 번째 에셋을 키로 handle 저장
        if (resources.Count > 0)
        {
            _handleMap[resources[0]] = handle;
        }

        return resources.ToArray();
    }

    public T[] LoadAll<T>(string label) where T : Object
    {
        var handle = Addressables.LoadAssetsAsync<T>((object)label, null);
        var resources = handle.WaitForCompletion();
        if (resources == null || resources.Count == 0)
        {
            return null;
        }

        if (resources.Count > 0)
        {
            _handleMap[resources[0]] = handle;
        }

        return resources.ToArray();
    }

    public void Release(Object obj)
    {
        if (obj == null) return;

        if (_handleMap.TryGetValue(obj, out var handle))
        {
            Addressables.Release(handle);
            _handleMap.Remove(obj);
        }
    }
}
#endif