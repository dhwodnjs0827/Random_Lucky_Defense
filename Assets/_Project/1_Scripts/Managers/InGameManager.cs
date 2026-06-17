using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 인게임 관리 담당 클래스
/// </summary>
public class InGameManager : MonoSingleton<InGameManager>, IEventListener
{
    protected override bool IsDontDestroyOnLoad => false;
    private bool isInitialized = false;
    
    private AbilityEffectFactory abilityEffectFactory;
    private InGameHeroLevelUpController heroLevelUpController;
    private InGameHeroBuffController heroBuffController;

    private readonly float[] gameSpeeds = { 1f, 1.5f, 2f };
    private int currentGameSpeedIndex;
    
    private Action<InGameFinishEventData> onGameFinish;
    
    public AbilityEffectFactory AbilityEffectFactory => abilityEffectFactory;
    public InGameHeroLevelUpController HeroLevelUpController => heroLevelUpController;
    public InGameHeroBuffController HeroBuffController => heroBuffController;
    public float CurrentGameSpeed => gameSpeeds[currentGameSpeedIndex];

    public async UniTask InitializeAsync()
    {
        if (isInitialized)
        {
            return;
        }

        ResetTimeScale();
        currentGameSpeedIndex = 0;
        
        abilityEffectFactory = new AbilityEffectFactory();
        heroLevelUpController =  new InGameHeroLevelUpController();
        heroBuffController = new InGameHeroBuffController();
        
        UIManager.Instance.Open<UIInGame>();
        var backgroundPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/Background");
        Instantiate(backgroundPrefab);
        
        SubscribeEvents();
        
        isInitialized = true;
        await UniTask.CompletedTask;
    }

    private void Update()
    {
        heroLevelUpController?.GainSpawnPointAbilityEffect();
    }

    protected override void OnDestroy()
    {
        ResetTimeScale();
        UnsubscribeEvents();
        base.OnDestroy();
    }

    public void SubscribeEvents()
    {
        onGameFinish += GameFinish;
        EventManager.Subscribe(GameEventType.InGameFinish, onGameFinish);
        
        abilityEffectFactory.SubscribeEvents();
        heroLevelUpController.SubscribeEvents();
        heroBuffController.SubscribeEvents();
        heroLevelUpController.RegisterAbilityEffect(abilityEffectFactory);
        heroBuffController.RegisterAbilityEffect(abilityEffectFactory);
    }

    public void UnsubscribeEvents()
    {
        heroBuffController.UnregisterAbilityEffect(abilityEffectFactory);
        heroBuffController.UnsubscribeEvents();
        heroLevelUpController.UnregisterAbilityEffect(abilityEffectFactory);
        heroLevelUpController.UnsubscribeEvents();
        abilityEffectFactory.UnsubscribeEvents();
        
        EventManager.Unsubscribe(GameEventType.InGameFinish, onGameFinish);
        onGameFinish -= GameFinish;
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
    /// TimeScale 복구
    /// </summary>
    private void ResetTimeScale()
    {
        Time.timeScale = 1f;
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
}