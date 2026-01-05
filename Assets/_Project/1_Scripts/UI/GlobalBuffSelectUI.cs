using UnityEngine;
using UnityEngine.UI;

public class GlobalBuffSelectUI : BaseUI
{
    [SerializeField] private GlobalBuffCard[] cards;
    [SerializeField] private Button refreshButton;

    private void Awake()
    {
        foreach (var card in cards)
        {
            card.InitializeCard(this);
        }
    }

    protected override void Opened(params object[] args)
    {
        InGameManager.Instance.PauseGame();
        foreach (var card in cards)
        {
            card.SetGlobalBuffData();
        }
    }

    protected override void Closed(params object[] args)
    {
        InGameManager.Instance.ResumeGame();
    }
}
