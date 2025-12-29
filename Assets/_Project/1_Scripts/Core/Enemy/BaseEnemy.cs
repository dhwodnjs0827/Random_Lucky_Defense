using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// 모든 Enemy의 부모 클래스
/// </summary>
public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] private SplineAnimate splineAnimate;

    /// <summary>
    /// 적 초기화
    /// </summary>
    public abstract void Initialize();

    public void InitializeSpline(SplineContainer splineContainer)
    {
        if (splineAnimate != null && splineAnimate.Container == null)
        {
            splineAnimate.Container = splineContainer;
        }
        
        splineAnimate.Play();
    }
}
