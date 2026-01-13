using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 리소스 관리 인터페이스 (Strategy Interface)
/// </summary>
public interface IResourceHandler
{
    /// <summary>
    /// 비동기 단일 리소스 불러오기
    /// </summary>
    public UniTask<T> LoadAsync<T>(string path) where T : Object;
    
    /// <summary>
    /// 동기 단일 리소스 불러오기
    /// </summary>
    public T Load<T>(string path) where T : Object;
    
    /// <summary>
    /// 비동기 다중 리소스 불러오기
    /// </summary>
    public UniTask<T[]> LoadAllAsync<T>(string path) where T : Object;
    
    /// <summary>
    /// 동기 다중 리소스 불러오기
    /// </summary>
    public T[] LoadAll<T>(string path) where T : Object;

    /// <summary>
    /// 리소스를 메모리에서 해제
    /// </summary>
    public void Release(Object obj);
}