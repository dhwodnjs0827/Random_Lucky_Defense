using System.Collections.Generic;
using UnityEngine;

public static class HeroGachaUtil
{
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

    public static List<HeroGachaResult> Gacha(int gachaCount)
    {
        List<HeroGachaResult> results = new();

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

        return HeroRankType.B; // 안전장치
    }

    private static HeroGachaResult GachaOnce()
    {
        return new HeroGachaResult
        {
            Class = RollClass(),
            Rank = RollRank(),
            Grade = RollGrade()
        };
    }
}

public struct HeroGachaResult
{
    public HeroClassType Class;
    public HeroRankType Rank;
    public HeroGradeType Grade;
}