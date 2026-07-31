using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 토스트 메세지 관리 담당 클래스
/// </summary>
public class ToastManager : MonoSingleton<ToastManager>
{
    private UIToast uiToast;

    protected override bool isInitialized { get; set; }

    public override async UniTask InitializeAsync()
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
        
        var prefab = await ResourceManager.Instance.LoadAsync<UIToast>("UI/UIToast");
        uiToast = Instantiate(prefab, canvas.transform);
        
        isInitialized = true;
    }

    public void Show(string message, float duration = 2f)
    {
        uiToast.Show(message, duration);
    }

    public void Clear()
    {
        uiToast.gameObject.SetActive(false);
    }
}
