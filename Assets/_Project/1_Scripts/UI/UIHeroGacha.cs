using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 영웅 가챠용 Popup UI 클래스
/// </summary>
public class UIHeroGacha : UIBase
{
    [SerializeField] private CloseButton closeButton;
    [SerializeField] private Button showResultButton;
    [SerializeField] private Transform resultContainerTransform;

    [SerializeField] private GachaResultContainer gachaResultPrefab;

    private int gachaCount;
    private List<HeroRuntimeData> gachaResults = new();
    private List<GachaResultContainer> currentGachaResultContainers = new();
    private CancellationTokenSource gachaAnimationCancellationTokenSource = new();

    private void Awake()
    {
        PreloadGachaResult();
        if (showResultButton != null)
        {
            showResultButton.onClick.AddListener(OnClickResultButton);
        }
    }

    protected override void Opened(params object[] args)
    {
        gachaCount = (int)args[0];
        gachaResults = HeroGachaUtil.Gacha(gachaCount);
        foreach (var gachaResult in gachaResults)
        {
            PlayerDataManager.Instance.AcquireHero(gachaResult, false);
        }

        PlayerDataManager.Instance.SaveData(SaveDataType.Hero);

        currentGachaResultContainers.Clear();
        showResultButton.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(false);
    }

    protected override void Closed(params object[] args)
    {
        gachaAnimationCancellationTokenSource.Cancel();
        gachaAnimationCancellationTokenSource.Dispose();
        gachaAnimationCancellationTokenSource = new CancellationTokenSource();

        foreach (var container in currentGachaResultContainers)
        {
            container.KillTween();
        }

        currentGachaResultContainers.Clear();
        ObjectPoolManager.Instance.Clear(gachaResultPrefab.gameObject);

        gachaResults.Clear();
    }

    private void OnClickResultButton()
    {
        showResultButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(true);

        foreach (var result in gachaResults)
        {
            var resultContainer = ObjectPoolManager.Instance.Get(gachaResultPrefab);
            resultContainer.transform.SetParent(resultContainerTransform.transform, true);
            resultContainer.SetGachaResultInfo(result);
            currentGachaResultContainers.Add(resultContainer);
        }

        GachaAnimation().Forget();
    }

    private void PreloadGachaResult()
    {
        ObjectPoolManager.Instance.Preload(gachaResultPrefab, 10, 600);
    }

    private async UniTaskVoid GachaAnimation()
    {
        gachaAnimationCancellationTokenSource.Cancel();
        gachaAnimationCancellationTokenSource.Dispose();
        gachaAnimationCancellationTokenSource = new CancellationTokenSource();

        try
        {
            foreach (var currentGachaResultContainer in currentGachaResultContainers)
            {
                await currentGachaResultContainer.ResultAnimation();
            }
        }
        catch (OperationCanceledException)
        {
            // 정상 취소
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}