using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인게임 버프 카드 표시용 Popup UI 클래스
/// </summary>
public class UIBuffCardSelect : UIBase
{
    [SerializeField] private AbilityUIComponent[] abilities;
    [SerializeField] private Button refreshButton;

    private void Awake()
    {
        refreshButton.onClick.AddListener(OnClickRefreshButton);
        
        foreach (var ability in abilities)
        {
            ability.InitializeAbility(this);
        }
    }

    protected override void Opened(params object[] args)
    {
        InGameManager.Instance.PauseGame();
        SetAbilities();
    }

    protected override void Closed(params object[] args)
    {
        InGameManager.Instance.ResumeGame();
    }
    
    private void OnClickRefreshButton()
    {
        SetAbilities();
    }

    private void SetAbilities()
    {
        var randomAbilities = InGameManager.Instance.AbilityEffectFactory.GetRandomAbilities();
        for (var i = 0; i < abilities.Length && i < randomAbilities.Length; i++)
        {
            abilities[i].SetAbilityData(randomAbilities[i]);
        }
    }
}
