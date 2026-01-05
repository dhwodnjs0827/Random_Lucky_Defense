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
    [SerializeField] private InGameLevelUpButtonComponent[] levelUpButtons;
    [SerializeField] private Button exchangeButton;
    [SerializeField] private Button sellButton;
    
    private InGameHeroLevelUpController levelUpController;

    private void Awake()
    {
        levelUpController = new InGameHeroLevelUpController();
        spawnPointCostText.text = $"영웅 소환\n{GameConstants.HERO_SPAWN_POINT_COST}";
    }

    private void OnEnable()
    {
        InitializeButtons();
        SubscribeEvents();
    }

    private void Update()
    {
        levelUpController?.GainSpawnPointCardEffect();
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

    private void OnClickExchangeButton()
    {
        //TODO: 영웅 교환 기능 구현 필요
    }

    private void OnClickSellButton()
    {
        //TODO: 영웅 판매 기능 구현 필요
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
        
        levelUpController.CurrentSpawnPoint.Subscribe(sp => spawnButton.interactable = sp >= GameConstants.HERO_SPAWN_POINT_COST).AddTo(this);
        levelUpController.CurrentSpawnPoint.Subscribe(sp => currentSpawnPointText.text = $"영웅 소환 재화: {sp}").AddTo(this);
        
        if (levelUpButtons != null)
        {
            foreach (var levelUpButton in levelUpButtons)
            {
                levelUpButton.SubscribeLevelUpController(levelUpController);
            }
        }
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