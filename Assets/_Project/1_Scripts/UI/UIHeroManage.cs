using UnityEngine;

/// <summary>
/// 로비에서 영웅 관리 UI 클래스
/// </summary>
public class UIHeroManage : UIBase
{
    [Header("UI Components")]
    [SerializeField] private CloseButton closeButton;
    [SerializeField] private SelectedHeroListViewComponent selectedHeroListViewComponent; // 현재 선택된 영웅들
    [SerializeField] private HeroListViewComponent heroListViewComponent; // 보유 중인 영웅들
    [SerializeField] private ChangeHeroClassViewButtonComponent[]  changeHeroClassViewButtonComponents; // 영웅 전환 버튼들
    
    private HeroClassType currentHeroClassViewType = HeroClassType.Magician;

    #region Override

    protected override void Opened(params object[] args)
    {
        currentHeroClassViewType = HeroClassType.Magician;
        selectedHeroListViewComponent.ChangeHeroView(currentHeroClassViewType);
        heroListViewComponent.ResetAlignmentType();
        heroListViewComponent.ChangeHeroView(currentHeroClassViewType);
        
        foreach (var changeHeroClassViewButtonComponent in changeHeroClassViewButtonComponents)
        {
            // 현재 보여지는 영웅 버튼 인디케이터 활성화
            changeHeroClassViewButtonComponent.ActiveIndicator(currentHeroClassViewType);
            
            // 하단 영웅 뷰 변경 버튼에 영웅 뷰 변경 이벤트 할당
            changeHeroClassViewButtonComponent.OnChangeHeroClassViewType += ChangeHeroView;
        }
    }
    
    protected override void Closed(params object[] args)
    {
        foreach (var changeHeroClassViewButtonComponent in changeHeroClassViewButtonComponents)
        {
            // 하단 영웅 뷰 변경 버튼에 영웅 뷰 변경 이벤트 해제
            changeHeroClassViewButtonComponent.OnChangeHeroClassViewType -= ChangeHeroView;
        }
    }

    #endregion Override
    
    /// <summary>
    /// 보여지는 영웅 리스트 뷰 전환
    /// </summary>
    /// <param name="heroClassType">보여질 영웅 클래스 타입</param>
    private void ChangeHeroView(HeroClassType heroClassType)
    {
        currentHeroClassViewType = heroClassType;
        selectedHeroListViewComponent.ChangeHeroView(currentHeroClassViewType);
        heroListViewComponent.ChangeHeroView(currentHeroClassViewType);
        foreach (var changeHeroClassViewButtonComponent in changeHeroClassViewButtonComponents)
        {
            changeHeroClassViewButtonComponent.ActiveIndicator(currentHeroClassViewType);
        }
    }
}
