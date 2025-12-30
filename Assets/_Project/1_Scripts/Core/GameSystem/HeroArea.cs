using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 영웅이 위치할 개별 삼각형 영역
/// </summary>
public class HeroArea : MonoBehaviour
{
    [SerializeField] private HeroAreaType areaType;
    private SpriteRenderer highlightRenderer;

    [Header("영역 설정")] [SerializeField] private Transform[] triAreaPoints;
    [SerializeField] private Transform[] rectAreaPoints;

    [Header("등급별 위치 설정")] [SerializeField] private GradePosition[] gradePositions;

    private List<BaseHero> heroes = new();
    private bool isHighlighted = false;

    public HeroAreaType AreaType => areaType;

    public IReadOnlyList<BaseHero> Heroes => heroes;

    public bool HasHeroes => heroes.Count > 0;

    public HeroClassType CurrentHeroClass
    {
        get
        {
            if (heroes.Count != 0)
            {
                return heroes[0].ClassType;
            }

            return HeroClassType.None;
        }
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
    /// 터치 지점이 영역 내부인지 검사 (삼각형 + 직사각형 영역)
    /// </summary>
    public bool ContainsPointInArea(Vector2 worldPoint)
    {
        // 삼각형 검사
        if (IsPointInTriangle(worldPoint))
        {
            return true;
        }

        // 직사각형 검사
        if (IsPointInRectangle(worldPoint))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 터치 지점이 영역 내부인지 검사 (삼각형 영역)
    /// </summary>
    private bool IsPointInTriangle(Vector2 worldPoint)
    {
        if (triAreaPoints == null || triAreaPoints.Length < 3) return false;

        Vector2 p0 = triAreaPoints[0].position;
        Vector2 p1 = triAreaPoints[1].position;
        Vector2 p2 = triAreaPoints[2].position;

        return IsPointInArea(worldPoint, p0, p1, p2);
    }

    /// <summary>
    /// 터치 지점이 영역 내부인지 검사 (직사각형 영역)
    /// </summary>
    private bool IsPointInRectangle(Vector2 worldPoint)
    {
        if (triAreaPoints == null || triAreaPoints.Length < 3) return false;
        if (rectAreaPoints == null || rectAreaPoints.Length < 2) return false;

        // 직사각형 4개 점: 삼각형 밑변(1,2) + 하단 점(rect 0,1)
        Vector2 topLeft = triAreaPoints[1].position;
        Vector2 topRight = triAreaPoints[2].position;
        Vector2 bottomLeft = rectAreaPoints[0].position;
        Vector2 bottomRight = rectAreaPoints[1].position;

        // 두 개의 삼각형으로 분할하여 검사
        return IsPointInArea(worldPoint, topLeft, bottomLeft, bottomRight) ||
               IsPointInArea(worldPoint, topLeft, bottomRight, topRight);
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
    /// 영웅 이동
    /// </summary>
    private void SetHeroPosition(BaseHero hero)
    {
        var targetPosition = GetRandomPosition(hero.GradeType);

        hero.Move(targetPosition);
    }

    /// <summary>
    /// 등급별 랜덤 위치
    /// </summary>
    private Vector2 GetRandomPosition(HeroGradeType gradeType)
    {
        var gradePosition = gradePositions[(int)gradeType];

        if (gradePosition.transforms == null || gradePosition.transforms.Length < 2)
        {
            return transform.position;
        }

        Vector2 pointA = gradePosition.transforms[0].position;
        Vector2 pointB = gradePosition.transforms[1].position;

        float t = UnityEngine.Random.Range(0f, 1f);
        return Vector2.Lerp(pointA, pointB, t);
    }

    private bool IsPointInArea(Vector2 worldPoint, Vector2 point1, Vector2 point2, Vector2 point3)
    {
        var distance1 = Sign(worldPoint, point1, point2);
        var distance2 = Sign(worldPoint, point2, point3);
        var distance3 = Sign(worldPoint, point3, point1);

        bool hasNeg = (distance1 < 0) || (distance2 < 0) || (distance3 < 0);
        bool hasPos = (distance1 > 0) || (distance2 > 0) || (distance3 > 0);

        return !(hasNeg && hasPos);
    }

    private float Sign(Vector2 point1, Vector2 point2, Vector2 point3)
    {
        return (point1.x - point3.x) * (point2.y - point3.y) - (point2.x - point3.x) * (point1.y - point3.y);
    }

    /// <summary>
    /// 등급별 위치 지정용
    /// </summary>
    [Serializable]
    private struct GradePosition
    {
        public HeroGradeType grade;
        public Transform[] transforms;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = isHighlighted ? Color.yellow : Color.green;

        // 삼각형
        if (triAreaPoints != null && triAreaPoints.Length >= 3)
        {
            // 삼각형 외곽선 (밑변 제외 - 직사각형과 공유)
            Gizmos.DrawLine(triAreaPoints[0].position, triAreaPoints[1].position);
            Gizmos.DrawLine(triAreaPoints[0].position, triAreaPoints[2].position);

            foreach (var point in triAreaPoints)
            {
                Gizmos.DrawSphere(point.position, 0.1f);
            }
        }

        // 직사각형 (삼각형 밑변 + 하단 점)
        if (triAreaPoints != null && triAreaPoints.Length >= 3 &&
            rectAreaPoints != null && rectAreaPoints.Length >= 2)
        {
            // 삼각형 밑변 (직사각형 윗변)
            Gizmos.DrawLine(triAreaPoints[1].position, triAreaPoints[2].position);
            // 직사각형 양쪽 변
            Gizmos.DrawLine(triAreaPoints[1].position, rectAreaPoints[0].position);
            Gizmos.DrawLine(triAreaPoints[2].position, rectAreaPoints[1].position);
            // 직사각형 밑변
            Gizmos.DrawLine(rectAreaPoints[0].position, rectAreaPoints[1].position);

            foreach (var point in rectAreaPoints)
            {
                Gizmos.DrawSphere(point.position, 0.1f);
            }
        }

        // 등급별 위치
        foreach (var gradePosition in gradePositions)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(gradePosition.transforms[0].position, 0.1f);
            Gizmos.DrawSphere(gradePosition.transforms[1].position, 0.1f);
            Gizmos.DrawLine(gradePosition.transforms[0].position, gradePosition.transforms[1].position);
        }
    }
#endif
}