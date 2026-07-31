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
    
    private AbilityEffectFactory abilityEffectFactory;
    private InGameHeroLevelUpController heroLevelUpController;
    private InGameHeroBuffController heroBuffController;
    private InGameCurrencyController currencyController;
    private InGameRewardController rewardController;

    private readonly float[] gameSpeeds = { 1f, 2f, 3f };
    private int currentGameSpeedIndex;
    
    private GameDifficultyType gameDifficulty;
    
    private Action<InGameFinishEventData> onGameFinish;
    
    public AbilityEffectFactory AbilityEffectFactory => abilityEffectFactory;
    public InGameHeroLevelUpController HeroLevelUpController => heroLevelUpController;
    public InGameHeroBuffController HeroBuffController => heroBuffController;
    public InGameCurrencyController CurrencyController => currencyController;
    public float CurrentGameSpeed => gameSpeeds[currentGameSpeedIndex];

    public override async UniTask InitializeAsync()
    {
        if (isInitialized)
        {
            return;
        }

        ResetGameSpeed();
        
        abilityEffectFactory = new AbilityEffectFactory();
        await abilityEffectFactory.InitializeDataAsync();
        heroLevelUpController =  new InGameHeroLevelUpController();
        await heroLevelUpController.InitializeLevelUpDataAsync();
        heroBuffController = new InGameHeroBuffController();
        currencyController = new InGameCurrencyController();
        rewardController = new InGameRewardController();

        await HeroAttackState.PreLoadProjectileAsync();
        
        UIManager.Instance.Open<UIInGame>();
        
        CreateBackground();
        
        SubscribeEvents();
        
        isInitialized = true;
        await UniTask.CompletedTask;
    }

    #region Unity Methods

    private void Update()
    {
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
        
        abilityEffectFactory.SubscribeEvents();
        
        heroLevelUpController.SubscribeEvents();
        
        heroBuffController.SubscribeEvents();
        heroBuffController?.RegisterAbilityEffect(abilityEffectFactory);
        
        currencyController.SubscribeEvents();
        currencyController.RegisterAbilityEffect(abilityEffectFactory);
    }

    public void UnsubscribeEvents()
    {
        currencyController?.UnregisterAbilityEffect(abilityEffectFactory);
        currencyController?.UnsubscribeEvents();
        
        heroBuffController?.UnregisterAbilityEffect(abilityEffectFactory);
        heroBuffController?.UnsubscribeEvents();
        
        heroLevelUpController?.UnsubscribeEvents();
        
        abilityEffectFactory?.UnsubscribeEvents();
        
        EventManager.Unsubscribe(GameEventType.GameFinish, onGameFinish);
        onGameFinish -= GameFinish;
    }
    
    #endregion

    /// <summary>
    /// 게임 난이도 설정
    /// </summary>
    public void SetGameDifficulty(GameDifficultyType difficulty)
    {
        gameDifficulty = difficulty;
        CDebug.Log($"[InGameManager] 게임 난이도: {gameDifficulty}");
    }

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
        UIManager.Instance.Open<UIGameResult>(eventData);
        CDebug.Log(eventData.IsGameVictory ? "[InGameManager] 게임 승리" : "[InGameManager] 게임 패배");
    }

    /// <summary>
    /// 인게임 스테이지 백그라운드 오브젝트 생성
    /// </summary>
    private void CreateBackground()
    {
        var backgroundPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/Background");
        var background = Instantiate(backgroundPrefab);
        background.transform.position = Vector3.zero;
    }
}