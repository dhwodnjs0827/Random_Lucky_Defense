using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인게임 버프 카드 표시용 Popup UI 클래스
/// </summary>
public class UIBuffCardSelect : UIBase
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
