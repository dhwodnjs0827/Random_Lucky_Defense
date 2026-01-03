using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class InGameLevelUpButtonComponent : MonoBehaviour, IEventListener
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

    private ReactiveProperty<int> heroCount = new();
    
    private Action<HeroSpawnEventData> onSpawnedHero;

    private void Awake()
    {
        levelUpButton ??= GetComponent<Button>();

        levelUpButton.onClick.AddListener(OnClick);
        heroCount.Value = 0;
        heroCountText.text = heroCount.Value.ToString();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
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

    public void SubscribeEvents()
    {
        onSpawnedHero += IncreaseHeroCount;
        EventManager.Subscribe(GameEventType.SpawnHero, onSpawnedHero);
        
        heroCount.Subscribe(count => levelText.text = $"Lv: {count.ToString()}").AddTo(this);
        heroCount.Subscribe(count => levelUpButton.interactable = count > 0).AddTo(this);
        
    }

    public void UnsubscribeEvents()
    {
        onSpawnedHero -= IncreaseHeroCount;
        EventManager.Unsubscribe(GameEventType.SpawnHero, onSpawnedHero);
    }

    private void IncreaseHeroCount(HeroSpawnEventData data)
    {
        if (data.SpawnedHero.ClassType == classType)
        {
            heroCount.Value++;
            heroCountText.text = heroCount.ToString();
        }
    }
}
