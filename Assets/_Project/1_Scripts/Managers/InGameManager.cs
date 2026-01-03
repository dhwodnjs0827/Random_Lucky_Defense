using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class InGameManager : MonoSingleton<InGameManager>, IEventListener
{
    protected override bool IsDontDestroyOnLoad => false;
    private bool isInitialized = false;

    private readonly float[] gameSpeeds = { 1f, 1.5f, 2f };
    private int currentGameSpeedIndex;
    
    private Action<GameFinishEventData> onGameFinish;
    
    public float CurrentGameSpeed => gameSpeeds[currentGameSpeedIndex];

    public async UniTask InitializeAsync()
    {
        if (isInitialized)
        {
            return;
        }

        ResetTimeScale();
        currentGameSpeedIndex = 0;
        
        isInitialized = true;
        await UniTask.CompletedTask;
    }
    
    protected override void Awake()
    {
        base.Awake();
        SubscribeEvents();
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
        EventManager.Subscribe(GameEventType.GameFinish, onGameFinish);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.GameFinish, onGameFinish);
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
    private void GameFinish(GameFinishEventData eventData)
    {
        PauseGame();
        UIManager.Instance.Open<GameResultUI>();
        if (eventData.IsGameVictory)
        {
            CDebug.Log("[InGameManager] 게임 승리");
        }
        else
        {
            CDebug.Log("[InGameManager] 게임 패배");
        }
    }
}