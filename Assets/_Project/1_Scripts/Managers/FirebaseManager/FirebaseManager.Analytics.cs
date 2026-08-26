using Firebase.Analytics;

public partial class FirebaseManager
{
    /// <summary>
    /// 커스텀 이벤트 로깅
    /// </summary>
    public void LogEvent(string eventName)
    {
        if (!isInitialized) return;
        FirebaseAnalytics.LogEvent(eventName);
        CDebug.Log($"[Firebase Analytics] eventName: {eventName}");
    }

    /// <summary>
    /// 파라미터와 함께 커스텀 이벤트 로깅
    /// </summary>
    public void LogEvent(string eventName, string paramName, string paramValue)
    {
        if (!isInitialized) return;
        FirebaseAnalytics.LogEvent(eventName, paramName, paramValue);
        CDebug.Log($"[Firebase Analytics] eventName: {eventName}, paramName: {paramName}, paramValue: {paramValue}");
    }

    /// <summary>
    /// 파라미터와 함께 커스텀 이벤트 로깅 (숫자)
    /// </summary>
    public void LogEvent(string eventName, string paramName, long paramValue)
    {
        if (!isInitialized) return;
        FirebaseAnalytics.LogEvent(eventName, paramName, paramValue);
        CDebug.Log($"[Firebase Analytics] eventName: {eventName}, paramName: {paramName}, paramValue: {paramValue}");
    }

    /// <summary>
    /// 사용자 ID 설정
    /// </summary>
    public void SetUserId(string userId)
    {
        if (!isInitialized) return;
        FirebaseAnalytics.SetUserId(userId);
    }

    /// <summary>
    /// 사용자 속성 설정
    /// </summary>
    public void SetUserProperty(string name, string value)
    {
        if (!isInitialized) return;
        FirebaseAnalytics.SetUserProperty(name, value);
    }
}