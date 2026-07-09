using UnityEngine;

public class RedDragonProjectile : BaseProjectile
{
    protected override void FlyToTarget()
    {
        base.FlyToTarget();
        HeadingToTarget();
    }

    private void HeadingToTarget()
    {
        var targetPos = projectileData.Target != null && projectileData.Target.Transform != null
            ? projectileData.Target.Transform.position
            : transform.position;

        var direction = targetPos - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 기본 방향이 왼쪽(180도)이므로 180도를 빼서 보정
        transform.rotation = Quaternion.Euler(0, 0, angle - 180f);
    }

    public override void OnRelease()
    {
        base.OnRelease();
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
