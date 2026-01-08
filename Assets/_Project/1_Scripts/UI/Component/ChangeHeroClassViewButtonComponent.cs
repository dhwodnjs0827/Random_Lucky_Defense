using System;
using UnityEngine;
using UnityEngine.UI;

public class ChangeHeroClassViewButtonComponent : MonoBehaviour
{
    [SerializeField] private HeroClassType heroClassType;
    [SerializeField] private Button button;
    [SerializeField] private GameObject indicator;
    
    public event Action<HeroClassType> OnChangeHeroClassViewType;

    private void Awake()
    {
        InitializeButton();
    }

    public void ActiveIndicator(HeroClassType selectedHeroClassType)
    {
        indicator.SetActive(selectedHeroClassType == heroClassType);
    }
    
    private void InitializeButton()
    {
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    private void OnClick()
    {
        OnChangeHeroClassViewType?.Invoke(heroClassType);
        ActiveIndicator(heroClassType);
    }
}
