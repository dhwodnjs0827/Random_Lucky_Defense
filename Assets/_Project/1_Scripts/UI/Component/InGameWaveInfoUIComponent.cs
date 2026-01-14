using System;
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
    
    private Action<WaveStartEventData> onWaveStart;

    public void SubscribeWaveTimer(ReactiveProperty<float> waveTimer)
    {
        waveTimer.Subscribe(waveTime => waveTimerText.text = TimeFormatUtil.ToMMSSms(waveTime))
            .AddTo(this);
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    public void SubscribeEvents()
    {
        onWaveStart += SetWaveDate;
        EventManager.Subscribe(GameEventType.WaveStart, onWaveStart);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.WaveStart, onWaveStart);
        onWaveStart -= SetWaveDate;
    }

    private void SetWaveDate(WaveStartEventData data)
    {
        enemyImage.sprite = ResourceManager.Instance.Load<Sprite>($"Sprites/Enemy/{data.CurrentEnemyData.MonsterType}_{data.CurrentEnemyData.EnemyType}");
        currentWaveText.text = $"WAVE {data.CurrentWaveData.WaveIndex}/101";
        monsterTypeText.text = $"{data.CurrentEnemyData.MonsterType}";
        enemyTypeText.text = $"{data.CurrentEnemyData.EnemyType}";
    }
}