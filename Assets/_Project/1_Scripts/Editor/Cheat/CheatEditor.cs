#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatEditor : EditorWindow
{
    private const string LOBBY_SCENE = "LobbyScene";
    private const string GAME_SCENE = "GameScene";

    // 탭 관련
    private int selectedLobbyTabIndex;
    private int selectedGameTabIndex;

    // 씬별 탭 목록
    private ICheatTab[] lobbyTabs;
    private ICheatTab[] gameTabs;
    private string[] lobbyTabNames;
    private string[] gameTabNames;

    private string lastSceneName;

    [MenuItem("Tools/Cheat/Cheat Editor")]
    public static void ShowWindow()
    {
        GetWindow<CheatEditor>("Cheat Editor");
    }

    private void OnEnable()
    {
        InitializeTabs();
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    private void InitializeTabs()
    {
        // 로비 씬 탭
        lobbyTabs = new ICheatTab[]
        {
            new CurrencyCheatTab()
        };
        lobbyTabNames = lobbyTabs.Select(t => t.TabName).ToArray();

        // 게임 씬 탭
        gameTabs = new ICheatTab[]
        {
            new StageCheatTab(),
            new InGameCurrencyCheatTab(),
            new InGameHeroCheatTab(),
            new AbilityCheatTab()
        };
        gameTabNames = gameTabs.Select(t => t.TabName).ToArray();
    }

    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            ResetAllTabs();
            selectedLobbyTabIndex = 0;
            selectedGameTabIndex = 0;
            lastSceneName = null;
        }
    }

    private void ResetAllTabs()
    {
        foreach (var tab in lobbyTabs)
        {
            tab.Reset();
        }

        foreach (var tab in gameTabs)
        {
            tab.Reset();
        }
    }

    private void OnGUI()
    {
        // 플레이 모드로 전환 중
        if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
        {
            GUILayout.Label("게임 실행 중...", EditorStyles.boldLabel);
            Repaint();
            return;
        }

        // 게임 실행 중이 아님
        if (!EditorApplication.isPlaying)
        {
            var centeredStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter
            };
            GUILayout.Label("게임 실행 필요", centeredStyle);

            if (GUILayout.Button("게임 실행", GUILayout.Height(30)))
            {
                EditorApplication.EnterPlaymode();
            }

            return;
        }

        // 현재 씬에 따라 탭 표시
        var activeSceneName = SceneManager.GetActiveScene().name;

        // 씬 변경 감지
        if (lastSceneName != activeSceneName)
        {
            OnSceneChanged(lastSceneName, activeSceneName);
            lastSceneName = activeSceneName;
        }

        if (activeSceneName == LOBBY_SCENE)
        {
            DrawTabs(lobbyTabs, lobbyTabNames, ref selectedLobbyTabIndex);
        }
        else if (activeSceneName == GAME_SCENE)
        {
            DrawTabs(gameTabs, gameTabNames, ref selectedGameTabIndex);
        }
    }

    private void OnSceneChanged(string previousScene, string newScene)
    {
        // 이전 씬 탭 OnExit 호출
        if (previousScene == LOBBY_SCENE && selectedLobbyTabIndex < lobbyTabs.Length)
        {
            lobbyTabs[selectedLobbyTabIndex].OnExit();
        }
        else if (previousScene == GAME_SCENE && selectedGameTabIndex < gameTabs.Length)
        {
            gameTabs[selectedGameTabIndex].OnExit();
        }

        // 새 씬 탭 OnEnter 호출
        if (newScene == LOBBY_SCENE && selectedLobbyTabIndex < lobbyTabs.Length)
        {
            lobbyTabs[selectedLobbyTabIndex].OnEnter();
        }
        else if (newScene == GAME_SCENE && selectedGameTabIndex < gameTabs.Length)
        {
            gameTabs[selectedGameTabIndex].OnEnter();
        }
    }

    private void DrawTabs(ICheatTab[] tabs, string[] tabNames, ref int selectedIndex)
    {
        int previousIndex = selectedIndex;
        selectedIndex = GUILayout.Toolbar(selectedIndex, tabNames);

        // 탭 변경 시 OnExit/OnEnter 호출
        if (previousIndex != selectedIndex)
        {
            tabs[previousIndex].OnExit();
            tabs[selectedIndex].OnEnter();
        }

        GUILayout.Space(10);

        if (selectedIndex < tabs.Length)
        {
            tabs[selectedIndex].DrawTab();
        }
    }
}
#endif