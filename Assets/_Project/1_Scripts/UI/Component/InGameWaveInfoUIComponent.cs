using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인게임 웨이브 정보 UI
/// </summary>
public class InGameWaveInfoUIComponent : MonoBehaviour, IEventListener
{
    [SerializeField] private TextMeshProUGUI currentWaveText;
    [SerializeField] private TextMeshProUGUI waveTimerText;
    [SerializeField] private Image enemyImage;
    [SerializeField] private TextMeshProUGUI enemyTypeText;
    [SerializeField] private TextMeshProUGUI monsterTypeText;
    [SerializeField] private Slider bossHealthSlider;
    
    private Action<WaveStartEventData> onWaveStart;

    public void SubscribeWaveTimer(ReactiveProperty<float> waveTimer)
    {
        waveTimer.Subscribe(waveTime => waveTimerText.text = TimeFormatUtil.ToMMSSms(waveTime))
            .AddTo(this);
    }

    public void SubscribeBossHealth(ReactiveProperty<float> bossHealth)
    {
        bossHealth.Subscribe(health => bossHealthSlider.value = health);
    }

    public void SubscribeEvents()
    {
        onWaveStart += SetWaveDate;
        EventManager.Subscribe(GameEventType.WaveStart, onWaveStart);
        EventManager.Subscribe(GameEventType.SpawnBossEnemy, ActiveBossHealthSlider);
        EventManager.Subscribe(GameEventType.BossEnemyDie, InactiveBossHealthSlider);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.BossEnemyDie, InactiveBossHealthSlider);
        EventManager.Unsubscribe(GameEventType.SpawnBossEnemy, ActiveBossHealthSlider);
        EventManager.Unsubscribe(GameEventType.WaveStart, onWaveStart);
        onWaveStart -= SetWaveDate;
    }

    private void SetWaveDate(WaveStartEventData data)
    {
        LoadDataAsync(data).Forget();
    }

    private async UniTask LoadDataAsync(WaveStartEventData data)
    {
        currentWaveText.text = $"WAVE {data.CurrentWaveData.WaveIndex}/101";
        monsterTypeText.text = $"{data.CurrentEnemyData.MonsterType}";
        enemyTypeText.text = $"{data.CurrentEnemyData.EnemyType}";
        enemyImage.sprite = await AddressableManager.Instance.LoadAsync<Sprite>($"Sprites/Enemy/{data.CurrentEnemyData.MonsterType}_{data.CurrentEnemyData.EnemyType}");
    }

    private void ActiveBossHealthSlider()
    {
        bossHealthSlider.value = bossHealthSlider.maxValue;
        bossHealthSlider.gameObject.SetActive(true);
    }
    
    private void InactiveBossHealthSlider()
    {
        bossHealthSlider.gameObject.SetActive(false);
    }
}