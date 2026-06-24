#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class AbilityCheatTab : ICheatTab
{
    public string TabName => "재능";
    
    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void DrawTab()
    {
        GUILayout.Label("재능 치트", EditorStyles.boldLabel);
    }

    public void Reset()
    {
    }
}

#endif