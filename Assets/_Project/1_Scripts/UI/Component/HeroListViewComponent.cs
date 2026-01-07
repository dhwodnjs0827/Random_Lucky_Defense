using TMPro;
using UnityEngine;

public class HeroListViewComponent : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI heroCollectionDamageBonusText;
    [SerializeField] private TMP_Dropdown alignmentDropdown;
    [Space]
    [SerializeField] private GameObject scrollViewContent;
    
    [Header("Hero View Prefab")]
    [SerializeField] private HeroViewComponent heroViewPrefab;

    private void Awake()
    {
        ReloadHeroView();
    }

    private void ReloadHeroView()
    {
        ObjectPoolManager.Instance.Preload(heroViewPrefab, 9, 36);
    }
}
