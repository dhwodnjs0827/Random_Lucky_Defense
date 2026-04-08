using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 각각의 영웅 세부 정보를 보여주는 UI
/// </summary>
public class HeroInfoUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI heroNameText;
    [SerializeField] private HeroViewComponent heroView;
    [SerializeField] private TextMeshProUGUI heroInfoText;
    [SerializeField] private TextMeshProUGUI heroOwnedBonusDamageText;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;
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

        equipButton.gameObject.SetActive(!currentHeroData.IsSelected);
        unequipButton.gameObject.SetActive(currentHeroData.IsSelected);
        levelUpButton.interactable = PlayerDataManager.Instance.Currency[CurrencyType.Gold] >= currentHeroData.LevelUpRequiredGold;
        levelUpRequiredGoldText.text = $"레벨업\n골드: {currentHeroData.LevelUpRequiredGold}";
    }

    private void InitializeButtons()
    {
        if (equipButton != null)
        {
            equipButton.onClick.AddListener(OnClickEquipButton);
        }

        if (unequipButton != null)
        {
            unequipButton.onClick.AddListener(OnClickUnequipButton);
        }

        if (levelUpButton != null)
        {
            levelUpButton.onClick.AddListener(OnClickLevelUpButton);
        }
    }

    private void OnClickEquipButton()
    {
        // 현재 선택한 영웅 정보 갖고오기
        var selectedHero = PlayerDataManager.Instance.HeroDB.GetSelectedHero(currentHeroData.Class, currentHeroData.Grade);
        // 사용 중인 영웅과 선택한 영웅 데이터 스왑
        PlayerDataManager.Instance.ChangeSelectedHero(selectedHero, currentHeroData);

        // 영웅 뷰 변경하기
        heroView.UpdateHeroViewUIComponent(currentHeroData, currentHeroData.IsSelected);

        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(true);

        // 이벤트 발송
        var data = new ChangeSelectedHeroEventData
        (
            selectedHero,
            currentHeroData
        );
        EventManager.Dispatch(GameEventType.ChangeSelectedHero, data);
    }

    private void OnClickUnequipButton()
    {
        PlayerDataManager.Instance.ChangeSelectedHero(currentHeroData, null);

        heroView.UpdateHeroViewUIComponent(currentHeroData, currentHeroData.IsSelected);
        
        equipButton.gameObject.SetActive(true);
        unequipButton.gameObject.SetActive(false);
        
        ChangeSelectedHeroEventData data = new ChangeSelectedHeroEventData
        (
            currentHeroData,
            null
        );
        EventManager.Dispatch(GameEventType.ChangeSelectedHero, data);
    }

    private void OnClickLevelUpButton()
    {
        if (currentHeroData.LevelUpRequiredStack > currentHeroData.AcquiredStack)
        {
            ToastManager.Instance.Show("요구량이 부족합니다");
            return;
        }

        if (!PlayerDataManager.Instance.ChangeCurrency(CurrencyType.Gold, -currentHeroData.LevelUpRequiredGold))
        {
            return;
        }
        currentHeroData.AcquiredStack -= currentHeroData.LevelUpRequiredStack;
        currentHeroData.Level++;
        heroView.UpdateHeroViewUIComponent(currentHeroData, currentHeroData.IsSelected);
        
        levelUpButton.interactable = PlayerDataManager.Instance.Currency[CurrencyType.Gold] >= currentHeroData.LevelUpRequiredGold;
        levelUpRequiredGoldText.text = $"레벨업\n골드: {currentHeroData.LevelUpRequiredGold}";

        LevelUpHeroEventData data = new LevelUpHeroEventData(currentHeroData);
        EventManager.Dispatch(GameEventType.LevelUpHero, data);

        PlayerDataManager.Instance.SaveData(SaveDataType.Hero);
    }
}