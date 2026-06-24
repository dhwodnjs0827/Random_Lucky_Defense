#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class InGameCurrencyCheatTab : ICheatTab
{
    public string TabName => "재화";

    private readonly string[] currencyTypeNames = { "SP", "행운석" };
    private int selectedCurrencyIndex;
    private int changeAmount;

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void DrawTab()
    {
        GUILayout.Label("인게임 재화 치트", EditorStyles.boldLabel);
        GUILayout.Space(10);

        DrawCurrencySection();
        GUILayout.Space(10);
        DrawSpGainRateSection();
    }

    public void Reset()
    {
        selectedCurrencyIndex = 0;
        changeAmount = 0;
    }

    private void DrawCurrencySection()
    {
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("재화 변경", EditorStyles.boldLabel);

        selectedCurrencyIndex = EditorGUILayout.Popup("재화 종류", selectedCurrencyIndex, currencyTypeNames);
        changeAmount = EditorGUILayout.IntSlider("변경량", changeAmount, -1000, 1000);
        
        if (GUILayout.Button("변경", GUILayout.Height(25)))
        {
            ApplyCurrencyChange();
        }

        if (GUILayout.Button("재화 0으로 초기화", GUILayout.Height(25)))
        {
            ResetCurrencyToZero();
        }

        EditorGUILayout.EndVertical();
    }

    private void ApplyCurrencyChange()
    {
        var currencyType = (CheatInGameCurrencyType)selectedCurrencyIndex;

        switch (currencyType)
        {
            case CheatInGameCurrencyType.SP:
                var currencyController = InGameManager.Instance?.CurrencyController;
                if (currencyController != null)
                {
                    currencyController.CheatAddSP(changeAmount);
                }

                break;

            case CheatInGameCurrencyType.LuckyStone:
                // TODO: 행운석 변경 기능 구현
                CDebug.Log($"[InGameCurrencyCheatTab] 행운석 {changeAmount:+#;-#;0} (미구현)");
                break;
        }
    }

    private void ResetCurrencyToZero()
    {
        var currencyType = (CheatInGameCurrencyType)selectedCurrencyIndex;

        switch (currencyType)
        {
            case CheatInGameCurrencyType.SP:
                var currencyController = InGameManager.Instance?.CurrencyController;
                if (currencyController != null)
                {
                    currencyController.CheatSetSP(0);
                }

                break;

            case CheatInGameCurrencyType.LuckyStone:
                // TODO: 행운석 초기화 기능 구현
                CDebug.Log("[InGameCurrencyCheatTab] 행운석 0으로 초기화 (미구현)");
                break;
        }
    }

    private void DrawSpGainRateSection()
    {
        var currencyController = InGameManager.Instance?.CurrencyController;
        if (currencyController == null)
        {
            return;
        }

        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("SP 획득률 정보", EditorStyles.boldLabel);

        var (isActive, interval, _) = currencyController.CheatGetSPGainRateInfo();

        if (isActive)
        {
            EditorGUILayout.LabelField("탐욕 재능 상태", "활성화");
            EditorGUILayout.LabelField("획득 간격", $"{interval}초");
        }
        else
        {
            EditorGUILayout.LabelField("탐욕 재능 상태", "비활성화");
        }

        EditorGUILayout.EndVertical();
    }

    private enum CheatInGameCurrencyType
    {
        SP,
        LuckyStone
    }
}

#endif