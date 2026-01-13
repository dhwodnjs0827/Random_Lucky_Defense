using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 토스트 메세지 관리 담당 클래스
/// </summary>
public class ToastManager : MonoSingleton<ToastManager>
{
    private bool isInitialized = false;
    private ToastUI toastUI;

    public async UniTask InitializeAsync()
    {
        if (isInitialized)
        {
            return;
        }
        
        var systemCanvasPrefab = await ResourceManager.Instance.LoadAsync<Canvas>("UI/Canvas/@System");
        var canvas = Instantiate(systemCanvasPrefab);
        canvas.sortingOrder = (int)UIType.System;
        canvas.name = $"@{nameof(UIType.System)}";
        DontDestroyOnLoad(canvas.gameObject);
        
        var prefab = await ResourceManager.Instance.LoadAsync<ToastUI>("UI/ToastUI");
        toastUI = Instantiate(prefab, canvas.transform);
        
        isInitialized = true;
    }

    public void Show(string message, float duration = 2f)
    {
        toastUI.Show(message, duration);
    }

    public void Clear()
    {
        toastUI.gameObject.SetActive(false);
    }
}
