using System.Collections.Generic;
using Generated;
using TMPro;
using UnityEngine;

public class HeroListViewComponent : MonoBehaviour
{
    [Header("UI Components")] [SerializeField]
    private TextMeshProUGUI heroCollectionDamageBonusText;

    [SerializeField] private TMP_Dropdown alignmentDropdown;
    [Space] [SerializeField] private GameObject scrollViewContent;

    [Header("Hero View Prefab")] [SerializeField]
    private HeroViewComponent heroViewPrefab;
    
    private List<HeroViewComponent> currentHeroes = new List<HeroViewComponent>();

    private void Awake()
    {
        PreloadHeroViewComponentPool();
    }

    public void ChangeHeroView(Dictionary<HeroGradeType, HeroDataSO> ownedHeroDataDict)
    {
        for (var i = currentHeroes.Count - 1; i >= 0; i--)
        {
            ObjectPoolManager.Instance.Release(currentHeroes[i]);
            currentHeroes.Remove(currentHeroes[i]);
        }
        
        foreach (var hero in ownedHeroDataDict)
        {
            var heroView = ObjectPoolManager.Instance.Get(heroViewPrefab);
            heroView.transform.SetParent(scrollViewContent.transform, true);
            heroView.UpdateHeroViewUIComponent(hero.Value, false);
            currentHeroes.Add(heroView);
        }
    }

    private void PreloadHeroViewComponentPool()
    {
        ObjectPoolManager.Instance.Preload(heroViewPrefab, 9, 36);
    }
}