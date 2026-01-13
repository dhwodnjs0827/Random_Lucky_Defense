/// <summary>
/// Boss 타입 Enemy 클래스
/// </summary>
public class BossEnemy : BaseEnemy
{
    public override void Die()
    {
        EventManager.Dispatch(GameEventType.BossEnemyDie);
        base.Die();
    }
}
