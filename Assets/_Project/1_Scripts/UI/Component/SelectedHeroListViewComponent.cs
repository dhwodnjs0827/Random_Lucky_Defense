using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HeroManagerUI에서 사용 선택 영웅 리스트 표시용 UI 클래스
/// </summary>
public class SelectedHeroListViewComponent : MonoBehaviour, IEventListener
{
    [SerializeField] private HeroViewComponent normalHero;
    [SerializeField] private HeroViewComponent superiorHero;
    [SerializeField] private HeroViewComponent rareHero;
    [SerializeField] private HeroViewComponent ancientHero;
    [SerializeField] private HeroViewComponent relicHero;
    [SerializeField] private HeroViewComponent legendHero;
    [SerializeField] private HeroViewComponent epicHero;
    [SerializeField] private HeroViewComponent mythHero;
    [SerializeField] private HeroViewComponent godHero;

    private Dictionary<HeroGradeType, HeroViewComponent> heroViewComponents;
    private HeroClassType currentHeroViewType;

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
        currentHeroViewType = heroClassViewType;
        var equippedHeroes = PlayerDataManager.Instance.HeroDB.GetSelectedHeroesByClass(heroClassViewType);

        foreach (var (grade, viewComponent) in heroViewComponents)
        {
            if (equippedHeroes.TryGetValue(grade, out var heroData))
            {
                viewComponent.gameObject.SetActive(true);
                viewComponent.UpdateHeroViewUIComponent(heroData, true);
            }
            else
            {
                viewComponent.gameObject.SetActive(false);
            }
        }
    }

    private void ChangeSelectedHero(ChangeSelectedHeroEventData eventData)
    {
        if (eventData.EquipHeroData != null)
        {
            heroViewComponents[eventData.EquipHeroData.Grade].gameObject.SetActive(true);
            heroViewComponents[eventData.EquipHeroData.Grade].UpdateHeroViewUIComponent(eventData.EquipHeroData, true);
        }
        else
        {
            heroViewComponents[eventData.UnequipHeroData.Grade].gameObject.SetActive(false);
        }
    }

    private void LevelUpHero(LevelUpHeroEventData eventData)
    {
        heroViewComponents[eventData.LevelUpHeroData.Grade].UpdateHeroViewUIComponent(eventData.LevelUpHeroData, true);
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
}