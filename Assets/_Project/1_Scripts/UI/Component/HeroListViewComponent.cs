using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HeroManagerUI에서 보유 영웅 리스트 표시용 UI 클래스
/// </summary>
public class HeroListViewComponent : MonoBehaviour, IEventListener
{
    [Header("UI Components")] [SerializeField]
    private TextMeshProUGUI heroCollectionDamageBonusText;

    [SerializeField] private Button alignmentButton;
    [SerializeField] private TextMeshProUGUI alignmentTypeText;
    [Space] [SerializeField] private GameObject scrollViewContent;

    [Header("Hero View Prefab")] [SerializeField]
    private HeroViewComponent heroViewPrefab;

    private HeroClassType currentHeroViewType;
    private List<HeroViewComponent> currentHeroes = new();
    private HeroAlignmentType heroAlignmentType;
    
    private Action<ChangeSelectedHeroEventData> onChangeSelectedHero;
    private Action<LevelUpHeroEventData> onLevelUpHero;

    #region Unity

    private void Awake()
    {
        PreloadHeroViewComponentPool();
        
        alignmentButton.onClick.AddListener(ChangeAlignmentType);
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
    /// 정렬 방법 기본값으로 초기화
    /// </summary>
    public void ResetAlignmentType()
    {
        heroAlignmentType = HeroAlignmentType.A;
    }

    /// <summary>
    /// 보여지는 영웅 리스트 뷰 변경
    /// </summary>
    /// <param name="heroClassViewType">보여질 영웅 클래스 타입</param>
    public void ChangeHeroView(HeroClassType heroClassViewType)
    {
        currentHeroViewType = heroClassViewType;
        heroCollectionDamageBonusText.text = $"영웅 보유 데미지 증가 : {PlayerDataManager.Instance.CalculateHeroAcquiredBonusDamage(heroClassViewType) * 100:N1}%";
        
        for (var i = currentHeroes.Count - 1; i >= 0; i--)
        {
            ObjectPoolManager.Instance.Release(currentHeroes[i]);
            currentHeroes.Remove(currentHeroes[i]);
        }

        var acquiredHeroes = PlayerDataManager.Instance.HeroDB.AllHeroes;
        foreach (var hero in acquiredHeroes)
        {
            if (hero.Class != heroClassViewType || !hero.IsAcquiredHero)
            {
                continue;
            }
            var heroView = ObjectPoolManager.Instance.Get(heroViewPrefab);
            heroView.transform.SetParent(scrollViewContent.transform, true);
            heroView.UpdateHeroViewUIComponent(hero, hero.IsSelected);
            currentHeroes.Add(heroView);
        }
        
        AlignmentHeroList(heroAlignmentType);
    }

    /// <summary>
    /// 영웅 슬롯 UI 컴포넌트 프리팹 풀 초기화
    /// </summary>
    private void PreloadHeroViewComponentPool()
    {
        ObjectPoolManager.Instance.Preload(heroViewPrefab, 9, 36);
    }

    /// <summary>
    /// 영웅 정렬 방법 변경
    /// </summary>
    private void ChangeAlignmentType()
    {
        heroAlignmentType = heroAlignmentType.Next();
        AlignmentHeroList(heroAlignmentType);
    }

    /// <summary>
    /// 영웅 슬롯 정렬
    /// </summary>
    /// <param name="alignmentType">정렬 방법 타입</param>
    private void AlignmentHeroList(HeroAlignmentType alignmentType)
    {
        switch (alignmentType)
        {
            case HeroAlignmentType.A:
                currentHeroes = currentHeroes.OrderBy(data => data.CurrentHeroData.Grade).ThenBy(data => data.CurrentHeroData.Rank).ToList();
                alignmentTypeText.text = "정렬방식 A";
                break;
            case HeroAlignmentType.B:
                currentHeroes = currentHeroes.OrderByDescending(data => data.CurrentHeroData.Grade).ThenByDescending(data => data.CurrentHeroData.Rank).ToList();
                alignmentTypeText.text = "정렬방식 B";
                break;
        }

        foreach (var hero in currentHeroes)
        {
            hero.transform.SetAsLastSibling();
        }
    }

    private void ChangeSelectedHero(ChangeSelectedHeroEventData eventData)
    {
        if (eventData.UnequipHeroData != null)
        {
            var unequipHeroIndex = currentHeroes.FindIndex(heroView => heroView.CurrentHeroData == eventData.UnequipHeroData);
            currentHeroes[unequipHeroIndex].UpdateHeroViewUIComponent(eventData.UnequipHeroData, eventData.UnequipHeroData.IsSelected);
        }

        if (eventData.EquipHeroData != null)
        {
            var equipHeroIndex = currentHeroes.FindIndex(heroView => heroView.CurrentHeroData == eventData.EquipHeroData);
            currentHeroes[equipHeroIndex].UpdateHeroViewUIComponent(eventData.EquipHeroData, eventData.EquipHeroData.IsSelected);
        }
    }

    private void LevelUpHero(LevelUpHeroEventData eventData)
    {
        var newHeroIndex = currentHeroes.FindIndex(heroView => heroView.CurrentHeroData == eventData.LevelUpHeroData);
        currentHeroes[newHeroIndex].UpdateHeroViewUIComponent(eventData.LevelUpHeroData, eventData.LevelUpHeroData.IsSelected);
        
        heroCollectionDamageBonusText.text = $"영웅 보유 데미지 증가 : {PlayerDataManager.Instance.CalculateHeroAcquiredBonusDamage(currentHeroViewType) * 100:N1}%";
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
    
    private enum HeroAlignmentType
    {
        A,
        B,
    }
}