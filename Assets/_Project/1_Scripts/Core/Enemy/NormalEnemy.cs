using UnityEngine;

public class NormalEnemy : BaseEnemy
{
    public override void Die()
    {
        EventManager.Dispatch(GameEventType.NormalEnemyDie);
        base.Die();
    }
}
