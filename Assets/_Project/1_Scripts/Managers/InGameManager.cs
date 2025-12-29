using UnityEngine;

public class InGameManager : MonoSingleton<InGameManager>, IEventListener
{
    protected override bool IsDontDestroyOnLoad => false;

    protected override void Awake()
    {
        base.Awake();
        SubscribeEvents();
    }

    protected override void OnDestroy()
    {
        UnsubscribeEvents();
        base.OnDestroy();
    }

    public void SubscribeEvents()
    {
    }

    public void UnsubscribeEvents()
    {
    }
}