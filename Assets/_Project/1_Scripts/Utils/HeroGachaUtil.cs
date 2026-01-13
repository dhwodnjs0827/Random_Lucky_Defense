using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 영웅 가챠 전용 유틸 클래스
/// </summary>
public static class HeroGachaUtil
{
    private static IList<HeroRuntimeData> HeroPool => PlayerDataManager.Instance.AllHeroes;

    private static readonly HeroClassType[] classTable =
    {
        HeroClassType.Magician,
        HeroClassType.Archer,
        HeroClassType.Warrior
    };

    private static readonly HeroGradeType[] gradeTable =
    {
        HeroGradeType.Normal,
        HeroGradeType.Superior,
        HeroGradeType.Rare,
        HeroGradeType.Ancient,
        HeroGradeType.Relic,
        HeroGradeType.Legend,
        HeroGradeType.Epic,
        HeroGradeType.Myth,
        HeroGradeType.God
    };

    private static readonly List<(HeroRankType rank, int probability)> rankTable = new()
    {
        (HeroRankType.B, GameConstants.RANK_B_CHANCE),
        (HeroRankType.A, GameConstants.RANK_A_CHANCE),
        (HeroRankType.S, GameConstants.RANK_S_CHANCE)
    };

    /// <summary>
    /// 영웅 랜덤 뽑기
    /// </summary>
    /// <param name="gachaCount">뽑기 횟수</param>
    public static List<HeroRuntimeData> Gacha(int gachaCount)
    {
        List<HeroRuntimeData> results = new();

        for (int i = 0; i < gachaCount; i++)
        {
            results.Add(GachaOnce());
        }

        return results;
    }

    private static HeroClassType RollClass()
    {
        var index = Random.Range(0, classTable.Length);
        return classTable[index];
    }

    private static HeroGradeType RollGrade()
    {
        var index = Random.Range(0, gradeTable.Length);
        return gradeTable[index];
    }

    private static HeroRankType RollRank()
    {
        float roll = Random.Range(0f, 100f);
        float cumulative = 0f;

        foreach (var entry in rankTable)
        {
            cumulative += entry.probability;
            if (roll <= cumulative)
                return entry.rank;
        }

        return HeroRankType.B;
    }

    private static HeroRuntimeData GachaOnce()
    {
        var heroClass = RollClass();
        var heroGrade = RollGrade();
        var heroRank = RollRank();

        var hero = HeroPool.FirstOrDefault(h =>
            h.Class == heroClass &&
            h.Grade == heroGrade &&
            h.Rank == heroRank);

        return hero;
    }
}

public struct HeroGachaResult
{
    public int HeroID;
    public HeroClassType Class;
    public HeroRankType Rank;
    public HeroGradeType Grade;
}