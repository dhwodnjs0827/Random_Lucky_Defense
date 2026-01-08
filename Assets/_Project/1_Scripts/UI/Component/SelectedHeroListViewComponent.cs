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

    public void ChangeHeroView(Dictionary<HeroGradeType, HeroDataSO> selectedHeroDataDict)
    {
        foreach (var heroData in selectedHeroDataDict)
        {
            heroViewComponents[heroData.Key].UpdateView(heroData.Value);
        }
    }
}

[Serializable]
public struct CurrentSelectedHeroView
{
    public TextMeshProUGUI HeroGradeText;
    public HeroViewComponent Hero;

    public void UpdateView(HeroDataSO heroData)
    {
        HeroGradeText.text = $"{heroData.GradeType}";
        Hero.UpdateHeroViewUIComponent(heroData, true);
    }
}