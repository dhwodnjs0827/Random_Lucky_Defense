using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인게임 영웅 컨트롤 UI
/// <para>영웅 소환/판매/교환 등</para>
/// </summary>
public class InGameHeroControlUIComponent : MonoBehaviour, IEventListener
{
    private int spawnedEnemyCount;

    [Header("UI Elements")]
    [SerializeField] private Slider spawnedEnemyCountSlider;
    [SerializeField] private TextMeshProUGUI spawnedEnemyCountText;
    [SerializeField] private TextMeshProUGUI currentSpawnPointText;
    [SerializeField] private TextMeshProUGUI spawnPointCostText;

    [Header("Buttons")] [SerializeField] private Button spawnButton;
    [SerializeField] private Button magicianLevelUpButton;
    [SerializeField] private Button archerLevelUpButton;
    [SerializeField] private Button warriorLevelUpButton;
    [SerializeField] private Button exchangeButton;
    [SerializeField] private Button sellButton;
    
    private InGameHeroLevelUpController levelUpController;

    private void Awake()
    {
        levelUpController = new InGameHeroLevelUpController();
    }

    private void OnEnable()
    {
        InitializeButtons();
        SubscribeEvents();
    }

    private void OnDisable()
    {
        ClearButtons();
        UnsubscribeEvents();
    }

    private void InitializeButtons()
    {
        if (spawnButton != null)
        {
            spawnButton.onClick.AddListener(OnClickSpawnButton);
        }

        if (magicianLevelUpButton != null)
        {
            magicianLevelUpButton.onClick.AddListener(OnClickMagicianLevelUpButton);
        }

        if (archerLevelUpButton != null)
        {
            archerLevelUpButton.onClick.AddListener(OnClickArcherLevelUpButton);
        }

        if (warriorLevelUpButton != null)
        {
            warriorLevelUpButton.onClick.AddListener(OnClickWarriorLevelUpButton);
        }

        if (exchangeButton != null)
        {
            exchangeButton.onClick.AddListener(OnClickExchangeButton);
        }

        if (sellButton != null)
        {
            sellButton.onClick.AddListener(OnClickSellButton);
        }
    }

    private void ClearButtons()
    {
        if (spawnButton != null)
        {
            spawnButton.onClick.RemoveAllListeners();
        }

        if (magicianLevelUpButton != null)
        {
            magicianLevelUpButton.onClick.RemoveAllListeners();
        }

        if (archerLevelUpButton != null)
        {
            archerLevelUpButton.onClick.RemoveAllListeners();
        }

        if (warriorLevelUpButton != null)
        {
            warriorLevelUpButton.onClick.RemoveAllListeners();
        }

        if (exchangeButton != null)
        {
            exchangeButton.onClick.RemoveAllListeners();
        }

        if (sellButton != null)
        {
            sellButton.onClick.RemoveAllListeners();
        }
    }

    private void OnClickSpawnButton()
    {
        EventManager.Dispatch(GameEventType.SpawnHero);
        levelUpController.OnSpawnHero();
    }

    private void OnClickMagicianLevelUpButton()
    {
        EventManager.Dispatch(GameEventType.LevelUpMagician);
        levelUpController.LevelUp(HeroClassType.Magician);
    }

    private void OnClickArcherLevelUpButton()
    {
        EventManager.Dispatch(GameEventType.LevelUpArcher);
        levelUpController.LevelUp(HeroClassType.Archer);
    }

    private void OnClickWarriorLevelUpButton()
    {
        EventManager.Dispatch(GameEventType.LevelUpWarrior);
        levelUpController.LevelUp(HeroClassType.Warrior);
    }

    private void OnClickExchangeButton()
    {
        CDebug.Log("[HUDUI] 영웅 교환 버튼 클릭");
    }

    private void OnClickSellButton()
    {
        CDebug.Log("[HUDUI] 영웅 판매 버튼 클릭");
    }

    public void SubscribeEvents()
    {
        SubscribeLevelUpController();
        
        EventManager.Subscribe(GameEventType.SpawnEnemy, IncreaseEnemyCount);
        EventManager.Subscribe(GameEventType.EnemyDie, DecreaseEnemyCount);
    }

    public void UnsubscribeEvents()
    {
        UnsubscribeLevelUpController();
        
        EventManager.Unsubscribe(GameEventType.SpawnEnemy, IncreaseEnemyCount);
        EventManager.Unsubscribe(GameEventType.EnemyDie, DecreaseEnemyCount);
    }

    private void SubscribeLevelUpController()
    {
        levelUpController.SubscribeEvents();
        levelUpController.CurrentSpawnPoint.Subscribe(sp => currentSpawnPointText.text = $"영웅 소환 재화: {sp}").AddTo(this);
    }

    private void UnsubscribeLevelUpController()
    {
        levelUpController.UnsubscribeEvents();
    }

    private void IncreaseEnemyCount()
    {
        spawnedEnemyCount++;
        spawnedEnemyCountText.text = $"{spawnedEnemyCount} / 100";
        spawnedEnemyCountSlider.value = spawnedEnemyCount / 100f;
    }

    private void DecreaseEnemyCount()
    {
        spawnedEnemyCount--;
        spawnedEnemyCountText.text = $"{spawnedEnemyCount} / 100";
        spawnedEnemyCountSlider.value = spawnedEnemyCount / 100f;
    }
}