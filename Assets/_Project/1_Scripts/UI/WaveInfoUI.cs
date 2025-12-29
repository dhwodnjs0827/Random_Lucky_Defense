using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class WaveInfoUI : UIBase
{
    [SerializeField] private TextMeshProUGUI currentWaveText;
    [SerializeField] private TextMeshProUGUI waveTimerText;
    [SerializeField] private Image enemyImage;
    [SerializeField] private TextMeshProUGUI enemyTypeText;
    [SerializeField] private TextMeshProUGUI monsterTypeText;

    protected override void Opened(params object[] args)
    {
        SubscribeEnemyController(args[0] as EnemyWaveController);
    }

    protected override void Closed(params object[] args)
    {
    }

    private void SubscribeEnemyController(EnemyWaveController controller)
    {
        controller.CurrentWaveData.Where(data => data != null)
            .Subscribe(waveData => currentWaveText.text = $"WAVE {waveData.WaveIndex}/101")
            .AddTo(this);

        controller.CurrentWaveTime.Subscribe(waveTime => waveTimerText.text = TimeFormatUtil.ToMMSSms(waveTime))
            .AddTo(this);
    }
}