using System;
using System.Collections.Generic;
using Generated;
using TMPro;
using UnityEngine;

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
        var selectedHeroes = PlayerDataManager.Instance.AcquiredHeroes;
        foreach (var heroData in selectedHeroes)
        {
            if (heroData.Class == heroClassViewType && heroData.isSelected)
            {
                heroViewComponents[heroData.Grade].UpdateView(heroData);
            }
        }
    }
}

[Serializable]
public struct CurrentSelectedHeroView
{
    public TextMeshProUGUI HeroGradeText;
    public HeroViewComponent Hero;

    public void UpdateView(HeroGameData heroData)
    {
        HeroGradeText.text = $"{heroData.Grade}";
        Hero.UpdateHeroViewUIComponent(heroData, true);
    }
}