using System;
using Generated;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroViewComponent : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Button heroInfoButton;
    [Space]
    [SerializeField] private Image heroRankImage;
    [SerializeField] private TextMeshProUGUI heroGradeText;
    [SerializeField] private Image heroImage;
    [SerializeField] private TextMeshProUGUI heroRankText;
    [Space]
    [SerializeField] private Image isSelectedImage;
    [Space]
    [SerializeField] private TextMeshProUGUI heroLevelText;
    [Space]
    [SerializeField] private Slider heroRequiredLevelSlider;
    [SerializeField] private TextMeshProUGUI heroRequiredLevelText;
    
    private HeroDataSO currentHeroData;
    
    public HeroDataSO CurrentHeroData => currentHeroData;

    /// <summary>
    /// 영웅 정보에 맞게 UI 요소들 초기화
    /// </summary>
    public void UpdateHeroViewUIComponent(HeroDataSO heroData, bool isSelected)
    {
        currentHeroData = heroData;
        
        //TODO: 임시 색 변경(추후 이미지 변경으로)
        //heroRankImage.sprite =
        switch (heroData.RankType)
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
        heroGradeText.text = $"{heroData.GradeType}";
        heroImage.sprite = ResourceManager.Instance.Load<Sprite>($"Sprites/Hero/{heroData.ClassType}_{heroData.GradeType}");
        heroRankText.text = $"{heroData.RankType}";
        
        isSelectedImage.gameObject.SetActive(isSelected);
        
        heroLevelText.text = $"레벨 {PlayerDataManager.Instance.CurrentHeroLevels[heroData.ID]}";

        //TODO: 임시 요구치 10 할당
        var currentStack = PlayerDataManager.Instance.CurrentHeroStacks[heroData.ID];
        heroRequiredLevelSlider.value = currentStack / 10f;
        heroRequiredLevelText.text = $"{currentStack}/10";
    }
}
