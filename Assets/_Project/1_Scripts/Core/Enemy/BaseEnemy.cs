using UnityEngine;

/// <summary>
/// 모든 Enemy의 부모 클래스
/// </summary>
public abstract class BaseEnemy : MonoBehaviour
{
    /// <summary>
    /// 적 초기화
    /// </summary>
    public abstract void Initialize();
}
