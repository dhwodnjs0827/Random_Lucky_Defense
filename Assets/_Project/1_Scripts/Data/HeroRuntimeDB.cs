using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 런타임용 HeroDataBase - Dictionary 기반 O(1) 조회 제공
/// </summary>
public class HeroRuntimeDB
{
    private readonly List<HeroRuntimeData> allHeroes;

    // 단일 키 조회용
    private readonly Dictionary<int, HeroRuntimeData> heroByID;
    private readonly Dictionary<HeroClassType, List<HeroRuntimeData>> heroesByClass;
    private readonly Dictionary<HeroGradeType, List<HeroRuntimeData>> heroesByGrade;

    // 복합 키 조회용 (Class + Grade)
    private readonly Dictionary<HeroClassType, Dictionary<HeroGradeType, List<HeroRuntimeData>>> heroesByClassAndGrade;

    // 선택된 영웅 조회용 (Class + Grade -> 단일 영웅)
    private readonly Dictionary<HeroClassType, Dictionary<HeroGradeType, HeroRuntimeData>> selectedHeroes;
    
    /// <summary>
    /// 전체 영웅 목록
    /// </summary>
    public IList<HeroRuntimeData> AllHeroes => allHeroes;

    /// <summary>
    /// 생성자 - 영웅 데이터로 초기화
    /// </summary>
    public HeroRuntimeDB(IList<HeroRuntimeData> heroes)
    {
        allHeroes = heroes.ToList();

        heroByID = new Dictionary<int, HeroRuntimeData>();
        heroesByClass = new Dictionary<HeroClassType, List<HeroRuntimeData>>();
        heroesByGrade = new Dictionary<HeroGradeType, List<HeroRuntimeData>>();
        heroesByClassAndGrade = new Dictionary<HeroClassType, Dictionary<HeroGradeType, List<HeroRuntimeData>>>();
        selectedHeroes = new Dictionary<HeroClassType, Dictionary<HeroGradeType, HeroRuntimeData>>();

        foreach (var hero in allHeroes)
        {
            // ID 인덱스
            heroByID[hero.ID] = hero;

            // Class 인덱스
            if (!heroesByClass.ContainsKey(hero.Class))
            {
                heroesByClass[hero.Class] = new List<HeroRuntimeData>();
            }
            heroesByClass[hero.Class].Add(hero);

            // Grade 인덱스
            if (!heroesByGrade.ContainsKey(hero.Grade))
            {
                heroesByGrade[hero.Grade] = new List<HeroRuntimeData>();
            }
            heroesByGrade[hero.Grade].Add(hero);

            // Class + Grade 복합 인덱스
            if (!heroesByClassAndGrade.ContainsKey(hero.Class))
            {
                heroesByClassAndGrade[hero.Class] = new Dictionary<HeroGradeType, List<HeroRuntimeData>>();
            }
            if (!heroesByClassAndGrade[hero.Class].ContainsKey(hero.Grade))
            {
                heroesByClassAndGrade[hero.Class][hero.Grade] = new List<HeroRuntimeData>();
            }
            heroesByClassAndGrade[hero.Class][hero.Grade].Add(hero);

            // 선택된 영웅 인덱스
            if (!selectedHeroes.ContainsKey(hero.Class))
            {
                selectedHeroes[hero.Class] = new Dictionary<HeroGradeType, HeroRuntimeData>();
            }
            if (hero.IsSelected)
            {
                selectedHeroes[hero.Class][hero.Grade] = hero;
            }
        }
    }

    #region 조회 메서드

    /// <summary>
    /// ID로 영웅 조회
    /// </summary>
    public HeroRuntimeData GetByID(int id)
    {
        return heroByID.TryGetValue(id, out var hero) ? hero : null;
    }

    /// <summary>
    /// 클래스별 영웅 목록 조회
    /// </summary>
    public List<HeroRuntimeData> GetByClass(HeroClassType classType)
    {
        return heroesByClass.TryGetValue(classType, out var heroes) ? heroes : new List<HeroRuntimeData>();
    }

    /// <summary>
    /// 등급별 영웅 목록 조회
    /// </summary>
    public List<HeroRuntimeData> GetByGrade(HeroGradeType grade)
    {
        return heroesByGrade.TryGetValue(grade, out var heroes) ? heroes : new List<HeroRuntimeData>();
    }

    /// <summary>
    /// 클래스 + 등급으로 영웅 목록 조회
    /// </summary>
    public List<HeroRuntimeData> GetByClassAndGrade(HeroClassType classType, HeroGradeType grade)
    {
        if (heroesByClassAndGrade.TryGetValue(classType, out var gradeDict))
        {
            if (gradeDict.TryGetValue(grade, out var heroes))
            {
                return heroes;
            }
        }
        return new List<HeroRuntimeData>();
    }

    /// <summary>
    /// 클래스 + 등급으로 선택된 영웅 조회
    /// </summary>
    public HeroRuntimeData GetSelectedHero(HeroClassType classType, HeroGradeType grade)
    {
        if (selectedHeroes.TryGetValue(classType, out var gradeDict))
        {
            if (gradeDict.TryGetValue(grade, out var hero))
            {
                return hero;
            }
        }
        return null;
    }

    /// <summary>
    /// 클래스별 선택된 영웅 목록 조회
    /// </summary>
    public Dictionary<HeroGradeType, HeroRuntimeData> GetSelectedHeroesByClass(HeroClassType classType)
    {
        return selectedHeroes.TryGetValue(classType, out var gradeDict)
            ? gradeDict
            : new Dictionary<HeroGradeType, HeroRuntimeData>();
    }

    /// <summary>
    /// 클래스별 획득한 영웅 목록 조회
    /// </summary>
    public List<HeroRuntimeData> GetAcquiredHeroesByClass(HeroClassType classType)
    {
        if (heroesByClass.TryGetValue(classType, out var heroes))
        {
            return heroes.Where(h => h.IsAcquiredHero).ToList();
        }
        return new List<HeroRuntimeData>();
    }

    #endregion

    #region 데이터 동기화 메서드

    /// <summary>
    /// 영웅 선택 상태 변경 시 호출
    /// </summary>
    public void UpdateSelectedHero(HeroRuntimeData unequipHero, HeroRuntimeData equipHero)
    {
        if (unequipHero != null)
        {
            if (selectedHeroes.TryGetValue(unequipHero.Class, out var gradeDict))
            {
                gradeDict.Remove(unequipHero.Grade);
            }
        }

        if (equipHero != null)
        {
            if (!selectedHeroes.ContainsKey(equipHero.Class))
            {
                selectedHeroes[equipHero.Class] = new Dictionary<HeroGradeType, HeroRuntimeData>();
            }
            selectedHeroes[equipHero.Class][equipHero.Grade] = equipHero;
        }
    }

    #endregion
}