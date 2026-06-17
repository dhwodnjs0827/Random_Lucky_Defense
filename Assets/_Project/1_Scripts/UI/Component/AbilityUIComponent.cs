using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인게임 재능 UI용 클래스
/// </summary>
public class AbilityUIComponent : MonoBehaviour
{
    [SerializeField] private Button selectButton;
    [SerializeField] private TextMeshProUGUI abilityName;
    [SerializeField] private Image abilityImage;
    [SerializeField] private TextMeshProUGUI abilityDescription;
    [SerializeField] private Image[] levelIcons;
    
    private UIBase parentUI;
    private AbilityContainer currentAbility;
    
    public void InitializeAbility(UIBase ui)
    {
        parentUI = ui;
        selectButton.onClick.AddListener(OnClickSelect);
    }

    public void SetAbilityData(AbilityContainer ability)
    {
        currentAbility = ability;
        abilityName.text = currentAbility.AbilityData.Name;
        abilityDescription.text = ReplaceAbilityDescriptionValues(ability.AbilityData.Description, ability.AbilityLevelData.value, ability.AbilityLevelData.value1);

        for (var i = 0; i < levelIcons.Length; i++)
        {
            levelIcons[i].gameObject.SetActive(i < currentAbility.AbilityLevelData.Level - 1);
        }
    }

    private void OnClickSelect()
    {
        EventManager.Dispatch(GameEventType.AbilitySelected, new AbilitySelectEventData(currentAbility));
        UIManager.Instance.Close(parentUI);
    }
    
    private string ReplaceAbilityDescriptionValues(string originalDesc, float value, float value1)
    {
        return originalDesc.Replace("{value}", value.ToString()).Replace("{value1}", value1.ToString());
    }
}
