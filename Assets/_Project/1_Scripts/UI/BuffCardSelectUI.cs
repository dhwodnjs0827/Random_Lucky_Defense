using UnityEngine;
using UnityEngine.UI;

public class BuffCardSelectUI : BaseUI
{
    [SerializeField] private BuffCardUIComponent[] cards;
    [SerializeField] private Button refreshButton;

    private void Awake()
    {
        refreshButton.onClick.AddListener(OnClickRefreshButton);
        
        foreach (var card in cards)
        {
            card.InitializeCard(this);
        }
    }

    protected override void Opened(params object[] args)
    {
        InGameManager.Instance.PauseGame();
        SetCards();
    }

    protected override void Closed(params object[] args)
    {
        InGameManager.Instance.ResumeGame();
    }
    
    private void OnClickRefreshButton()
    {
        SetCards();
    }

    private void SetCards()
    {
        var randomCards = InGameManager.Instance.CardEffectFactory.GetRandomCards();
        for (var i = 0; i < cards.Length && i < randomCards.Length; i++)
        {
            cards[i].SetBuffCardData(randomCards[i]);
        }
    }
}
