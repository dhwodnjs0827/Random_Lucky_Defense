using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인게임 영웅 레벨업 버튼용 클래스
/// </summary>
public class InGameLevelUpButtonComponent : MonoBehaviour, IEventListener
{
    [Header("영웅 클래스 설정")]
    [SerializeField] private HeroClassType classType;

    [Header("UI Elements")]
    [SerializeField] private Button levelUpButton;
    [SerializeField] private TextMeshProUGUI heroCountText;
    [SerializeField] private TextMeshProUGUI classDamageRateText;
    [SerializeField] private Image heroImage;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI levelUpCostText;
    
    private InGameHeroLevelUpController levelUpController;
    private readonly ReactiveProperty<int> heroCount = new();
    private Action<HeroSpawnEventData> onSpawnedHero;
    private Action<GameWaveStartEventData> onWaveStart;

    private void Awake()
    {
        levelUpButton ??= GetComponent<Button>();

        levelUpButton.onClick.AddListener(OnClick);
        heroCount.Value = 0;
        heroCountText.text = heroCount.Value.ToString();
        levelUpButton.interactable = false;
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
        levelUpController.CurrentSpawnPoint.Subscribe(sp => levelUpButton.interactable = sp >= levelUpController.LevelUpDataDict[classType][levelUpController.CurrentLevelDict[classType].Value].LevelUpCost && heroCount.Value > 0).AddTo(this);
        levelUpController.CurrentLevelDict[classType].Subscribe(level => levelText.text = $"Lv: {level.ToString()}")
            .AddTo(this);
        levelUpController.CurrentLevelDict[classType].Subscribe(level => levelUpCostText.text = $"비용: {levelUpController.LevelUpDataDict[classType][level].LevelUpCost}")
            .AddTo(this);
    }

    private void OnClick()
    {
        EventManager.Dispatch(GameEventType.InGameHeroLevelUpRequest, classType);
    }

    public void SubscribeEvents()
    {
        onSpawnedHero += IncreaseHeroCount;
        EventManager.Subscribe(GameEventType.SpawnHero, onSpawnedHero);
        onWaveStart += ChangeDamageRate;
        EventManager.Subscribe(GameEventType.WaveStart, onWaveStart);
        
        heroCount.Subscribe(count => heroCountText.text = count.ToString()).AddTo(this);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.WaveStart, onWaveStart);
        onWaveStart -= ChangeDamageRate;
        EventManager.Unsubscribe(GameEventType.SpawnHero, onSpawnedHero);
        onSpawnedHero -= IncreaseHeroCount;
    }

    private void IncreaseHeroCount(HeroSpawnEventData data)
    {
        if (data.SpawnedHero.ClassType == classType)
        {
            heroCount.Value++;
        }
    }

    private void ChangeDamageRate(GameWaveStartEventData data)
    {
        var currentEnemyData = data.CurrentEnemyData;
        switch (currentEnemyData.MonsterType)
        {
           case MonsterType.Undead:
               classDamageRateText.text =
                   $"{DamageCalculator.DamageRateByClassData[(classType, MonsterType.Undead)].DamageRate * 100f :N0}";
               break;
           case MonsterType.Troll:
               classDamageRateText.text =
                   $"{DamageCalculator.DamageRateByClassData[(classType, MonsterType.Troll)].DamageRate * 100f :N0}";
               break;
           case MonsterType.Orc:
               classDamageRateText.text =
                   $"{DamageCalculator.DamageRateByClassData[(classType, MonsterType.Orc)].DamageRate * 100f :N0}";
               break;
        }
    }
}
