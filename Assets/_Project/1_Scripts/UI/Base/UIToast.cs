using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

/// <summary>
/// 토스트 UI 전용 클래스
/// </summary>
public class UIToast : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI toastText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float fadeInDuration = 0.2f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private float slideDistance = 50f;
    [SerializeField] private Ease slideEase = Ease.OutBack;

    private CancellationTokenSource cts;
    private float originalY;

    private void Awake()
    {
        originalY = rectTransform.anchoredPosition.y;
    }

    private void OnDisable()
    {
        Cancel();
    }

    public void Show(string key, float duration)
    {
        Cancel();
        cts = new CancellationTokenSource();
        ShowAsync(key, duration, cts.Token).Forget();
    }

    private async UniTaskVoid ShowAsync(string key, float duration, CancellationToken token)
    {
        try
        {
            var message = await LocalizationSettings.StringDatabase.GetLocalizedStringAsync(GameConstants.LOCALIZATION_TABLE_NAME, key);
            
            toastText.text = message;
            gameObject.SetActive(true);

            // 시작 상태 설정 (아래로, 투명)
            canvasGroup.alpha = 0f;
            rectTransform.anchoredPosition = new Vector2(
                rectTransform.anchoredPosition.x,
                originalY - slideDistance
            );

            // Fade In + Slide Up 동시 실행
            await UniTask.WhenAll(
                canvasGroup.DOFade(1f, fadeInDuration).SetEase(Ease.OutQuad).ToUniTask(cancellationToken: token),
                rectTransform.DOAnchorPosY(originalY, fadeInDuration).SetEase(slideEase)
                    .ToUniTask(cancellationToken: token)
            );

            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: token);

            // Fade Out
            await canvasGroup
                .DOFade(0f, fadeOutDuration)
                .SetEase(Ease.InQuad)
                .ToUniTask(cancellationToken: token);

            gameObject.SetActive(false);
        }
        catch (OperationCanceledException)
        {
            // 취소됨 - 정상 처리
        }
        catch (Exception e)
        {
            CDebug.LogException(e);
        }
    }

    private void Cancel()
    {
        cts?.Cancel();
        cts?.Dispose();
        cts = null;
        rectTransform.DOKill();
        canvasGroup.DOKill();
    }
}