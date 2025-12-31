using Cysharp.Threading.Tasks;
using UnityEngine;

public class InGameManager : MonoSingleton<InGameManager>, IEventListener
{
    protected override bool IsDontDestroyOnLoad => false;
    private bool isInitialized = false;

    private readonly float[] gameSpeeds = { 1f, 1.5f, 2f };
    private int currentGameSpeedIndex;
    
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
        EventManager.Subscribe(GameEventType.GameOver, GameOver);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.GameOver, GameOver);
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
    /// 게임 오버
    /// </summary>
    private void GameOver()
    {
        PauseGame();
        UIManager.Instance.Open<GameResultUI>();
        CDebug.Log("[InGameManager] 게임 오버");
    }
}