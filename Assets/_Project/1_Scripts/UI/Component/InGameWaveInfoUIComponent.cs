using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인게임 웨이브 정보 UI
/// </summary>
public class InGameWaveInfoUIComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentWaveText;
    [SerializeField] private TextMeshProUGUI waveTimerText;
    [SerializeField] private Image enemyImage;
    [SerializeField] private TextMeshProUGUI enemyTypeText;
    [SerializeField] private TextMeshProUGUI monsterTypeText;

    public void SubscribeEnemyController(EnemyWaveController controller)
    {
        controller.CurrentWaveData.Where(data => data != null)
            .Subscribe(waveData => currentWaveText.text = $"WAVE {waveData.WaveIndex}/101")
            .AddTo(this);

        controller.CurrentWaveTime.Subscribe(waveTime => waveTimerText.text = TimeFormatUtil.ToMMSSms(waveTime))
            .AddTo(this);
    }
}