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

    private Dictionary<HeroGradeType, HeroViewComponent> heroViewComponents; // 영웅 등급과 1대1 매칭을 위한 Dictionary

    private Action<ChangeSelectedHeroEventData> onChangeSelectedHero;
    private Action<LevelUpHeroEventData> onLevelUpHero;

    #region Unity

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

    #endregion Unity
    
    /// <summary>
    /// 유저가 선택한 영웅 데이터로 UI 변경
    /// </summary>
    /// <param name="heroClassViewType">보여질 영웅 클래스 타입</param>
    public void ChangeHeroView(HeroClassType heroClassViewType)
    {
        // 유저가 선택한 영웅 클래스 타입에 맞는 영웅들의 데이터 가져오기
        var equippedHeroes = PlayerDataManager.Instance.HeroDB.GetSelectedHeroesByClass(heroClassViewType);

        // 등급에 맞게 ViewComponent에 데이터 넣기
        foreach (var (gradeType, viewComponent) in heroViewComponents)
        {
            if (equippedHeroes.TryGetValue(gradeType, out var heroData)) // 영웅 데이터가 있으면 ViewComponent 활성화 및 데이터 세팅
            {
                viewComponent.gameObject.SetActive(true);
                viewComponent.UpdateHeroViewUIComponent(heroData, true);
            }
            else // 영웅 데이터가 없으면 ViewComponent 비활성화
            {
                viewComponent.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 사용할 영웅 변경 이벤트
    /// </summary>
    /// <param name="eventData">변경할 영웅 데이터</param>
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

    /// <summary>
    /// 현재 선택된 영웅 레벨업 이벤트
    /// </summary>
    /// <param name="eventData">레벨 업 할 영웅 데이터</param>
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