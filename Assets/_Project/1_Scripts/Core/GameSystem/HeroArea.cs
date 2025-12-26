using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 개별 삼각형 영역
/// </summary>
public class HeroArea : MonoBehaviour
{
    [SerializeField] private HeroAreaType areaType;
    private SpriteRenderer highlightRenderer;
    [SerializeField] private Transform centerPoint;

    private List<BaseHero> heroes = new();
    private bool isHighlighted = false;

    public HeroAreaType AreaType => areaType;
    
    public IReadOnlyList<BaseHero> Heroes => heroes;
    
    public bool HasHeroes => heroes.Count > 0;
    
    public HeroClassType CurrentHeroClass { get; private set; }

    private void Awake()
    {
        SetInitialClassType();
    }

    /// <summary>
    /// 단일 영웅 추가
    /// </summary>
    public void AddSingleHero(BaseHero hero)
    {
        if (heroes.Contains(hero)) return;

        heroes.Add(hero);
        hero.transform.SetParent(transform);
        SetHeroPosition(hero);
    }

    /// <summary>
    /// 단일 영웅 제거
    /// </summary>
    public void RemoveSingleHero(BaseHero hero)
    {
        if (!heroes.Contains(hero)) return;

        heroes.Remove(hero);
    }

    /// <summary>
    /// 모든 영웅 제거 및 반환
    /// </summary>
    public List<BaseHero> RemoveAllHeroes()
    {
        var removedHeroes = new List<BaseHero>(heroes);
        heroes.Clear();
        return removedHeroes;
    }

    /// <summary>
    /// 영역 하이라이트 설정
    /// </summary>
    public void SetHighlight(bool highlight)
    {
        isHighlighted = highlight;
        if (highlightRenderer != null)
        {
            highlightRenderer.enabled = highlight;
        }
    }

    /// <summary>
    /// 초기 클래스 타입 지정
    /// </summary>
    private void SetInitialClassType()
    {
        CurrentHeroClass = areaType switch
        {
            HeroAreaType.Top => HeroClassType.Warrior,
            HeroAreaType.Left => HeroClassType.Archer,
            HeroAreaType.Right => HeroClassType.None,
            HeroAreaType.Bottom => HeroClassType.Magician,
            _ => CurrentHeroClass
        };
    }

    /// <summary>
    /// 영웅 이동
    /// </summary>
    private void SetHeroPosition(BaseHero hero)
    {
        hero.Move(centerPoint.position);
    }
}