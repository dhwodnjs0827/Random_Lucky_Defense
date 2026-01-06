using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour, IPoolable
{
    [SerializeField] TextMeshPro damageText;

    [Header("데미지 텍스트 설정")]
    [SerializeField] private int normalFontSize = 4;
    [SerializeField] private int criticalFontSize = 6;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color criticalColor = Color.red;

    [Header("Tween 설정")] [SerializeField] private float firstScale = 1.5f;
    [SerializeField] private float firstScaleTime = 0.2f;
    [SerializeField] private float secondScale = 1f;
    [SerializeField] private float secondScaleTime = 0.3f;
    [SerializeField] private float displayTime = 0.5f;
    [SerializeField] private float fadeTime = 0.5f;

    private Sequence damageTextSequence;

    private void Awake()
    {
        damageTextSequence = DOTween.Sequence();

        damageTextSequence
            .Append(transform.DOScale(firstScale, firstScaleTime))
            .Append(transform.DOScale(secondScale, secondScaleTime))
            .AppendInterval(displayTime)
            .Append(damageText.DOFade(0f, fadeTime))
            .OnComplete(() => ObjectPoolManager.Instance.Release(gameObject))
            .SetAutoKill(false)
            .Pause();
    }

    public void PlayDamageTextSequence(float damage, bool isCritical)
    {
        damageText.alpha = 1f;
        damageText.color = isCritical ? criticalColor : normalColor;
        damageText.fontSize = isCritical ? criticalFontSize : normalFontSize;
        damageText.text = damage.ToString("N0");
        
        damageTextSequence.Restart();
    }

    public void OnGet()
    {
        transform.localScale = Vector3.zero;
    }

    public void OnRelease()
    {
        damageTextSequence.Pause();
    }
}