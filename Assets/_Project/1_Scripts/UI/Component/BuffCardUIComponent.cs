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
        cardName.text = currentCard.Name;
        cardDescription.text = currentCard.Description;
    }

    private void OnClickSelect()
    {
        UIManager.Instance.Close(parentUI);
    }
}
