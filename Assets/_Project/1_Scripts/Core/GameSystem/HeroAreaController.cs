using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 4개 영역 관리, 클래스→영역 매핑, 드래그/스왑 처리
/// </summary>
public class HeroAreaController : MonoBehaviour
{
    [Header("영역 설정")]
    [SerializeField] private HeroArea topArea; // 초기 영역: 전사
    [SerializeField] private HeroArea leftArea; // 초기 영역: 궁수
    [SerializeField] private HeroArea bottomArea; // 초기 영역: 마법사
    [SerializeField] private HeroArea rightArea; // 초기 영역: 빈 영역
    [SerializeField] private Transform spawnPoint; // 중앙 스폰 위치
    
    private Camera mainCamera;

    private Dictionary<HeroAreaType, HeroArea> areas;

    private HeroArea selectedArea;
    private bool isDragging;

    private void Awake()
    {
        mainCamera = Camera.main;
        
        InitializeAreas();
    }

    private void Update()
    {
        HandleInput();
    }

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
                CDebug.Log($"[HeroAreaController] {hero.name}을(를) {kvp.Value.CurrentHeroClass} 영역에 배치");
                return;
            }
        }
    }

    /// <summary>
    /// 입력 처리
    /// </summary>
    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnPointerDown();
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            OnPointerDrag();
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            OnPointerUp();
        }
    }

    /// <summary>
    /// 터치/클릭 시작
    /// </summary>
    private void OnPointerDown()
    {
        var hitArea = GetAreaAtPosition(Input.mousePosition);
        if (hitArea == null || !hitArea.HasHeroes) return;

        selectedArea = hitArea;
        selectedArea.SetHighlight(true);
        isDragging = true;
    }

    /// <summary>
    /// 드래그 중
    /// </summary>
    private void OnPointerDrag()
    {
        //TODO: 드래그 중 시각적 피드백
    }

    /// <summary>
    /// 터치/클릭 종료
    /// </summary>
    private void OnPointerUp()
    {
        if (selectedArea == null)
        {
            isDragging = false;
            return;
        }

        var targetArea = GetAreaAtPosition(Input.mousePosition);

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

        CDebug.Log($"[HeroAreaController] {fromArea.AreaType} ↔ {toArea.AreaType} 스왑 완료");
    }

    /// <summary>
    /// 화면 위치에서 영역 찾기
    /// </summary>
    private HeroArea GetAreaAtPosition(Vector3 screenPosition)
    {
        var worldPos = mainCamera.ScreenToWorldPoint(screenPosition);
        worldPos.z = 0;

        // 중앙 기준으로 어느 삼각형 영역인지 판별
        var center = spawnPoint.position;
        var direction = worldPos - center;

        // 대각선 기준으로 4개 영역 판별
        // 위쪽: y > |x|
        // 아래쪽: y < -|x|
        // 왼쪽: x < -|y|
        // 오른쪽: x > |y|

        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);

        HeroAreaType areaType;

        if (direction.y > absX)
        {
            areaType = HeroAreaType.Top;
        }
        else if (direction.y < -absX)
        {
            areaType = HeroAreaType.Bottom;
        }
        else if (direction.x < -absY)
        {
            areaType = HeroAreaType.Left;
        }
        else
        {
            areaType = HeroAreaType.Right;
        }

        return areas[areaType];
    }

    /// <summary>
    /// 중앙 스폰 위치 반환
    /// </summary>
    public Vector3 GetSpawnPosition()
    {
        return spawnPoint.position;
    }
}