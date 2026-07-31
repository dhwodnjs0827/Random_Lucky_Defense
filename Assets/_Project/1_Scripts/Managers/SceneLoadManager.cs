using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Scene 전환 및 초기화 관리 클래스
/// </summary>
public class SceneLoadManager : MonoSingleton<SceneLoadManager>
{
    private Dictionary<SceneType, BaseScene> scenes;

    private BaseScene currentScene;
    private bool isLoading = false;

    protected override bool isInitialized { get; set; }

    /// <summary>
    /// SceneLoadManager 초기화
    /// </summary>
    public override async UniTask InitializeAsync()
    {
        if (isInitialized)
        {
            // SceneLoadManager의 초기화는 한 번만 필요
            return;
        }
        
        // 초기화 플래그 설정
        isInitialized = true;
        
        InitializeSceneList();
        await InitializeCurrentActiveScene();
    }

    /// <summary>
    /// 비동기 Scene 로딩
    /// </summary>
    /// <param name="sceneType">로딩할 Scene</param>
    public async UniTask LoadSceneAsync(SceneType sceneType)
    {
        if (isLoading)
        {
            CDebug.LogWarning("[SceneLoadManager] 이미 씬 로딩 중입니다.");
            return;
        }

        if (!scenes.TryGetValue(sceneType, out var scene))
        {
            CDebug.LogError($"[SceneLoadManager] {sceneType} 씬이 등록되지 않았습니다.");
            return;
        }

        // 중복 로딩 방지 플래그
        isLoading = true;

        try
        {
            // 1. 현재 씬 정리
            if (currentScene != null)
            {
                await currentScene.CleanupAsync();
            }

            // 2. 로딩 UI 표시
            //TODO: UILoading 활성화, 필요 시, Fade 연출 추가
            var loadingUI = await UIManager.Instance.OpenAsync<UILoading>();
            // await FadeOut();

            // 3. 씬 로드
            var operation = SceneManager.LoadSceneAsync(sceneType.ToString());
            if (operation == null)
            {
                CDebug.LogError($"[SceneLoadManager] {sceneType} 씬 로드 실패");
                return;
            }

            operation.allowSceneActivation = false;
            float displayProgress = 0f;
            float minLoadTime = 1f;  // 최소 로딩 시간
            float elapsedTime = 0f;

            // 4. 로딩 연출 (실제 진행률 + 시간 기반)
            while (displayProgress < 1f)
            {
                elapsedTime += Time.unscaledDeltaTime;

                // 실제 진행률 (0.9가 최대)
                var realProgress = operation.progress / 0.9f;

                // 시간 기반 진행률
                var timeProgress = elapsedTime / minLoadTime;

                // 둘 중 작은 값 사용 (부드럽게 증가)
                var targetProgress = Mathf.Min(realProgress, timeProgress);
                displayProgress = Mathf.MoveTowards(displayProgress, targetProgress, Time.unscaledDeltaTime * 2f);

                // UI 업데이트
                loadingUI.UpdateProgress(displayProgress);

                await UniTask.Yield();
            }

            // 4. 씬 활성화
            operation.allowSceneActivation = true;
            await operation.ToUniTask();
            
            // 5. 새 씬의 초기화
            currentScene = scene;
            await currentScene.InitializeAsync();

            // 6. UILoading 닫기
            // await FadeIn();
            UIManager.Instance.Close<UILoading>(loadingUI);
        }
        catch (Exception e)
        {
            CDebug.LogError($"[SceneLoadManager] 씬 로드 중 에러 발생: {e}");
        }
        finally
        {
            // 7. 씬 준비 완료
            isLoading = false;
        }
    }

    /// <summary>
    /// 각 Scene 클래스 초기화
    /// </summary>
    private void InitializeSceneList()
    {
        scenes = new Dictionary<SceneType, BaseScene>()
        {
            { SceneType.TitleScene, new TitleScene() },
            { SceneType.LobbyScene, new LobbyScene() },
            { SceneType.GameScene, new GameScene() },
            { SceneType.SampleScene, new SampleScene() }
        };
    }

    /// <summary>
    /// 게임 시작 시, 첫 Scene 초기화
    /// </summary>
    private async UniTask InitializeCurrentActiveScene()
    {
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (scenes.TryGetValue((SceneType)sceneIndex, out var scene))
        {
            currentScene = scene;
            await currentScene.InitializeAsync();
        }
    }
}