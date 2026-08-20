#if UNITY_EDITOR

using System;
using System.Linq;
using Generated;
using UnityEditor;
using UnityEngine;

public class InGameWaveCheatTab : ICheatTab
{
    public string TabName => "웨이브";

    private WaveDataSO[] waveDataList;
    private int selectedWaveDataIndex;
    private string[] waveDataNames;

    public void OnEnter()
    {
        LoadWaveDataList();
    }

    public void OnExit()
    {
    }

    public void DrawTab()
    {
        GUILayout.Label("웨이브 치트", EditorStyles.boldLabel);
        GUILayout.Space(10);

        DrawCurrentWaveSection();
        DrawWaveChangeSection();
    }

    public void Reset()
    {
    }

    private void LoadWaveDataList()
    {
        if (waveDataList != null) return;

        waveDataList = DataManager.Instance.WaveDataList.ToArray();

        if (waveDataList == null || waveDataList.Length == 0)
        {
            CDebug.LogWarning("[InGameWaveCheatTab] WaveDataSO가 없습니다.");
            return;
        }

        waveDataNames = waveDataList.Select(data => $"{data.WaveIndex}웨이브 - {data.WaveType}").ToArray();

        CDebug.Log($"[InGameWaveCheatTab] {waveDataList.Length}개의 웨이브 데이터 로드 완료");
    }

    private void DrawCurrentWaveSection()
    {
        var controller = InGameManager.Instance.WaveController;
        if (controller == null)
        {
            return;
        }

        EditorGUILayout.BeginHorizontal("box");
        
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("현재 웨이브 정보", EditorStyles.boldLabel);
        var index = Math.Max(controller.CurrentWaveDataIndex, 1);
        EditorGUILayout.LabelField("현재 웨이브", $"{index}웨이브");
        EditorGUILayout.LabelField("웨이브 종류", $"{waveDataList[index - 1].WaveType}");
        EditorGUILayout.LabelField("웨이브 체력 배율", $"{waveDataList[index - 1].WaveHpCoefficients}");
        EditorGUILayout.LabelField("웨이브 방어력 배율", $"{waveDataList[index - 1].WaveDefenseCoefficients}");
        EditorGUILayout.LabelField("웨이브 이동속도 배율", $"{waveDataList[index - 1].WaveSpeedCoefficients}");
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("현재 적 정보", EditorStyles.boldLabel);
        var currentEnemyData = DataManager.Instance.EnemyDataList.FirstOrDefault(data => data.ID.Equals(waveDataList[index - 1].SpawnEnemyID));
        EditorGUILayout.LabelField("적 타입", $"{currentEnemyData?.MonsterType}");
        EditorGUILayout.LabelField("적 체력", $"{currentEnemyData?.Health}");
        EditorGUILayout.LabelField("적 방어력", $"{currentEnemyData?.Defense}");
        EditorGUILayout.LabelField("적 이동속도", $"{currentEnemyData?.MoveSpeed}");
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
    }

    private void DrawWaveChangeSection()
    {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("웨이브 변경", EditorStyles.boldLabel);
        if (GUILayout.Button("새로고침", GUILayout.Width(60)))
        {
            waveDataList = null;
            LoadWaveDataList();
        }

        EditorGUILayout.EndHorizontal();

        if (waveDataList != null && waveDataList.Length > 0)
        {
            selectedWaveDataIndex = EditorGUILayout.Popup("변경할 웨이브", selectedWaveDataIndex, waveDataNames);

            if (GUILayout.Button("웨이브 변경", GUILayout.Height(25)))
            {
                ChangeWave();
            }
        }
        else
        {
            GUILayout.Label("변경 가능한 웨이브가 없습니다.");
            if (GUILayout.Button("웨이브 데이터 로드"))
            {
                LoadWaveDataList();
            }
        }

        EditorGUILayout.EndVertical();
    }

    private void ChangeWave()
    {
        var controller = InGameManager.Instance.WaveController;
        controller.CheatChangeWave(selectedWaveDataIndex);
    }
}

#endif