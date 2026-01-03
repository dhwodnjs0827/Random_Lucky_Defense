using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class InGameLevelUpButtonComponent : MonoBehaviour
{
    [SerializeField] private HeroClassType classType;

    [Header("UI Elements")]
    [SerializeField] private Button levelUpButton;
    [SerializeField] private TextMeshProUGUI heroCountText;
    [SerializeField] private TextMeshProUGUI classDamageRateText;
    [SerializeField] private Image heroImage;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI levelUpCostText;
    
    private InGameHeroLevelUpController levelUpController;

    private void Awake()
    {
        levelUpButton ??= GetComponent<Button>();

        levelUpButton.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        levelUpButton.onClick.RemoveListener(OnClick);
    }
    
    public void SubscribeLevelUpController(InGameHeroLevelUpController controller)
    {
        levelUpController = controller;
        levelUpController.CurrentSpawnPoint.Subscribe(sp => levelUpButton.interactable = sp >= levelUpController.LevelUpDataDict[classType][levelUpController.CurrentLevelDict[classType].Value].LevelUpCost).AddTo(this);
        levelUpController.CurrentLevelDict[classType].Subscribe(level => levelText.text = $"Lv: {level.ToString()}")
            .AddTo(this);
        levelUpController.CurrentLevelDict[classType].Subscribe(level => levelUpCostText.text = $"비용: {levelUpController.LevelUpDataDict[classType][levelUpController.CurrentLevelDict[classType].Value].LevelUpCost}")
            .AddTo(this);
    }

    private void OnClick()
    {
        levelUpController.LevelUp(classType);
    }
}
