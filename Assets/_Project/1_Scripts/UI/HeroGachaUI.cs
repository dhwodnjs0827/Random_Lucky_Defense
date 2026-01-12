using Generated;
using UnityEngine;
using UnityEngine.UI;

public class HeroGachaUI : BaseUI
{
    [SerializeField] private CloseButton closeButton;
    [SerializeField] private Button resultButton;
    [SerializeField] private Image resultImage;

    private void Awake()
    {
        if (resultButton != null)
        {
            resultButton.onClick.AddListener(OnClickResultButton);
        }
    }

    protected override void Opened(params object[] args)
    {
        resultButton.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(false);
        resultImage.gameObject.SetActive(false);
        
    }

    protected override void Closed(params object[] args)
    {
    }

    private HeroGachaResult GetRandomHero()
    {
        var gachaResult = HeroGachaUtil.RollOnce();
        CDebug.Log($"[HeroGachaUI] 뽑기 결과: {gachaResult.Grade}, {gachaResult.Rank}");
        return gachaResult;
    }

    private void OnClickResultButton()
    {
        resultButton.gameObject.SetActive(false);
        var result = GetRandomHero();
        closeButton.gameObject.SetActive(true);
        resultImage.gameObject.SetActive(true);
        resultImage.sprite = ResourceManager.Instance.Load<Sprite>($"Sprites/Hero/{result.Class}_{result.Grade}");
    }
}