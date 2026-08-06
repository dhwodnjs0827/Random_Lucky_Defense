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

    private void Awake()
    {
        spawnPointCostText.text = $"영웅 소환\n{GameConstants.HERO_SPAWN_POINT_COST}";
        InitializeButtons();
    }

    private void Start()
    {
        SubscribeController(InGameManager.Instance.HeroLevelUpController, InGameManager.Instance.CurrencyController);
    }

    private void OnDestroy()
    {
        ClearButtons();
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
        EventManager.Subscribe(GameEventType.EnemySpawned, IncreaseEnemyCount);
        EventManager.Subscribe(GameEventType.EnemyDie, DecreaseEnemyCount);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.EnemySpawned, IncreaseEnemyCount);
        EventManager.Unsubscribe(GameEventType.EnemyDie, DecreaseEnemyCount);
    }

    private void SubscribeController(InGameHeroLevelUpController levelUp, InGameCurrencyController currency)
    {
        currency.CurrentSpawnPoint.Subscribe(sp => spawnButton.interactable = sp >= GameConstants.HERO_SPAWN_POINT_COST).AddTo(this);
        currency.CurrentSpawnPoint.Subscribe(sp => currentSpawnPointText.text = $"영웅 소환 재화: {sp}").AddTo(this);
        
        if (levelUpButtons != null)
        {
            foreach (var levelUpButton in levelUpButtons)
            {
                levelUpButton.SubscribeController(levelUp, currency);
            }
        }
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