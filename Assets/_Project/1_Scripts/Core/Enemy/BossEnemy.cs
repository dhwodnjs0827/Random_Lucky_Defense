using Generated;
using UniRx;
using UnityEngine.Splines;

/// <summary>
/// Boss 타입 Enemy 클래스
/// </summary>
public class BossEnemy : BaseEnemy
{
    private ReactiveProperty<float> healthPercentage;
    
    public override void Initialize(EnemyDataSO data, WaveDataSO waveData, SplineContainer splineContainer)
    {
        base.Initialize(data, waveData, splineContainer);
        healthPercentage = new();
        healthPercentage.Value = currentHealth / maxHealth;
        InGameManager.Instance.UIController.UIInGame.WaveInfoUI.SubscribeBossHealth(healthPercentage);
    }

    public override void TakeDamage(DamageContext damageContext)
    {
        base.TakeDamage(damageContext);
        healthPercentage.Value = currentHealth / maxHealth;
    }

    public override void Die()
    {
        EventManager.Dispatch(GameEventType.BossEnemyDie);
        healthPercentage.Dispose();
        base.Die();
    }
}
