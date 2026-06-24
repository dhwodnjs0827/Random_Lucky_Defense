#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using UnityEngine;

public class AbilityCheatTab : ICheatTab
{
    public string TabName => "재능";

    private AbilityContainer[] availableAbilities;
    private string[] abilityNames;
    private int selectedAbilityIndex;
    
    private Vector2 activatedAbilitiesScrollPos;

    public void OnEnter()
    {
        LoadAvailableAbilities();
    }

    public void OnExit() { }

    public void DrawTab()
    {
        GUILayout.Label("재능 치트", EditorStyles.boldLabel);
        GUILayout.Space(10);

        DrawActivatedAbilitiesSection();
        GUILayout.Space(10);
        DrawAbilitySection();
    }

    public void Reset()
    {
        availableAbilities = null;
        abilityNames = null;
        selectedAbilityIndex = 0;
    }

    private void DrawAbilitySection()
    {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("재능 활성화", EditorStyles.boldLabel);
        if (GUILayout.Button("새로고침", GUILayout.Width(60)))
        {
            LoadAvailableAbilities();
        }
        EditorGUILayout.EndHorizontal();

        if (availableAbilities != null && availableAbilities.Length > 0)
        {
            selectedAbilityIndex = Mathf.Clamp(selectedAbilityIndex, 0, availableAbilities.Length - 1);
            selectedAbilityIndex = EditorGUILayout.Popup("선택할 재능", selectedAbilityIndex, abilityNames);

            // 선택된 재능 정보 표시
            var selectedAbility = availableAbilities[selectedAbilityIndex];
            var desc = selectedAbility.AbilityData.Description.ReplaceValues(
                ("{value}", selectedAbility.AbilityLevelData.value),
                ("{value1}", selectedAbility.AbilityLevelData.value1));
            EditorGUILayout.HelpBox($"효과: {desc}", MessageType.Info);

            if (GUILayout.Button("재능 활성화", GUILayout.Height(25)))
            {
                ActivateSelectedAbility();
            }
        }
        else
        {
            GUILayout.Label("선택 가능한 재능이 없습니다.");
            if (GUILayout.Button("재능 데이터 로드"))
            {
                LoadAvailableAbilities();
            }
        }

        EditorGUILayout.EndVertical();
    }

    private void LoadAvailableAbilities()
    {
        var factory = InGameManager.Instance?.AbilityEffectFactory;
        if (factory == null)
        {
            CDebug.LogWarning("[AbilityCheatTab] AbilityEffectFactory를 찾을 수 없습니다.");
            availableAbilities = null;
            abilityNames = null;
            return;
        }

        availableAbilities = factory.CheatGetAvailableAbilities();

        if (availableAbilities == null || availableAbilities.Length == 0)
        {
            abilityNames = null;
            return;
        }

        abilityNames = availableAbilities
            .Select(a => $"{a.AbilityData.Name} (Lv.{a.AbilityLevelData.Level})")
            .ToArray();

        CDebug.Log($"[AbilityCheatTab] {availableAbilities.Length}개의 재능 로드 완료");
    }

    private void ActivateSelectedAbility()
    {
        if (availableAbilities == null || selectedAbilityIndex >= availableAbilities.Length) return;

        var factory = InGameManager.Instance?.AbilityEffectFactory;
        if (factory == null)
        {
            CDebug.LogError("[AbilityCheatTab] AbilityEffectFactory를 찾을 수 없습니다.");
            return;
        }

        var selectedAbility = availableAbilities[selectedAbilityIndex];
        factory.CheatActivateAbility(selectedAbility);

        // 활성화 후 목록 새로고침
        LoadAvailableAbilities();
    }

    private void DrawActivatedAbilitiesSection()
    {
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("활성화된 재능 목록", EditorStyles.boldLabel);

        var factory = InGameManager.Instance?.AbilityEffectFactory;
        if (factory == null)
        {
            GUILayout.Label("AbilityEffectFactory를 찾을 수 없습니다.");
            EditorGUILayout.EndVertical();
            return;
        }

        var activatedAbilities = factory.CheatGetActivatedAbilities();

        if (activatedAbilities.Count == 0)
        {
            GUILayout.Label("활성화된 재능이 없습니다.");
        }
        else
        {
            activatedAbilitiesScrollPos = EditorGUILayout.BeginScrollView(
                activatedAbilitiesScrollPos, GUILayout.MaxHeight(150));

            foreach (var (data, level) in activatedAbilities)
            {
                EditorGUILayout.BeginHorizontal("helpbox");
                GUILayout.Label($"{data.Name}", GUILayout.Width(150));
                GUILayout.Label($"Lv.{level}", GUILayout.Width(50));
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        EditorGUILayout.EndVertical();
    }
}

#endif