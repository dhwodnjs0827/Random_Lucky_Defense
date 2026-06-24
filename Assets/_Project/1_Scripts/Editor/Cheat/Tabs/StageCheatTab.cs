#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class StageCheatTab : ICheatTab
{
    public string TabName => "스테이지";

    public void OnEnter() { }

    public void OnExit() { }

    public void DrawTab()
    {
        GUILayout.Label("스테이지 치트", EditorStyles.boldLabel);
    }

    public void Reset() { }
}

#endif