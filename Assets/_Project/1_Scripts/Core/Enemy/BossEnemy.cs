using UnityEngine;

public class BossEnemy : BaseEnemy
{
    public override void Die()
    {
        EventManager.Dispatch(GameEventType.BossEnemyDie);
        base.Die();
    }
}
