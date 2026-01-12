using System.Collections.Generic;
using Generated;
using UnityEngine;

public class HeroManageUI : BaseUI
{
    [SerializeField] private CloseButton closeButton;
    [SerializeField] private SelectedHeroListViewComponent selectedHeroListViewComponent; // 현재 선택된 영웅들
    [SerializeField] private HeroListViewComponent heroListViewComponent; // 보유 중인 영웅들
    [SerializeField] private ChangeHeroClassViewButtonComponent[]  changeHeroClassViewButtonComponents; // 영웅 전환 버튼들
    
    private HeroClassType currentHeroClassViewType = HeroClassType.Magician;
    private Dictionary<HeroClassType, Dictionary<HeroGradeType, HeroDataSO>> currentSelectedHeros;
    
    public Dictionary<HeroClassType, Dictionary<HeroGradeType, HeroDataSO>> CurrentSelectedHeros => currentSelectedHeros;

    private void Awake()
    {
        currentSelectedHeros = new(PlayerDataManager.Instance.SelectedHeroes);
        
        foreach (var changeHeroClassViewButtonComponent in changeHeroClassViewButtonComponents)
        {
            changeHeroClassViewButtonComponent.OnChangeHeroClassViewType += ChangeHeroView;
        }
    }

    protected override void Opened(params object[] args)
    {
        currentHeroClassViewType = HeroClassType.Magician;
        selectedHeroListViewComponent.ChangeHeroView(currentSelectedHeros[currentHeroClassViewType]);
        heroListViewComponent.ResetAlignmentType();
        heroListViewComponent.ChangeHeroView(currentSelectedHeros[currentHeroClassViewType]);
        
        foreach (var changeHeroClassViewButtonComponent in changeHeroClassViewButtonComponents)
        {
            changeHeroClassViewButtonComponent.ActiveIndicator(currentHeroClassViewType);
        }
    }

    protected override void Closed(params object[] args)
    {
    }

    private void ChangeHeroView(HeroClassType heroClassType)
    {
        currentHeroClassViewType = heroClassType;
        selectedHeroListViewComponent.ChangeHeroView(currentSelectedHeros[currentHeroClassViewType]);
        heroListViewComponent.ChangeHeroView(currentSelectedHeros[currentHeroClassViewType]);
        foreach (var changeHeroClassViewButtonComponent in changeHeroClassViewButtonComponents)
        {
            changeHeroClassViewButtonComponent.ActiveIndicator(currentHeroClassViewType);
        }
    }
}
