using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 4개 영역 관리, 클래스→영역 매핑, 드래그/스왑 처리
/// </summary>
public class HeroAreaController : MonoBehaviour
{
    [Header("영역 설정")] [SerializeField] private HeroArea topArea; // 초기 영역: 전사
    [SerializeField] private HeroArea leftArea; // 초기 영역: 궁수
    [SerializeField] private HeroArea bottomArea; // 초기 영역: 마법사
    [SerializeField] private HeroArea rightArea; // 초기 영역: 빈 영역
    [SerializeField] private Transform spawnPoint; // 중앙 스폰 위치

    private Camera mainCamera;

    private Dictionary<HeroAreaType, HeroArea> areas;

    private HeroArea selectedArea;
    private bool isDragging;

    public Transform SpawnPoint => spawnPoint;

    #region Unity Methods

    private void Awake()
    {
        mainCamera = Camera.main;

        InitializeAreas();
    }

    private void Update()
    {
        HandleInput();
    }

    #endregion

    /// <summary>
    /// 영역 초기화
    /// </summary>
    private void InitializeAreas()
    {
        areas = new Dictionary<HeroAreaType, HeroArea>
        {
            { HeroAreaType.Top, topArea },
            { HeroAreaType.Left, leftArea },
            { HeroAreaType.Bottom, bottomArea },
            { HeroAreaType.Right, rightArea }
        };
    }

    /// <summary>
    /// 영웅을 클래스에 맞는 영역에 배치
    /// </summary>
    public void PlaceHero(BaseHero hero)
    {
        var classType = hero.ClassType;

        foreach (var kvp in areas)
        {
            if (kvp.Value.CurrentHeroClass == classType)
            {
                kvp.Value.AddSingleHero(hero);
                return;
            }
        }

        // 모든 영역에 영웅 존재하지 않으면 Fallback
        PlaceHeroFallback(hero);
    }

    /// <summary>
    /// 일치하는 타입 없을 경우, 우선순위로 배치 
    /// </summary>
    /// <remarks>좌 -> 상 -> 하 -> 우 순서로 배치</remarks>
    private void PlaceHeroFallback(BaseHero hero)
    {
        if (areas[HeroAreaType.Left].CurrentHeroClass == HeroClassType.None)
        {
            areas[HeroAreaType.Left].AddSingleHero(hero);
        }
        else if (areas[HeroAreaType.Top].CurrentHeroClass == HeroClassType.None)
        {
            areas[HeroAreaType.Top].AddSingleHero(hero);
        }
        else if (areas[HeroAreaType.Bottom].CurrentHeroClass == HeroClassType.None)
        {
            areas[HeroAreaType.Bottom].AddSingleHero(hero);
        }
        else if (areas[HeroAreaType.Right].CurrentHeroClass == HeroClassType.None)
        {
            areas[HeroAreaType.Right].AddSingleHero(hero);
        }
    }

    /// <summary>
    /// 입력 처리
    /// </summary>
    private void HandleInput()
    {
        var pointer = Pointer.current;
        if (pointer == null)
        {
            return;
        }

        // 터치/클릭 시작
        if (pointer.press.wasPressedThisFrame)
        {
            var worldPos = GetWorldPosition(pointer.position.ReadValue());
            var hitArea = GetAreaAtPosition(worldPos);

            if (hitArea != null)
            {
                OnPointerDown(hitArea);
            }
        }
        // 터치/클릭 드래그
        else if (pointer.press.isPressed && isDragging)
        {
            OnPointerDrag(pointer.position.ReadValue());
        }
        // 터치/클릭 끝
        else if (pointer.press.wasReleasedThisFrame && isDragging)
        {
            var worldPos = GetWorldPosition(pointer.position.ReadValue());
            OnPointerUp(worldPos);
        }
    }

    /// <summary>
    /// 터치/클릭 시작
    /// </summary>
    private void OnPointerDown(HeroArea hitArea)
    {
        if (!hitArea.HasHeroes)
        {
            return;
        }

        selectedArea = hitArea;
        selectedArea.SetHighlight(true);
        isDragging = true;
    }

    /// <summary>
    /// 드래그 중
    /// </summary>
    private void OnPointerDrag(Vector2 screenPosition)
    {
        //TODO: 드래그 중 시각적 피드백
    }

    /// <summary>
    /// 터치/클릭 종료
    /// </summary>
    private void OnPointerUp(Vector2 worldPosition)
    {
        if (selectedArea == null)
        {
            isDragging = false;
            return;
        }

        var targetArea = GetAreaAtPosition(worldPosition);

        // 다른 영역에 드롭했을 경우 스왑
        if (targetArea != null && targetArea != selectedArea)
        {
            SwapAreas(selectedArea, targetArea);
        }

        selectedArea.SetHighlight(false);
        selectedArea = null;
        isDragging = false;
    }

    /// <summary>
    /// 두 영역의 영웅들 스왑
    /// </summary>
    private void SwapAreas(HeroArea fromArea, HeroArea toArea)
    {
        var fromHeroes = fromArea.RemoveAllHeroes();
        var toHeroes = toArea.RemoveAllHeroes();

        foreach (var hero in fromHeroes)
        {
            toArea.AddSingleHero(hero);
        }

        foreach (var hero in toHeroes)
        {
            fromArea.AddSingleHero(hero);
        }
    }

    /// <summary>
    /// 화면 좌표를 월드 좌표로 변환
    /// </summary>
    private Vector2 GetWorldPosition(Vector2 screenPosition)
    {
        var worldPos = mainCamera.ScreenToWorldPoint(screenPosition);
        return new Vector2(worldPos.x, worldPos.y);
    }

    /// <summary>
    /// 월드 좌표에서 해당 영역 찾기
    /// </summary>
    private HeroArea GetAreaAtPosition(Vector2 worldPosition)
    {
        foreach (var kvp in areas)
        {
            if (kvp.Value.ContainsPointInArea(worldPosition))
            {
                return kvp.Value;
            }
        }

        return null;
    }

    #region Cheat

#if UNITY_EDITOR

    /// <summary>
    /// 현재 소환된 모든 영웅 반환 (치트용)
    /// </summary>
    public List<BaseHero> CheatGetAllSpawnedHeroes()
    {
        var allHeroes = new List<BaseHero>();
        foreach (var area in areas)
        {
            allHeroes.AddRange(area.Value.Heroes);
        }

        return allHeroes;
    }

    /// <summary>
    /// 특정 영웅 제거 (치트용)
    /// </summary>
    public void CheatRemoveHero(BaseHero hero)
    {
        foreach (var area in areas)
        {
            if (area.Value.Heroes.Contains(hero))
            {
                area.Value.RemoveSingleHero(hero);
                Destroy(hero.gameObject);
                return;
            }
        }
    }

    /// <summary>
    /// 모든 영웅 제거 (치트용)
    /// </summary>
    public void CheatRemoveAllHeroes()
    {
        foreach (var area in areas)
        {
            var heroes = area.Value.RemoveAllHeroes();
            foreach (var hero in heroes)
            {
                Destroy(hero.gameObject);
            }
        }
    }

#endif

    #endregion
}