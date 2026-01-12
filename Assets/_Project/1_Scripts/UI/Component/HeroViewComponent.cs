using System;
using System.Linq;
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
    
    private HeroGameData currentHeroData;
    
    public HeroGameData CurrentHeroData => currentHeroData;

    /// <summary>
    /// 영웅 정보에 맞게 UI 요소들 초기화
    /// </summary>
    public void UpdateHeroViewUIComponent(HeroGameData heroData, bool isSelected)
    {
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

        var heroGameData = PlayerDataManager.Instance.AcquiredHeroes[heroData.ID];
        heroLevelText.text = $"레벨 {heroGameData.Level}";

        //TODO: 임시 요구치 10 할당
        heroRequiredLevelSlider.value = heroGameData.AcquiredStack / 10f;
        heroRequiredLevelText.text = $"{heroGameData.AcquiredStack}/10";
    }
}
