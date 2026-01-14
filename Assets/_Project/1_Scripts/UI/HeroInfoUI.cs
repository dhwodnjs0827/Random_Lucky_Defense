using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfoUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI heroNameText;
    [SerializeField] private HeroViewComponent heroView;
    [SerializeField] private TextMeshProUGUI heroInfoText;
    [SerializeField] private TextMeshProUGUI heroOwnedBonusDamageText;
    [SerializeField] private Button equipButton;
    [SerializeField] private TextMeshProUGUI equipButtonText;
    [SerializeField] private Button levelUpButton;
    [SerializeField] private TextMeshProUGUI levelUpRequiredGoldText;

    private HeroRuntimeData currentHeroData;

    private void Awake()
    {
        InitializeButtons();
    }

    protected override void Opened(params object[] args)
    {
        if (args[0] is HeroRuntimeData)
        {
            currentHeroData = args[0] as HeroRuntimeData;
            UpdateHeroInfoUI();
        }
        else
        {
            CDebug.LogError("[HeroInfoUI] 잘못된 매개변수 전달");
        }
    }

    protected override void Closed(params object[] args)
    {
        currentHeroData = null;
    }

    private void UpdateHeroInfoUI()
    {
        heroNameText.text = currentHeroData.Name;
        heroView.UpdateHeroViewUIComponent(currentHeroData, currentHeroData.IsSelected);
        heroInfoText.text =
            $"공격력 : {currentHeroData.HeroData.AttackPower}\n공격속도 : {currentHeroData.HeroData.AttackSpeed}\n공격범위 : {currentHeroData.HeroData.AttackRange}\n스플래쉬 범위 : {currentHeroData.HeroData.SplashRange}";
        equipButtonText.text = currentHeroData.IsSelected ? "장착취소" : "장착";
    }

    private void InitializeButtons()
    {
        if (equipButton != null)
        {
            equipButton.onClick.AddListener(OnClickEquipButton);
        }

        if (levelUpButton != null)
        {
            levelUpButton.onClick.AddListener(OnClickLevelUpButton);
        }
    }

    private void OnClickEquipButton()
    {
        var selectedHero = PlayerDataManager.Instance.AllHeroes.First(data =>
            data.IsSelected == true && data.Class == currentHeroData.Class && data.Grade == currentHeroData.Grade);
        PlayerDataManager.Instance.ChangeSelectedHero(selectedHero.ID, currentHeroData.ID);
        
        heroView.UpdateHeroViewUIComponent(currentHeroData, currentHeroData.IsSelected);
        equipButtonText.text = currentHeroData.IsSelected ? "장착취소" : "장착";
        
        ChangeSelectedHeroEventData data = new ChangeSelectedHeroEventData
        (
            selectedHero,
            currentHeroData
        );
        EventManager.Dispatch(GameEventType.ChangeSelectedHero, data);
    }

    private void OnClickLevelUpButton()
    {
    }
}