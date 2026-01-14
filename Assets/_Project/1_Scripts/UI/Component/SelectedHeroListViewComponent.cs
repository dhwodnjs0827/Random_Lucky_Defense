using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// HeroManagerUI에서 사용 선택 영웅 리스트 표시용 UI 클래스
/// </summary>
public class SelectedHeroListViewComponent : MonoBehaviour, IEventListener
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

    private Action<ChangeSelectedHeroEventData> onChangeSelectedHero;
    private Action<LevelUpHeroEventData> onLevelUpHero;

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

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
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
    
    private void ChangeSelectedHero(ChangeSelectedHeroEventData eventData)
    {
        heroViewComponents[eventData.OldHeroData.Grade].UpdateView(eventData.NewHeroData);
    }

    private void LevelUpHero(LevelUpHeroEventData eventData)
    {
        heroViewComponents[eventData.LevelUpHeroData.Grade].UpdateView(eventData.LevelUpHeroData);
    }

    public void SubscribeEvents()
    {
        onChangeSelectedHero += ChangeSelectedHero;
        EventManager.Subscribe(GameEventType.ChangeSelectedHero, onChangeSelectedHero);
        onLevelUpHero += LevelUpHero;
        EventManager.Subscribe(GameEventType.LevelUpHero, onLevelUpHero);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.ChangeSelectedHero, onChangeSelectedHero);
        onChangeSelectedHero -= ChangeSelectedHero;
        EventManager.Unsubscribe(GameEventType.LevelUpHero, onLevelUpHero);
        onLevelUpHero -= LevelUpHero;
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