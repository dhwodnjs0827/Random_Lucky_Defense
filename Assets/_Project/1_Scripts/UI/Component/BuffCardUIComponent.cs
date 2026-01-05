using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffCardUIComponent : MonoBehaviour
{
    [SerializeField] private Button selectButton;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI cardDescription;
    [SerializeField] private Image[] levelIcons;
    
    private BaseUI parentUI;
    private BuffCardContainer currentCard;
    
    public void InitializeCard(BaseUI ui)
    {
        parentUI = ui;
        selectButton.onClick.AddListener(OnClickSelect);
    }

    public void SetBuffCardData(BuffCardContainer card)
    {
        currentCard = card;
        cardName.text = currentCard.CardData.Name;
        cardDescription.text = currentCard.CardData.Description;

        for (var i = 0; i < levelIcons.Length; i++)
        {
            levelIcons[i].gameObject.SetActive(i < currentCard.CardLevelData.Level - 1);
        }
    }

    private void OnClickSelect()
    {
        EventManager.Dispatch(GameEventType.BuffCardSelected, new GameBuffCardSelectEventData(currentCard));
        UIManager.Instance.Close(parentUI);
    }
}
