/// <summary>
/// Normal 타입 Enemy 클래스
/// </summary>
public class NormalEnemy : BaseEnemy
{
    public override void Die()
    {
        EventManager.Dispatch(GameEventType.NormalEnemyDie);
        base.Die();
    }
}
