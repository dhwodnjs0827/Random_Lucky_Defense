#if UNITY_EDITOR

using System.Linq;
using Generated;
using UnityEditor;
using UnityEngine;

public class InGameHeroCheatTab : ICheatTab
{
    public string TabName => "영웅";

    private HeroDataSO[] heroDataList;
    private string[] heroDataNames;
    private int selectedHeroDataIndex;
    private int selectedSpawnedHeroIndex;

    public void OnEnter()
    {
        LoadHeroDataList();
    }

    public void OnExit()
    {
    }

    public void DrawTab()
    {
        GUILayout.Label("영웅 치트", EditorStyles.boldLabel);
        GUILayout.Space(10);

        DrawSpawnSection();
        GUILayout.Space(10);
        DrawRemoveSection();
    }

    public void Reset()
    {
        heroDataList = null;
        heroDataNames = null;
        selectedHeroDataIndex = 0;
        selectedSpawnedHeroIndex = 0;
    }

    private void DrawSpawnSection()
    {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("영웅 소환", EditorStyles.boldLabel);
        if (GUILayout.Button("새로고침", GUILayout.Width(60)))
        {
            heroDataList = null;
            heroDataNames = null;
            LoadHeroDataList();
        }

        EditorGUILayout.EndHorizontal();

        if (heroDataList != null && heroDataList.Length > 0)
        {
            selectedHeroDataIndex = EditorGUILayout.Popup("소환할 영웅", selectedHeroDataIndex, heroDataNames);

            if (GUILayout.Button("영웅 소환", GUILayout.Height(25)))
            {
                SpawnSelectedHero();
            }
        }
        else
        {
            GUILayout.Label("소환 가능한 영웅이 없습니다.");
            if (GUILayout.Button("영웅 데이터 로드"))
            {
                LoadHeroDataList();
            }
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawRemoveSection()
    {
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("영웅 제거", EditorStyles.boldLabel);

        var areaController = Object.FindFirstObjectByType<HeroAreaController>();
        if (areaController != null)
        {
            var spawnedHeroes = areaController.CheatGetAllSpawnedHeroes();

            if (spawnedHeroes.Count > 0)
            {
                var heroNames = spawnedHeroes.Select((h, i) => $"[{i}] {h.name}").ToArray();

                selectedSpawnedHeroIndex = Mathf.Clamp(selectedSpawnedHeroIndex, 0, heroNames.Length - 1);
                selectedSpawnedHeroIndex = EditorGUILayout.Popup("제거할 영웅", selectedSpawnedHeroIndex, heroNames);

                if (GUILayout.Button("선택 영웅 제거", GUILayout.Height(25)))
                {
                    if (selectedSpawnedHeroIndex < spawnedHeroes.Count)
                    {
                        areaController.CheatRemoveHero(spawnedHeroes[selectedSpawnedHeroIndex]);
                        selectedSpawnedHeroIndex = 0;
                    }
                }
            }
            else
            {
                GUILayout.Label("소환된 영웅이 없습니다.");
            }

            GUILayout.Space(5);

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("모든 영웅 제거", GUILayout.Height(25)))
            {
                areaController.CheatRemoveAllHeroes();
                selectedSpawnedHeroIndex = 0;
            }

            GUI.backgroundColor = Color.white;
        }
        else
        {
            GUILayout.Label("HeroAreaController를 찾을 수 없습니다.");
        }

        EditorGUILayout.EndVertical();
    }

    private void LoadHeroDataList()
    {
        if (heroDataList != null) return;

        heroDataList = Resources.LoadAll<HeroDataSO>("Data/SO/HeroData");

        if (heroDataList == null || heroDataList.Length == 0)
        {
            CDebug.LogWarning("[HeroCheatTab] HeroDataSO를 찾을 수 없습니다. 경로: Resources/Data/SO/HeroData");
            return;
        }

        heroDataList = heroDataList
            .OrderBy(h => h.ClassType)
            .ThenBy(h => h.GradeType)
            .ToArray();

        heroDataNames = heroDataList
            .Select(h => $"{h.Name}")
            .ToArray();

        CDebug.Log($"[HeroCheatTab] {heroDataList.Length}개의 영웅 데이터 로드 완료");
    }

    private void SpawnSelectedHero()
    {
        if (heroDataList == null || selectedHeroDataIndex >= heroDataList.Length) return;

        var areaController = Object.FindFirstObjectByType<HeroAreaController>();
        if (areaController == null)
        {
            CDebug.LogError("[HeroCheatTab] HeroAreaController를 찾을 수 없습니다.");
            return;
        }

        var heroData = heroDataList[selectedHeroDataIndex];
        var prefab = Resources.Load<BaseHero>($"Prefabs/Hero/{heroData.Name}");

        if (prefab == null)
        {
            CDebug.LogError($"[HeroCheatTab] {heroData.Name} 프리팹을 찾을 수 없습니다.");
            return;
        }

        var hero = Object.Instantiate(prefab, areaController.SpawnPoint.position, Quaternion.identity);
        hero.Initialize(heroData);
        areaController.PlaceHero(hero);
    }
}

#endif