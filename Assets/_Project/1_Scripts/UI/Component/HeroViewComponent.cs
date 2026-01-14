using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HeroManagerUI에서 영웅 표시용 UI 클래스
/// </summary>
public class HeroViewComponent : MonoBehaviour
{
    [Header("UI Components")] [SerializeField]
    private Button heroInfoButton;

    [Space] [SerializeField] private Image heroRankImage;
    [SerializeField] private TextMeshProUGUI heroGradeText;
    [SerializeField] private Image heroImage;
    [SerializeField] private TextMeshProUGUI heroRankText;
    [Space] [SerializeField] private Image isSelectedImage;
    [Space] [SerializeField] private TextMeshProUGUI heroLevelText;
    [Space] [SerializeField] private Slider heroRequiredLevelSlider;
    [SerializeField] private TextMeshProUGUI heroRequiredLevelText;

    private HeroRuntimeData currentHeroData;

    public HeroRuntimeData CurrentHeroData => currentHeroData;

    private void Awake()
    {
        if (heroInfoButton != null)
        {
            heroInfoButton.onClick.AddListener(OnClickHeroInfoButton);
        }
    }

    /// <summary>
    /// 영웅 정보에 맞게 UI 요소들 초기화
    /// </summary>
    public void UpdateHeroViewUIComponent(HeroRuntimeData heroData, bool isSelected)
    {
        currentHeroData = heroData;

        //TODO: 임시 색 변경(추후 이미지 변경으로)
        //heroRankImage.sprite =
        switch (heroData.Rank)
        {
            case HeroRankType.B:
                heroRankImage.color = Color.gray;
                break;
            case HeroRankType.A:
                heroRankImage.color = Color.green;
                break;
            case HeroRankType.S:
                heroRankImage.color = Color.orange;
                break;
        }

        heroGradeText.text = $"{heroData.Grade}";
        heroImage.sprite = ResourceManager.Instance.Load<Sprite>($"Sprites/Hero/{heroData.Class}_{heroData.Grade}");
        heroRankText.text = $"{heroData.Rank}";

        isSelectedImage.gameObject.SetActive(isSelected);

        heroLevelText.text = $"레벨 {heroData.Level}";
        
        heroRequiredLevelText.text = $"{heroData.AcquiredStack}/ {heroData.LevelUpRequiredStack}";
        heroRequiredLevelSlider.value = (float)heroData.AcquiredStack / heroData.LevelUpRequiredStack;
    }

    private void OnClickHeroInfoButton()
    {
        UIManager.Instance.Open<HeroInfoUI>(currentHeroData);
    }
}