using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HeroManageUI 하단에 있는 영웅 View 변경 버튼 UI용 클래스
/// </summary>
public class ChangeHeroClassViewButtonComponent : MonoBehaviour
{
    [SerializeField] private HeroClassType heroClassType;
    [SerializeField] private Button button;
    [SerializeField] private GameObject indicator;
    
    public event Action<HeroClassType> OnChangeHeroClassViewType;

    #region Unity
    
    private void Awake()
    {
        InitializeButton();
    }
    
    #endregion Unity
    
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
