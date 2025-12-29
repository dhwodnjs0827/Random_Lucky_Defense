using TMPro;
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
    }

    protected override void Closed(params object[] args)
    {
    }
}
