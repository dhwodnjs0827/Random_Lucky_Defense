using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GachaResultContainer : MonoBehaviour, IPoolable
{
    [SerializeField] private Image resultImage;
    private HeroRuntimeData heroData;
    private Tween currentTween;

    public void SetGachaResultInfo(HeroRuntimeData resultHeroData)
    {
        heroData = resultHeroData;
        resultImage.sprite = ResourceManager.Instance.Load<Sprite>($"Sprites/Hero/{resultHeroData.Class}_{resultHeroData.Grade}");
    }

    public async UniTask ResultAnimation()
    {
        currentTween = transform.DOScale(new Vector3(1, 1, 1), 0.1f).SetLink(gameObject);
        await currentTween;
    }

    public void KillTween()
    {
        currentTween?.Kill();
        currentTween = null;
    }

    public void OnGet()
    {
        transform.localScale = Vector3.zero;
    }

    public void OnRelease()
    {
        KillTween();
    }
}
