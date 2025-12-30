using Cysharp.Threading.Tasks;

/// <summary>
/// Scene 관리용 기본 클래스
/// </summary>
public abstract class BaseScene
{
    public abstract SceneType SceneType { get; }

    /// <summary>
    /// Scene 초기화
    /// </summary>
    public virtual UniTask InitializeAsync() => UniTask.CompletedTask;
    
    /// <summary>
    /// Scene 정리
    /// </summary>
    public virtual UniTask CleanupAsync() => UniTask.CompletedTask;
}
