using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// HeroManagerUI에서 사용 선택 영웅 리스트 표시용 UI 클래스
/// </summary>
public class SelectedHeroListViewComponent : MonoBehaviour
{
    [SerializeField] private CurrentSelectedHeroView normalHero;
    [SerializeField] private CurrentSelectedHeroView superiorHero;
    [SerializeField] private CurrentSelectedHeroView rareHero;
    [SerializeField] private CurrentSelectedHeroView ancientHero;
    [SerializeField] private CurrentSelectedHeroView relicHero;
    [SerializeField] private CurrentSelectedHeroView legendHero;
    [SerializeField] private CurrentSelectedHeroView epicHero;
    [SerializeField] private CurrentSelectedHeroView mythHero;
    [SerializeField] private CurrentSelectedHeroView godHero;

    private Dictionary<HeroGradeType, CurrentSelectedHeroView> heroViewComponents;

    private void Awake()
    {
        heroViewComponents = new()
        {
            { HeroGradeType.Normal, normalHero },
            { HeroGradeType.Superior, superiorHero },
            { HeroGradeType.Rare, rareHero },
            { HeroGradeType.Ancient, ancientHero },
            { HeroGradeType.Relic, relicHero },
            { HeroGradeType.Legend, legendHero },
            { HeroGradeType.Epic, epicHero },
            { HeroGradeType.Myth, mythHero },
            { HeroGradeType.God, godHero }
        };
    }

    public void ChangeHeroView(HeroClassType heroClassViewType)
    {
        var allHeroes = PlayerDataManager.Instance.AllHeroes;
        foreach (var heroData in allHeroes)
        {
            if (heroData.Class == heroClassViewType && heroData.IsSelected)
            {
                heroViewComponents[heroData.Grade].UpdateView(heroData);
            }
        }
    }

    [Serializable]
    private struct CurrentSelectedHeroView
    {
        public TextMeshProUGUI HeroGradeText;
        public HeroViewComponent Hero;

        public void UpdateView(HeroRuntimeData heroData)
        {
            HeroGradeText.text = $"{heroData.Grade}";
            Hero.UpdateHeroViewUIComponent(heroData, true);
        }
    }
}