using System.Collections.Generic;
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

    private HeroClassType currentHeroClassView = HeroClassType.Magician;
    private List<HeroViewComponent> currentHeroes = new List<HeroViewComponent>();

    private void Awake()
    {
        ReloadHeroView();
    }

    private void OnEnable()
    {
        UpdateHeroListView();
    }

    private void OnDisable()
    {
        for (var i = currentHeroes.Count - 1; i >= 0; i--)
        {
            ObjectPoolManager.Instance.Release(currentHeroes[i]);
            currentHeroes.Remove(currentHeroes[i]);
        }
    }

    private void UpdateHeroListView()
    {
        var selectedHeroes = PlayerDataManager.Instance.SelectedHeroes;
        foreach (var hero in selectedHeroes[currentHeroClassView])
        {
            var heroView = ObjectPoolManager.Instance.Get(heroViewPrefab);
            heroView.transform.SetParent(scrollViewContent.transform, false);
            heroView.UpdateHeroViewUIComponent(hero.Value, false);
            currentHeroes.Add(heroView);
        }
    }

    private void ReloadHeroView()
    {
        ObjectPoolManager.Instance.Preload(heroViewPrefab, 9, 36);
    }
}