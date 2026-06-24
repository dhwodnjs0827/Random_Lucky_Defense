#if UNITY_EDITOR

public interface ICheatTab
{
    string TabName { get; }
    void OnEnter();
    void OnExit();
    void DrawTab();
    void Reset();
}

#endif