using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalBuffCard : MonoBehaviour
{
    [SerializeField] private Button selectButton;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI cardDescription;
    [SerializeField] private Image[] levelIcons;
    
    private BaseUI parentUI;
    
    public void InitializeCard(BaseUI ui)
    {
        parentUI = ui;
        selectButton.onClick.AddListener(OnClickSelect);
    }

    public void SetGlobalBuffData()
    {
        
    }

    private void OnClickSelect()
    {
        UIManager.Instance.Close(parentUI);
    }
}
