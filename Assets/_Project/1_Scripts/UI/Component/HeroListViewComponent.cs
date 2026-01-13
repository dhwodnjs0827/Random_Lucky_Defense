using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HeroManagerUI에서 보유 영웅 리스트 표시용 UI 클래스
/// </summary>
public class HeroListViewComponent : MonoBehaviour
{
    [Header("UI Components")] [SerializeField]
    private TextMeshProUGUI heroCollectionDamageBonusText;

    [SerializeField] private Button alignmentButton;
    [SerializeField] private TextMeshProUGUI alignmentTypeText;
    [Space] [SerializeField] private GameObject scrollViewContent;

    [Header("Hero View Prefab")] [SerializeField]
    private HeroViewComponent heroViewPrefab;
    
    private List<HeroViewComponent> currentHeroes = new();
    private HeroAlignmentType heroAlignmentType;
    

    private void Awake()
    {
        PreloadHeroViewComponentPool();
        
        alignmentButton.onClick.AddListener(ChangeAlignmentType);
    }

    public void ResetAlignmentType()
    {
        heroAlignmentType = HeroAlignmentType.A;
    }

    public void ChangeHeroView(HeroClassType heroClassViewType)
    {
        for (var i = currentHeroes.Count - 1; i >= 0; i--)
        {
            ObjectPoolManager.Instance.Release(currentHeroes[i]);
            currentHeroes.Remove(currentHeroes[i]);
        }

        var acquiredHeroes = PlayerDataManager.Instance.AllHeroes;
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

    private void PreloadHeroViewComponentPool()
    {
        ObjectPoolManager.Instance.Preload(heroViewPrefab, 9, 36);
    }

    private void ChangeAlignmentType()
    {
        heroAlignmentType = heroAlignmentType.Next();
        AlignmentHeroList(heroAlignmentType);
    }

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

    private enum HeroAlignmentType
    {
        A,
        B,
    }
}