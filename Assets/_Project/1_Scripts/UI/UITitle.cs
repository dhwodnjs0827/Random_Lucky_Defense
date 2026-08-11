using UnityEngine;
using UnityEngine.UI;

public class UITitle : MonoBehaviour, IEventListener
{
    [SerializeField] private Slider loadingBar;

    private void Awake()
    {
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    public void SubscribeEvents()
    {
        EventManager.Subscribe<float>(GameEventType.GameInitializeProgress, UpdateLoadingBar);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe<float>(GameEventType.GameInitializeProgress, UpdateLoadingBar);
    }

    private void UpdateLoadingBar(float progress)
    {
        loadingBar.value = progress;
    }
}
