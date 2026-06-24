#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class InGameCurrencyCheatTab : ICheatTab
{
    public string TabName => "재화";
    
    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void DrawTab()
    {
        GUILayout.Label("재화 치트", EditorStyles.boldLabel);
    }

    public void Reset()
    {
    }
}

#endif