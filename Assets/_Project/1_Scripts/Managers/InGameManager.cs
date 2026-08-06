using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 인게임 관리 담당 클래스
/// </summary>
public class InGameManager : MonoSingleton<InGameManager>, IEventListener
{
    protected override bool isInitialized { get; set; }
    protected override bool IsDontDestroyOnLoad => false;
    
    private InGameUIController uiController;

    private WaveController waveController;
    
    private AbilityEffectFactory abilityEffectFactory;
    private InGameHeroLevelUpController heroLevelUpController;
    private InGameHeroBuffController heroBuffController;
    private InGameCurrencyController currencyController;
    private InGameRewardController rewardController;
    private SummonController summonController;
    
    private HeroSpawnPool heroSpawnPool;
    private EnemySpawnPool enemySpawnPool;

    private readonly float[] gameSpeeds = { 1f, 2f, 3f };
    private int currentGameSpeedIndex;
    
    private Action<InGameFinishEventData> onGameFinish;
    
    public AbilityEffectFactory AbilityEffectFactory => abilityEffectFactory;
    public InGameHeroLevelUpController HeroLevelUpController => heroLevelUpController;
    public InGameHeroBuffController HeroBuffController => heroBuffController;
    public InGameCurrencyController CurrencyController => currencyController;
    public SummonController SummonController => summonController;
    public HeroSpawnPool HeroSpawnPool => heroSpawnPool;
    public EnemySpawnPool EnemySpawnPool => enemySpawnPool;
    public float CurrentGameSpeed => gameSpeeds[currentGameSpeedIndex];

    public override async UniTask InitializeAsync()
    {
        if (isInitialized)
        {
            return;
        }

        ResetGameSpeed();
        
        uiController = new InGameUIController();
        
        waveController = new WaveController();
        
        abilityEffectFactory = new AbilityEffectFactory();
        heroLevelUpController =  new InGameHeroLevelUpController();
        heroBuffController = new InGameHeroBuffController();
        currencyController = new InGameCurrencyController();
        rewardController = new InGameRewardController();
        summonController = new SummonController();
        
        heroSpawnPool = new HeroSpawnPool();
        enemySpawnPool = new EnemySpawnPool();

        await uiController.InitializeAsync();

        await summonController.InitializeAsync();
        
        await heroSpawnPool.InitializeAsync();
        await enemySpawnPool.InitializeAsync();
        await DamageCalculator.InitializeAsync();
        await HeroAttackState.PreLoadProjectileAsync();
        
        SubscribeEvents();
        
        EventManager.Dispatch(GameEventType.GameStart);
        
        isInitialized = true;
    }

    #region Unity Methods

    private void Update()
    {
        waveController?.Update();
        currencyController?.GainSpawnPointAbilityEffect();
    }

    protected override void OnDestroy()
    {
        ResetGameSpeed();
        UnsubscribeEvents();
        base.OnDestroy();
    }

    #endregion
    
    #region IEventListener implementation
    
    public void SubscribeEvents()
    {
        onGameFinish += GameFinish;
        EventManager.Subscribe(GameEventType.GameFinish, onGameFinish);
        
        waveController.SubscribeEvents();
        
        uiController.SubscribeEvents();
        uiController.UIInGame.WaveInfoUI.SubscribeWaveTimer(waveController.CurrentWaveTime);
        
        abilityEffectFactory.SubscribeEvents();
        
        heroLevelUpController.SubscribeEvents();
        
        heroBuffController.SubscribeEvents();
        heroBuffController?.RegisterAbilityEffect(abilityEffectFactory);
        
        currencyController.SubscribeEvents();
        currencyController.RegisterAbilityEffect(abilityEffectFactory);
        
        summonController.RegisterAbilityEffect(abilityEffectFactory);
    }

    public void UnsubscribeEvents()
    {
        summonController?.UnregisterAbilityEffect(abilityEffectFactory);
        
        currencyController?.UnregisterAbilityEffect(abilityEffectFactory);
        currencyController?.UnsubscribeEvents();
        
        heroBuffController?.UnregisterAbilityEffect(abilityEffectFactory);
        heroBuffController?.UnsubscribeEvents();
        
        heroLevelUpController?.UnsubscribeEvents();
        
        abilityEffectFactory?.UnsubscribeEvents();
        
        uiController?.UnsubscribeEvents();
        
        waveController?.UnsubscribeEvents();
        
        EventManager.Unsubscribe(GameEventType.GameFinish, onGameFinish);
        onGameFinish -= GameFinish;
    }
    
    #endregion

    /// <summary>
    /// 게임 속도 변경 (순회 변경)
    /// </summary>
    public void ToggleGameSpeed()
    {
        currentGameSpeedIndex = (currentGameSpeedIndex + 1) % gameSpeeds.Length;
        Time.timeScale = gameSpeeds[currentGameSpeedIndex];
    }

    /// <summary>
    /// 게임 일시정지
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    /// <summary>
    /// 게임 재게
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = gameSpeeds[currentGameSpeedIndex];
    }
    
    /// <summary>
    /// 게임 속도 리셋 (TimeScale = 1f)
    /// </summary>
    private void ResetGameSpeed()
    {
        currentGameSpeedIndex = 0;
        Time.timeScale = gameSpeeds[currentGameSpeedIndex];
    }

    /// <summary>
    /// 게임 종료
    /// </summary>
    private void GameFinish(InGameFinishEventData eventData)
    {
        PauseGame();
        UIManager.Instance.OpenAsync<UIGameResult>(eventData).Forget();
        CDebug.Log(eventData.IsGameVictory ? "[InGameManager] 게임 승리" : "[InGameManager] 게임 패배");
    }
}